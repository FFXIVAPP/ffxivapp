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
            
            stats.Add("TotalOverallDamageTaken", new TotalStat("TotalOverallDamageTaken"));
            stats.Add("RegularDamageTaken", new TotalStat("RegularDamageTaken"));
            stats.Add("CriticalDamageTaken", new TotalStat("CriticalDamageTaken"));
            stats.Add("TotalDamageTakenActionsUsed", new CounterStat("TotalDamageTakenActionsUsed"));
            stats.Add("DPS", new PerSecondAverageStat("DPS", stats["TotalOverallDamageTaken"]));
            stats.Add("DamageTakenRegHit", new TotalStat("DamageTakenRegHit"));
            stats.Add("DamageTakenRegMiss", new TotalStat("DamageTakenRegMiss"));
            stats.Add("DamageTakenRegAccuracy", new AccuracyStat("DamageTakenRegAccuracy", stats["TotalDamageTakenActionsUsed"], stats["DamageTakenRegMiss"]));
            stats.Add("DamageTakenRegLow", new MinStat("DamageTakenRegLow", stats["RegularDamageTaken"]));
            stats.Add("DamageTakenRegHigh", new MaxStat("DamageTakenRegHigh", stats["RegularDamageTaken"]));
            stats.Add("DamageTakenRegAverage", new AverageStat("DamageTakenRegAverage", stats["TotalOverallDamageTaken"]));
            stats.Add("DamageTakenCritHit", new TotalStat("DamageTakenCritHit"));
            stats.Add("DamageTakenCritPercent", new PercentStat("DamageTakenCritPercent", stats["DamageTakenCritHit"], stats["DamageTakenRegHit"]));
            stats.Add("DamageTakenCritLow", new MinStat("DamageTakenCritLow", stats["CriticalDamageTaken"]));
            stats.Add("DamageTakenCritHigh", new MaxStat("DamageTakenCritHigh", stats["CriticalDamageTaken"]));
            stats.Add("DamageTakenCritAverage", new AverageStat("DamageTakenCritAverage", stats["CriticalDamageTaken"]));

            return stats.Select(s => s.Value)
                        .ToList();
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        private IEnumerable<Stat<decimal>> DamageStatList()
        {
            var stats = new Dictionary<string, Stat<decimal>>();

            stats.Add("TotalOverallDamageTaken", new TotalStat("TotalOverallDamageTaken"));
            stats.Add("RegularDamageTaken", new TotalStat("RegularDamageTaken"));
            stats.Add("CriticalDamageTaken", new TotalStat("CriticalDamageTaken"));
            stats.Add("TotalDamageTakenActionsUsed", new CounterStat("TotalDamageTakenActionsUsed"));
            stats.Add("DPS", new PerSecondAverageStat("DPS", stats["TotalOverallDamageTaken"]));
            stats.Add("DamageTakenRegHit", new TotalStat("DamageTakenRegHit"));
            stats.Add("DamageTakenRegMiss", new TotalStat("DamageTakenRegMiss"));
            stats.Add("DamageTakenRegAccuracy", new AccuracyStat("DamageTakenRegAccuracy", stats["TotalDamageTakenActionsUsed"], stats["DamageTakenRegMiss"]));
            stats.Add("DamageTakenRegLow", new MinStat("DamageTakenRegLow", stats["RegularDamageTaken"]));
            stats.Add("DamageTakenRegHigh", new MaxStat("DamageTakenRegHigh", stats["RegularDamageTaken"]));
            stats.Add("DamageTakenRegAverage", new AverageStat("DamageTakenRegAverage", stats["TotalOverallDamageTaken"]));
            stats.Add("DamageTakenCritHit", new TotalStat("DamageTakenCritHit"));
            stats.Add("DamageTakenCritPercent", new PercentStat("DamageTakenCritPercent", stats["DamageTakenCritHit"], stats["DamageTakenRegHit"]));
            stats.Add("DamageTakenCritLow", new MinStat("DamageTakenCritLow", stats["CriticalDamageTaken"]));
            stats.Add("DamageTakenCritHigh", new MaxStat("DamageTakenCritHigh", stats["CriticalDamageTaken"]));
            stats.Add("DamageTakenCritAverage", new AverageStat("DamageTakenCritAverage", stats["CriticalDamageTaken"]));

            stats.Add("DamageTakenCounter", new CounterStat("DamageTakenCounter"));
            stats.Add("DamageTakenCounterPercent", new PercentStat("DamageTakenCounterPercent", stats["DamageTakenCounter"], stats["TotalDamageTakenActionsUsed"]));
            stats.Add("DamageTakenCounterReduction", new TotalStat("DamageTakenCounterReduction"));
            stats.Add("DamageTakenCounterReductionAverage", new AverageStat("DamageTakenCounterReductionAverage", stats["DamageTakenCounterReduction"]));
            stats.Add("DamageTakenBlock", new CounterStat("DamageTakenBlock"));
            stats.Add("DamageTakenBlockPercent", new PercentStat("DamageTakenBlockPercent", stats["DamageTakenBlock"], stats["TotalDamageTakenActionsUsed"]));
            stats.Add("DamageTakenBlockReduction", new TotalStat("DamageTakenBlockReduction"));
            stats.Add("DamageTakenBlockReductionAverage", new AverageStat("DamageTakenBlockReductionAverage", stats["DamageTakenBlockReduction"]));
            stats.Add("DamageTakenParry", new CounterStat("DamageTakenParry"));
            stats.Add("DamageTakenParryPercent", new PercentStat("DamageTakenParryPercent", stats["DamageTakenParry"], stats["TotalDamageTakenActionsUsed"]));
            stats.Add("DamageTakenParryReduction", new TotalStat("DamageTakenParryReduction"));
            stats.Add("DamageTakenParryReductionAverage", new AverageStat("DamageTakenParryReductionAverage", stats["DamageTakenParryReduction"]));
            stats.Add("DamageTakenResist", new CounterStat("DamageTakenResist"));
            stats.Add("DamageTakenResistPercent", new PercentStat("DamageTakenResistPercent", stats["DamageTakenResist"], stats["TotalDamageTakenActionsUsed"]));
            stats.Add("DamageTakenResistReduction", new TotalStat("DamageTakenResistReduction"));
            stats.Add("DamageTakenResistReductionAverage", new AverageStat("DamageTakenResistReductionAverage", stats["DamageTakenResistReduction"]));
            stats.Add("DamageTakenEvade", new CounterStat("DamageTakenEvade"));
            stats.Add("DamageTakenEvadePercent", new PercentStat("DamageTakenEvadePercent", stats["DamageTakenEvade"], stats["TotalDamageTakenActionsUsed"]));
            stats.Add("DamageTakenEvadeReduction", new TotalStat("DamageTakenEvadeReduction"));
            stats.Add("DamageTakenEvadeReductionAverage", new AverageStat("DamageTakenEvadeReductionAverage", stats["DamageTakenEvadeReduction"]));

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
    }
}
