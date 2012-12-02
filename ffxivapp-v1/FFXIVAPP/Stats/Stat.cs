// FFXIVAPP
// Stat.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System;
using System.ComponentModel;

namespace FFXIVAPP.Stats
{
    public abstract class Stat<T> : INotifyPropertyChanged
    {
        private T _value;
        private String _name;
        public event EventHandler<StatChangedEvent> OnValueChanged;

        /// <summary>
        /// </summary>
        public T Value
        {
            get { return _value; }
            set
            {
                var oldVal = Value;
                _value = value;
                var converter = TypeDescriptor.GetConverter(typeof (T));
                if ((converter.ConvertToString(oldVal) != converter.ConvertToString(_value)))
                {
                    DoValueChanged(this, oldVal, Value);
                }
            }
        }

        /// <summary>
        /// </summary>
        public String Name
        {
            get { return _name; }
            set
            {
                _name = value;
                DoPropertyChanged("Name");
            }
        }

        /// <summary>
        /// </summary>
        protected Stat()
        {
            Name = "";
            _value = default(T);
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        protected Stat(String name)
        {
            Name = name;
            _value = default(T);
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="value"> </param>
        protected Stat(String name, T value)
        {
            Name = name;
            _value = value;
        }

        /// <summary>
        /// </summary>
        /// <param name="src"> </param>
        /// <param name="oldValue"> </param>
        /// <param name="newValue"> </param>
        private void DoValueChanged(Stat<T> src, T oldValue, T newValue)
        {
            var changedEvent = OnValueChanged;
            if (changedEvent != null)
            {
                changedEvent(this, new StatChangedEvent(this, oldValue, newValue));
            }
            var propChangedEvent = PropertyChanged;
            if (propChangedEvent != null)
            {
                DoPropertyChanged("Value");
            }
        }

        /// <summary>
        /// </summary>
        public void Reset()
        {
            _value = default(T);
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void DoPropertyChanged(string whichProp)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(whichProp));
            }
        }

        #endregion
    }
}