// FFXIVAPP.Client
// AboutViewModel.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.ViewModelBase;

#endregion

namespace FFXIVAPP.Client.ViewModels
{
    [Export(typeof (AboutViewModel))]
    internal sealed class AboutViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static AboutViewModel _instance;

        public static AboutViewModel Instance
        {
            get { return _instance ?? (_instance = new AboutViewModel()); }
        }

        #endregion

        #region Declarations

        public ICommand ManualUpdateCommand { get; private set; }

        #endregion

        public AboutViewModel()
        {
            ManualUpdateCommand = new DelegateCommand(ManualUpdate);
        }

        #region Loading Functions

        #endregion

        #region Utility Functions

        #endregion

        #region Command Bindings

        /// <summary>
        /// </summary>
        private static void ManualUpdate()
        {
            DispatcherHelper.Invoke(() => ShellView.CloseApplication(true));
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
