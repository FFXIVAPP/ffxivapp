// FFXIVAPP.Client
// EventParser.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Linq;
using FFXIVAPP.Client.Plugins.Parse.Enums;

#endregion

namespace FFXIVAPP.Client.Plugins.Parse.Models.Events {
    public class EventParser {
        #region Property Bindings

        private SortedDictionary<UInt32, EventCode> EventCodes {
            get { return _eventCodes; }
        }

        #endregion

        #region Events

        public event EventHandler<Event> OnLogEvent = delegate { };
        public event EventHandler<Event> OnUnknownLogEvent = delegate { };

        #endregion

        #region Declarations

        public const UInt32 DirectionMask = 0x0000007F;
        public const UInt32 SubjectMask = 0x00001F80;
        public const UInt32 TypeMask = 0x3FFFE000;
        public const UInt32 AllEvents = 0xFFFFFFFF;
        public const UInt32 UnknownEvent = 0x00000000;
        private static EventParser _instance;
        private readonly SortedDictionary<UInt32, EventCode> _eventCodes = new SortedDictionary<UInt32, EventCode>();

        #endregion

        #region Initialization

        /// <summary>
        /// </summary>
        /// <param name="xml"> </param>
        private EventParser(string xml) {
            if (String.IsNullOrWhiteSpace(xml)) {
                return;
            }
            LoadCodes(XElement.Parse(xml));
        }

        /// <summary>
        /// </summary>
        public static EventParser Instance {
            get { return _instance ?? (_instance = new EventParser(Constants.ChatCodesXml)); }
        }

        /// <summary>
        /// </summary>
        /// <param name="root"> </param>
        private void LoadCodes(XContainer root) {
            foreach (var group in root.Elements("Group")) {
                LoadGroups(group, new EventGroup("All"));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="root"> </param>
        /// <param name="parent"> </param>
        private void LoadGroups(XElement root, EventGroup parent) {
            var thisGroup = new EventGroup((string) root.Attribute("Name"), parent);
            var type = (String) root.Attribute("Type");
            var subject = (String) root.Attribute("Subject");
            var direction = (String) root.Attribute("Direction");
            if (type != null) {
                switch (type) {
                    case "Damage":
                        thisGroup.Type = EventType.Damage;
                        break;
                    case "Failed":
                        thisGroup.Type = EventType.Failed;
                        break;
                    case "Actions":
                        thisGroup.Type = EventType.Actions;
                        break;
                    case "Items":
                        thisGroup.Type = EventType.Items;
                        break;
                    case "Cure":
                        thisGroup.Type = EventType.Cure;
                        break;
                    case "Beneficial":
                        thisGroup.Type = EventType.Beneficial;
                        break;
                    case "Detrimental":
                        thisGroup.Type = EventType.Detrimental;
                        break;
                    case "System":
                        thisGroup.Type = EventType.System;
                        break;
                    case "Battle":
                        thisGroup.Type = EventType.Battle;
                        break;
                    case "Synthesis":
                        thisGroup.Type = EventType.Synthesis;
                        break;
                    case "Gathering":
                        thisGroup.Type = EventType.Gathering;
                        break;
                    case "Error":
                        thisGroup.Type = EventType.Error;
                        break;
                    case "Echo":
                        thisGroup.Type = EventType.Echo;
                        break;
                    case "Dialogue":
                        thisGroup.Type = EventType.Dialogue;
                        break;
                    case "Loot":
                        thisGroup.Type = EventType.Loot;
                        break;
                    case "Progression":
                        thisGroup.Type = EventType.Progression;
                        break;
                    case "Defeats":
                        thisGroup.Type = EventType.Defeats;
                        break;
                }
            }
            if (subject != null) {
                switch (subject) {
                    case "You":
                        thisGroup.Subject = EventSubject.You;
                        break;
                    case "Party":
                        thisGroup.Subject = EventSubject.Party;
                        break;
                    case "Other":
                        thisGroup.Subject = EventSubject.Other;
                        break;
                    case "NPC":
                        thisGroup.Subject = EventSubject.NPC;
                        break;
                    case "Engaged":
                        thisGroup.Subject = EventSubject.Engaged;
                        break;
                    case "UnEngaged":
                        thisGroup.Subject = EventSubject.UnEngaged;
                        break;
                }
            }
            if (direction != null) {
                switch (direction) {
                    case "Self":
                        thisGroup.Direction = EventDirection.Self;
                        break;
                    case "You":
                        thisGroup.Direction = EventDirection.You;
                        break;
                    case "Party":
                        thisGroup.Direction = EventDirection.Party;
                        break;
                    case "Other":
                        thisGroup.Direction = EventDirection.Other;
                        break;
                    case "NPC":
                        thisGroup.Direction = EventDirection.NPC;
                        break;
                    case "Engaged":
                        thisGroup.Direction = EventDirection.Engaged;
                        break;
                    case "UnEngaged":
                        thisGroup.Direction = EventDirection.UnEngaged;
                        break;
                }
            }
            foreach (var group in root.Elements("Group")) {
                LoadGroups(group, thisGroup);
            }
            foreach (var xElement in root.Elements("Code")) {
                var xKey = Convert.ToUInt32((string) xElement.Attribute("Key"), 16);
                var xDescription = (string) xElement.Element("Description");
                _eventCodes.Add(xKey, new EventCode(xDescription, xKey, thisGroup));
            }
        }

        #endregion

        #region Parsing

        /// <summary>
        /// </summary>
        /// <param name="code"> </param>
        /// <param name="line"> </param>
        /// <returns> </returns>
        private Event Parse(UInt32 code, string line) {
            EventCode eventCode;
            if (EventCodes.TryGetValue(code, out eventCode)) {
                return new Event(eventCode, line);
            }
            var unknownEventCode = new EventCode {
                Code = code
            };
            return new Event(unknownEventCode, line);
        }

        /// <summary>
        /// </summary>
        /// <param name="code"> </param>
        /// <param name="line"> </param>
        /// <param name="live"></param>
        public void ParseAndPublish(UInt32 code, string line, bool live = true) {
            var @event = Parse(code, line);
            var eventHandler = @event.IsUnknown ? OnUnknownLogEvent : OnLogEvent;
            if (eventHandler == null) {
                return;
            }
            lock (eventHandler) {
                if (live) {
                    Thread.Sleep(10);
                }
                eventHandler(this, @event);
            }
        }

        #endregion
    }
}
