// FFXIVAPP.Client
// Line.cs
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
using System.Text.RegularExpressions;
using FFXIVAPP.Client.Enums.Parse;
using FFXIVAPP.Client.Helpers.Parse;
using FFXIVAPP.Client.Models.Parse.Events;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Helpers;

namespace FFXIVAPP.Client.Models.Parse
{
    public class Line
    {
        // battle data
        public Line(ChatLogEntry chatLogEntry = null)
        {
            if (chatLogEntry != null)
            {
                ChatLogEntry = chatLogEntry;
            }
        }

        #region Event Information

        public EventDirection EventDirection { get; set; }
        public EventSubject EventSubject { get; set; }
        public EventType EventType { get; set; }

        #endregion

        #region Type Information

        public TimelineType SourceTimelineType { get; set; }
        public TimelineType TargetTimelineType { get; set; }

        #endregion

        public decimal Amount { get; set; }
        public decimal Modifier { get; set; }
        public string RecLossType { get; set; }
        public bool Hit { get; set; }
        public bool Miss { get; set; }
        public bool Crit { get; set; }
        public bool Counter { get; set; }
        public bool Block { get; set; }
        public bool Parry { get; set; }
        public bool Resist { get; set; }
        public bool Evade { get; set; }
        public ChatLogEntry ChatLogEntry { get; set; }

        #region Property Backings

        private string _action;
        private string _direction;
        private string _part;
        private string _source;
        private string _target;

        public string Source
        {
            get { return _source ?? ""; }
            set
            {
                var name = StringHelper.TitleCase(value);
                _source = ParseHelper.GetTaggedName(name, new Expressions(new Event()), SourceTimelineType);
            }
        }

        public string Target
        {
            get { return _target ?? ""; }
            set
            {
                var name = StringHelper.TitleCase(value);
                _target = ParseHelper.GetTaggedName(name, new Expressions(new Event()), TargetTimelineType);
            }
        }

        public string Action
        {
            get { return _action ?? ""; }
            set { _action = StringHelper.TitleCase(value); }
        }

        public string Direction
        {
            get { return _direction ?? ""; }
            set { _direction = StringHelper.TitleCase(value); }
        }

        public string Part
        {
            get { return _part ?? ""; }
            set { _part = StringHelper.TitleCase(value); }
        }

        #endregion

        public bool IsEmpty()
        {
            return String.IsNullOrWhiteSpace(Source) || String.IsNullOrWhiteSpace(Target) || String.IsNullOrWhiteSpace(Action);
        }

        private bool IsYou(string name)
        {
            return Regex.IsMatch(name, @"^(([Dd](ich|ie|u))|You|Vous)$") || String.Equals(name, Settings.Default.CharacterName, Constants.InvariantComparer);
        }
    }
}
