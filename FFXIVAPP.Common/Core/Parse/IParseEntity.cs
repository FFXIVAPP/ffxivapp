// FFXIVAPP.Common
// IParseEntity.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;

namespace FFXIVAPP.Common.Core.Parse
{
    public interface IParseEntity
    {
        List<PlayerEntity> Players { get; set; }
        decimal DPS { get; set; }
        decimal HPS { get; set; }
        decimal DTPS { get; set; }
        decimal TotalOverallDamage { get; set; }
        decimal TotalOverallHealing { get; set; }
        decimal TotalOverallDamageTaken { get; set; }
    }
}
