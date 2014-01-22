// FFXIVAPP.Common
// ParseEntity.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;
using FFXIVAPP.Common.Core.Parse.Interfaces;

namespace FFXIVAPP.Common.Core.Parse
{
    public class ParseEntity : IParseEntity
    {
        private List<PlayerEntity> _players;

        public List<PlayerEntity> Players
        {
            get { return _players ?? (_players = new List<PlayerEntity>()); }
            set { _players = value; }
        }

        public decimal CombinedDPS { get; set; }
        public decimal DPS { get; set; }
        public decimal DOTPS { get; set; }
        public decimal CombinedHPS { get; set; }
        public decimal HPS { get; set; }
        public decimal HOTPS { get; set; }
        public decimal HOHPS { get; set; }
        public decimal HMPS { get; set; }
        public decimal CombinedDTPS { get; set; }
        public decimal DTPS { get; set; }
        public decimal DTOTPS { get; set; }
        public decimal CombinedTotalOverallDamage { get; set; }
        public decimal TotalOverallDamage { get; set; }
        public decimal TotalOverallDamageOverTime { get; set; }
        public decimal CombinedTotalOverallHealing { get; set; }
        public decimal TotalOverallHealing { get; set; }
        public decimal TotalOverallHealingOverTime { get; set; }
        public decimal TotalOverallHealingOverHealing { get; set; }
        public decimal TotalOverallHealingMitigated { get; set; }
        public decimal CombinedTotalOverallDamageTaken { get; set; }
        public decimal TotalOverallDamageTaken { get; set; }
        public decimal TotalOverallDamageTakenOverTime { get; set; }
        public decimal PercentOfTotalOverallDamage { get; set; }
        public decimal PercentOfTotalOverallDamageOverTime { get; set; }
        public decimal PercentOfTotalOverallHealing { get; set; }
        public decimal PercentOfTotalOverallHealingOverTime { get; set; }
        public decimal PercentOfTotalOverallHealingOverHealing { get; set; }
        public decimal PercentOfTotalOverallHealingMitigated { get; set; }
        public decimal PercentOfTotalOverallDamageTaken { get; set; }
        public decimal PercentOfTotalOverallDamageTakenOverTime { get; set; }
    }
}
