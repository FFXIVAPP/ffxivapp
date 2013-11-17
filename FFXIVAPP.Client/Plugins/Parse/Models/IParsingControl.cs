// FFXIVAPP.Client
// IParseControl.cs
// 
// © 2013 Ryan Wilson

#region Usings

using FFXIVAPP.Client.Plugins.Parse.Models.Timelines;
using FFXIVAPP.Client.Plugins.Parse.Monitors;

#endregion

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
