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

        public decimal DPS { get; set; }
        public decimal HPS { get; set; }
        public decimal DTPS { get; set; }
        public decimal TotalOverallDamage { get; set; }
        public decimal TotalOverallHealing { get; set; }
        public decimal TotalOverallDamageTaken { get; set; }
    }
}
