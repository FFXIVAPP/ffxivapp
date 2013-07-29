// FFXIVAPP.Plugin.Parse
// Stat.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models.Stats
{
    public abstract class Stat<T> : INotifyPropertyChanged
    {
        private string _name;
        private T _value;

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="value"> </param>
        protected Stat(string name = "", T value = default(T))
        {
            Name = name;
            Value = value;
        }

        public string Name
        {
            get { return _name; }
            private set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        public T Value
        {
            get { return _value; }
            set
            {
                var previousValue = Value;
                _value = value;
                OnValueChanged(this, new StatChangedEvent(this, previousValue, Value));
                RaisePropertyChanged();
            }
        }

        #region Events

        public event EventHandler<StatChangedEvent> OnValueChanged = delegate { };

        #endregion

        /// <summary>
        /// </summary>
        public void Reset()
        {
            Value = default(T);
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
