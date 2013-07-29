// FFXIVAPP.Plugin.Event
// AboutViewModel.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.ComponentModel;
using System.Runtime.CompilerServices;

#endregion

namespace FFXIVAPP.Plugin.Event.ViewModels
{
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
