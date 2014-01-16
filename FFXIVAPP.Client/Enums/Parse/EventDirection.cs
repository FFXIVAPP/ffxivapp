// FFXIVAPP.Client
// EventDirection.cs
// 
// © 2013 Ryan Wilson

using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Enums.Parse
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
        Alliance = 0x00000000020,
        FriendlyNPC = 0x00000000040,
        Pet = 0x00000000080,
        PetParty = 0x00000000100,
        PetAlliance = 0x00000000200,
        PetOther = 0x00000000400,
        Engaged = 0x00000000800,
        UnEngaged = 0x00000001000
    }
}
