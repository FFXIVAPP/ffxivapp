// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Initializer.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Initializer.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Cache;
    using System.Net.NetworkInformation;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Xml.Linq;
    using Avalonia.Controls;
    using FFXIVAPP.Client.Helpers;
    using FFXIVAPP.Client.Memory;
    using FFXIVAPP.Client.Models;
    using FFXIVAPP.Client.SettingsProviders.Application;
    using FFXIVAPP.Client.Utilities;
    using FFXIVAPP.Client.ViewModels;
    using FFXIVAPP.Client.Views;
    using FFXIVAPP.Common.Helpers;
    using FFXIVAPP.Common.Models;
    using FFXIVAPP.Common.RegularExpressions;
    using FFXIVAPP.Common.Utilities;

    using Newtonsoft.Json.Linq;

    using NLog;

    using Sharlayan;
    using Sharlayan.Events;
    using Sharlayan.Models;

    using ExceptionEvent = Sharlayan.Events.ExceptionEvent;

    internal static class Initializer {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static ActorWorker _actorWorker;

        private static ChatLogWorker _chatLogWorker;

        private static HotBarRecastWorker _hotBarRecastWorker;

        private static InventoryWorker _inventoryWorker;

        private static PartyInfoWorker _partyInfoWorker;

        private static PlayerInfoWorker _playerInfoWorker;

        private static TargetWorker _targetWorker;

        /* TODO: Network
        public static bool NetworkWorking {
            get {
                return NetworkHandler.Instance.IsRunning;
            }
        }
        */

        /// <summary>
        /// </summary>
        public static void CheckUpdates() {
            try {
                Process[] updaters = Process.GetProcessesByName("FFXIVAPP.Updater");
                foreach (Process updater in updaters) {
                    updater.Kill();
                }

                if (File.Exists("FFXIVAPP.Updater.exe")) {
                    File.Delete("FFXIVAPP.Updater.Backup.exe");
                }
                else {
                    if (File.Exists("FFXIVAPP.Updater.Backup.exe")) {
                        File.Move("FFXIVAPP.Updater.Backup.exe", "FFXIVAPP.Updater.exe");
                    }
                }
            }
            catch (Exception ex) {
                Logging.Log(Logger, new LogItem(ex, true));
            }

            Func<bool> update = delegate {
                var current = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                AppViewModel.Instance.CurrentVersion = current;
                var httpWebRequest = (HttpWebRequest) WebRequest.Create("https://api.github.com/repos/Icehunter/ffxivapp/releases");
                httpWebRequest.UserAgent = "Icehunter-FFXIVAPP";
                httpWebRequest.Headers.Add("Accept-Language", "en;q=0.8");
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                using (var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse()) {
                    using (Stream response = httpResponse.GetResponseStream()) {
                        var responseText = string.Empty;
                        if (response != null) {
                            using (var streamReader = new StreamReader(response)) {
                                responseText = streamReader.ReadToEnd();
                            }
                        }

                        var latestBuild = new BuildNumber();
                        var currentBuild = new BuildNumber();
                        if (httpResponse.StatusCode != HttpStatusCode.OK || string.IsNullOrWhiteSpace(responseText)) {
                            AppViewModel.Instance.HasNewVersion = false;
                            AppViewModel.Instance.LatestVersion = "Unknown";
                        }
                        else {
                            JArray releases = JArray.Parse(responseText);
                            JToken release = releases.FirstOrDefault(r => r?["target_commitish"].ToString() == "master");
                            var latest = release?["name"].ToString() ?? "Unknown";
                            latest = latest.Split(' ')[0];
                            AppViewModel.Instance.LatestVersion = latest;
                            switch (latest) {
                                case "Unknown":
                                    AppViewModel.Instance.HasNewVersion = false;
                                    break;
                                default:
                                    AppViewModel.Instance.DownloadUri = string.Format("https://github.com/Icehunter/ffxivapp/releases/download/{0}/{0}.zip", latest);
                                    AppViewModel.Instance.HasNewVersion = BuildUtilities.NeedsUpdate(latest, current, ref latestBuild, ref currentBuild);
                                    break;
                            }

                            if (AppViewModel.Instance.HasNewVersion) {
                                var title = $"{AppViewModel.Instance.Locale["app_DownloadNoticeHeader"]} {AppViewModel.Instance.Locale["app_DownloadNoticeMessage"]}";
                                var message = new StringBuilder();
                                try {
                                    DateTime latestBuildDateTime = new DateTime(2000, 1, 1).Add(new TimeSpan(TimeSpan.TicksPerDay * latestBuild.Build + TimeSpan.TicksPerSecond * 2 * latestBuild.Revision));
                                    DateTime currentBuildDateTime = new DateTime(2000, 1, 1).Add(new TimeSpan(TimeSpan.TicksPerDay * currentBuild.Build + TimeSpan.TicksPerSecond * 2 * currentBuild.Revision));
                                    TimeSpan timeSpan = latestBuildDateTime - currentBuildDateTime;
                                    if (timeSpan.TotalSeconds > 0) {
                                        message.AppendLine(string.Format("Missing {0} days, {1} hours and {2} seconds of updates.", timeSpan.Days, timeSpan.Hours, timeSpan.Seconds));
                                    }
                                }
                                catch (Exception ex) {
                                    Logging.Log(Logger, new LogItem(ex, true));
                                }
                                finally {
                                    message.AppendLine(AppViewModel.Instance.Locale["app_AlwaysReadUpdatesMessage"]);
                                }

                                MessageBoxHelper.ShowMessageAsync(title, message.ToString(), () => App.CloseApplication(true), delegate { });
                            }

                            var uri = "https://ffxiv-app.com/Analytics/Google/?eCategory=Application Launch&eAction=Version Check&eLabel=FFXIVAPP";
                            // TODO: DispatcherHelper.Invoke(() => MainView.View.GoogleAnalytics.Navigate(uri));
                        }
                    }
                }

                return true;
            };
            
            Task.Run(() => update());
        }

        /// <summary>
        /// </summary>
        public static void GetHomePlugin() {
            var homePlugin = Settings.Default.HomePlugin;
            var called = false;
            switch (homePlugin) {
                case "None":
                    break;
                default:
                    try {
                        var index = SettingsViewModel.Instance.HomePluginList.IndexOf(homePlugin);
                        if (index > 0) {
                            SetHomePlugin(--index);
                            called = true;
                        }
                    }
                    catch (Exception ex) {
                        Logging.Log(Logger, new LogItem(ex, true));
                    }

                    break;
            }

            if (called == false) {
                if (AppViewModel.Instance.PluginTabSelected == null && AppViewModel.Instance.PluginTabItems?.Count > 0) {
                    SetHomePlugin(0);
                }

                AppViewModel.Instance.TabSelected = ShellView.View.ShellViewTC.Items.Cast<TabItem>().First();
            }
        }

        /// <summary>
        /// </summary>
        public static void LoadApplicationSettings() {
            if (Settings.Default != null) { }
        }

        /// <summary>
        /// </summary>
        public static void LoadAutoTranslate() {
            if (Constants.XAutoTranslate != null) {
                foreach (XElement xElement in Constants.XAutoTranslate.Descendants().Elements("Code")) {
                    var xKey = (string) xElement.Attribute("Key");
                    var xValue = (string) xElement.Element(Settings.Default.GameLanguage);
                    if (string.IsNullOrWhiteSpace(xKey) || string.IsNullOrWhiteSpace(xValue)) {
                        continue;
                    }

                    Constants.AutoTranslate.Add(xKey, xValue);
                }
            }
        }

        /* TODO: Audio
        /// <summary>
        /// </summary>
        public static void LoadAvailableAudioDevices() {
            foreach (DirectSoundDeviceInfo device in App.AvailableAudioDevices) {
                SettingsViewModel.Instance.AvailableAudioDevicesList.Add(device.Description);
            }
        }
        */
        /* TODO: Network
        public static void LoadAvailableNetworkDevices() {
            foreach (NetworkInterface networkInterface in App.AvailableNetworkInterfaces) {
                SettingsViewModel.Instance.AvailableNetworkInterfacesList.Add(networkInterface.Name);
            }
        }
        */
        
        public static void LoadAvailablePlugins() {
            UpdateViewModel.Instance.UpdatingAvailablePlugins = true;
            UpdateViewModel.Instance.AvailablePlugins.Clear();

            Func<bool> update = delegate {
                List<PluginSourceItem> pluginSourceList = new List<PluginSourceItem>();
                try {
                    //var httpWebRequest = (HttpWebRequest) WebRequest.Create("https://github.com/Icehunter/ffxivapp/raw/master/PACKAGES.json");
                    var httpWebRequest = (HttpWebRequest) WebRequest.Create("http://localhost/teast/ffxivapp/PACKAGES.json");
                    httpWebRequest.UserAgent = "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_3; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.70 Safari/533.4";
                    httpWebRequest.Headers.Add("Accept-Language", "en;q=0.8");
                    httpWebRequest.ContentType = "application/text; charset=utf-8";
                    httpWebRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                    using (var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse()) {
                        using (Stream response = httpResponse.GetResponseStream()) {
                            var responseText = string.Empty;
                            if (response != null) {
                                using (var streamReader = new StreamReader(response)) {
                                    responseText = streamReader.ReadToEnd();
                                }
                            }

                            if (httpResponse.StatusCode == HttpStatusCode.OK || !string.IsNullOrWhiteSpace(responseText)) {
                                JArray jsonResult = JArray.Parse(responseText);
                                foreach (JToken jToken in jsonResult) {
                                    bool enabled;
                                    bool.TryParse(jToken["Enabled"].ToString(), out enabled);
                                    var sourceURI = jToken["SourceURI"].ToString();

                                    if (enabled) {
                                        pluginSourceList.Add(
                                            new PluginSourceItem {
                                                Enabled = true,
                                                Key = Guid.NewGuid(),
                                                SourceURI = sourceURI
                                            });
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) {
                    Logging.Log(Logger, new LogItem(ex, true));
                }

                foreach (PluginSourceItem pluginSourceItem in UpdateViewModel.Instance.AvailableSources) {
                    if (pluginSourceList.Any(p => string.Equals(p.SourceURI, pluginSourceItem.SourceURI, Constants.InvariantComparer))) {
                        continue;
                    }

                    pluginSourceList.Add(pluginSourceItem);
                }

                foreach (PluginSourceItem item in pluginSourceList) {
                    if (item.Enabled) {
                        try {
                            var httpWebRequest = (HttpWebRequest) WebRequest.Create(item.SourceURI);
                            httpWebRequest.UserAgent = "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_3; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.70 Safari/533.4";
                            httpWebRequest.Headers.Add("Accept-Language", "en;q=0.8");
                            httpWebRequest.ContentType = "application/text; charset=utf-8";
                            httpWebRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                            using (var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse()) {
                                using (Stream response = httpResponse.GetResponseStream()) {
                                    var responseText = string.Empty;
                                    if (response != null) {
                                        using (var streamReader = new StreamReader(response)) {
                                            responseText = streamReader.ReadToEnd();
                                        }
                                    }

                                    if (httpResponse.StatusCode == HttpStatusCode.OK || !string.IsNullOrWhiteSpace(responseText)) {
                                        JObject jsonResult = JObject.Parse(responseText);
                                        JToken pluginInfo = jsonResult["PluginInfo"];
                                        JToken[] pluginFiles = pluginInfo["Files"].ToArray();
                                        var pluginDownload = new PluginDownloadItem {
                                            Files = new List<PluginFile>(
                                                pluginFiles.Select(
                                                    pluginFile => new PluginFile {
                                                        Location = pluginFile["Location"].ToString(),
                                                        Name = pluginFile["Name"].ToString(),
                                                        Checksum = pluginFile["Checksum"] == null
                                                                       ? string.Empty
                                                                       : pluginFile["Checksum"].ToString()
                                                    })),
                                            Name = pluginInfo["Name"].ToString(),
                                            FriendlyName = pluginInfo["FriendlyName"] == null
                                                               ? pluginInfo["Name"].ToString()
                                                               : pluginInfo["FriendlyName"].ToString(),
                                            Description = pluginInfo["Description"] == null
                                                              ? string.Empty
                                                              : pluginInfo["Description"].ToString(),
                                            SourceURI = pluginInfo["SourceURI"].ToString(),
                                            LatestVersion = pluginInfo["Version"].ToString()
                                        };
                                        PluginInstance found = App.Plugins.Loaded.Find(pluginDownload.Name);
                                        if (found != null) {
                                            var latest = pluginDownload.LatestVersion;
                                            var current = found.Instance.Version;
                                            pluginDownload.CurrentVersion = current;
                                            pluginDownload.Status = PluginStatus.Installed;
                                            var latestBuild = new BuildNumber();
                                            var currentBuild = new BuildNumber();
                                            if (BuildUtilities.NeedsUpdate(latest, current, ref latestBuild, ref currentBuild)) {
                                                pluginDownload.Status = PluginStatus.UpdateAvailable;
                                                AppViewModel.Instance.HasNewPluginUpdate = true;

                                                DispatcherHelper.Invoke(() => UpdateViewModel.Instance.AvailablePluginUpdates++);
                                            }
                                            else {
                                                if (!found.Loaded) {
                                                    pluginDownload.Status = PluginStatus.OutOfDate;
                                                }
                                            }
                                        }

                                        DispatcherHelper.Invoke(() => UpdateViewModel.Instance.AvailablePlugins.Add(pluginDownload));
                                    }
                                }
                            }
                        }
                        catch (Exception ex) {
                            Logging.Log(Logger, new LogItem(ex, true));
                        }
                    }
                }

                /* TODO: Can implement when we got DataGrid for installed plugins (or changed current datagrid so it can group)
                DispatcherHelper.Invoke(
                    delegate {
                        if (UpdateViewModel.Instance.AvailableDG.Count == UpdateViewModel.Instance.AvailablePlugins.Count){ 
                            // TODO: Hide/Collapse available plugins because they are all installed
                        }
                    });
                */

                UpdateViewModel.Instance.UpdatingAvailablePlugins = false;
                return true;
            };
            Task.Run(() => update());
        }

        /// <summary>
        /// </summary>
        public static void LoadChatCodes() {
            if (Constants.XChatCodes != null) {
                foreach (XElement xElement in Constants.XChatCodes.Descendants().Elements("Code")) {
                    var xKey = (string) xElement.Attribute("Key");
                    var xDescription = (string) xElement.Element("Description");
                    if (string.IsNullOrWhiteSpace(xKey) || string.IsNullOrWhiteSpace(xDescription)) {
                        continue;
                    }

                    Constants.ChatCodes.Add(xKey, xDescription);
                }
            }
        }

        /// <summary>
        /// </summary>
        public static void LoadColors() {
            if (Constants.XColors != null) {
                foreach (XElement xElement in Constants.XColors.Descendants().Elements("Color")) {
                    var xKey = (string) xElement.Attribute("Key");
                    var xValue = (string) xElement.Element("Value");
                    var xDescription = (string) xElement.Element("Description");
                    if (string.IsNullOrWhiteSpace(xKey) || string.IsNullOrWhiteSpace(xValue)) {
                        continue;
                    }

                    if (Constants.ChatCodes.ContainsKey(xKey)) {
                        if (xDescription.ToLower().Contains("unknown") || string.IsNullOrWhiteSpace(xDescription)) {
                            xDescription = Constants.ChatCodes[xKey];
                        }
                    }

                    Constants.Colors.Add(KeyValuePair.Create(
                        xKey,
                        new[] {
                            xValue,
                            xDescription
                        }));
                }

                foreach (KeyValuePair<string, string> chatCode in Constants.ChatCodes.Where(chatCode => !Constants.ColorsToKeyValue.Any(k => k.Key == chatCode.Key))) {
                    Constants.Colors.Add(KeyValuePair.Create(
                        chatCode.Key,
                        new[] {
                            "FFFFFF",
                            chatCode.Value
                        }));
                }
            }
        }

        /// <summary>
        /// </summary>
        public static void LoadPlugins() {
            var pluginsDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Plugins");
            App.Plugins.LoadPlugins(pluginsDirectory);
            AppViewModel.Instance.HasPlugins = App.Plugins.Loaded.Count > 0;
        }

        public static void LoadPluginTabs() {
            foreach (PluginInstance pluginInstance in App.Plugins.Loaded) {
                TabItemHelper.LoadPluginTabItem(pluginInstance);
                pluginInstance.Instance.Locale = AppViewModel.Instance.Locale;
            }
        }

        /* TODO: Audio
        /// <summary>
        /// </summary>
        public static void LoadSoundsIntoCache() {
            SoundPlayerHelper.CacheSoundFiles();
        }
        */
        public static void RefreshMemoryWorkers() {
            StopMemoryWorkers();
            StartMemoryWorkers();
        }

        /* TODO: Network
        public static void RefreshNetworkWorker() {
            StopNetworkWorker();
            StartNetworkWorker();
        }
        */

        /// <summary>
        /// </summary>
        public static void ResetProcessID() {
            Constants.ProcessModel = new ProcessModel();
        }

        /// <summary>
        /// </summary>
        public static void SetGlobals() {
            Constants.CharacterName = Settings.Default.CharacterName;
            Constants.ServerName = Settings.Default.ServerName;
            Constants.GameLanguage = Settings.Default.GameLanguage;
        }

        /// <summary>
        /// </summary>
        public static void SetProcessID() {
            StopMemoryWorkers();
            if (SettingsViewModel.Instance.PIDSelect == null) {
                return;
            }

            var ID = SettingsViewModel.Instance.PIDSelect.ProcessID;
            UpdateProcessID(Constants.ProcessModels.FirstOrDefault(pm => pm.ProcessID == ID));
            StartMemoryWorkers();
        }

        /// <summary>
        /// </summary>
        public static void SetupCurrentUICulture() {
            var cultureInfo = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var currentCulture = new CultureInfo(cultureInfo);
            Constants.CultureInfo = Settings.Default.CultureSet
                                        ? Settings.Default.Culture
                                        : currentCulture;
            Settings.Default.CultureSet = true;
        }

        /// <summary>
        /// </summary>
        public static void StartMemoryWorkers() {
            StopMemoryWorkers();
            var id = SettingsViewModel.Instance.PIDSelect == null
                         ? GetProcessID()
                         : Constants.ProcessModel.ProcessID;
            Constants.IsOpen = true;
            if (id < 0) {
                Constants.IsOpen = false;
                return;
            }

            MemoryHandler.Instance.ExceptionEvent += MemoryHandler_ExceptionEvent;
            MemoryHandler.Instance.SignaturesFoundEvent += MemoryHandler_SignaturesFoundEvent;

            MemoryHandler.Instance.SetProcess(Constants.ProcessModel, Settings.Default.GameLanguage, "latest", Settings.Default.UseLocalMemoryJSONDataCache);

            _chatLogWorker = new ChatLogWorker();
            _chatLogWorker.StartScanning();
            _actorWorker = new ActorWorker();
            _actorWorker.StartScanning();
            _playerInfoWorker = new PlayerInfoWorker();
            _playerInfoWorker.StartScanning();
            _targetWorker = new TargetWorker();
            _targetWorker.StartScanning();
            _partyInfoWorker = new PartyInfoWorker();
            _partyInfoWorker.StartScanning();
            _inventoryWorker = new InventoryWorker();
            _inventoryWorker.StartScanning();
            _hotBarRecastWorker = new HotBarRecastWorker();
            _hotBarRecastWorker.StartScanning();
        }

        /* TODO: Network
        public static void StartNetworkWorker() {
            StopNetworkWorker();

            NetworkHandler.Instance.ExceptionEvent += NetworkHandler_ExceptionEvent;
            NetworkHandler.Instance.NewNetworkPacketEvent += NetworkHandler_NewPacketEvent;

            var config = new NetworkConfig();
            config.ApplicationName = AssemblyHelper.Name;
            config.CurrentProcessID = Constants.ProcessModel.ProcessID;
            config.ExecutablePath = Application.ExecutablePath;
            config.UserSelectedInterface = Settings.Default.DefaultNetworkInterface;
            config.UseWinPCap = Settings.Default.NetworkUseWinPCap;

            NetworkHandler.Instance.SetProcess(config);

            NetworkHandler.Instance.StartDecrypting();
        }
        */

        /// <summary>
        /// </summary>
        public static void StopMemoryWorkers() {
            MemoryHandler.Instance.ExceptionEvent -= MemoryHandler_ExceptionEvent;
            MemoryHandler.Instance.SignaturesFoundEvent -= MemoryHandler_SignaturesFoundEvent;

            if (_chatLogWorker != null) {
                _chatLogWorker.StopScanning();
                _chatLogWorker.Dispose();
            }

            if (_actorWorker != null) {
                _actorWorker.StopScanning();
                _actorWorker.Dispose();
            }

            if (_playerInfoWorker != null) {
                _playerInfoWorker.StopScanning();
                _playerInfoWorker.Dispose();
            }

            if (_targetWorker != null) {
                _targetWorker.StopScanning();
                _targetWorker.Dispose();
            }

            if (_partyInfoWorker != null) {
                _partyInfoWorker.StopScanning();
                _partyInfoWorker.Dispose();
            }

            if (_inventoryWorker != null) {
                _inventoryWorker.StopScanning();
                _inventoryWorker.Dispose();
            }

            if (_hotBarRecastWorker != null) {
                _hotBarRecastWorker.StopScanning();
                _hotBarRecastWorker.Dispose();
            }
        }

        /* TODO: Network
        public static void StopNetworkWorker() {
            NetworkHandler.Instance.ExceptionEvent -= NetworkHandler_ExceptionEvent;
            NetworkHandler.Instance.NewNetworkPacketEvent -= NetworkHandler_NewPacketEvent;

            NetworkHandler.Instance.StopDecrypting();
        }
        */
        public static void UpdatePluginConstants() {
            ConstantsHelper.UpdatePluginConstants();
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        private static int GetProcessID() {
            if (Constants.IsOpen && Constants.ProcessModel.ProcessID > 0) {
                try {
                    Process.GetProcessById(Constants.ProcessModel.ProcessID);
                    return Constants.ProcessModel.ProcessID;
                }
                catch (ArgumentException) {
                    Constants.IsOpen = false;
                }
            }

            Constants.ProcessModels = new List<ProcessModel>();
            foreach (Process process in Process.GetProcessesByName("ffxiv")) {
                Constants.ProcessModels.Add(
                    new ProcessModel {
                        Process = process
                    });
            }

            foreach (Process process in Process.GetProcessesByName("ffxiv_dx11")) {
                Constants.ProcessModels.Add(
                    new ProcessModel {
                        Process = process,
                        IsWin64 = true
                    });
            }

            // Linux wine has .exe at the end of the process name
            foreach (Process process in Process.GetProcessesByName("ffxiv.exe")) {
                Constants.ProcessModels.Add(
                    new ProcessModel {
                        Process = process
                    });
            }

            foreach (Process process in Process.GetProcessesByName("ffxiv_dx11.exe")) {
                Constants.ProcessModels.Add(
                    new ProcessModel {
                        Process = process,
                        IsWin64 = true
                    });
            }

            if (Constants.ProcessModels.Any()) {
                Constants.IsOpen = true;
                foreach (ProcessModel processModel in Constants.ProcessModels) {
                    SettingsViewModel.Instance.PIDSelectItems.Add(processModel);
                }

                SettingsViewModel.Instance.PIDSelect = SettingsViewModel.Instance.PIDSelectItems.First();
                UpdateProcessID(Constants.ProcessModels.First());
                return Constants.ProcessModels.First().Process.Id;
            }

            Constants.IsOpen = false;
            return -1;
        }

        private static void MemoryHandler_ExceptionEvent(object sender, ExceptionEvent e) {
            Logging.Log(e.Logger, new LogItem(e.Exception, e.LevelIsError));
        }

        private static void MemoryHandler_SignaturesFoundEvent(object sender, SignaturesFoundEvent e) {
            foreach (KeyValuePair<string, Signature> kvp in e.Signatures) {
                Logging.Log(e.Logger, new LogItem($"Signature [{kvp.Key}] Found At Address: [{((IntPtr) kvp.Value).ToString("X")}]"));
                // TODO: Remove when we do not want to output to console...
                Console.WriteLine($"Signature [{kvp.Key}] Found At Address: [{((IntPtr) kvp.Value).ToString("X")}]");
            }
        }

        /* TODO: Network
        private static void NetworkHandler_ExceptionEvent(object sender, Machina.Events.ExceptionEvent e) {
            Logging.Log(e.Logger, new LogItem(e.Exception, e.LevelIsError));
        }

        private static void NetworkHandler_NewPacketEvent(object sender, NewNetworkPacketEvent e) {
            NetworkPacket packet = e.NetworkPacket;

            AppContextHelper.Instance.RaiseNetworkPacketReceived(
                new Common.Core.Network.NetworkPacket {
                    Buffer = packet.Buffer,
                    CurrentPosition = packet.CurrentPosition,
                    Key = packet.Key,
                    MessageSize = packet.MessageSize,
                    PacketDate = packet.PacketDate
                });
        }
        */

        /// <summary>
        /// </summary>
        /// <param name="pluginIndex"></param>
        private static void SetHomePlugin(int pluginIndex) {
            AppViewModel.Instance.TabSelected = ShellView.View.ShellViewTC.Items.Cast<TabItem>().Skip(1).First();
            ShellView.View.PluginsTC.SelectedIndex = pluginIndex;
        }

        /// <summary>
        /// </summary>
        /// <param name="processModel"> </param>
        private static void UpdateProcessID(ProcessModel processModel) {
            Constants.ProcessModel = processModel;
        }
    }
}