// FFXIVAPP.Common
// IParseEntity.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;

namespace FFXIVAPP.Common.Core.Parse.Interfaces
{
    public interface IParseEntity
    {
        List<PlayerEntity> Players { get; set; }
        decimal CombinedDPS { get; set; }
        decimal DPS { get; set; }
        decimal DOTPS { get; set; }
        decimal CombinedHPS { get; set; }
        decimal HPS { get; set; }
        decimal HOTPS { get; set; }
        decimal HOHPS { get; set; }
        decimal HMPS { get; set; }
        decimal CombinedDTPS { get; set; }
        decimal DTPS { get; set; }
        decimal DTOTPS { get; set; }
        decimal CombinedTotalOverallDamage { get; set; }
        decimal TotalOverallDamage { get; set; }
        decimal TotalOverallDamageOverTime { get; set; }
        decimal CombinedTotalOverallHealing { get; set; }
        decimal TotalOverallHealing { get; set; }
        decimal TotalOverallHealingOverTime { get; set; }
        decimal TotalOverallHealingOverHealing { get; set; }
        decimal TotalOverallHealingMitigated { get; set; }
        decimal CombinedTotalOverallDamageTaken { get; set; }
        decimal TotalOverallDamageTaken { get; set; }
        decimal TotalOverallDamageTakenOverTime { get; set; }
        decimal PercentOfTotalOverallDamage { get; set; }
        decimal PercentOfTotalOverallDamageOverTime { get; set; }
        decimal PercentOfTotalOverallHealing { get; set; }
        decimal PercentOfTotalOverallHealingOverTime { get; set; }
        decimal PercentOfTotalOverallHealingOverHealing { get; set; }
        decimal PercentOfTotalOverallHealingMitigated { get; set; }
        decimal PercentOfTotalOverallDamageTaken { get; set; }
        decimal PercentOfTotalOverallDamageTakenOverTime { get; set; }
    }
}
