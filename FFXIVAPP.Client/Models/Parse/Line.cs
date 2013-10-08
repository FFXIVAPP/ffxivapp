// FFXIVAPP.Plugin.Parse
// Line.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using FFXIVAPP.Client.Enums;
using FFXIVAPP.Common.Helpers;

#endregion

namespace FFXIVAPP.Client.Models.Parse
{
    public class Line
    {
        // misc.
        public string ActionType { get; set; }

        // map information
        public int MapID { get; set; }

        // battle data
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
        public string RawLine { get; set; }

        // status effect info
        public StatusEffect StatusEffect { get; set; }
        public string StatusEffectName { get; set; }

        #region Property Backings

        private string _action;
        private string _direction;
        private string _job;
        private string _part;
        private Position _position;
        private string _source;
        private string _target;

        public Position Position
        {
            get { return _position ?? (new Position()); }
            set { _position = value; }
        }

        public string Job
        {
            get { return _job ?? ""; }
            set { _job = StringHelper.TitleCase(value); }
        }

        public string Source
        {
            get { return _source ?? ""; }
            set { _source = StringHelper.TitleCase(value); }
        }

        public string Target
        {
            get { return _target ?? ""; }
            set { _target = StringHelper.TitleCase(value); }
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
    }
}
