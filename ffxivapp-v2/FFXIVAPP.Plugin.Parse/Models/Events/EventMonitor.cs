// FFXIVAPP.Plugin.Parse
// EventMonitor.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FFXIVAPP.Plugin.Parse.Models.Stats;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models.Events
{
    public class EventMonitor : StatGroup
    {
        #region Property Bindings

        private ushort _filter;
        private DateTime _lastEventReceived;
        private ParseControl _parseControl;

        private DateTime LastEventReceived
        {
            get { return _lastEventReceived; }
            set
            {
                _lastEventReceived = value;
                RaisePropertyChanged();
            }
        }

        protected UInt16 Filter
        {
            private get { return _filter; }
            set
            {
                _filter = value;
                RaisePropertyChanged();
            }
        }

        protected ParseControl ParseControl
        {
            get { return _parseControl; }
            private set
            {
                _parseControl = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="parseControl"> </param>
        protected EventMonitor(string name, ParseControl parseControl) : base(name)
        {
            Initialize(parseControl);
            EventParser.Instance.OnLogEvent += FilterEvent;
            EventParser.Instance.OnUnknownLogEvent += FilterUnknownEvent;
        }

        /// <summary>
        /// </summary>
        /// <param name="instance"> </param>
        private void Initialize(ParseControl instance)
        {
            ParseControl = instance;
            InitStats();
        }

        /// <summary>
        /// </summary>
        protected virtual void InitStats()
        {
            foreach (var stat in Stats)
            {
                stat.OnValueChanged += DoStatChanged;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="source"> </param>
        /// <param name="e"> </param>
        private void FilterEvent(object source, Event e)
        {
            if (!e.MatchesFilter(Filter))
            {
                return;
            }
            LastEventReceived = e.Timestamp;
            HandleEvent(e);
        }

        /// <summary>
        /// </summary>
        /// <param name="source"> </param>
        /// <param name="e"> </param>
        private void FilterUnknownEvent(object source, Event e)
        {
            if (!e.MatchesFilter(Filter))
            {
                return;
            }
            LastEventReceived = e.Timestamp;
            HandleUnknownEvent(e);
        }

        /// <summary>
        /// </summary>
        /// <param name="e"> </param>
        protected virtual void HandleEvent(Event e)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="e"> </param>
        protected virtual void HandleUnknownEvent(Event e)
        {
        }

        public event EventHandler<StatChangedEvent> OnStatChanged = delegate { };

        private void DoStatChanged(object source, StatChangedEvent e)
        {
            OnStatChanged(this, e);
        }

        #region Implementation of INotifyPropertyChanged

        public new event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
