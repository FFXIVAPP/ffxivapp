// FFXIVAPP.Client
// App.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.Utilities;
using NAudio.Wave;
using NLog;
using NLog.Config;

namespace FFXIVAPP.Client
{
    public partial class App
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static List<DirectSoundDeviceInfo> _availableAudioDevices;
        private static IEnumerable<NetworkInterface> _availableNetworkInterfaces;

        #endregion

        #region Property Bindings

        internal static PluginHost Plugins
        {
            get { return PluginHost.Instance; }
        }

        internal static IEnumerable<DirectSoundDeviceInfo> AvailableAudioDevices
        {
            get { return _availableAudioDevices ?? (_availableAudioDevices = new List<DirectSoundDeviceInfo>(DirectSoundOut.Devices.Where(d => d.Guid != Guid.Empty))); }
        }

        internal static IEnumerable<NetworkInterface> AvailableNetworkInterfaces
        {
            get { return _availableNetworkInterfaces ?? (_availableNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces()); }
        }

        public static string[] MArgs { get; private set; }

        #endregion

        private App()
        {
            Startup += ApplicationStartup;
            StartupUri = new Uri("ShellView.xaml", UriKind.Relative);
            var resourceLocater = new Uri("/FFXIVAPP.Client;component/App.xaml", UriKind.Relative);
            LoadComponent(this, resourceLocater);
            ConfigureNLog();
            Dispatcher.UnhandledException += OnDispatcherUnhandledException;
            Dispatcher.UnhandledExceptionFilter += OnUnhandledExceptionFilter;
            Settings.Default.PropertyChanged += SettingsPropertyChanged;
            Settings.Default.SettingChanging += SettingsSettingChanging;
            CheckSettings();
        }

        /// <summary>
        ///     Application Entry Point.
        /// </summary>
        [STAThread]
        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        [LoaderOptimization(LoaderOptimization.MultiDomainHost)]
        public static void Main()
        {
            var app = new App();
            app.Run();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private static void ApplicationStartup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length > 0)
            {
                MArgs = e.Args;
            }
        }

        private static void CheckSettings()
        {
            Common.Constants.EnableNLog = Settings.Default.EnableNLog;
            try
            {
                if (!Settings.Default.Application_UpgradeRequired)
                {
                    Settings.Default.Reload();
                    return;
                }
                Settings.Default.Upgrade();
                Settings.Default.Reload();
                Settings.Default.Application_UpgradeRequired = false;
            }
            catch (Exception ex)
            {
                SettingsHelper.Default();
            }
        }

        /// <summary>
        /// </summary>
        private static void ConfigureNLog()
        {
            var hasLocal = false;
            const string fileName = "./FFXIVAPP.Client.exe.nlog";
            if (File.Exists(fileName))
            {
                hasLocal = true;
            }
            var resource = ResourceHelper.StreamResource(Common.Constants.AppPack + "Resources/FFXIVAPP.Client.exe.nlog");
            if (resource == null)
            {
                return;
            }
            StringReader stringReader;
            if (hasLocal)
            {
                stringReader = new StringReader(XElement.Load(fileName)
                                                       .ToString());
            }
            else
            {
                stringReader = new StringReader(XElement.Load(resource.Stream)
                                                       .ToString());
            }
            using (var xmlReader = XmlReader.Create(stringReader))
            {
                LogManager.Configuration = new XmlLoggingConfiguration(xmlReader, null);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            var ex = e.Exception;
            Logging.Log(Logger, new LogItem("", ex, LogLevel.Error));
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnhandledExceptionFilter(object sender, DispatcherUnhandledExceptionFilterEventArgs e)
        {
            e.RequestCatch = true;
            var ex = e.Exception;
            Logging.Log(Logger, new LogItem("", ex, LogLevel.Error));
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private static void SettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Logging.Log(Logger, String.Format("PropertyChanged : {0}", e.PropertyName));
            try
            {
                switch (e.PropertyName)
                {
                    case "CharacterName":
                        Constants.CharacterName = Settings.Default.CharacterName;
                        break;
                    case "GameLanguage":
                        Constants.GameLanguage = Settings.Default.GameLanguage;
                        var lang = Settings.Default.GameLanguage.ToLower();
                        var cultureInfo = new CultureInfo("en");
                        switch (lang)
                        {
                            case "french":
                                cultureInfo = new CultureInfo("fr");
                                break;
                            case "japanese":
                                cultureInfo = new CultureInfo("ja");
                                break;
                            case "german":
                                cultureInfo = new CultureInfo("de");
                                break;
                            case "chinese":
                                cultureInfo = new CultureInfo("zh");
                                break;
                        }
                        Constants.CultureInfo = Settings.Default.Culture = cultureInfo;
                        LocaleHelper.Update(Settings.Default.Culture);
                        break;
                    case "ServerName":
                        Constants.ServerName = Settings.Default.ServerName;
                        break;
                    case "EnableNLog":
                        Common.Constants.EnableNLog = Constants.EnableNLog = Settings.Default.EnableNLog;
                        break;
                    case "EnableNetworkReading":
                        Common.Constants.EnableNetworkReading = Constants.EnableNetworkReading = Settings.Default.EnableNetworkReading;
                        if (Settings.Default.EnableNetworkReading)
                        {
                            Initializer.StartNetworkWorker();
                        }
                        else
                        {
                            Initializer.StopNetworkWorker();
                        }
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
                    case "TopMost":
                        if (ShellView.View != null)
                        {
                            ShellView.View.Topmost = Settings.Default.TopMost;
                        }
                        break;
                    case "DefaultAudioDevice":
                        if (Settings.Default.DefaultAudioDevice == "System Default")
                        {
                            Common.Constants.DefaultAudioDevice = Guid.Empty;
                        }
                        else
                        {
                            foreach (var audioDevice in AvailableAudioDevices.Where(device => device.Guid != Guid.Empty))
                            {
                                if (audioDevice.Description == Settings.Default.DefaultAudioDevice)
                                {
                                    Common.Constants.DefaultAudioDevice = audioDevice.Guid;
                                }
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private static void SettingsSettingChanging(object sender, SettingChangingEventArgs e)
        {
            Logging.Log(Logger, String.Format("SettingChanging : [{0},{1}]", e.SettingKey, e.NewValue));
        }
    }
}
