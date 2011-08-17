using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParseModXIV.Classes
{

    public class Event : EventArgs
    {
        public DateTime Timestamp { get; set; }
        public EventCode EventCode { get; set; }
        public EventSubject Subject
        {
            get
            {
                if (EventCode != null) return EventCode.Subject;
                else return EventSubject.Unknown;
            }
        }
        public EventType Type
        {
            get
            {
                if (EventCode != null) return EventCode.Type;
                else return EventType.Unknown;
            }
        }
        public EventDirection Direction
        {
            get
            {
                if (EventCode != null) return EventCode.Direction;
                else return EventDirection.Unknown;
            }
        }
        public UInt16 Code
        {
            get
            {
                if (EventCode != null) return EventCode.Code;
                else return 0x0000;
            }
        }
        public Boolean IsUnknown
        {
            get
            {
                if ((EventCode == null) || (EventCode.flags == EventParser.UNKNOWN_EVENT))
                {
                    return true;
                }
                return false;
            }
        }
        public string RawLine { get; set; }

        public Event()
        {
            initialize(DateTime.Now, null, null);
        }

        public Event(EventCode ec)
        {
            initialize(DateTime.Now, ec, null);
        }

        public Event(EventCode ec, string rawLine)
        {
            initialize(DateTime.Now, ec, rawLine);
        }

        private void initialize(DateTime ts, EventCode ec, string rawLine)
        {
            Timestamp = ts;
            EventCode = ec;
            RawLine = rawLine;
        }

        public Boolean MatchesFilter(UInt16 filter)
        {
            return (((UInt16)Subject & filter) != 0 &&
                    ((UInt16)Type & filter) != 0 &&
                    ((UInt16)Direction & filter) != 0);
        }

        #region "Equality methods"
        public static bool operator ==(Event e1, Event e2)
        {
            return (e1.Timestamp == e2.Timestamp) && new EventCodeComparer().Equals(e1.EventCode, e2.EventCode);
        }

        public static bool operator !=(Event e1, Event e2)
        {
            return !(e1 == e2);
        }

        public override bool Equals(object obj)
        {
            if (obj is Event)
            {
                return (this == (Event)obj);
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public override int GetHashCode()
        {
            return (Timestamp.GetHashCode() ^ Subject.GetHashCode() ^ Type.GetHashCode() ^ Direction.GetHashCode());
        }
        #endregion
    }
}
