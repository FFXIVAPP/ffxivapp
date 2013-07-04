// FFXIVAPP.Plugin.Parse
// Monster.cs
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
            LineHistory = new List<LineHistory>();
        }

        public List<LineHistory> LineHistory { get; set; }

        private TotalStat TotalOverallDrops { get; set; }
        private CounterStat TotalKilled { get; set; }

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

            TotalOverallDrops = new TotalStat("TotalOverallDrops");
            TotalKilled = new CounterStat("TotalKilled");

            stats.Add("TotalOverallDrops", TotalOverallDrops);
            stats.Add("TotalKilled", TotalKilled);
            stats.Add("AverageHP", new NumericStat("AverageHP"));

            //setup monster damage taken stats
            var damageTakenStats = DamageTakenStats();
            foreach (var damageTakenStat in damageTakenStats)
            {
                stats.Add(damageTakenStat.Key, damageTakenStat.Value);
            }

            //link to main monster stats
            StatMonitor.TotalOverallDamageTakenMonster.AddDependency(stats["TotalOverallDamageTaken"]);
            StatMonitor.RegularDamageTakenMonster.AddDependency(stats["RegularDamageTaken"]);
            StatMonitor.CriticalDamageTakenMonster.AddDependency(stats["CriticalDamageTaken"]);

            //setup global "percent of" stats
            stats.Add("PercentOfTotalOverallDamageTaken", new PercentStat("PercentOfTotalOverallDamageTaken", stats["TotalOverallDamageTaken"], StatMonitor.TotalOverallDamageTakenMonster));
            stats.Add("PercentOfRegularDamageTaken", new PercentStat("PercentOfRegularDamageTaken", stats["RegularDamageTaken"], StatMonitor.RegularDamageTakenMonster));
            stats.Add("PercentOfCriticalDamageTaken", new PercentStat("PercentOfCriticalDamageTaken", stats["CriticalDamageTaken"], StatMonitor.CriticalDamageTakenMonster));

            return stats.Select(s => s.Value)
                        .ToList();
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        private IEnumerable<Stat<decimal>> DamageTakenStatList()
        {
            var stats = DamageTakenStats();

            //setup per damage taken "percent of" stats
            stats.Add("PercentOfTotalOverallDamageTaken", new PercentStat("PercentOfTotalOverallDamageTaken", stats["TotalOverallDamageTaken"], Stats.GetStat("TotalOverallDamageTaken")));
            stats.Add("PercentOfRegularDamageTaken", new PercentStat("PercentOfRegularDamageTaken", stats["RegularDamageTaken"], Stats.GetStat("RegularDamageTaken")));
            stats.Add("PercentOfCriticalDamageTaken", new PercentStat("PercentOfCriticalDamageTaken", stats["CriticalDamageTaken"], Stats.GetStat("CriticalDamageTaken")));

            return stats.Select(s => s.Value)
                        .ToList();
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        private IEnumerable<Stat<decimal>> DropStatList()
        {
            var stats = new Dictionary<string, Stat<decimal>>();

            stats.Add("TotalDrops", new CounterStat("TotalDrops"));
            stats.Add("DropPercent", new PercentStat("DropPercent", stats["TotalDrops"], TotalKilled));

            TotalOverallDrops.AddDependency(stats["TotalDrops"]);

            return stats.Select(s => s.Value)
                        .ToList();
        }

        #region Stat Generation Methods

        private static Dictionary<string, Stat<decimal>> DamageTakenStats()
        {
            var stats = new Dictionary<string, Stat<decimal>>();

            stats.Add("TotalOverallDamageTaken", new TotalStat("TotalOverallDamageTaken"));
            stats.Add("RegularDamageTaken", new TotalStat("RegularDamageTaken"));
            stats.Add("CriticalDamageTaken", new TotalStat("CriticalDamageTaken"));
            stats.Add("TotalDamageTakenActionsUsed", new CounterStat("TotalDamageTakenActionsUsed"));
            stats.Add("DTPS", new PerSecondAverageStat("DTPS", stats["TotalOverallDamageTaken"]));
            stats.Add("DamageTakenRegHit", new TotalStat("DamageTakenRegHit"));
            stats.Add("DamageTakenRegMiss", new TotalStat("DamageTakenRegMiss"));
            stats.Add("DamageTakenRegAccuracy", new AccuracyStat("DamageTakenRegAccuracy", stats["TotalDamageTakenActionsUsed"], stats["DamageTakenRegMiss"]));
            stats.Add("DamageTakenRegLow", new MinStat("DamageTakenRegLow", stats["RegularDamageTaken"]));
            stats.Add("DamageTakenRegHigh", new MaxStat("DamageTakenRegHigh", stats["RegularDamageTaken"]));
            stats.Add("DamageTakenRegAverage", new AverageStat("DamageTakenRegAverage", stats["RegularDamageTaken"]));
            stats.Add("DamageTakenRegMod", new TotalStat("DamageTakenRegMod"));
            stats.Add("DamageTakenRegModAverage", new AverageStat("DamageTakenRegModAverage", stats["DamageTakenRegMod"]));
            stats.Add("DamageTakenCritHit", new TotalStat("DamageTakenCritHit"));
            stats.Add("DamageTakenCritPercent", new PercentStat("DamageTakenCritPercent", stats["DamageTakenCritHit"], stats["TotalDamageTakenActionsUsed"]));
            stats.Add("DamageTakenCritLow", new MinStat("DamageTakenCritLow", stats["CriticalDamageTaken"]));
            stats.Add("DamageTakenCritHigh", new MaxStat("DamageTakenCritHigh", stats["CriticalDamageTaken"]));
            stats.Add("DamageTakenCritAverage", new AverageStat("DamageTakenCritAverage", stats["CriticalDamageTaken"]));
            stats.Add("DamageTakenCritMod", new TotalStat("DamageTakenCritMod"));
            stats.Add("DamageTakenCritModAverage", new AverageStat("DamageTakenCritModAverage", stats["DamageTakenCritMod"]));

            stats.Add("DamageTakenCounter", new CounterStat("DamageTakenCounter"));
            stats.Add("DamageTakenCounterPercent", new PercentStat("DamageTakenCounterPercent", stats["DamageTakenCounter"], stats["TotalDamageTakenActionsUsed"]));
            stats.Add("DamageTakenCounterMod", new TotalStat("DamageTakenCounterMod"));
            stats.Add("DamageTakenCounterModAverage", new AverageStat("DamageTakenCounterModAverage", stats["DamageTakenCounterMod"]));
            stats.Add("DamageTakenBlock", new CounterStat("DamageTakenBlock"));
            stats.Add("DamageTakenBlockPercent", new PercentStat("DamageTakenBlockPercent", stats["DamageTakenBlock"], stats["TotalDamageTakenActionsUsed"]));
            stats.Add("DamageTakenBlockMod", new TotalStat("DamageTakenBlockMod"));
            stats.Add("DamageTakenBlockModAverage", new AverageStat("DamageTakenBlockModAverage", stats["DamageTakenBlockMod"]));
            stats.Add("DamageTakenParry", new CounterStat("DamageTakenParry"));
            stats.Add("DamageTakenParryPercent", new PercentStat("DamageTakenParryPercent", stats["DamageTakenParry"], stats["TotalDamageTakenActionsUsed"]));
            stats.Add("DamageTakenParryMod", new TotalStat("DamageTakenParryMod"));
            stats.Add("DamageTakenParryModAverage", new AverageStat("DamageTakenParryModAverage", stats["DamageTakenParryMod"]));
            stats.Add("DamageTakenResist", new CounterStat("DamageTakenResist"));
            stats.Add("DamageTakenResistPercent", new PercentStat("DamageTakenResistPercent", stats["DamageTakenResist"], stats["TotalDamageTakenActionsUsed"]));
            stats.Add("DamageTakenResistMod", new TotalStat("DamageTakenResistMod"));
            stats.Add("DamageTakenResistModAverage", new AverageStat("DamageTakenResistModAverage", stats["DamageTakenResistMod"]));
            stats.Add("DamageTakenEvade", new CounterStat("DamageTakenEvade"));
            stats.Add("DamageTakenEvadePercent", new PercentStat("DamageTakenEvadePercent", stats["DamageTakenEvade"], stats["TotalDamageTakenActionsUsed"]));
            stats.Add("DamageTakenEvadeMod", new TotalStat("DamageTakenEvadeMod"));
            stats.Add("DamageTakenEvadeModAverage", new AverageStat("DamageTakenEvadeModAverage", stats["DamageTakenEvadeMod"]));

            return stats;
        }

        #endregion
    }
}
