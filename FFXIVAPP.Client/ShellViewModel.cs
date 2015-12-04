// FFXIVAPP.Client ~ ShellViewModel.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
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
using System.ComponentModel.Composition;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Common.ViewModelBase;

namespace FFXIVAPP.Client
{
    [Export(typeof (ShellViewModel))]
    internal sealed class ShellViewModel : INotifyPropertyChanged
    {
        public ShellViewModel()
        {
            SetLocaleCommand = new DelegateCommand(SetLocale);
            SaveAndClearHistoryCommand = new DelegateCommand(SaveAndClearHistory);
            ScreenShotCommand = new DelegateCommand(ScreenShot);
            UpdateSelectedPluginCommand = new DelegateCommand(UpdateSelectedPlugin);
            UpdateTitleCommand = new DelegateCommand(UpdateTitle);
        }

        #region Property Bindings

        private static ShellViewModel _instance;

        public static ShellViewModel Instance
        {
            get { return _instance ?? (_instance = new ShellViewModel()); }
        }

        #endregion

        #region Declarations

        public ICommand SetLocaleCommand { get; set; }
        public ICommand SaveAndClearHistoryCommand { get; private set; }
        public ICommand ScreenShotCommand { get; private set; }
        public ICommand UpdateSelectedPluginCommand { get; private set; }
        public ICommand UpdateTitleCommand { get; private set; }

        #endregion

        #region Loading Functions

        #endregion

        #region Utility Functions

        #endregion

        #region Command Bindings

        private static List<string> SupportedGameLanguages = new List<string>
        {
            "English",
            "Japanese",
            "French",
            "German",
            "Chinese",
            "Korean"
        };

        private static void SetLocale()
        {
            var uiLanguage = ShellView.View.LanguageSelect.SelectedValue.ToString();
            if (String.IsNullOrWhiteSpace(uiLanguage))
            {
                return;
            }
            if (uiLanguage == Settings.Default.GameLanguage)
            {
                return;
            }
            if (SupportedGameLanguages.Contains(uiLanguage))
            {
                if (uiLanguage == Settings.Default.GameLanguage)
                {
                    return;
                }
                Action ok = () => { Settings.Default.GameLanguage = uiLanguage; };
                Action cancel = () => { };
                var title = AppViewModel.Instance.Locale["app_WarningMessage"];
                var message = AppViewModel.Instance.Locale["app_UILanguageChangeWarningGeneral"];
                if (uiLanguage == "Chinese" || Settings.Default.GameLanguage == "Chinese")
                {
                    message = message + AppViewModel.Instance.Locale["app_UILanguageChangeWarningChinese"];
                }
                MessageBoxHelper.ShowMessageAsync(title, message, ok, cancel);
            }
            else
            {
                var title = AppViewModel.Instance.Locale["app_WarningMessage"];
                var message = AppViewModel.Instance.Locale["app_UILanguageChangeWarningNoGameLanguage"];
                MessageBoxHelper.ShowMessageAsync(title, message);
            }
        }

        /// <summary>
        /// </summary>
        private static void SaveAndClearHistory()
        {
            SavedlLogsHelper.SaveCurrentLog();
        }

        /// <summary>
        /// </summary>
        private static void ScreenShot()
        {
            try
            {
                var date = DateTime.Now.ToString("yyyy_MM_dd_HH.mm.ss_");
                var fileName = Path.Combine(AppViewModel.Instance.ScreenShotsPath, String.Format("{0}.jpg", date));
                var screenShot = ScreenCapture.GetJpgImage(ShellView.View, 1, 100);
                var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
                using (var stream = new BinaryWriter(fileStream))
                {
                    stream.Write(screenShot);
                }
            }
            catch (Exception ex)
            {
                var title = AppViewModel.Instance.Locale["app_WarningMessage"];
                MessageBoxHelper.ShowMessageAsync(title, ex.Message);
            }
        }

        /// <summary>
        /// </summary>
        private static void UpdateSelectedPlugin()
        {
            var selectedItem = ((TabItem) ShellView.View.PluginsTC.SelectedItem);
            try
            {
                AppViewModel.Instance.Selected = selectedItem.Header.ToString();
            }
            catch (Exception)
            {
                AppViewModel.Instance.Selected = "(NONE)";
            }
            UpdateTitle();
        }

        /// <summary>
        /// </summary>
        private static void UpdateTitle()
        {
            var currentMain = ((TabItem) ShellView.View.ShellViewTC.SelectedItem).Name;
            switch (currentMain)
            {
                case "PluginsTI":
                    AppViewModel.Instance.AppTitle = String.Format("{0}", AppViewModel.Instance.Selected);
                    break;
                default:
                    AppViewModel.Instance.AppTitle = currentMain.Substring(0, currentMain.Length - 2);
                    break;
            }
        }

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
