// FFXIVAPP.Client
// TimelineEventType.cs
// 
// © 2013 Ryan Wilson

using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Plugins.Parse.Enums
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
