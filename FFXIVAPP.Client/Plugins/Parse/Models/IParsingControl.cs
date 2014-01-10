// FFXIVAPP.Client
// IParsingControl.cs
// 
// © 2013 Ryan Wilson

using System;
using FFXIVAPP.Client.Plugins.Parse.Models.Timelines;
using FFXIVAPP.Client.Plugins.Parse.Monitors;

namespace FFXIVAPP.Client.Plugins.Parse.Models
{
    public interface IParsingControl
    {
        DateTime StartTime { get; set; }
        DateTime EndTime { get; set; }
        IParsingControl Instance { get; }
        Timeline Timeline { get; set; }
        StatMonitor StatMonitor { get; set; }
        TimelineMonitor TimelineMonitor { get; set; }
        void Initialize();
        void Reset();
    }
}
