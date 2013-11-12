// FFXIVAPP.Client
// Monster.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Collections.Generic;
using System.Linq;
using FFXIVAPP.Client.Plugins.Parse.Models.LinkedStats;
using FFXIVAPP.Client.Plugins.Parse.Models.Stats;
using FFXIVAPP.Client.Plugins.Parse.Monitors;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Plugins.Parse.Models.StatGroups
{
    [DoNotObfuscate]
    public partial class Monster : StatGroup
    {
        private static readonly IList<string> LD = new[]
        {
            "Counter", "Block", "Parry", "Resist", "Evade"
        };

        public Dictionary<string, decimal> LastDamageAmountByAction = new Dictionary<string, decimal>();

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

            //link to main monster stats
            StatMonitor.TotalOverallDamageMonster.AddDependency(stats["TotalOverallDamage"]);
            StatMonitor.RegularDamageMonster.AddDependency(stats["RegularDamage"]);
            StatMonitor.CriticalDamageMonster.AddDependency(stats["CriticalDamage"]);
            StatMonitor.TotalOverallHealingMonster.AddDependency(stats["TotalOverallHealing"]);
            StatMonitor.RegularHealingMonster.AddDependency(stats["RegularHealing"]);
            StatMonitor.CriticalHealingMonster.AddDependency(stats["CriticalHealing"]);
            StatMonitor.TotalOverallDamageTakenMonster.AddDependency(stats["TotalOverallDamageTaken"]);
            StatMonitor.RegularDamageTakenMonster.AddDependency(stats["RegularDamageTaken"]);
            StatMonitor.CriticalDamageTakenMonster.AddDependency(stats["CriticalDamageTaken"]);

            //setup global "percent of" stats
            stats.Add("PercentOfTotalOverallDamage", new PercentStat("PercentOfTotalOverallDamage", stats["TotalOverallDamage"], StatMonitor.TotalOverallDamageMonster));
            stats.Add("PercentOfRegularDamage", new PercentStat("PercentOfRegularDamage", stats["RegularDamage"], StatMonitor.RegularDamageMonster));
            stats.Add("PercentOfCriticalDamage", new PercentStat("PercentOfCriticalDamage", stats["CriticalDamage"], StatMonitor.CriticalDamageMonster));
            stats.Add("PercentOfTotalOverallHealing", new PercentStat("PercentOfTotalOverallHealing", stats["TotalOverallHealing"], StatMonitor.TotalOverallHealingMonster));
            stats.Add("PercentOfRegularHealing", new PercentStat("PercentOfRegularHealing", stats["RegularHealing"], StatMonitor.RegularHealingMonster));
            stats.Add("PercentOfCriticalHealing", new PercentStat("PercentOfCriticalHealing", stats["CriticalHealing"], StatMonitor.CriticalHealingMonster));
            stats.Add("PercentOfTotalOverallDamageTaken", new PercentStat("PercentOfTotalOverallDamageTaken", stats["TotalOverallDamageTaken"], StatMonitor.TotalOverallDamageTakenMonster));
            stats.Add("PercentOfRegularDamageTaken", new PercentStat("PercentOfRegularDamageTaken", stats["RegularDamageTaken"], StatMonitor.RegularDamageTakenMonster));
            stats.Add("PercentOfCriticalDamageTaken", new PercentStat("PercentOfCriticalDamageTaken", stats["CriticalDamageTaken"], StatMonitor.CriticalDamageTakenMonster));

            return stats.Select(s => s.Value)
                        .ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="sub"> </param>
        /// <param name="useSub"></param>
        /// <returns> </returns>
        private IEnumerable<Stat<decimal>> DamageStatList(StatGroup sub, bool useSub = false)
        {
            var stats = DamageStats();

            //setup per ability "percent of" stats
            switch (useSub)
            {
                case true:
                    stats.Add("PercentOfTotalOverallDamage", new PercentStat("PercentOfTotalOverallDamage", stats["TotalOverallDamage"], sub.Stats.GetStat("TotalOverallDamage")));
                    stats.Add("PercentOfRegularDamage", new PercentStat("PercentOfRegularDamage", stats["RegularDamage"], sub.Stats.GetStat("RegularDamage")));
                    stats.Add("PercentOfCriticalDamage", new PercentStat("PercentOfCriticalDamage", stats["CriticalDamage"], sub.Stats.GetStat("CriticalDamage")));
                    break;
                case false:
                    stats.Add("PercentOfTotalOverallDamage", new PercentStat("PercentOfTotalOverallDamage", stats["TotalOverallDamage"], Stats.GetStat("TotalOverallDamage")));
                    stats.Add("PercentOfRegularDamage", new PercentStat("PercentOfRegularDamage", stats["RegularDamage"], Stats.GetStat("RegularDamage")));
                    stats.Add("PercentOfCriticalDamage", new PercentStat("PercentOfCriticalDamage", stats["CriticalDamage"], Stats.GetStat("CriticalDamage")));
                    break;
            }

            return stats.Select(s => s.Value)
                        .ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="sub"></param>
        /// <param name="useSub"></param>
        /// <returns></returns>
        private IEnumerable<Stat<decimal>> HealingStatList(StatGroup sub, bool useSub = false)
        {
            var stats = HealingStats();

            //setup per healing "percent of" stats
            switch (useSub)
            {
                case true:
                    stats.Add("PercentOfTotalOverallHealing", new PercentStat("PercentOfTotalOverallHealing", stats["TotalOverallHealing"], sub.Stats.GetStat("TotalOverallHealing")));
                    stats.Add("PercentOfRegularHealing", new PercentStat("PercentOfRegularHealing", stats["RegularHealing"], sub.Stats.GetStat("RegularHealing")));
                    stats.Add("PercentOfCriticalHealing", new PercentStat("PercentOfCriticalHealing", stats["CriticalHealing"], sub.Stats.GetStat("CriticalHealing")));
                    break;
                case false:
                    stats.Add("PercentOfTotalOverallHealing", new PercentStat("PercentOfTotalOverallHealing", stats["TotalOverallHealing"], Stats.GetStat("TotalOverallHealing")));
                    stats.Add("PercentOfRegularHealing", new PercentStat("PercentOfRegularHealing", stats["RegularHealing"], Stats.GetStat("RegularHealing")));
                    stats.Add("PercentOfCriticalHealing", new PercentStat("PercentOfCriticalHealing", stats["CriticalHealing"], Stats.GetStat("CriticalHealing")));
                    break;
            }

            return stats.Select(s => s.Value)
                        .ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="sub"></param>
        /// <param name="useSub"></param>
        /// <returns></returns>
        private IEnumerable<Stat<decimal>> DamageTakenStatList(StatGroup sub, bool useSub = false)
        {
            var stats = DamageTakenStats();

            //setup per damage taken "percent of" stats
            switch (useSub)
            {
                case true:
                    stats.Add("PercentOfTotalOverallDamageTaken", new PercentStat("PercentOfTotalOverallDamageTaken", stats["TotalOverallDamageTaken"], sub.Stats.GetStat("TotalOverallDamageTaken")));
                    stats.Add("PercentOfRegularDamageTaken", new PercentStat("PercentOfRegularDamageTaken", stats["RegularDamageTaken"], sub.Stats.GetStat("RegularDamageTaken")));
                    stats.Add("PercentOfCriticalDamageTaken", new PercentStat("PercentOfCriticalDamageTaken", stats["CriticalDamageTaken"], sub.Stats.GetStat("CriticalDamageTaken")));
                    break;
                case false:
                    stats.Add("PercentOfTotalOverallDamageTaken", new PercentStat("PercentOfTotalOverallDamageTaken", stats["TotalOverallDamageTaken"], Stats.GetStat("TotalOverallDamageTaken")));
                    stats.Add("PercentOfRegularDamageTaken", new PercentStat("PercentOfRegularDamageTaken", stats["RegularDamageTaken"], Stats.GetStat("RegularDamageTaken")));
                    stats.Add("PercentOfCriticalDamageTaken", new PercentStat("PercentOfCriticalDamageTaken", stats["CriticalDamageTaken"], Stats.GetStat("CriticalDamageTaken")));
                    break;
            }

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

        private static Dictionary<string, Stat<decimal>> DamageStats()
        {
            var stats = new Dictionary<string, Stat<decimal>>();

            stats.Add("TotalOverallDamage", new TotalStat("TotalOverallDamage"));
            stats.Add("RegularDamage", new TotalStat("RegularDamage"));
            stats.Add("CriticalDamage", new TotalStat("CriticalDamage"));
            stats.Add("TotalDamageActionsUsed", new CounterStat("TotalDamageActionsUsed"));
            stats.Add("DPS", new PerSecondAverageStat("DPS", stats["TotalOverallDamage"]));
            stats.Add("DamageDOT", new TotalStat("DamageDOT"));
            stats.Add("DamageDOTAverage", new AverageStat("DamageDOTAverage", stats["DamageDOT"]));
            stats.Add("DamageRegHit", new TotalStat("DamageRegHit"));
            stats.Add("DamageRegMiss", new TotalStat("DamageRegMiss"));
            stats.Add("DamageRegAccuracy", new AccuracyStat("DamageRegAccuracy", stats["TotalDamageActionsUsed"], stats["DamageRegMiss"]));
            stats.Add("DamageRegLow", new MinStat("DamageRegLow", stats["RegularDamage"]));
            stats.Add("DamageRegHigh", new MaxStat("DamageRegHigh", stats["RegularDamage"]));
            stats.Add("DamageRegAverage", new AverageStat("DamageRegAverage", stats["RegularDamage"]));
            stats.Add("DamageRegMod", new TotalStat("DamageRegMod"));
            stats.Add("DamageRegModAverage", new AverageStat("DamageRegModAverage", stats["DamageRegMod"]));
            stats.Add("DamageCritHit", new TotalStat("DamageCritHit"));
            stats.Add("DamageCritPercent", new PercentStat("DamageCritPercent", stats["DamageCritHit"], stats["TotalDamageActionsUsed"]));
            stats.Add("DamageCritLow", new MinStat("DamageCritLow", stats["CriticalDamage"]));
            stats.Add("DamageCritHigh", new MaxStat("DamageCritHigh", stats["CriticalDamage"]));
            stats.Add("DamageCritAverage", new AverageStat("DamageCritAverage", stats["CriticalDamage"]));
            stats.Add("DamageCritMod", new TotalStat("DamageCritMod"));
            stats.Add("DamageCritModAverage", new AverageStat("DamageCritModAverage", stats["DamageCritMod"]));

            stats.Add("DamageCounter", new CounterStat("DamageCounter"));
            stats.Add("DamageCounterPercent", new PercentStat("DamageCounterPercent", stats["DamageCounter"], stats["TotalDamageActionsUsed"]));
            stats.Add("DamageCounterMod", new TotalStat("DamageCounterMod"));
            stats.Add("DamageCounterModAverage", new AverageStat("DamageCounterModAverage", stats["DamageCounterMod"]));
            stats.Add("DamageBlock", new CounterStat("DamageBlock"));
            stats.Add("DamageBlockPercent", new PercentStat("DamageBlockPercent", stats["DamageBlock"], stats["TotalDamageActionsUsed"]));
            stats.Add("DamageBlockMod", new TotalStat("DamageBlockMod"));
            stats.Add("DamageBlockModAverage", new AverageStat("DamageBlockModAverage", stats["DamageBlockMod"]));
            stats.Add("DamageParry", new CounterStat("DamageParry"));
            stats.Add("DamageParryPercent", new PercentStat("DamageParryPercent", stats["DamageParry"], stats["TotalDamageActionsUsed"]));
            stats.Add("DamageParryMod", new TotalStat("DamageParryMod"));
            stats.Add("DamageParryModAverage", new AverageStat("DamageParryModAverage", stats["DamageParryMod"]));
            stats.Add("DamageResist", new CounterStat("DamageResist"));
            stats.Add("DamageResistPercent", new PercentStat("DamageResistPercent", stats["DamageResist"], stats["TotalDamageActionsUsed"]));
            stats.Add("DamageResistMod", new TotalStat("DamageResistMod"));
            stats.Add("DamageResistModAverage", new AverageStat("DamageResistModAverage", stats["DamageResistMod"]));
            stats.Add("DamageEvade", new CounterStat("DamageEvade"));
            stats.Add("DamageEvadePercent", new PercentStat("DamageEvadePercent", stats["DamageEvade"], stats["TotalDamageActionsUsed"]));
            stats.Add("DamageEvadeMod", new TotalStat("DamageEvadeMod"));
            stats.Add("DamageEvadeModAverage", new AverageStat("DamageEvadeModAverage", stats["DamageEvadeMod"]));

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
            stats.Add("HealingRegHit", new TotalStat("HealingRegHit"));
            stats.Add("HealingRegLow", new MinStat("HealingRegLow", stats["RegularHealing"]));
            stats.Add("HealingRegHigh", new MaxStat("HealingRegHigh", stats["RegularHealing"]));
            stats.Add("HealingRegAverage", new AverageStat("HealingRegAverage", stats["RegularHealing"]));
            stats.Add("HealingRegMod", new TotalStat("HealingRegMod"));
            stats.Add("HealingRegModAverage", new AverageStat("HealingRegModAverage", stats["HealingRegMod"]));
            stats.Add("HealingCritHit", new TotalStat("HealingCritHit"));
            stats.Add("HealingCritPercent", new PercentStat("HealingCritPercent", stats["HealingCritHit"], stats["TotalHealingActionsUsed"]));
            stats.Add("HealingCritLow", new MinStat("HealingCritLow", stats["CriticalHealing"]));
            stats.Add("HealingCritHigh", new MaxStat("HealingCritHigh", stats["CriticalHealing"]));
            stats.Add("HealingCritAverage", new AverageStat("HealingCritAverage", stats["CriticalHealing"]));
            stats.Add("HealingCritMod", new TotalStat("HealingCritMod"));
            stats.Add("HealingCritModAverage", new AverageStat("HealingCritModAverage", stats["HealingCritMod"]));

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
            stats.Add("DamageTakenDOT", new TotalStat("DamageTakenDOT"));
            stats.Add("DamageTakenDOTAverage", new AverageStat("DamageTakenDOTAverage", stats["DamageTakenDOT"]));
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
