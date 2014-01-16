// FFXIVAPP.Client
// TimelineEventType.cs
// 
// © 2013 Ryan Wilson

using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Enums.Parse
{
    [DoNotObfuscate]
    public enum TimelineEventType
    {
        PartyJoin,
        PartyLeave,
        PartyDisband,
        PartyMonsterFighting,
        PartyMonsterKilled,
        AllianceMonsterFighting,
        AllianceMonsterKilled,
        OtherMonsterFighting,
        OtherMonsterKilled
    };
}
