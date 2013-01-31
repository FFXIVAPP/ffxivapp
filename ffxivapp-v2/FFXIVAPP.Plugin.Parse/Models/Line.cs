// FFXIVAPP.Plugin.Parse
// Line.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using FFXIVAPP.Common.Helpers;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models
{
    public class Line
    {
        private string _action;
        private string _direction;
        private string _job;
        private string _part;


        public string Job
        {
            get { return _job; }
            set { _job = StringHelper.TitleCase(value); }
        }

        public string Source { get; set; }
        public string Target { get; set; }

        public string Action
        {
            get { return _action; }
            set { _action = StringHelper.TitleCase(value); }
        }

        public string Direction
        {
            get { return _direction; }
            set { _direction = StringHelper.TitleCase(value); }
        }

        public string Part
        {
            get { return _part; }
            set { _part = StringHelper.TitleCase(value); }
        }

        public decimal Amount { get; set; }
        public string Type { get; set; }
        public bool Hit { get; set; }
        public bool Miss { get; set; }
        public bool Crit { get; set; }
        public bool Counter { get; set; }
        public bool Block { get; set; }
        public bool Parry { get; set; }
        public bool Resist { get; set; }
        public bool Evade { get; set; }
        public bool Partial { get; set; }
    }
}
