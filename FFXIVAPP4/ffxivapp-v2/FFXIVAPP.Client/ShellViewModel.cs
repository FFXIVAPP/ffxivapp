// FFXIVAPP.Client
// ShellViewModel.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Windows;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Common.ViewModelBase;
using MahApps.Metro.Controls;

#endregion

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

        public ICommand ShowCreditsCommand { get; private set; }
        public ICommand SaveAndClearHistoryCommand { get; private set; }
        public ICommand ScreenShotCommand { get; private set; }
        public ICommand UpdateSelectedPluginCommand { get; private set; }
        public ICommand UpdateTitleCommand { get; private set; }

        #endregion

        public ShellViewModel()
        {
            ShowCreditsCommand = new DelegateCommand(ShowCredits);
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
        private static void ShowCredits()
        {
            ShellView.View.ShellViewTC.SelectedIndex = 3;
        }

        /// <summary>
        /// </summary>
        private static void SaveAndClearHistory()
        {
            XmlLogHelper.SaveCurrentLog();
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
            AppViewModel.Instance.Selected = ((TabItem) ShellView.View.PluginsTC.SelectedItem).Header.ToString();
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
