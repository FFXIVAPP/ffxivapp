// FFXIVAPP.Client
// HealingType.cs
// 
// © 2013 Ryan Wilson

using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Enums.Parse
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
