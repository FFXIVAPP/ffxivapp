using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Text;

namespace ParseModXIV.Classes
{
    public enum EventDirection { Unknown = 0x0000, On = 0x0001, By = 0x0002 }
    public enum EventSubject { Unknown = 0x0000, You = 0x0004, Party = 0x0008, Enemy = 0x0010, Other = 0x0020 }
    public enum EventType { Unknown = 0x0000, Attack = 0x0040, Heal = 0x0080, Buff = 0x0100, Debuff = 0x0200, SkillPoints = 0x0400, Crafting = 0x0800, Gathering = 0x1000, Chat = 0x2000, Notice=0x4000 }

    public class EventGroup
    {
        public String Name { get; set; }
        public List<EventGroup> Children = new List<EventGroup>();
        private EventGroup parent = null;
        private UInt16 flags = 0x0000;
        public UInt16 Flags
        {
            get
            {
                if (Parent != null)
                {
                    UInt16 combinedFlags = 0x0000;
                    if ((flags & EventParser.DIRECTION_MASK) != 0)
                    {
                        combinedFlags |= (UInt16)(flags & EventParser.DIRECTION_MASK);
                    }
                    else
                    {
                        combinedFlags |= (UInt16)Parent.Direction;
                    }

                    if ((flags & EventParser.SUBJECT_MASK) != 0)
                    {
                        combinedFlags |= (UInt16)(flags & EventParser.SUBJECT_MASK);
                    }
                    else
                    {
                        combinedFlags |= (UInt16)Parent.Subject;
                    }

                    if ((flags & EventParser.TYPE_MASK) != 0)
                    {
                        combinedFlags |= (UInt16)(flags & EventParser.TYPE_MASK);
                    }
                    else
                    {
                        combinedFlags |= (UInt16)Parent.Type;
                    }
                    return combinedFlags;
                }
                else
                {
                    return flags;
                }
            }
        }
        public EventDirection Direction
        {
            get
            {
                return (EventDirection)(Flags & EventParser.DIRECTION_MASK);
            }
            set
            {
                flags = (UInt16)((flags & ~EventParser.DIRECTION_MASK) | (UInt16)value);
            }
        }
        public EventSubject Subject
        {
            get
            {
                return (EventSubject)(Flags & EventParser.SUBJECT_MASK);
            }
            set
            {
                flags = (UInt16)((flags & ~EventParser.SUBJECT_MASK) | (UInt16)value);
            }
        }
        public EventType Type
        {
            get
            {
                return (EventType)(Flags & EventParser.TYPE_MASK);
            }
            set
            {
                flags = (UInt16)((flags & ~EventParser.TYPE_MASK) | (UInt16)value);
            }
        }

        public List<EventCode> Codes { get; private set; }

        public EventGroup Parent
        {
            get
            {
                return parent;
            }
            set
            {
                if ((parent != null) && (value != null))
                {
                    parent.Children.Remove(this);
                }
                if (value != null)
                {
                    parent = value;
                    value.Children.Add(this);
                }
            }
        }

        public EventGroup()
        {
        }

        public EventGroup(String name)
        {
            init(name, null);
        }

        public EventGroup(String name, EventGroup parent)
        {
            init(name, parent);
        }

        private void init(String name, EventGroup parent)
        {
            Name = name;
            Parent = parent;
        }

        public EventGroup AddChild(EventGroup kid)
        {
            kid.Parent = this;
            return this;
        }

    }

    public class EventCode
    {
        public String Description { get; set; }
        public UInt16 Code { get; set; }
        public UInt16 flags
        {
            get
            {
                if (Group == null)
                {
                    return 0x0000;
                }
                return Group.Flags;
            }
        }
        public EventGroup Group { get; set; }
        public EventDirection Direction
        {
            get
            {
                if (Group == null)
                {
                    return EventDirection.Unknown;
                }
                return Group.Direction;
            }
        }
        public EventSubject Subject
        {
            get
            {
                if (Group == null)
                {
                    return EventSubject.Unknown;
                }
                return Group.Subject;
            }
        }
        public EventType Type
        {
            get
            {
                if (Group == null)
                {
                    return EventType.Unknown;
                }
                return Group.Type;
            }
        }

        public EventCode()
        {
        }

        public EventCode(String description, UInt16 code, EventGroup group)
        {
            Description = description;
            Code = code;
            Group = group;
        }
    }

    public class EventCodeComparer : IEqualityComparer<EventCode>
    {
        public bool Equals(EventCode ec1, EventCode ec2)
        {
            return (ec1.Code == ec2.Code);
        }

        public int GetHashCode(EventCode ec)
        {
            return ec.Code.GetHashCode();
        }
    }

    public class PartyEventArgs : EventArgs
    {
        public enum PartyEventType
        {
            Join,
            Leave,
            Disband
        };

        public PartyEventType EventType
        {
            get; set;
        }

        public String Info
        {
            get; set;
        }

        public PartyEventArgs(PartyEventType t, String info)
        {
            EventType = t;
            Info = info;
        }
    }

    public class EventParser
    {
        public static UInt16 DIRECTION_MASK = 0x0003;
        public static UInt16 SUBJECT_MASK = 0x003C;
        public static UInt16 TYPE_MASK = 0x7FC0;
        public static UInt16 ALL_EVENTS = 0xFFFF;
        public static UInt16 UNKNOWN_EVENT = 0x0000;
        public Boolean InParty { get; set; }

        private static EventParser instance;
        private readonly SortedDictionary<UInt16, EventCode> eventCodes = new SortedDictionary<UInt16, EventCode>();
        public SortedDictionary<UInt16, EventCode> EventCodes
        {
            get
            {
                return eventCodes;
            }
        }

        #region "Events"
        public event EventHandler<Event> OnLogEvent;
        public event EventHandler<Event> OnUnknownLogEvent;
        public event EventHandler<PartyEventArgs> OnPartyChanged;

        public virtual void DoPartyChanged(object src, PartyEventArgs e)
        {
            switch(e.EventType)
            {
                case PartyEventArgs.PartyEventType.Disband:
                    InParty = false;
                    break;
                case PartyEventArgs.PartyEventType.Join:
                    InParty = true;
                    break;
                case PartyEventArgs.PartyEventType.Leave:
                    InParty = e.Info.ToLower() != "you";
                    break;
            }
            var partyChanged = OnPartyChanged;
            if (partyChanged != null) partyChanged(src, e);
        }
        #endregion

        #region "Initialization"
        private EventParser()
        {
            InParty = false;
            loadCodes(XElement.Load("Resources/ChatCodes.xml"));
        }

        public EventParser(String xml)
        {
            loadCodes(XElement.Parse(xml));
        }

        public static EventParser Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EventParser();
                }
                return instance;
            }
        }

        private void loadCodes(XElement root)
        {
            foreach (XElement group in root.Elements("Group"))
            {
                loadGroups(group, new EventGroup("All"));
            }
        }

        private void loadGroups(XElement root, EventGroup parent)
        {
            EventGroup thisGroup = new EventGroup((string)root.Attribute("name"), parent);
            String type = (String)root.Attribute("type");
            String subject = (String)root.Attribute("subject");
            String direction = (String)root.Attribute("direction");
            if (type != null)
            {
                switch (type)
                {
                    case "Attack": thisGroup.Type = EventType.Attack; break;
                    case "Heal": thisGroup.Type = EventType.Heal; break;
                    case "Buff": thisGroup.Type = EventType.Buff; break;
                    case "Debuff": thisGroup.Type = EventType.Debuff; break;
                    case "SkillPoints": thisGroup.Type = EventType.SkillPoints; break;
                    case "Crafting": thisGroup.Type = EventType.Crafting; break;
                    case "Gathering": thisGroup.Type = EventType.Gathering; break;
                    case "Chat": thisGroup.Type = EventType.Chat; break;
                    case "Notice":
                        thisGroup.Type = EventType.Notice;
                        break;
                }
            }
            if (subject != null)
            {
                switch (subject)
                {
                    case "You": thisGroup.Subject = EventSubject.You; break;
                    case "Party": thisGroup.Subject = EventSubject.Party; break;
                    case "Enemy": thisGroup.Subject = EventSubject.Enemy; break;
                    case "Other": thisGroup.Subject = EventSubject.Other; break;
                }
            }
            if (direction != null)
            {
                switch (direction)
                {
                    case "By": thisGroup.Direction = EventDirection.By; break;
                    case "On": thisGroup.Direction = EventDirection.On; break;
                }
            }

            foreach (XElement group in root.Elements("Group"))
            {
                loadGroups(group, thisGroup);
            }

            IEnumerable<EventCode> codes = from e in root.Elements("ChatCode")
                                           select new EventCode { Description = (string)e.Attribute("Desc"), Code = Convert.ToUInt16((string)e.Attribute("id"), 16), Group = thisGroup };
            foreach (EventCode c in codes)
            {
                eventCodes.Add(c.Code, c);
            }
        }

        #endregion

        #region "Parsing"

        public Event Parse(UInt16 code, string line)
        {
            EventCode ec;
            if (EventCodes.TryGetValue(code, out ec))
            {
                return new Event(ec, line);
            }
            else
            {
                EventCode unknownEventCode = new EventCode { Code = code };
                return new Event(unknownEventCode, line);

            }
        }

        public virtual void ParseAndPublish(UInt16 code, string line)
        {
            Event e = Parse(code, line);
            EventHandler<Event> tmp = null;
            if (e.IsUnknown)
            {
                tmp = OnUnknownLogEvent;
            }
            else
            {
                tmp = OnLogEvent;
            }
            if (tmp != null)
            {
                tmp(this, e);
            }
        }
        #endregion
    }
}
