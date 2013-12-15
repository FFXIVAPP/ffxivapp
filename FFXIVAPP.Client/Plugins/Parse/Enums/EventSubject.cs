// FFXIVAPP.Client
// EventSubject.cs
// 
// © 2013 Ryan Wilson

using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Plugins.Parse.Enums
{
    [DoNotObfuscate]
    public enum EventSubject : long
    {
        Unknown = 0x0,
        You = 0x00000002000,
        Party = 0x00000004000,
        Other = 0x00000008000,
        NPC = 0x00000010000,
        AllianceMember = 0x00000020000,
        FriendlyNPC = 0x00000040000,
        Pets = 0x00000080000,
        PetsParty = 0x00000100000,
        PetsAlliance = 0x00000200000,
        PetsOther = 0x00000400000,
        Engaged = 0x00000800000,
        UnEngaged = 0x00001000000
    }
}
