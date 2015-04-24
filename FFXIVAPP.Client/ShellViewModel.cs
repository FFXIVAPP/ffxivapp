// FFXIVAPP.Client
// ShellViewModel.cs
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

        public ShellViewModel()
        {
            SetLocaleCommand = new DelegateCommand<string>(SetLocale);
            SaveAndClearHistoryCommand = new DelegateCommand(SaveAndClearHistory);
            ScreenShotCommand = new DelegateCommand(ScreenShot);
            UpdateSelectedPluginCommand = new DelegateCommand(UpdateSelectedPlugin);
            UpdateTitleCommand = new DelegateCommand(UpdateTitle);
        }

        #region Loading Functions

        #endregion

        #region Utility Functions

        #endregion

        #region Command Bindings

        private static void SetLocale(string language)
        {
            if (language == Settings.Default.GameLanguage)
            {
                return;
            }
            if (language == "Chinese" || Settings.Default.GameLanguage == "Chinese")
            {
                Action ok = () => { Settings.Default.GameLanguage = language; };
                Action cancel = () => { };
                var title = AppViewModel.Instance.Locale["app_WarningMessage"];
                var message = "FFXIVAPP will restart to perform this change. Do you wish to continue?";
                MessageBoxHelper.ShowMessageAsync(title, message, ok, cancel);
            }
            else
            {
                Settings.Default.GameLanguage = language;
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
