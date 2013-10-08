// FFXIVAPP.Plugin.Parse
// EventDirection.cs
// 
// © 2013 Ryan Wilson

namespace FFXIVAPP.Client.Enums
{
    public enum EventDirection
    {
        Unknown = 0x00000000,
        Self = 0x00000001,
        You = 0x00000002,
        Party = 0x00000004,
        Other = 0x00000008,
        NPC = 0x00000010,
        Engaged = 0x00000020,
        UnEngaged = 0x00000040
    }
}
