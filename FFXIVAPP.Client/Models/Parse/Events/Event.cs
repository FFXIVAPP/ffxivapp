// FFXIVAPP.Client
// Event.cs
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
using FFXIVAPP.Client.Enums.Parse;
using FFXIVAPP.Common.Core.Memory;

namespace FFXIVAPP.Client.Models.Parse.Events
{
    public class Event : EventArgs, INotifyPropertyChanged
    {
        #region Property Bindings

        private ChatLogEntry _chatLogEntry;
        private EventCode _eventCode;
        private DateTime _timestamp;

        public DateTime Timestamp
        {
            get { return _timestamp; }
            private set
            {
                _timestamp = value;
                RaisePropertyChanged();
            }
        }

        private EventCode EventCode
        {
            get { return _eventCode; }
            set
            {
                _eventCode = value;
                RaisePropertyChanged();
            }
        }

        public ChatLogEntry ChatLogEntry
        {
            get { return _chatLogEntry; }
            set
            {
                _chatLogEntry = value;
                RaisePropertyChanged();
            }
        }

        public EventSubject Subject
        {
            get { return EventCode != null ? EventCode.Subject : EventSubject.Unknown; }
        }

        public EventType Type
        {
            get { return EventCode != null ? EventCode.Type : EventType.Unknown; }
        }

        public EventDirection Direction
        {
            get { return EventCode != null ? EventCode.Direction : EventDirection.Unknown; }
        }

        public ulong Code
        {
            get { return (EventCode != null ? EventCode.Code : 0x0); }
        }

        public bool IsUnknown
        {
            get { return (EventCode == null) || (EventCode.Flags == EventParser.UnknownEvent); }
        }

        #endregion

        public Event(EventCode eventCode = null, ChatLogEntry chatLogEntry = null)
        {
            Initialize(DateTime.Now, eventCode, chatLogEntry);
        }

        private void Initialize(DateTime timeStamp, EventCode eventCode, ChatLogEntry chatLogEntry)
        {
            Timestamp = timeStamp;
            EventCode = eventCode;
            ChatLogEntry = chatLogEntry;
        }

        #region Utility Functions

        /// <summary>
        /// </summary>
        /// <param name="filter"> </param>
        /// <returns> </returns>
        public bool MatchesFilter(UInt64 filter, Event e)
        {
            return (((UInt64) Subject & filter) != 0 && ((UInt64) Type & filter) != 0 && ((UInt64) Direction & filter) != 0);
        }

        #endregion

        #region Equality Methods

        /// <summary>
        /// </summary>
        /// <param name="event1"> </param>
        /// <param name="event2"> </param>
        /// <returns> </returns>
        public static bool operator ==(Event event1, Event event2)
        {
            return event2 != null && (event1 != null && ((event1.Timestamp == event2.Timestamp) && new EventCodeComparer().Equals(event1.EventCode, event2.EventCode)));
        }

        /// <summary>
        /// </summary>
        /// <param name="event1"> </param>
        /// <param name="event2"> </param>
        /// <returns> </returns>
        public static bool operator !=(Event event1, Event event2)
        {
            return !(event1 == event2);
        }

        /// <summary>
        /// </summary>
        /// <param name="source"> </param>
        /// <returns> </returns>
        public override bool Equals(object source)
        {
            return source is Event ? this == (Event) source : base.Equals(source);
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public override int GetHashCode()
        {
            return (Timestamp.GetHashCode() ^ Subject.GetHashCode() ^ Type.GetHashCode() ^ Direction.GetHashCode());
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
