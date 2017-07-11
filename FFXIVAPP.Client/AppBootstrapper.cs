// FFXIVAPP.Client ~ AppBootstrapper.cs
// 
// Copyright © 2007 - 2017 Ryan Wilson - All Rights Reserved
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Timers;
using FFXIVAPP.Client.Models;
using FFXIVAPP.Client.Views;
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.Utilities;
using NLog;
using Sharlayan.Models;

namespace FFXIVAPP.Client
{
    internal class AppBootstrapper : INotifyPropertyChanged
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        /*main entry to app
         * used for:
         *  initializing settings
         *  configuring collections
         *  setting up dependencies
         */

        public Timer ProcessDetachCheckTimer = new Timer(30000);

        private AppBootstrapper()
        {
            if (App.MArgs != null)
            {
                foreach (var argument in App.MArgs)
                {
                    Logging.Log(Logger, $"ArgumentProvided : {argument}");
                }
            }
            Constants.IsOpen = false;
            Constants.ProcessModel = new ProcessModel();
            //initialize application data
            AppViewModel.Instance.ConfigurationsPath = Common.Constants.ConfigurationsPath;
            AppViewModel.Instance.LogsPath = Common.Constants.LogsPath;
            AppViewModel.Instance.SavedLogsDirectoryList = new List<string>
            {
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
                "Yell"
            };
            AppViewModel.Instance.ScreenShotsPath = Common.Constants.ScreenShotsPath;
            AppViewModel.Instance.SoundsPath = Common.Constants.SoundsPath;
            AppViewModel.Instance.SettingsPath = Common.Constants.SettingsPath;
            AppViewModel.Instance.PluginsSettingsPath = Common.Constants.PluginsSettingsPath;

            #region Culture BootStrapping

            AppViewModel.Instance.UILanguages.Add(new UILanguage
            {
                Language = "English",
                ImageURI = "/FFXIVAPP.Client;component/Resources/Media/Icons/en.png",
                Title = "English",
                CultureInfo = new CultureInfo("en")
            });
            AppViewModel.Instance.UILanguages.Add(new UILanguage
            {
                Language = "Japanese",
                ImageURI = "/FFXIVAPP.Client;component/Resources/Media/Icons/ja.png",
                Title = "日本語",
                CultureInfo = new CultureInfo("ja")
            });
            AppViewModel.Instance.UILanguages.Add(new UILanguage
            {
                Language = "French",
                ImageURI = "/FFXIVAPP.Client;component/Resources/Media/Icons/fr.png",
                Title = "Français",
                CultureInfo = new CultureInfo("fr")
            });
            AppViewModel.Instance.UILanguages.Add(new UILanguage
            {
                Language = "German",
                ImageURI = "/FFXIVAPP.Client;component/Resources/Media/Icons/de.png",
                Title = "Deutsch",
                CultureInfo = new CultureInfo("de")
            });
            AppViewModel.Instance.UILanguages.Add(new UILanguage
            {
                Language = "Chinese",
                ImageURI = "/FFXIVAPP.Client;component/Resources/Media/Icons/cn.png",
                Title = "中國",
                CultureInfo = new CultureInfo("zh")
            });
            AppViewModel.Instance.UILanguages.Add(new UILanguage
            {
                Language = "Korean",
                ImageURI = "/FFXIVAPP.Client;component/Resources/Media/Icons/ko.png",
                Title = "한국어",
                CultureInfo = new CultureInfo("ko")
            });
            AppViewModel.Instance.UILanguages.Add(new UILanguage
            {
                Language = "Russian",
                ImageURI = "/FFXIVAPP.Client;component/Resources/Media/Icons/ru.png",
                Title = "Русский",
                CultureInfo = new CultureInfo("ru")
            });

            #endregion

            #region Initial BootStrapping

            Initializer.SetupCurrentUICulture();
            Initializer.LoadChatCodes();
            Initializer.LoadAutoTranslate();
            Initializer.LoadActions();
            Initializer.LoadColors();
            Initializer.LoadApplicationSettings();
            Initializer.LoadAvailableAudioDevices();
            Initializer.LoadAvailableNetworkDevices();
            Initializer.LoadSoundsIntoCache();
            Initializer.LoadPlugins();

            #endregion

            ProcessDetachCheckTimer.Elapsed += ProcessDetachCheckTimerOnElapsed;
        }

        private void ProcessDetachCheckTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            try
            {
                switch (Constants.IsOpen)
                {
                    case true:
                        try
                        {
                            Process.GetProcessById(Constants.ProcessModel.ProcessID);
                        }
                        catch (ArgumentException)
                        {
                            Constants.IsOpen = false;
                        }
                        break;
                    case false:
                        SettingsView.View.PIDSelect.Items.Clear();
                        Initializer.StopMemoryWorkers();
                        Initializer.ResetProcessID();
                        Initializer.StartMemoryWorkers();
                        break;
                }
            }
            catch (Exception ex)
            {
                Logging.Log(Logger, new LogItem(ex, true));
            }
        }

        #region Property Bindings

        private static AppBootstrapper _instance;

        public static AppBootstrapper Instance
        {
            get { return _instance ?? (_instance = new AppBootstrapper()); }
        }

        #endregion

        #region Declarations

        #endregion

        #region Loading Functions

        #endregion

        #region Utility Functions

        #endregion

        #region Command Bindings

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
