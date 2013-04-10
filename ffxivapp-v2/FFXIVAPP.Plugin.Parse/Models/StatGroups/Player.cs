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

            //setup player ability stats
            var damageStats = DamageStats();
            foreach (var damageStat in damageStats)
            {
                stats.Add(damageStat.Key, damageStat.Value);
            }

            //setup player healing stats
            var healingStats = HealingStats();
            foreach (var healingStat in healingStats)
            {
                stats.Add(healingStat.Key, healingStat.Value);
            }

            //setup player damage taken stats
            var damageTakenStats = DamageTakenStats();
            foreach (var damageTakenStat in damageTakenStats)
            {
                stats.Add(damageTakenStat.Key, damageTakenStat.Value);
            }

            //link to main party stats
            StatMonitor.TotalOverallDamage.AddDependency(stats["TotalOverallDamage"]);
            StatMonitor.RegularDamage.AddDependency(stats["RegularDamage"]);
            StatMonitor.CriticalDamage.AddDependency(stats["CriticalDamage"]);
            StatMonitor.TotalOverallHealing.AddDependency(stats["TotalOverallHealing"]);
            StatMonitor.RegularHealing.AddDependency(stats["RegularHealing"]);
            StatMonitor.CriticalHealing.AddDependency(stats["CriticalHealing"]);
            StatMonitor.TotalOverallDamageTaken.AddDependency(stats["TotalOverallDamageTaken"]);
            StatMonitor.RegularDamageTaken.AddDependency(stats["RegularDamageTaken"]);
            StatMonitor.CriticalDamageTaken.AddDependency(stats["CriticalDamageTaken"]);

            //setup global "percent of" stats
            stats.Add("PercentOfOverallDamage", new PercentStat("PercentOfOverallDamage", stats["TotalOverallDamage"], StatMonitor.TotalOverallDamage));
            stats.Add("PercentOfRegularDamage", new PercentStat("PercentOfRegularDamage", stats["RegularDamage"], StatMonitor.RegularDamage));
            stats.Add("PercentOfCriticalDamage", new PercentStat("PercentOfCriticalDamage", stats["CriticalDamage"], StatMonitor.CriticalDamage));
            stats.Add("PercentOfOverallHealing", new PercentStat("PercentOfOverallHealing", stats["TotalOverallHealing"], StatMonitor.TotalOverallHealing));
            stats.Add("PercentOfRegularHealing", new PercentStat("PercentOfRegularHealing", stats["RegularHealing"], StatMonitor.RegularHealing));
            stats.Add("PercentOfCriticalHealing", new PercentStat("PercentOfCriticalHealing", stats["CriticalHealing"], StatMonitor.CriticalHealing));
            stats.Add("PercentOfOverallDamageTaken", new PercentStat("PercentOfOverallDamageTaken", stats["TotalOverallDamageTaken"], StatMonitor.TotalOverallDamageTaken));
            stats.Add("PercentOfRegularDamageTaken", new PercentStat("PercentOfRegularDamageTaken", stats["RegularDamageTaken"], StatMonitor.RegularDamageTaken));
            stats.Add("PercentOfCriticalDamageTaken", new PercentStat("PercentOfCriticalDamageTaken", stats["CriticalDamageTaken"], StatMonitor.CriticalDamageTaken));

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
            var stats = DamageStats();

            //setup per ability "percent of" stats
            switch (type)
            {
                case "a":
                case "m":
                    stats.Add("PercentOfTotalOverallDamage", new PercentStat("PercentOfTotalOverallDamage", stats["TotalOverallDamage"], Stats.GetStat("TotalOverallDamage")));
                    stats.Add("PercentOfRegularDamage", new PercentStat("PercentOfRegularDamage", stats["RegularDamage"], Stats.GetStat("RegularDamage")));
                    stats.Add("PercentOfCriticalDamage", new PercentStat("PercentOfCriticalDamage", stats["CriticalDamage"], Stats.GetStat("CriticalDamage")));
                    break;
                case "ma":
                    stats.Add("PercentOfTotalOverallDamage", new PercentStat("PercentOfTotalOverallDamage", stats["TotalOverallDamage"], sub.Stats.GetStat("TotalOverallDamage")));
                    stats.Add("PercentOfRegularDamage", new PercentStat("PercentOfRegularDamage", stats["RegularDamage"], sub.Stats.GetStat("RegularDamage")));
                    stats.Add("PercentOfCriticalDamage", new PercentStat("PercentOfCriticalDamage", stats["CriticalDamage"], sub.Stats.GetStat("CriticalDamage")));
                    break;
            }

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
            var stats = HealingStats();

            //setup per healing "percent of" stats
            switch (type)
            {
                case "a":
                case "p":
                    stats.Add("PercentOfTotalOverallHealing", new PercentStat("PercentOfTotalOverallHealing", stats["TotalOverallHealing"], Stats.GetStat("TotalOverallHealing")));
                    stats.Add("PercentOfRegularHealing", new PercentStat("PercentOfRegularHealing", stats["RegularHealing"], Stats.GetStat("RegularHealing")));
                    stats.Add("PercentOfCriticalHealing", new PercentStat("PercentOfCriticalHealing", stats["CriticalHealing"], Stats.GetStat("CriticalHealing")));
                    break;
                case "pa":
                    stats.Add("PercentOfTotalOverallHealing", new PercentStat("PercentOfTotalOverallHealing", stats["TotalOverallHealing"], sub.Stats.GetStat("TotalOverallHealing")));
                    stats.Add("PercentOfRegularHealing", new PercentStat("PercentOfRegularHealing", stats["RegularHealing"], sub.Stats.GetStat("RegularHealing")));
                    stats.Add("PercentOfCriticalHealing", new PercentStat("PercentOfCriticalHealing", stats["CriticalHealing"], sub.Stats.GetStat("CriticalHealing")));
                    break;
            }

            return stats.Select(s => s.Value)
                        .ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="type"> </param>
        /// <param name="sub"> </param>
        /// <returns> </returns>
        private IEnumerable<Stat<decimal>> DamageTakenStatList(string type, StatGroup sub)
        {
            var stats = DamageTakenStats();

            //setup per damage taken "percent of" stats
            switch (type)
            {
                case "m":
                    stats.Add("PercentOfTotalOverallDamageTaken", new PercentStat("PercentOfTotalOverallDamageTaken", stats["TotalOverallDamageTaken"], Stats.GetStat("TotalOverallDamageTaken")));
                    stats.Add("PercentOfRegularDamageTaken", new PercentStat("PercentOfRegularDamageTaken", stats["RegularDamageTaken"], Stats.GetStat("RegularDamageTaken")));
                    stats.Add("PercentOfCriticalDamageTaken", new PercentStat("PercentOfCriticalDamageTaken", stats["CriticalDamageTaken"], Stats.GetStat("CriticalDamageTaken")));
                    break;
                case "a":
                    stats.Add("PercentOfTotalOverallDamageTaken", new PercentStat("PercentOfTotalOverallDamageTaken", stats["TotalOverallDamageTaken"], sub.Stats.GetStat("TotalOverallDamageTaken")));
                    stats.Add("PercentOfRegularDamageTaken", new PercentStat("PercentOfRegularDamageTaken", stats["RegularDamageTaken"], sub.Stats.GetStat("RegularDamageTaken")));
                    stats.Add("PercentOfCriticalDamageTaken", new PercentStat("PercentOfCriticalDamageTaken", stats["CriticalDamageTaken"], sub.Stats.GetStat("CriticalDamageTaken")));
                    break;
            }

            return stats.Select(s => s.Value)
                        .ToList();
        }

        #region Stat Generation Methods

        private static Dictionary<string, Stat<decimal>> DamageStats()
        {
            var stats = new Dictionary<string, Stat<decimal>>();

            stats.Add("TotalOverallDamage", new TotalStat("TotalOverallDamage"));
            stats.Add("RegularDamage", new TotalStat("RegularDamage"));
            stats.Add("CriticalDamage", new TotalStat("CriticalDamage"));
            stats.Add("TotalDamageActionsUsed", new CounterStat("TotalDamageActionsUsed"));
            stats.Add("DPS", new PerSecondAverageStat("DPS", stats["TotalOverallDamage"]));
            stats.Add("DamageRegHit", new TotalStat("DamageRegHit"));
            stats.Add("DamageRegMiss", new TotalStat("DamageRegMiss"));
            stats.Add("DamageRegAccuracy", new AccuracyStat("DamageRegAccuracy", stats["TotalDamageActionsUsed"], stats["DamageRegMiss"]));
            stats.Add("DamageRegLow", new MinStat("DamageRegLow", stats["RegularDamage"]));
            stats.Add("DamageRegHigh", new MaxStat("DamageRegHigh", stats["RegularDamage"]));
            stats.Add("DamageRegAverage", new AverageStat("DamageRegAverage", stats["TotalOverallDamage"]));
            stats.Add("DamageCritHit", new TotalStat("DamageCritHit"));
            stats.Add("DamageCritPercent", new PercentStat("DamageCritPercent", stats["DamageCritHit"], stats["DamageRegHit"]));
            stats.Add("DamageCritLow", new MinStat("DamageCritLow", stats["CriticalDamage"]));
            stats.Add("DamageCritHigh", new MaxStat("DamageCritHigh", stats["CriticalDamage"]));
            stats.Add("DamageCritAverage", new AverageStat("DamageCritAverage", stats["CriticalDamage"]));

            stats.Add("DamageCounter", new CounterStat("DamageCounter"));
            stats.Add("DamageCounterPercent", new PercentStat("DamageCounterPercent", stats["DamageCounter"], stats["TotalDamageActionsUsed"]));
            stats.Add("DamageCounterReduction", new TotalStat("DamageCounterReduction"));
            stats.Add("DamageCounterReductionAverage", new AverageStat("DamageCounterReductionAverage", stats["DamageCounterReduction"]));
            stats.Add("DamageBlock", new CounterStat("DamageBlock"));
            stats.Add("DamageBlockPercent", new PercentStat("DamageBlockPercent", stats["DamageBlock"], stats["TotalDamageActionsUsed"]));
            stats.Add("DamageBlockReduction", new TotalStat("DamageBlockReduction"));
            stats.Add("DamageBlockReductionAverage", new AverageStat("DamageBlockReductionAverage", stats["DamageBlockReduction"]));
            stats.Add("DamageParry", new CounterStat("DamageParry"));
            stats.Add("DamageParryPercent", new PercentStat("DamageParryPercent", stats["DamageParry"], stats["TotalDamageActionsUsed"]));
            stats.Add("DamageParryReduction", new TotalStat("DamageParryReduction"));
            stats.Add("DamageParryReductionAverage", new AverageStat("DamageParryReductionAverage", stats["DamageParryReduction"]));
            stats.Add("DamageResist", new CounterStat("DamageResist"));
            stats.Add("DamageResistPercent", new PercentStat("DamageResistPercent", stats["DamageResist"], stats["TotalDamageActionsUsed"]));
            stats.Add("DamageResistReduction", new TotalStat("DamageResistReduction"));
            stats.Add("DamageResistReductionAverage", new AverageStat("DamageResistReductionAverage", stats["DamageResistReduction"]));
            stats.Add("DamageEvade", new CounterStat("DamageEvade"));
            stats.Add("DamageEvadePercent", new PercentStat("DamageEvadePercent", stats["DamageEvade"], stats["TotalDamageActionsUsed"]));
            stats.Add("DamageEvadeReduction", new TotalStat("DamageEvadeReduction"));
            stats.Add("DamageEvadeReductionAverage", new AverageStat("DamageEvadeReductionAverage", stats["DamageEvadeReduction"]));

            return stats;
        }

        private static Dictionary<string, Stat<decimal>> HealingStats()
        {
            var stats = new Dictionary<string, Stat<decimal>>();

            stats.Add("TotalOverallHealing", new TotalStat("TotalOverallHealing"));
            stats.Add("RegularHealing", new TotalStat("RegularHealing"));
            stats.Add("CriticalHealing", new TotalStat("CriticalHealing"));
            stats.Add("TotalHealingActionsUsed", new CounterStat("TotalHealingActionsUsed"));
            stats.Add("HPS", new PerSecondAverageStat("HPS", stats["TotalOverallHealing"]));
            stats.Add("HealingRegLow", new MinStat("HealingRegLow", stats["RegularHealing"]));
            stats.Add("HealingRegHigh", new MaxStat("HealingRegHigh", stats["RegularHealing"]));
            stats.Add("HealingRegAverage", new AverageStat("HealingRegAverage", stats["TotalOverallHealing"]));
            stats.Add("HealingCritLow", new MinStat("HealingCritLow", stats["CriticalHealing"]));
            stats.Add("HealingCritHigh", new MaxStat("HealingCritHigh", stats["CriticalHealing"]));
            stats.Add("HealingCritAverage", new AverageStat("HealingCritAverage", stats["CriticalHealing"]));

            return stats;
        }

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
            stats.Add("DamageTakenRegAverage", new AverageStat("DamageTakenRegAverage", stats["TotalOverallDamageTaken"]));
            stats.Add("DamageTakenCritHit", new TotalStat("DamageTakenCritHit"));
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

            return stats;
        }

        #endregion
    }
}
