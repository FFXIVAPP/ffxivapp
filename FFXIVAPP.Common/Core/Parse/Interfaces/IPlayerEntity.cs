// FFXIVAPP.Common
// IPlayerEntity.cs
// 
// © 2013 Ryan Wilson

using FFXIVAPP.Common.Core.Memory.Enums;

namespace FFXIVAPP.Common.Core.Parse.Interfaces
{
    public interface IPlayerEntity
    {
        string Name { get; set; }
        Actor.Job Job { get; set; }
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
