// FFXIVAPP.Client
// HealingType.cs
// 
// © 2013 Ryan Wilson

using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Plugins.Parse.Enums
{
    [DoNotObfuscate]
    public enum HealingType
    {
        Normal,
        HealingOverTime,
        OverHealing,
        HealingOverTimeOverHealing,
        Mitigated
    }
}
