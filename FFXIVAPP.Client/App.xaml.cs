using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Models;
using FFXIVAPP.Client.SettingsProviders.Application;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.ResourceFiles;
using NLog;
using NLog.Config;

namespace FFXIVAPP.Client
{
    public class App : Application {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        // TODO: private static List<DirectSoundDeviceInfo> _availableAudioDevices;

        // TODO: private static IEnumerable<NetworkInterface> _availableNetworkInterfaces;

        public App()
        {
            ConfigureNLog();
            Settings.Default.PropertyChanged += SettingsPropertyChanged;
            CheckSettings();
        }

        public static string[] MArgs { get; private set; }

        /* TODO: Audio and Network
        internal static IEnumerable<DirectSoundDeviceInfo> AvailableAudioDevices {
            get {
                return _availableAudioDevices ?? (_availableAudioDevices = new List<DirectSoundDeviceInfo>(DirectSoundOut.Devices.Where(d => d.Guid != Guid.Empty)));
            }
        }

        internal static IEnumerable<NetworkInterface> AvailableNetworkInterfaces {
            get {
                return _availableNetworkInterfaces ?? (_availableNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces());
            }
        }
        */

        internal static PluginHost Plugins {
            get {
                return PluginHost.Instance;
            }
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            AppBootstrapper.Initialize();
        }

        private static void CheckSettings() {
            Common.Constants.EnableNLog = Settings.Default.EnableNLog;
            try {
                if (!Settings.Default.Application_UpgradeRequired) {
                    Settings.Reload();
                    return;
                }

                Settings.Reload();
                Settings.Default.Application_UpgradeRequired = false;
            }
            catch (Exception) {
                SettingsHelper.Default();
            }
        }

        /// <summary>
        /// </summary>
        private static void ConfigureNLog() {
            var hasLocal = false;
            const string fileName = "./FFXIVAPP.Client.exe.nlog";
            if (File.Exists(fileName)) {
                hasLocal = true;
            }

            var resource = ResourceHelper.StreamResource(Constants.AppPack + "Resources.FFXIVAPP.Client.exe.nlog");
            if (resource == null) {
                return;
            }

            StringReader stringReader;
            if (hasLocal) {
                stringReader = new StringReader(XElement.Load(fileName).ToString());
            }
            else {
                stringReader = new StringReader(XElement.Load(resource).ToString());
            }

            using (XmlReader xmlReader = XmlReader.Create(stringReader)) {
                LogManager.Configuration = new XmlLoggingConfiguration(xmlReader, null);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private static void SettingsPropertyChanged(object sender, PropertyChangedEventArgs e) {
            Logging.Log(Logger, $"PropertyChanged : {e.PropertyName}");
            try {
                switch (e.PropertyName) {
                    case "CharacterName":
                        Constants.CharacterName = Settings.Default.CharacterName;
                        break;
                    case "GameLanguage":
                        Constants.GameLanguage = Settings.Default.GameLanguage;
                        break;
                    case "UILanguage":
                        if (AppViewModel.Instance.UILanguages.Any(ui => ui.Language == Settings.Default.UILanguage)) {
                            UILanguage uiLanguage = AppViewModel.Instance.UILanguages.First(ui => ui.Language == Settings.Default.UILanguage);
                            Constants.CultureInfo = Settings.Default.Culture = uiLanguage.CultureInfo;
                            LocaleHelper.Update(Settings.Default.Culture);
                        }

                        break;
                    case "ServerName":
                        Constants.ServerName = Settings.Default.ServerName;
                        break;
                    case "EnableNLog":
                        Common.Constants.EnableNLog = Constants.EnableNLog = Settings.Default.EnableNLog;
                        break;
                    case "EnableNetworkReading":
                        /* TODO: Network
                        Common.Constants.EnableNetworkReading = Constants.EnableNetworkReading = Settings.Default.EnableNetworkReading;
                        if (Settings.Default.EnableNetworkReading) {
                            if (!Initializer.NetworkWorking && Constants.IsOpen) {
                                Initializer.StartNetworkWorker();
                            }
                        }
                        else {
                            Initializer.StopNetworkWorker();
                        }
                        */
                        break;
                    case "NetworkUseWinPCap":
                        /* TODO: Network
                        if (Initializer.NetworkWorking) {
                            Initializer.RefreshNetworkWorker();
                        }
                        */
                        break;
                    case "EnableHelpLabels":
                        Constants.EnableHelpLabels = Settings.Default.EnableHelpLabels;
                        break;
                    case "Theme":
                        Constants.Theme = Settings.Default.Theme;
                        break;
                    case "UIScale":
                        Constants.UIScale = Settings.Default.UIScale;
                        break;
                    case "DefaultAudioDevice":
                        /*  TODO: Audio
                        if (Settings.Default.DefaultAudioDevice == "System Default") {
                            Common.Constants.DefaultAudioDevice = Guid.Empty;
                        }
                        else {
                            foreach (DirectSoundDeviceInfo audioDevice in AvailableAudioDevices.Where(device => device.Guid != Guid.Empty)) {
                                if (audioDevice.Description == Settings.Default.DefaultAudioDevice) {
                                    Common.Constants.DefaultAudioDevice = audioDevice.Guid;
                                }
                            }
                        }
                        */
                        break;
                    case nameof(SettingModel.Top):
                    case nameof(SettingModel.Left):
                    {
                        var iX = (int)ShellView.View.Position.X;
                        var iY = (int)ShellView.View.Position.Y;
                        if (iX != Settings.Default.Left || iY != Settings.Default.Top) {
                            ShellView.View.Position = new Avalonia.Point(Settings.Default.Left, Settings.Default.Top);
                        }
                        break;
                    }
                }
            }
            catch (Exception ex) {
                Logging.Log(Logger, new LogItem(ex, true));
            }
        }

        protected override void OnExiting(object sender, System.EventArgs e)
        {
            CloseApplication(false);
        }

        /// <summary>
        /// </summary>
        /// <param name="update"></param>
        public static void CloseApplication(bool update = false) {
            SettingsHelper.Save(update);
            foreach (PluginInstance pluginInstance in App.Plugins.Loaded.Cast<PluginInstance>().Where(pluginInstance => pluginInstance.Loaded)) {
                pluginInstance.Instance.Dispose(update);
            }

            SavedlLogsHelper.SaveCurrentLog(false);

            foreach(var window in Avalonia.Application.Current.Windows.ToList()) {
                window.Close();
            }

            CloseDelegate(update);
        }

        /// <summary>
        /// </summary>
        /// <param name="update"></param>
        private static void CloseDelegate(bool update = false) {
            // TODO: AppViewModel.Instance.NotifyIcon.Visible = false;
            if (update) {
                /* TODO: Updater process handling
                try {
                    Process[] updaters = Process.GetProcessesByName("FFXIVAPP.Updater");
                    foreach (Process updater in updaters) {
                        updater.Kill();
                    }

                    if (File.Exists("FFXIVAPP.Updater.exe")) {
                        File.Delete("FFXIVAPP.Updater.Backup.exe");
                    }

                    File.Move("FFXIVAPP.Updater.exe", "FFXIVAPP.Updater.Backup.exe");
                }
                catch (Exception ex) {
                    Logging.Log(Logger, new LogItem(ex, true));
                }

                Process.Start("FFXIVAPP.Updater.Backup.exe", $"{AppViewModel.Instance.DownloadUri} {AppViewModel.Instance.LatestVersion}");
                */
            }
        }
   }
}