// FFXIVAPP.Client
// ShellViewModel.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Plugins.Parse.Models;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Common.ViewModelBase;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client
{
    [DoNotObfuscate]
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

        public ICommand SaveAndClearHistoryCommand { get; private set; }
        public ICommand ScreenShotCommand { get; private set; }
        public ICommand UpdateSelectedPluginCommand { get; private set; }
        public ICommand UpdateTitleCommand { get; private set; }

        #endregion

        public ShellViewModel()
        {
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

        /// <summary>
        /// </summary>
        private static void SaveAndClearHistory()
        {
            SavedlLogsHelper.SaveCurrentLog();
            if (Settings.Default.ParsePluginEnabled)
            {
                ParseControl.Instance.Reset();
            }
        }

        /// <summary>
        /// </summary>
        private static void ScreenShot()
        {
            var date = DateTime.Now.ToString("yyyy_MM_dd_HH.mm.ss_");
            var fileName = String.Format("{0}{1}.jpg", AppViewModel.Instance.ScreenShotsPath, date);
            var screenShot = ScreenCapture.GetJpgImage(ShellView.View, 1, 100);
            var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
            using (var stream = new BinaryWriter(fileStream))
            {
                stream.Write(screenShot);
            }
        }

        /// <summary>
        /// </summary>
        private static void UpdateSelectedPlugin()
        {
            var selectedItem = ((TabItem) ShellView.View.PluginsTC.SelectedItem);
            AppViewModel.Instance.Selected = selectedItem.Header.ToString();
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
