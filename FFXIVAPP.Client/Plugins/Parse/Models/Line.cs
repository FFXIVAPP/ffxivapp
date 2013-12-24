// FFXIVAPP.Client
// Line.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Text.RegularExpressions;
using FFXIVAPP.Client.Plugins.Parse.Enums;
using FFXIVAPP.Client.Plugins.Parse.Helpers;
using FFXIVAPP.Client.Plugins.Parse.Models.Events;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Helpers;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Plugins.Parse.Models
{
    [DoNotObfuscate]
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
