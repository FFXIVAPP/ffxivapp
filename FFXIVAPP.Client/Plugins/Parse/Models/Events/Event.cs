// FFXIVAPP.Client
// Event.cs
// 
// © 2013 Ryan Wilson

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FFXIVAPP.Client.Plugins.Parse.Enums;
using FFXIVAPP.Common.Core.Memory;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Plugins.Parse.Models.Events
{
    [DoNotObfuscate]
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
