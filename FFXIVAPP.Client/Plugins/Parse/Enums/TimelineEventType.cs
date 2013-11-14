// FFXIVAPP.Client
// TimelineEventType.cs
// 
// © 2013 Ryan Wilson

#region Usings

using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Plugins.Parse.Enums
{
    [DoNotObfuscate]
    public enum TimelineEventType
    {
        PartyJoin,
        PartyLeave,
        PartyDisband,
        MobFighting,
        MobKilled,
        BeneficialGain,
        BeneficialLose,
        DetrimentalGain,
        DetrimentalLose,
    };
}
