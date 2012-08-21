// FFXIVAPP
// EventParser.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Xml.Linq;
using NLog;

namespace FFXIVAPP.Models
{
    public class EventParser
    {
        public const UInt16 DirectionMask = 0x0003;
        public const UInt16 SubjectMask = 0x003C;
        public const UInt16 TypeMask = 0x7FC0;
        public static UInt16 AllEvents = 0xFFFF;
        public static UInt16 UnknownEvent;
        private static EventParser _instance;
        private readonly SortedDictionary<UInt16, EventCode> _eventCodes = new SortedDictionary<UInt16, EventCode>();
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// </summary>
        private SortedDictionary<UInt16, EventCode> EventCodes
        {
            get { return _eventCodes; }
        }

        #region Events

        public event EventHandler<Event> OnLogEvent;
        public event EventHandler<Event> OnUnknownLogEvent;

        #endregion

        #region Initialization

        /// <summary>
        /// </summary>
        private EventParser()
        {
            try
            {
                var resourceUri = new Uri("pack://application:,,,/FFXIVAPP;component/Resources/ChatCodes.xml");
                var resource = Application.GetResourceStream(resourceUri);
                if (resource != null)
                {
                    LoadCodes(XElement.Load(resource.Stream));
                }
            }
            catch (Exception ex)
            {
                Logger.Error("{0} :\n{1}", ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="xml"> </param>
        public EventParser(String xml)
        {
            LoadCodes(XElement.Parse(xml));
        }

        /// <summary>
        /// </summary>
        public static EventParser Instance
        {
            get { return _instance ?? (_instance = new EventParser()); }
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
            var codes = from e in root.Elements("ChatCode") select new EventCode {Description = (string) e.Attribute("Desc"), Code = Convert.ToUInt16((string) e.Attribute("ID"), 16), Group = thisGroup};
            foreach (var c in codes)
            {
                _eventCodes.Add(c.Code, c);
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
            EventCode ec;
            if (EventCodes.TryGetValue(code, out ec))
            {
                return new Event(ec, line);
            }
            var unknownEventCode = new EventCode {Code = code};
            return new Event(unknownEventCode, line);
        }

        /// <summary>
        /// </summary>
        /// <param name="code"> </param>
        /// <param name="line"> </param>
        public void ParseAndPublish(UInt16 code, string line)
        {
            lock (this)
            {
                Thread.Sleep(App.MArgs == null ? 6 : 3);
                var e = Parse(code, line);
                var tmp = e.IsUnknown ? OnUnknownLogEvent : OnLogEvent;
                if (tmp != null)
                {
                    tmp(this, e);
                }
            }
        }

        #endregion
    }
}