// FFXIVAPP.Plugin.Parse
// EventParser.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using FFXIVAPP.Plugin.Parse.Enums;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models.Events
{
    public class EventParser
    {
        #region Property Bindings

        private SortedDictionary<UInt16, EventCode> EventCodes
        {
            get { return _eventCodes; }
        }

        #endregion

        #region Events

        public event EventHandler<Event> OnLogEvent = delegate { };
        public event EventHandler<Event> OnUnknownLogEvent = delegate { };

        #endregion

        #region Declarations

        public const UInt16 DirectionMask = 0x0003;
        public const UInt16 SubjectMask = 0x003C;
        public const UInt16 TypeMask = 0x7FC0;
        public static UInt16 AllEvents = 0xFFFF;
        public static UInt16 UnknownEvent;
        private static EventParser _instance;
        private readonly SortedDictionary<UInt16, EventCode> _eventCodes = new SortedDictionary<UInt16, EventCode>();

        #endregion

        #region Initialization

        /// <summary>
        /// </summary>
        /// <param name="xml"> </param>
        private EventParser(string xml)
        {
            LoadCodes(XElement.Parse(xml));
        }

        /// <summary>
        /// </summary>
        public static EventParser Instance
        {
            get { return _instance ?? (_instance = new EventParser(Common.Constants.ChatCodesXml)); }
        }

        /// <summary>
        /// </summary>
        /// <param name="root"> </param>
        private void LoadCodes(XContainer root)
        {
            foreach (var group in root.Elements("Group"))
            {
                LoadGroups(group, new EventGroup("All"));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="root"> </param>
        /// <param name="parent"> </param>
        private void LoadGroups(XElement root, EventGroup parent)
        {
            var thisGroup = new EventGroup((string) root.Attribute("Name"), parent);
            var type = (String) root.Attribute("Type");
            var subject = (String) root.Attribute("Subject");
            var direction = (String) root.Attribute("Direction");
            if (type != null)
            {
                switch (type)
                {
                    case "Attack":
                        thisGroup.Type = EventType.Attack;
                        break;
                    case "Heal":
                        thisGroup.Type = EventType.Heal;
                        break;
                    case "Buff":
                        thisGroup.Type = EventType.Buff;
                        break;
                    case "Debuff":
                        thisGroup.Type = EventType.Debuff;
                        break;
                    case "SkillPoints":
                        thisGroup.Type = EventType.SkillPoints;
                        break;
                    case "Crafting":
                        thisGroup.Type = EventType.Crafting;
                        break;
                    case "Gathering":
                        thisGroup.Type = EventType.Gathering;
                        break;
                    case "Chat":
                        thisGroup.Type = EventType.Chat;
                        break;
                    case "Notice":
                        thisGroup.Type = EventType.Notice;
                        break;
                }
            }
            if (subject != null)
            {
                switch (subject)
                {
                    case "You":
                        thisGroup.Subject = EventSubject.You;
                        break;
                    case "Party":
                        thisGroup.Subject = EventSubject.Party;
                        break;
                    case "Enemy":
                        thisGroup.Subject = EventSubject.Enemy;
                        break;
                    case "Other":
                        thisGroup.Subject = EventSubject.Other;
                        break;
                }
            }
            if (direction != null)
            {
                switch (direction)
                {
                    case "By":
                        thisGroup.Direction = EventDirection.By;
                        break;
                    case "On":
                        thisGroup.Direction = EventDirection.On;
                        break;
                }
            }
            foreach (var group in root.Elements("Group"))
            {
                LoadGroups(group, thisGroup);
            }
            foreach (var xElement in root.Elements("Code"))
            {
                var xKey = Convert.ToUInt16((string) xElement.Attribute("Key"), 16);
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
        private Event Parse(UInt16 code, string line)
        {
            EventCode eventCode;
            if (EventCodes.TryGetValue(code, out eventCode))
            {
                return new Event(eventCode, line);
            }
            var unknownEventCode = new EventCode
            {
                Code = code
            };
            return new Event(unknownEventCode, line);
        }

        /// <summary>
        /// </summary>
        /// <param name="code"> </param>
        /// <param name="line"> </param>
        public void ParseAndPublish(UInt16 code, string line)
        {
            var @event = Parse(code, line);
            var eventHandler = @event.IsUnknown ? OnUnknownLogEvent : OnLogEvent;
            if (eventHandler == null)
            {
                return;
            }
            lock (eventHandler)
            {
                eventHandler(this, @event);
            }
        }

        #endregion
    }
}
