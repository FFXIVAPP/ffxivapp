// FFXIVAPP.Client
// EventMonitor.cs
// 
// Copyright © 2007 - 2014 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FFXIVAPP.Client.Models.Parse.Stats;

namespace FFXIVAPP.Client.Models.Parse.Events
{
    public class EventMonitor : StatGroup
    {
        #region Property Bindings

        private UInt64 _filter;
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

        protected internal UInt64 Filter
        {
            get { return _filter; }
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
            if (!e.MatchesFilter(Filter, e))
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
