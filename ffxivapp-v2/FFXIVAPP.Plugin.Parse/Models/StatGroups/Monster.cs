// FFXIVAPP.Plugin.Parse
// Monster.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System.Collections.Generic;
using System.Linq;
using FFXIVAPP.Plugin.Parse.Models.LinkedStats;
using FFXIVAPP.Plugin.Parse.Models.Stats;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups
{
    public partial class Monster : StatGroup
    {
        private static readonly IList<string> LD = new[]
        {
            "Counter", "Block", "Parry", "Resist", "Evade"
        };


        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        public Monster(string name) : base(name)
        {
            InitStats();
        }

        private TotalStat TotalDrops { get; set; }
        private CounterStat Killed { get; set; }

        /// <summary>
        /// </summary>
        private void InitStats()
        {
            Stats.AddStats(TotalStatList());
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        private IEnumerable<Stat<decimal>> TotalStatList()
        {
            var stats = new Dictionary<string, Stat<decimal>>();
            TotalDrops = new TotalStat("TotalDrops");
            Killed = new CounterStat("Killed");
            stats.Add("TotalDrops", TotalDrops);
            stats.Add("Killed", Killed);
            stats.Add("Total", new CounterStat("Total"));
            stats.Add("Reg", new CounterStat("Reg"));
            stats.Add("AvgHP", new NumericStat("AvgHP"));
            stats.Add("Crit", new CounterStat("Crit"));
            stats.Add("Hit", new CounterStat("Hit"));
            stats.Add("CHit", new CounterStat("CHit"));
            stats.Add("Low", new MinStat("Low", stats["Reg"]));
            stats.Add("High", new MaxStat("High", stats["Reg"]));
            stats.Add("CLow", new MinStat("CLow", stats["Crit"]));
            stats.Add("CHigh", new MaxStat("CHigh", stats["Crit"]));
            stats.Add("Avg", new AverageStat("Avg", stats["Reg"]));
            stats.Add("CAvg", new AverageStat("CAvg", stats["Crit"]));
            return stats.Select(s => s.Value)
                        .ToList();
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        private IEnumerable<Stat<decimal>> DamageStatList()
        {
            var stats = new Dictionary<string, Stat<decimal>>();
            stats.Add("Total", new CounterStat("Total"));
            stats.Add("Used", new CounterStat("Used"));
            stats.Add("Reg", new CounterStat("Reg"));
            stats.Add("Crit", new CounterStat("Crit"));
            stats.Add("Hit", new CounterStat("Hit"));
            stats.Add("CHit", new CounterStat("CHit"));
            stats.Add("Low", new MinStat("Low", stats["Reg"]));
            stats.Add("High", new MaxStat("High", stats["Reg"]));
            stats.Add("CLow", new MinStat("CLow", stats["Crit"]));
            stats.Add("CHigh", new MaxStat("CHigh", stats["Crit"]));
            stats.Add("Avg", new AverageStat("Avg", stats["Reg"]));
            stats.Add("CAvg", new AverageStat("CAvg", stats["Crit"]));
            stats.Add("Counter", new CounterStat("Counter"));
            stats.Add("CounterPer", new PercentStat("CounterPer", stats["Counter"], stats["Used"]));
            stats.Add("Block", new CounterStat("Block"));
            stats.Add("keBlockPer", new PercentStat("BlockPer", stats["Block"], stats["Used"]));
            stats.Add("Parry", new CounterStat("Parry"));
            stats.Add("ParryPer", new PercentStat("ParryPer", stats["Parry"], stats["Used"]));
            stats.Add("Resist", new CounterStat("Resist"));
            stats.Add("ResistPer", new PercentStat("ResistPer", stats["Resist"], stats["Used"]));
            stats.Add("Evade", new CounterStat("Evade"));
            stats.Add("EvadePer", new PercentStat("EvadePer", stats["Evade"], stats["Used"]));
            return stats.Select(s => s.Value)
                        .ToList();
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        private IEnumerable<Stat<decimal>> DropStatList()
        {
            var stats = new Dictionary<string, Stat<decimal>>();
            stats.Add("Total", new CounterStat("Total"));
            TotalDrops.AddDependency(stats["Total"]);
            stats.Add("DropPer", new PercentStat("DropPer", stats["Total"], Killed));
            return stats.Select(s => s.Value)
                        .ToList();
        }
    }
}
