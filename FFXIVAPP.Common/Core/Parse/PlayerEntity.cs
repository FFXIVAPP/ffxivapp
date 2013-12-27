// FFXIVAPP.Common
// PlayerEntity.cs
// 
// © 2013 Ryan Wilson

using FFXIVAPP.Common.Core.Memory.Enums;
using FFXIVAPP.Common.Core.Parse.Interfaces;
using FFXIVAPP.Common.Helpers;

namespace FFXIVAPP.Common.Core.Parse
{
    public class PlayerEntity : IPlayerEntity
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = StringHelper.TitleCase(value); }
        }

        public Actor.Job Job { get; set; }
        public decimal DPS { get; set; }
        public decimal HPS { get; set; }
        public decimal DTPS { get; set; }
        public decimal TotalOverallDamage { get; set; }
        public decimal TotalOverallHealing { get; set; }
        public decimal TotalOverallDamageTaken { get; set; }
        public decimal PercentOfTotalOverallDamage { get; set; }
        public decimal PercentOfTotalOverallHealing { get; set; }
        public decimal PercentOfTotalOverallDamageTaken { get; set; }
    }
}
