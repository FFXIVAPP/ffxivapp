// FFXIVAPP.Client
// FilterType.cs
// 
// © 2013 Ryan Wilson

using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Enums.Parse
{
    [DoNotObfuscate]
    public enum FilterType
    {
        Unknown,
        You,
        Pet,
        Party,
        PetParty,
        MonsterParty,
        Alliance,
        PetAlliance,
        MonsterAlliance,
        Other,
        PetOther,
        MonsterOther
    }
}
