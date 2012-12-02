// FFXIVAPP.Plugin.Parse
// TimelineChangedEvent.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System;
using FFXIVAPP.Plugin.Parse.Enums;

namespace FFXIVAPP.Plugin.Parse.Models.Timelines
{
    public class TimelineChangedEvent : EventArgs
    {
        private TimelineEventType EventType { get; set; }
        private object[] EventArgs { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="eventType"> </param>
        /// <param name="eventArgs"> </param>
        public TimelineChangedEvent(TimelineEventType eventType, params object[] eventArgs)
        {
            EventType = eventType;
            EventArgs = eventArgs;
        }
    }
}