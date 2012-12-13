// FFXIVAPP.Plugin.Parse
// Fight.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System.ComponentModel;
using System.Runtime.CompilerServices;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models.Fights
{
    public sealed class Fight : INotifyPropertyChanged
    {
        #region Property Bindings

        private string _mobName;

        public string MobName
        {
            get { return _mobName; }
            private set
            {
                _mobName = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public Fight(string mobName = "")
        {
            MobName = mobName;
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
