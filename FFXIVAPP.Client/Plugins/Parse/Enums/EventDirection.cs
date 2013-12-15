// FFXIVAPP.Client
// EventDirection.cs
// 
// © 2013 Ryan Wilson

using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Plugins.Parse.Enums
{
    [DoNotObfuscate]
    public enum EventDirection : long
    {
        Unknown = 0x0,
        Self = 0x00000000001,
        You = 0x00000000002,
        Party = 0x00000000004,
        Other = 0x00000000008,
        NPC = 0x00000000010,
        AllianceMember = 0x00000000020,
        FriendlyNPC = 0x00000000040,
        Pets = 0x00000000080,
        PetsParty = 0x00000000100,
        PetsAlliance = 0x00000000200,
        PetsOther = 0x00000000400,
        Engaged = 0x00000000800,
        UnEngaged = 0x00000001000
    }
}
