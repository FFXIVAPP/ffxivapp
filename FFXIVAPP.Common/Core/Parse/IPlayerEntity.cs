// FFXIVAPP.Common
// IPlayerData.cs
// 
// © 2013 Ryan Wilson

namespace FFXIVAPP.Common.Core.Parse
{
    public interface IPlayerEntity
    {
        string Name { get; set; }
        decimal DPS { get; set; }
        decimal HPS { get; set; }
        decimal DTPS { get; set; }
        decimal TotalOverallDamage { get; set; }
        decimal TotalOverallHealing { get; set; }
        decimal TotalOverallDamageTaken { get; set; }
        decimal PercentOfTotalOverallDamage { get; set; }
        decimal PercentOfTotalOverallHealing { get; set; }
        decimal PercentOfTotalOverallDamageTaken { get; set; }
    }
}
