// FFXIVAPP.Plugin.Event
// SoundEvent.cs
// 
// © 2013 Ryan Wilson

#region Usings

#endregion

namespace FFXIVAPP.Plugin.Event.Models
{
    public class SoundEvent
    {
        public string Sound { get; set; }
        public int Delay { get; set; }
        public string RegEx { get; set; }
    }
}
