// FFXIVAPP.Client
// EventParser.cs
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
using System.Collections.Generic;
using System.Xml.Linq;
using FFXIVAPP.Client.Enums.Parse;
using FFXIVAPP.Common.Core.Memory;

namespace FFXIVAPP.Client.Models.Parse.Events
{
    public class EventParser
    {
        #region Property Bindings

        private SortedDictionary<UInt64, EventCode> EventCodes
        {
            get { return _eventCodes; }
        }

        #endregion

        #region Events

        public event EventHandler<Event> OnLogEvent = delegate { };
        public event EventHandler<Event> OnUnknownLogEvent = delegate { };

        #endregion

        #region Declarations

        #region Filtered Events

        public const UInt64 Alliance = ((UInt64) EventDirection.Alliance | (UInt64) EventSubject.Alliance);
        public const UInt64 Engaged = ((UInt64) EventDirection.Engaged | (UInt64) EventSubject.Engaged);
        public const UInt64 FriendlyNPC = ((UInt64) EventDirection.FriendlyNPC | (UInt64) EventSubject.FriendlyNPC);
        public const UInt64 NPC = ((UInt64) EventDirection.NPC | (UInt64) EventSubject.NPC);
        public const UInt64 Other = ((UInt64) EventDirection.Other | (UInt64) EventSubject.Other);
        public const UInt64 Party = ((UInt64) EventDirection.Party | (UInt64) EventSubject.Party);
        public const UInt64 Pet = ((UInt64) EventDirection.Pet | (UInt64) EventSubject.Pet);
        public const UInt64 PetAlliance = ((UInt64) EventDirection.PetAlliance | (UInt64) EventSubject.PetAlliance);
        public const UInt64 PetOther = ((UInt64) EventDirection.PetOther | (UInt64) EventSubject.PetOther);
        public const UInt64 PetParty = ((UInt64) EventDirection.PetParty | (UInt64) EventSubject.PetParty);
        public const UInt64 Self = ((UInt64) EventDirection.Self);
        public const UInt64 UnEngaged = ((UInt64) EventDirection.UnEngaged | (UInt64) EventSubject.UnEngaged);
        public const UInt64 Unknown = ((UInt64) EventDirection.Unknown | (UInt64) EventSubject.Unknown);
        public const UInt64 You = ((UInt64) EventDirection.You | (UInt64) EventSubject.You);

        #endregion

        public const UInt64 DirectionMask = 0x1FFF;
        public const UInt64 SubjectMask = 0x1FFE000;
        public const UInt64 TypeMask = 0x3FFFE000000;
        public const UInt64 AllEvents = 0xFFFFFFFFFFF;
        public const UInt64 UnknownEvent = 0x0;
        private static EventParser _instance;
        private readonly SortedDictionary<UInt64, EventCode> _eventCodes = new SortedDictionary<UInt64, EventCode>();

        #endregion

        #region Initialization

        /// <summary>
        /// </summary>
        /// <param name="xml"> </param>
        private EventParser(string xml)
        {
            if (String.IsNullOrWhiteSpace(xml))
            {
                return;
            }
            LoadCodes(XElement.Parse(xml));
        }

        /// <summary>
        /// </summary>
        public static EventParser Instance
        {
            get { return _instance ?? (_instance = new EventParser(Constants.ChatCodesXml)); }
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
                    case "Other":
                        thisGroup.Subject = EventSubject.Other;
                        break;
                    case "NPC":
                        thisGroup.Subject = EventSubject.NPC;
                        break;
                    case "Alliance":
                        thisGroup.Subject = EventSubject.Alliance;
                        break;
                    case "FriendlyNPC":
                        thisGroup.Subject = EventSubject.FriendlyNPC;
                        break;
                    case "Pet":
                        thisGroup.Subject = EventSubject.Pet;
                        break;
                    case "PetParty":
                        thisGroup.Subject = EventSubject.PetParty;
                        break;
                    case "PetAlliance":
                        thisGroup.Subject = EventSubject.PetAlliance;
                        break;
                    case "PetOther":
                        thisGroup.Subject = EventSubject.PetOther;
                        break;
                    case "Engaged":
                        thisGroup.Subject = EventSubject.Engaged;
                        break;
                    case "UnEngaged":
                        thisGroup.Subject = EventSubject.UnEngaged;
                        break;
                }
            }
            if (direction != null)
            {
                switch (direction)
                {
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
                    case "Alliance":
                        thisGroup.Direction = EventDirection.Alliance;
                        break;
                    case "FriendlyNPC":
                        thisGroup.Direction = EventDirection.FriendlyNPC;
                        break;
                    case "Pet":
                        thisGroup.Direction = EventDirection.Pet;
                        break;
                    case "PetParty":
                        thisGroup.Direction = EventDirection.PetParty;
                        break;
                    case "PetAlliance":
                        thisGroup.Direction = EventDirection.PetAlliance;
                        break;
                    case "PetOther":
                        thisGroup.Direction = EventDirection.PetOther;
                        break;
                    case "Engaged":
                        thisGroup.Direction = EventDirection.Engaged;
                        break;
                    case "UnEngaged":
                        thisGroup.Direction = EventDirection.UnEngaged;
                        break;
                }
            }
            foreach (var group in root.Elements("Group"))
            {
                LoadGroups(group, thisGroup);
            }
            foreach (var xElement in root.Elements("Code"))
            {
                var xKey = Convert.ToUInt64((string) xElement.Attribute("Key"), 16);
                var xDescription = (string) xElement.Element("Description");
                _eventCodes.Add(xKey, new EventCode(xDescription, xKey, thisGroup));
            }
        }

        #endregion

        #region Parsing

        /// <summary>
        /// </summary>
        /// <param name="chatLogEntry"></param>
        /// <returns></returns>
        private Event Parse(ChatLogEntry chatLogEntry)
        {
            EventCode eventCode;
            var code = Convert.ToUInt64(chatLogEntry.Code, 16);
            if (EventCodes.TryGetValue(code, out eventCode))
            {
                return new Event(eventCode, chatLogEntry);
            }
            var unknownEventCode = new EventCode
            {
                Code = code
            };
            return new Event(unknownEventCode, chatLogEntry);
        }

        /// <summary>
        /// </summary>
        /// <param name="chatLogEntry"></param>
        public void ParseAndPublish(ChatLogEntry chatLogEntry)
        {
            var @event = Parse(chatLogEntry);
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
