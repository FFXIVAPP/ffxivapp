// FFXIVAPP.Plugin.Parse
// Player.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.Collections.Generic;
using System.Linq;
using FFXIVAPP.Plugin.Parse.Models.LinkedStats;
using FFXIVAPP.Plugin.Parse.Models.Stats;
using FFXIVAPP.Plugin.Parse.Monitors;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups
{
    public partial class Player : StatGroup
    {
        private static readonly IList<string> LD = new[]
        {
            "Counter", "Block", "Parry", "Resist", "Evade"
        };

        public Player(string name) : base(name)
        {
            InitStats();
        }

        private void InitStats()
        {
            Stats.AddStats(TotalStatList());
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        private static IEnumerable<Stat<decimal>> TotalStatList()
        {
            var stats = new Dictionary<string, Stat<decimal>>();
            stats.Add("Total", new TotalStat("Total"));
            stats.Add("Reg", new TotalStat("Reg"));
            stats.Add("Crit", new TotalStat("Crit"));
            stats.Add("HTotal", new TotalStat("HTotal"));
            stats.Add("DTTotal", new TotalStat("DTTotal"));
            stats.Add("DTReg", new TotalStat("DTReg"));
            stats.Add("DTCrit", new TotalStat("DTCrit"));
            StatMonitor.TotalDamage.AddDependency(stats["Total"]);
            StatMonitor.PartyDamage.AddDependency(stats["Reg"]);
            StatMonitor.PartyCritDamage.AddDependency(stats["Crit"]);
            StatMonitor.PartyHealing.AddDependency(stats["HTotal"]);
            StatMonitor.PartyTotalTaken.AddDependency(stats["DTTotal"]);
            StatMonitor.PartyTotalRegTaken.AddDependency(stats["DTReg"]);
            StatMonitor.PartyTotalCritTaken.AddDependency(stats["DTCrit"]);
            stats.Add("Hit", new TotalStat("Hit"));
            stats.Add("CHit", new TotalStat("CHit"));
            stats.Add("DTHit", new TotalStat("DTHit"));
            stats.Add("DTCHit", new TotalStat("DTCHit"));
            stats.Add("Miss", new TotalStat("Miss"));
            stats.Add("Acc", new AccuracyStat("Acc", stats["Hit"], stats["Miss"]));
            stats.Add("Counter", new CounterStat("Counter"));
            stats.Add("Block", new CounterStat("Block"));
            stats.Add("Parry", new CounterStat("Parry"));
            stats.Add("Resist", new CounterStat("Resist"));
            stats.Add("Evade", new CounterStat("Evade"));
            stats.Add("PerOfDmg", new PercentStat("PerOfDmg", stats["Total"], StatMonitor.TotalDamage));
            stats.Add("PerOfCrit", new PercentStat("PerOfCrit", stats["Crit"], StatMonitor.PartyCritDamage));
            stats.Add("PerOfDTDmg", new PercentStat("PerOfDTDmg", stats["DTTotal"], StatMonitor.PartyTotalTaken));
            stats.Add("PerOfDTCDmg", new PercentStat("PerOfDTCDmg", stats["DTCrit"], StatMonitor.PartyTotalCritTaken));
            stats.Add("CritPer", new PercentStat("CritPer", stats["CHit"], stats["Hit"]));
            stats.Add("PerOfHeal", new PercentStat("PerOfHeal", stats["HTotal"], StatMonitor.PartyHealing));
            stats.Add("Low", new MinStat("Low", stats["Reg"]));
            stats.Add("High", new MaxStat("High", stats["Reg"]));
            stats.Add("CLow", new MinStat("CLow", stats["Crit"]));
            stats.Add("CHigh", new MaxStat("CHigh", stats["Crit"]));
            stats.Add("HealLow", new MinStat("HealLow", stats["HTotal"]));
            stats.Add("HealHigh", new MaxStat("HealHigh", stats["HTotal"]));
            stats.Add("DTLow", new MinStat("DTLow", stats["DTReg"]));
            stats.Add("DTHigh", new MaxStat("DTHigh", stats["DTReg"]));
            stats.Add("DTCLow", new MinStat("DTCLow", stats["DTCrit"]));
            stats.Add("DTCHigh", new MaxStat("DTCHigh", stats["DTCrit"]));
            stats.Add("Avg", new AverageStat("Avg", stats["Total"]));
            stats.Add("CAvg", new AverageStat("CAvg", stats["Crit"]));
            stats.Add("DTAvg", new AverageStat("DTAvg", stats["DTReg"]));
            stats.Add("DTCAvg", new AverageStat("DTCAvg", stats["DTCrit"]));
            stats.Add("HealAvg", new AverageStat("HealAvg", stats["HTotal"]));
            stats.Add("DPS", new PerSecondAverageStat("DPS", stats["Total"]));
            stats.Add("HPS", new PerSecondAverageStat("HPS", stats["HTotal"]));
            stats.Add("DTPS", new PerSecondAverageStat("DTPS", stats["DTTotal"]));
            return stats.Select(s => s.Value)
                        .ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="type"> </param>
        /// <param name="sub"> </param>
        /// <returns> </returns>
        private IEnumerable<Stat<decimal>> AbilityStatList(string type, StatGroup sub)
        {
            var stats = new Dictionary<string, Stat<decimal>>();
            stats.Add("Total", new CounterStat("Total"));
            stats.Add("Used", new CounterStat("Used"));
            stats.Add("DPS", new PerSecondAverageStat("DPS", stats["Total"]));
            stats.Add("Reg", new CounterStat("Reg"));
            stats.Add("Crit", new CounterStat("Crit"));
            switch (type)
            {
                case "a":
                    stats.Add("PerOfReg", new PercentStat("PerOfReg", stats["Reg"], Stats.GetStat("Reg")));
                    stats.Add("PerOfCrit", new PercentStat("PerOfCrit", stats["Crit"], Stats.GetStat("Crit")));
                    break;
                case "m":
                    stats.Add("PerOfReg", new PercentStat("PerOfReg", stats["Reg"], Stats.GetStat("Reg")));
                    stats.Add("PerOfCrit", new PercentStat("PerOfCrit", stats["Crit"], Stats.GetStat("Crit")));
                    break;
                case "ma":
                    stats.Add("PerOfReg", new PercentStat("PerOfReg", stats["Reg"], sub.Stats.GetStat("Reg")));
                    stats.Add("PerOfCrit", new PercentStat("PerOfCrit", stats["Crit"], sub.Stats.GetStat("Crit")));
                    break;
            }
            stats.Add("Hit", new CounterStat("Hit"));
            stats.Add("Miss", new CounterStat("Miss"));
            stats.Add("CHit", new CounterStat("CHit"));
            stats.Add("Acc", new AccuracyStat("Acc", stats["Used"], stats["Miss"]));
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
        /// <param name="type"> </param>
        /// <param name="sub"> </param>
        /// <returns> </returns>
        private IEnumerable<Stat<decimal>> HealingStatList(string type, StatGroup sub)
        {
            var stats = new Dictionary<string, Stat<decimal>>();
            stats.Add("Total", new CounterStat("Total"));
            stats.Add("HPS", new PerSecondAverageStat("HPS", stats["Total"]));
            switch (type)
            {
                case "a":
                    stats.Add("PerOfHeal", new PercentStat("PerOfHeal", stats["Total"], Stats.GetStat("HTotal")));
                    break;
                case "p":
                    stats.Add("PerOfHeal", new PercentStat("PerOfHeal", stats["Total"], Stats.GetStat("HTotal")));
                    break;
                case "pa":
                    stats.Add("PerOfHeal", new PercentStat("PerOfHeal", stats["Total"], sub.Stats.GetStat("Total")));
                    break;
            }
            stats.Add("Low", new MinStat("Low", stats["Total"]));
            stats.Add("High", new MaxStat("High", stats["Total"]));
            stats.Add("Avg", new AverageStat("Avg", stats["Total"]));
            return stats.Select(s => s.Value)
                        .ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="type"> </param>
        /// <param name="sub"> </param>
        /// <returns> </returns>
        private IEnumerable<Stat<decimal>> DamageStatList(string type, StatGroup sub)
        {
            var stats = new Dictionary<string, Stat<decimal>>();
            stats.Add("Total", new CounterStat("Total"));
            stats.Add("Used", new CounterStat("Used"));
            stats.Add("DTPS", new PerSecondAverageStat("DTPS", stats["Total"]));
            stats.Add("Reg", new CounterStat("Reg"));
            stats.Add("Crit", new CounterStat("Crit"));
            switch (type)
            {
                case "m":
                    stats.Add("PerOfReg", new PercentStat("PerOfReg", stats["Reg"], Stats.GetStat("Reg")));
                    stats.Add("PerOfCrit", new PercentStat("PerOfCrit", stats["Crit"], Stats.GetStat("Crit")));
                    break;
                case "a":
                    stats.Add("PerOfReg", new PercentStat("PerOfReg", stats["Reg"], sub.Stats.GetStat("Reg")));
                    stats.Add("PerOfCrit", new PercentStat("PerOfCrit", stats["Crit"], sub.Stats.GetStat("Crit")));
                    break;
            }
            stats.Add("Hit", new CounterStat("Hit"));
            stats.Add("Miss", new CounterStat("Miss"));
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
    }
}
