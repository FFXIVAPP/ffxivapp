// FFXIVAPP.Client
// SoundEvent.cs
// 
// © 2013 Ryan Wilson

#region Usings

using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Plugins.Event.Models
{
    [DoNotObfuscate]
    public class SoundEvent
    {
        public string Sound { get; set; }
        public int Delay { get; set; }
        public string RegEx { get; set; }
    }
}
