// FFXIVAPP.Plugin.Parse
// ParseMonsterDamageTakenViewModel.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.ComponentModel;
using System.Runtime.CompilerServices;

#endregion

namespace FFXIVAPP.Plugin.Parse.ViewModels
{
    internal sealed class ParseMonsterDamageTakenViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static ParseMonsterDamageTakenViewModel _instance;

        public static ParseMonsterDamageTakenViewModel Instance
        {
            get { return _instance ?? (_instance = new ParseMonsterDamageTakenViewModel()); }
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
