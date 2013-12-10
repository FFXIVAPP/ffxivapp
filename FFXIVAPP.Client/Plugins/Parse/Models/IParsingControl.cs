// FFXIVAPP.Client
// IParsingControl.cs
// 
// © 2013 Ryan Wilson

using FFXIVAPP.Client.Plugins.Parse.Models.Timelines;
using FFXIVAPP.Client.Plugins.Parse.Monitors;

namespace FFXIVAPP.Client.Plugins.Parse.Models
{
    public interface IParsingControl
    {
        IParsingControl Instance { get; }
        Timeline Timeline { get; set; }
        StatMonitor StatMonitor { get; set; }
        TimelineMonitor TimelineMonitor { get; set; }
        void Initialize();
        void Reset();
    }
}
