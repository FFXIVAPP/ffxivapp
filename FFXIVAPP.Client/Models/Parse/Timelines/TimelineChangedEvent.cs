// FFXIVAPP.Client
// TimelineChangedEvent.cs
// 
// © 2013 Ryan Wilson

using System;
using FFXIVAPP.Client.Enums.Parse;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models.Parse.Timelines
{
    [DoNotObfuscate]
    public class TimelineChangedEvent : EventArgs
    {
        /// <summary>
        /// </summary>
        /// <param name="eventType"> </param>
        /// <param name="eventArgs"> </param>
        public TimelineChangedEvent(TimelineEventType eventType, params object[] eventArgs)
        {
            EventType = eventType;
            EventArgs = eventArgs;
        }

        private TimelineEventType EventType { get; set; }
        private object[] EventArgs { get; set; }
    }
}
