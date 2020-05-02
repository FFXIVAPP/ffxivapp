// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppBootstrapper.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2020 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   AppBootstrapper.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    using FFXIVAPP.Client.Models;
    using FFXIVAPP.Common.Utilities;
    using FFXIVAPP.ResourceFiles;

    using NLog;

    using Sharlayan.Models;

    internal class AppBootstrapper : INotifyPropertyChanged {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static Lazy<AppBootstrapper> _instance = new Lazy<AppBootstrapper>(() => new AppBootstrapper());

        /*main entry to app
         * used for:
         *  initializing settings
         *  configuring collections
         *  setting up dependencies
         */
        private AppBootstrapper() {
            if (App.MArgs != null) {
                foreach (var argument in App.MArgs) {
                    Logging.Log(Logger, $"ArgumentProvided : {argument}");
                }
            }

            Constants.IsOpen = false;
            Constants.ProcessModel = new ProcessModel();

            // initialize application data
            AppViewModel.Instance.ConfigurationsPath = Common.Constants.ConfigurationsPath;
            AppViewModel.Instance.LogsPath = Common.Constants.LogsPath;
            AppViewModel.Instance.SavedLogsDirectoryList = new List<string> {
                "Say",
                "Shout",
                "Party",
                "Tell",
                "LS1",
                "LS2",
                "LS3",
                "LS4",
                "LS5",
                "LS6",
                "LS7",
                "LS8",
                "FC",
                "Yell",
            };
            AppViewModel.Instance.ScreenShotsPath = Common.Constants.ScreenShotsPath;
            AppViewModel.Instance.SoundsPath = Common.Constants.SoundsPath;
            AppViewModel.Instance.SettingsPath = Common.Constants.SettingsPath;
            AppViewModel.Instance.PluginsSettingsPath = Common.Constants.PluginsSettingsPath;

            AppViewModel.Instance.UILanguages.Add(
                new UILanguage {
                    Language = "English",
                    ImageURI = Theme.GetImagePackURI("en"),
                    Title = "English",
                    CultureInfo = new CultureInfo("en"),
                });
            AppViewModel.Instance.UILanguages.Add(
                new UILanguage {
                    Language = "Japanese",
                    ImageURI = Theme.GetImagePackURI("ja"),
                    Title = "日本語",
                    CultureInfo = new CultureInfo("ja"),
                });
            AppViewModel.Instance.UILanguages.Add(
                new UILanguage {
                    Language = "French",
                    ImageURI = Theme.GetImagePackURI("fr"),
                    Title = "Français",
                    CultureInfo = new CultureInfo("fr"),
                });
            AppViewModel.Instance.UILanguages.Add(
                new UILanguage {
                    Language = "German",
                    ImageURI = Theme.GetImagePackURI("de"),
                    Title = "Deutsch",
                    CultureInfo = new CultureInfo("de"),
                });
            AppViewModel.Instance.UILanguages.Add(
                new UILanguage {
                    Language = "Chinese",
                    ImageURI = Theme.GetImagePackURI("cn"),
                    Title = "中國",
                    CultureInfo = new CultureInfo("zh"),
                });
            AppViewModel.Instance.UILanguages.Add(
                new UILanguage {
                    Language = "Korean",
                    ImageURI = Theme.GetImagePackURI("ko"),
                    Title = "한국어",
                    CultureInfo = new CultureInfo("ko"),
                });
            AppViewModel.Instance.UILanguages.Add(
                new UILanguage {
                    Language = "Russian",
                    ImageURI = Theme.GetImagePackURI("ru"),
                    Title = "Русский",
                    CultureInfo = new CultureInfo("ru"),
                });

            Initializer.SetupCurrentUICulture();
            Initializer.LoadChatCodes();
            Initializer.LoadAutoTranslate();
            Initializer.LoadColors();
            Initializer.LoadApplicationSettings();
            Initializer.LoadAvailableAudioDevices();
            Initializer.LoadAvailableNetworkDevices();
            Initializer.LoadSoundsIntoCache();
            Initializer.LoadPlugins();
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public static AppBootstrapper Instance {
            get {
                return _instance.Value;
            }
        }

        private void RaisePropertyChanged([CallerMemberName,] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
    }
}