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
using System.Globalization;
using System.Runtime.CompilerServices;
using FFXIVAPP.Client.Models;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.ResourceFiles;
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
                ImageURI = Theme.GetImagePackURI("en"),
                Title = "English",
                CultureInfo = new CultureInfo("en")
            });
            AppViewModel.Instance.UILanguages.Add(new UILanguage
            {
                Language = "Japanese",
                ImageURI = Theme.GetImagePackURI("ja"),
                Title = "日本語",
                CultureInfo = new CultureInfo("ja")
            });
            AppViewModel.Instance.UILanguages.Add(new UILanguage
            {
                Language = "French",
                ImageURI = Theme.GetImagePackURI("fr"),
                Title = "Français",
                CultureInfo = new CultureInfo("fr")
            });
            AppViewModel.Instance.UILanguages.Add(new UILanguage
            {
                Language = "German",
                ImageURI = Theme.GetImagePackURI("de"),
                Title = "Deutsch",
                CultureInfo = new CultureInfo("de")
            });
            AppViewModel.Instance.UILanguages.Add(new UILanguage
            {
                Language = "Chinese",
                ImageURI = Theme.GetImagePackURI("cn"),
                Title = "中國",
                CultureInfo = new CultureInfo("zh")
            });
            AppViewModel.Instance.UILanguages.Add(new UILanguage
            {
                Language = "Korean",
                ImageURI = Theme.GetImagePackURI("ko"),
                Title = "한국어",
                CultureInfo = new CultureInfo("ko")
            });
            AppViewModel.Instance.UILanguages.Add(new UILanguage
            {
                Language = "Russian",
                ImageURI = Theme.GetImagePackURI("ru"),
                Title = "Русский",
                CultureInfo = new CultureInfo("ru")
            });

            #endregion

            #region Initial BootStrapping

            Initializer.SetupCurrentUICulture();
            Initializer.LoadChatCodes();
            Initializer.LoadAutoTranslate();
            Initializer.LoadColors();
            Initializer.LoadApplicationSettings();
            Initializer.LoadAvailableAudioDevices();
            Initializer.LoadAvailableNetworkDevices();
            Initializer.LoadSoundsIntoCache();
            Initializer.LoadPlugins();

            #endregion
        }

        #region Property Bindings

        private static Lazy<AppBootstrapper> _instance = new Lazy<AppBootstrapper>(() => new AppBootstrapper());

        public static AppBootstrapper Instance
        {
            get { return _instance.Value; }
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
