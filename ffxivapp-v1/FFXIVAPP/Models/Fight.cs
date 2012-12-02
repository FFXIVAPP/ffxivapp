// FFXIVAPP
// Fight.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System;
using System.ComponentModel;

namespace FFXIVAPP.Models
{
    public sealed class Fight : INotifyPropertyChanged
    {
        private String _mobName = String.Empty;

        /// <summary>
        /// </summary>
        public String MobName
        {
            get { return _mobName; }
            private set
            {
                _mobName = value;
                DoPropertyChanged("MobName");
            }
        }

        /// <summary>
        /// </summary>
        private Fight()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="mob"> </param>
        public Fight(String mob)
        {
            MobName = mob;
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void DoPropertyChanged(String name)
        {
            var propChanged = PropertyChanged;
            if (propChanged != null)
            {
                propChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion
    }
}