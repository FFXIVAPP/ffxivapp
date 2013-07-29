// ParseModXIV
// EventMonitor.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using ParseModXIV.Classes;
using ParseModXIV.Stats;

namespace ParseModXIV.Model
{
    public class EventMonitor : StatGroup
    {
        private DateTime LastEventReceived { get; set; }
        protected UInt16 Filter { private get; set; }
        protected ParseMod ParseModInstance { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parseModInstance"></param>
        protected EventMonitor(String name, ParseMod parseModInstance) : base(name)
        {
            DoInit(parseModInstance);
            EventParser.Instance.OnLogEvent += FilterEvent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        private void DoInit(ParseMod instance)
        {
            ParseModInstance = instance;
            InitStats();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void InitStats()
        {
            foreach (var stat in Stats)
            {
                stat.OnValueChanged += DoStatChanged;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="e"></param>
        private void FilterEvent(object src, Event e)
        {
            if (e.MatchesFilter(Filter))
            {
                LastEventReceived = e.Timestamp;
                HandleEvent(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void HandleEvent(Event e)
        {
        }

        /// <summary>
        /// HOOK INTO THIS EVENT IN THE GUI OR ELSEWHERE IF YOU WANT TO GET NOTIFIED WHENEVER A STAT IS UPDATED!
        /// </summary>
        public event EventHandler<StatChangedEvent> OnStatChanged;

        private void DoStatChanged(object src, StatChangedEvent e)
        {
            var onStatChange = OnStatChanged;
            if (onStatChange != null)
            {
                OnStatChanged(this, e);
            }
        }
    }
}