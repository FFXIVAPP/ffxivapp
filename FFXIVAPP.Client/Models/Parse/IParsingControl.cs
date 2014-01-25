// FFXIVAPP.Client
// IParsingControl.cs
// 
// © 2013 Ryan Wilson

using System;
using FFXIVAPP.Client.Models.Parse.Timelines;
using FFXIVAPP.Client.Monitors;

namespace FFXIVAPP.Client.Models.Parse
{
    public interface IParsingControl
    {
        string Name { get; set; }
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
