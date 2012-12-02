// FFXIVAPP
// Event.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System;

namespace FFXIVAPP.Models
{
    public class Event : EventArgs
    {
        public DateTime Timestamp { get; private set; }
        private EventCode EventCode { get; set; }
        public string RawLine { get; set; }

        /// <summary>
        /// </summary>
        private EventSubject Subject
        {
            get { return EventCode != null ? EventCode.Subject : EventSubject.Unknown; }
        }

        /// <summary>
        /// </summary>
        public EventType Type
        {
            get { return EventCode != null ? EventCode.Type : EventType.Unknown; }
        }

        /// <summary>
        /// </summary>
        public EventDirection Direction
        {
            get { return EventCode != null ? EventCode.Direction : EventDirection.Unknown; }
        }

        /// <summary>
        /// </summary>
        public UInt16 Code
        {
            get { return (ushort) (EventCode != null ? EventCode.Code : 0x0000); }
        }

        /// <summary>
        /// </summary>
        public Boolean IsUnknown
        {
            get { return (EventCode == null) || (EventCode.Flags == EventParser.UnknownEvent); }
        }

        /// <summary>
        /// </summary>
        public Event()
        {
            Initialize(DateTime.Now, null, null);
        }

        /// <summary>
        /// </summary>
        /// <param name="ec"> </param>
        public Event(EventCode ec)
        {
            Initialize(DateTime.Now, ec, null);
        }

        /// <summary>
        /// </summary>
        /// <param name="ec"> </param>
        /// <param name="rawLine"> </param>
        public Event(EventCode ec, string rawLine)
        {
            Initialize(DateTime.Now, ec, rawLine);
        }

        /// <summary>
        /// </summary>
        /// <param name="ts"> </param>
        /// <param name="ec"> </param>
        /// <param name="rawLine"> </param>
        private void Initialize(DateTime ts, EventCode ec, string rawLine)
        {
            Timestamp = ts;
            EventCode = ec;
            RawLine = rawLine;
        }

        /// <summary>
        /// </summary>
        /// <param name="filter"> </param>
        /// <returns> </returns>
        public Boolean MatchesFilter(UInt16 filter)
        {
            return (((UInt16) Subject & filter) != 0 && ((UInt16) Type & filter) != 0 && ((UInt16) Direction & filter) != 0);
        }

        #region Equality Methods

        /// <summary>
        /// </summary>
        /// <param name="e1"> </param>
        /// <param name="e2"> </param>
        /// <returns> </returns>
        public static bool operator ==(Event e1, Event e2)
        {
            return e2 != null && (e1 != null && ((e1.Timestamp == e2.Timestamp) && new EventCodeComparer().Equals(e1.EventCode, e2.EventCode)));
        }

        /// <summary>
        /// </summary>
        /// <param name="e1"> </param>
        /// <param name="e2"> </param>
        /// <returns> </returns>
        public static bool operator !=(Event e1, Event e2)
        {
            return !(e1 == e2);
        }

        /// <summary>
        /// </summary>
        /// <param name="obj"> </param>
        /// <returns> </returns>
        public override bool Equals(object obj)
        {
            return obj is Event ? this == (Event) obj : base.Equals(obj);
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public override int GetHashCode()
        {
            return (Timestamp.GetHashCode() ^ Subject.GetHashCode() ^ Type.GetHashCode() ^ Direction.GetHashCode());
        }

        #endregion
    }
}