// FFXIVAPP.Client
// MainViewModel.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FFXIVAPP.Client.Views;
using FFXIVAPP.Common.ViewModelBase;

#endregion

namespace FFXIVAPP.Client.ViewModels
{
    [Export(typeof (MainViewModel))]
    internal sealed class MainViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static MainViewModel _instance;

        public static MainViewModel Instance
        {
            get { return _instance ?? (_instance = new MainViewModel()); }
        }

        #endregion

        #region Declarations

        #endregion

        public MainViewModel()
        {
            
        }

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
