// FFXIVAPP.Client
// Monster.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;
using System.Linq;
using System.Timers;
using FFXIVAPP.Client.Models.Parse.LinkedStats;
using FFXIVAPP.Client.Models.Parse.Stats;
using FFXIVAPP.Common.Core.Memory;
using NLog;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models.Parse.StatGroups
{
    [DoNotObfuscate]
    public partial class Monster : StatGroup
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        private static readonly IList<string> LD = new[]
        {
            "Counter", "Block", "Parry", "Resist", "Evade"
        };

        public readonly Timer StatusUpdateTimer = new Timer(1000);

        public List<StatusEntry> StatusEntriesMonsters = new List<StatusEntry>();
        public List<StatusEntry> StatusEntriesPlayers = new List<StatusEntry>();
        public List<StatusEntry> StatusEntriesSelf = new List<StatusEntry>();

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        public Monster(string name, ParseControl parseControl) : base(name)
        {
            Controller = parseControl;
            ID = 0;
            InitStats();
            LineHistory = new List<LineHistory>();
            StatusUpdateTimer.Elapsed += StatusUpdateTimerOnElapsed;
            if (!Controller.IsHistoryBased)
            {
                StatusUpdateTimer.Start();
            }
        }

        private ParseControl Controller { get; set; }

        public uint ID { get; set; }

        public ActorEntity NPCEntry { get; set; }

        public List<LineHistory> LineHistory { get; set; }

        private TotalStat TotalOverallDrops { get; set; }

        private CounterStat TotalKilled { get; set; }

        private void StatusUpdateTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            StatusEntriesSelf.Clear();
            StatusEntriesPlayers.Clear();
            StatusEntriesMonsters.Clear();
        }

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

            //setup monster ability stats
            foreach (var damageStat in StatGeneration.DamageStats(Controller.IsHistoryBased))
            {
                stats.Add(damageStat.Key, damageStat.Value);
            }

            foreach (var damageStat in StatGeneration.DamageOverTimeStats(Controller.IsHistoryBased))
            {
                stats.Add(damageStat.Key, damageStat.Value);
            }

            //setup monster healing stats
            foreach (var healingStat in StatGeneration.HealingStats(Controller.IsHistoryBased))
            {
                stats.Add(healingStat.Key, healingStat.Value);
            }

            foreach (var healingStat in StatGeneration.HealingOverHealingStats(Controller.IsHistoryBased))
            {
                stats.Add(healingStat.Key, healingStat.Value);
            }

            foreach (var healingStat in StatGeneration.HealingOverTimeStats(Controller.IsHistoryBased))
            {
                stats.Add(healingStat.Key, healingStat.Value);
            }

            foreach (var healingStat in StatGeneration.HealingMitigatedStats(Controller.IsHistoryBased))
            {
                stats.Add(healingStat.Key, healingStat.Value);
            }

            //setup monster damage taken stats
            foreach (var damageTakenStat in StatGeneration.DamageTakenStats(Controller.IsHistoryBased))
            {
                stats.Add(damageTakenStat.Key, damageTakenStat.Value);
            }

            foreach (var damageTakenStat in StatGeneration.DamageTakenOverTimeStats(Controller.IsHistoryBased))
            {
                stats.Add(damageTakenStat.Key, damageTakenStat.Value);
            }

            //setup combined stats
            foreach (var combinedStat in StatGeneration.CombinedStats(Controller.IsHistoryBased))
            {
                stats.Add(combinedStat.Key, combinedStat.Value);
            }

            //link to main monster stats
            var oStats = Controller.Timeline.Overall.Stats.ToDictionary(o => o.Name);

            #region Damage

            ((TotalStat) oStats["TotalOverallDamageMonster"]).AddDependency(stats["TotalOverallDamage"]);
            ((TotalStat) oStats["RegularDamageMonster"]).AddDependency(stats["RegularDamage"]);
            ((TotalStat) oStats["CriticalDamageMonster"]).AddDependency(stats["CriticalDamage"]);

            ((TotalStat) oStats["TotalOverallDamageOverTimeMonster"]).AddDependency(stats["TotalOverallDamageOverTime"]);
            ((TotalStat) oStats["RegularDamageOverTimeMonster"]).AddDependency(stats["RegularDamageOverTime"]);
            ((TotalStat) oStats["CriticalDamageOverTimeMonster"]).AddDependency(stats["CriticalDamageOverTime"]);

            #endregion

            #region Healing

            ((TotalStat) oStats["TotalOverallHealingMonster"]).AddDependency(stats["TotalOverallHealing"]);
            ((TotalStat) oStats["RegularHealingMonster"]).AddDependency(stats["RegularHealing"]);
            ((TotalStat) oStats["CriticalHealingMonster"]).AddDependency(stats["CriticalHealing"]);

            ((TotalStat) oStats["TotalOverallHealingOverHealingMonster"]).AddDependency(stats["TotalOverallHealingOverHealing"]);
            ((TotalStat) oStats["RegularHealingOverHealingMonster"]).AddDependency(stats["RegularHealingOverHealing"]);
            ((TotalStat) oStats["CriticalHealingOverHealingMonster"]).AddDependency(stats["CriticalHealingOverHealing"]);

            ((TotalStat) oStats["TotalOverallHealingOverTimeMonster"]).AddDependency(stats["TotalOverallHealingOverTime"]);
            ((TotalStat) oStats["RegularHealingOverTimeMonster"]).AddDependency(stats["RegularHealingOverTime"]);
            ((TotalStat) oStats["CriticalHealingOverTimeMonster"]).AddDependency(stats["CriticalHealingOverTime"]);

            ((TotalStat) oStats["TotalOverallHealingMitigatedMonster"]).AddDependency(stats["TotalOverallHealingMitigated"]);
            ((TotalStat) oStats["RegularHealingMitigatedMonster"]).AddDependency(stats["RegularHealingMitigated"]);
            ((TotalStat) oStats["CriticalHealingMitigatedMonster"]).AddDependency(stats["CriticalHealingMitigated"]);

            #endregion

            #region Damage Taken

            ((TotalStat) oStats["TotalOverallDamageTakenMonster"]).AddDependency(stats["TotalOverallDamageTaken"]);
            ((TotalStat) oStats["RegularDamageTakenMonster"]).AddDependency(stats["RegularDamageTaken"]);
            ((TotalStat) oStats["CriticalDamageTakenMonster"]).AddDependency(stats["CriticalDamageTaken"]);

            ((TotalStat) oStats["TotalOverallDamageTakenOverTimeMonster"]).AddDependency(stats["TotalOverallDamageTakenOverTime"]);
            ((TotalStat) oStats["RegularDamageTakenOverTimeMonster"]).AddDependency(stats["RegularDamageTakenOverTime"]);
            ((TotalStat) oStats["CriticalDamageTakenOverTimeMonster"]).AddDependency(stats["CriticalDamageTakenOverTime"]);

            #endregion

            #region Global Percent Of Total Stats

            stats.Add("PercentOfTotalOverallDamage", new PercentStat("PercentOfTotalOverallDamage", stats["TotalOverallDamage"], ((TotalStat) oStats["TotalOverallDamageMonster"])));
            stats.Add("PercentOfRegularDamage", new PercentStat("PercentOfRegularDamage", stats["RegularDamage"], ((TotalStat) oStats["RegularDamageMonster"])));
            stats.Add("PercentOfCriticalDamage", new PercentStat("PercentOfCriticalDamage", stats["CriticalDamage"], ((TotalStat) oStats["CriticalDamageMonster"])));

            stats.Add("PercentOfTotalOverallDamageOverTime", new PercentStat("PercentOfTotalOverallDamageOverTime", stats["TotalOverallDamageOverTime"], ((TotalStat) oStats["TotalOverallDamageOverTimeMonster"])));
            stats.Add("PercentOfRegularDamageOverTime", new PercentStat("PercentOfRegularDamageOverTime", stats["RegularDamageOverTime"], ((TotalStat) oStats["RegularDamageOverTimeMonster"])));
            stats.Add("PercentOfCriticalDamageOverTime", new PercentStat("PercentOfCriticalDamageOverTime", stats["CriticalDamageOverTime"], ((TotalStat) oStats["CriticalDamageOverTimeMonster"])));

            // healing
            stats.Add("PercentOfTotalOverallHealing", new PercentStat("PercentOfTotalOverallHealing", stats["TotalOverallHealing"], ((TotalStat) oStats["TotalOverallHealingMonster"])));
            stats.Add("PercentOfRegularHealing", new PercentStat("PercentOfRegularHealing", stats["RegularHealing"], ((TotalStat) oStats["RegularHealingMonster"])));
            stats.Add("PercentOfCriticalHealing", new PercentStat("PercentOfCriticalHealing", stats["CriticalHealing"], ((TotalStat) oStats["CriticalHealingMonster"])));

            stats.Add("PercentOfTotalOverallHealingOverHealing", new PercentStat("PercentOfTotalOverallHealingOverHealing", stats["TotalOverallHealingOverHealing"], ((TotalStat) oStats["TotalOverallHealingOverHealingMonster"])));
            stats.Add("PercentOfRegularHealingOverHealing", new PercentStat("PercentOfRegularHealingOverHealing", stats["RegularHealingOverHealing"], ((TotalStat) oStats["RegularHealingOverHealingMonster"])));
            stats.Add("PercentOfCriticalHealingOverHealing", new PercentStat("PercentOfCriticalHealingOverHealing", stats["CriticalHealingOverHealing"], ((TotalStat) oStats["CriticalHealingOverHealingMonster"])));

            stats.Add("PercentOfTotalOverallHealingOverTime", new PercentStat("PercentOfTotalOverallHealingOverTime", stats["TotalOverallHealingOverTime"], ((TotalStat) oStats["TotalOverallHealingOverTimeMonster"])));
            stats.Add("PercentOfRegularHealingOverTime", new PercentStat("PercentOfRegularHealingOverTime", stats["RegularHealingOverTime"], ((TotalStat) oStats["RegularHealingOverTimeMonster"])));
            stats.Add("PercentOfCriticalHealingOverTime", new PercentStat("PercentOfCriticalHealingOverTime", stats["CriticalHealingOverTime"], ((TotalStat) oStats["CriticalHealingOverTimeMonster"])));

            stats.Add("PercentOfTotalOverallHealingMitigated", new PercentStat("PercentOfTotalOverallHealingMitigated", stats["TotalOverallHealingMitigated"], ((TotalStat) oStats["TotalOverallHealingMitigatedMonster"])));
            stats.Add("PercentOfRegularHealingMitigated", new PercentStat("PercentOfRegularHealingMitigated", stats["RegularHealingMitigated"], ((TotalStat) oStats["RegularHealingMitigatedMonster"])));
            stats.Add("PercentOfCriticalHealingMitigated", new PercentStat("PercentOfCriticalHealingMitigated", stats["CriticalHealingMitigated"], ((TotalStat) oStats["CriticalHealingMitigatedMonster"])));

            // damage taken
            stats.Add("PercentOfTotalOverallDamageTaken", new PercentStat("PercentOfTotalOverallDamageTaken", stats["TotalOverallDamageTaken"], ((TotalStat) oStats["TotalOverallDamageTakenMonster"])));
            stats.Add("PercentOfRegularDamageTaken", new PercentStat("PercentOfRegularDamageTaken", stats["RegularDamageTaken"], ((TotalStat) oStats["RegularDamageTakenMonster"])));
            stats.Add("PercentOfCriticalDamageTaken", new PercentStat("PercentOfCriticalDamageTaken", stats["CriticalDamageTaken"], ((TotalStat) oStats["CriticalDamageTakenMonster"])));

            stats.Add("PercentOfTotalOverallDamageTakenOverTime", new PercentStat("PercentOfTotalOverallDamageTakenOverTime", stats["TotalOverallDamageTakenOverTime"], ((TotalStat) oStats["TotalOverallDamageTakenOverTimeMonster"])));
            stats.Add("PercentOfRegularDamageTakenOverTime", new PercentStat("PercentOfRegularDamageTakenOverTime", stats["RegularDamageTakenOverTime"], ((TotalStat) oStats["RegularDamageTakenOverTimeMonster"])));
            stats.Add("PercentOfCriticalDamageTakenOverTime", new PercentStat("PercentOfCriticalDamageTakenOverTime", stats["CriticalDamageTakenOverTime"], ((TotalStat) oStats["CriticalDamageTakenOverTimeMonster"])));

            #endregion

            #region Monster Combined

            ((TotalStat) stats["CombinedTotalOverallDamage"]).AddDependency(stats["TotalOverallDamage"]);
            ((TotalStat) stats["CombinedTotalOverallDamage"]).AddDependency(stats["TotalOverallDamageOverTime"]);
            ((TotalStat) stats["CombinedCriticalDamage"]).AddDependency(stats["CriticalDamage"]);
            ((TotalStat) stats["CombinedRegularDamage"]).AddDependency(stats["RegularDamage"]);

            ((TotalStat) stats["CombinedTotalOverallHealing"]).AddDependency(stats["TotalOverallHealing"]);
            ((TotalStat) stats["CombinedTotalOverallHealing"]).AddDependency(stats["TotalOverallHealingOverTime"]);
            ((TotalStat) stats["CombinedTotalOverallHealing"]).AddDependency(stats["TotalOverallHealingMitigated"]);
            ((TotalStat) stats["CombinedCriticalHealing"]).AddDependency(stats["CriticalHealing"]);
            ((TotalStat) stats["CombinedRegularHealing"]).AddDependency(stats["RegularHealing"]);

            ((TotalStat) stats["CombinedTotalOverallDamageTaken"]).AddDependency(stats["TotalOverallDamageTaken"]);
            ((TotalStat) stats["CombinedTotalOverallDamageTaken"]).AddDependency(stats["TotalOverallDamageTakenOverTime"]);
            ((TotalStat) stats["CombinedCriticalDamageTaken"]).AddDependency(stats["CriticalDamageTaken"]);
            ((TotalStat) stats["CombinedRegularDamageTaken"]).AddDependency(stats["RegularDamageTaken"]);

            #endregion

            #region Global Combined

            ((TotalStat) oStats["CombinedTotalOverallDamageMonster"]).AddDependency(stats["CombinedTotalOverallDamage"]);
            ((TotalStat) oStats["CombinedRegularDamageMonster"]).AddDependency(stats["CombinedRegularDamage"]);
            ((TotalStat) oStats["CombinedCriticalDamageMonster"]).AddDependency(stats["CombinedCriticalDamage"]);

            ((TotalStat) oStats["CombinedTotalOverallHealingMonster"]).AddDependency(stats["CombinedTotalOverallHealing"]);
            ((TotalStat) oStats["CombinedRegularHealingMonster"]).AddDependency(stats["CombinedRegularHealing"]);
            ((TotalStat) oStats["CombinedCriticalHealingMonster"]).AddDependency(stats["CombinedCriticalHealing"]);

            ((TotalStat) oStats["CombinedTotalOverallDamageTakenMonster"]).AddDependency(stats["CombinedTotalOverallDamageTaken"]);
            ((TotalStat) oStats["CombinedRegularDamageTakenMonster"]).AddDependency(stats["CombinedRegularDamageTaken"]);
            ((TotalStat) oStats["CombinedCriticalDamageTakenMonster"]).AddDependency(stats["CombinedCriticalDamageTaken"]);

            #endregion

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
            var stats = StatGeneration.DamageStats(Controller.IsHistoryBased);

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
        /// <param name="sub"> </param>
        /// <param name="useSub"></param>
        /// <returns> </returns>
        private IEnumerable<Stat<decimal>> DamageOverTimeStatList(StatGroup sub, bool useSub = false)
        {
            var stats = StatGeneration.DamageOverTimeStats(Controller.IsHistoryBased);

            //setup per ability "percent of" stats
            switch (useSub)
            {
                case true:
                    stats.Add("PercentOfTotalOverallDamageOverTime", new PercentStat("PercentOfTotalOverallDamageOverTime", stats["TotalOverallDamageOverTime"], sub.Stats.GetStat("TotalOverallDamageOverTime")));
                    stats.Add("PercentOfRegularDamageOverTime", new PercentStat("PercentOfRegularDamageOverTime", stats["RegularDamageOverTime"], sub.Stats.GetStat("RegularDamageOverTime")));
                    stats.Add("PercentOfCriticalDamageOverTime", new PercentStat("PercentOfCriticalDamageOverTime", stats["CriticalDamageOverTime"], sub.Stats.GetStat("CriticalDamageOverTime")));
                    break;
                case false:
                    stats.Add("PercentOfTotalOverallDamageOverTime", new PercentStat("PercentOfTotalOverallDamageOverTime", stats["TotalOverallDamageOverTime"], Stats.GetStat("TotalOverallDamageOverTime")));
                    stats.Add("PercentOfRegularDamageOverTime", new PercentStat("PercentOfRegularDamageOverTime", stats["RegularDamageOverTime"], Stats.GetStat("RegularDamageOverTime")));
                    stats.Add("PercentOfCriticalDamageOverTime", new PercentStat("PercentOfCriticalDamageOverTime", stats["CriticalDamageOverTime"], Stats.GetStat("CriticalDamageOverTime")));
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
            var stats = StatGeneration.HealingStats(Controller.IsHistoryBased);

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
        private IEnumerable<Stat<decimal>> HealingOverHealingStatList(StatGroup sub, bool useSub = false)
        {
            var stats = StatGeneration.HealingOverHealingStats(Controller.IsHistoryBased);

            //setup per HealingOverHealing "percent of" stats
            switch (useSub)
            {
                case true:
                    stats.Add("PercentOfTotalOverallHealingOverHealing", new PercentStat("PercentOfTotalOverallHealingOverHealing", stats["TotalOverallHealingOverHealing"], sub.Stats.GetStat("TotalOverallHealingOverHealing")));
                    stats.Add("PercentOfRegularHealingOverHealing", new PercentStat("PercentOfRegularHealingOverHealing", stats["RegularHealingOverHealing"], sub.Stats.GetStat("RegularHealingOverHealing")));
                    stats.Add("PercentOfCriticalHealingOverHealing", new PercentStat("PercentOfCriticalHealingOverHealing", stats["CriticalHealingOverHealing"], sub.Stats.GetStat("CriticalHealingOverHealing")));
                    break;
                case false:
                    stats.Add("PercentOfTotalOverallHealingOverHealing", new PercentStat("PercentOfTotalOverallHealingOverHealing", stats["TotalOverallHealingOverHealing"], Stats.GetStat("TotalOverallHealingOverHealing")));
                    stats.Add("PercentOfRegularHealingOverHealing", new PercentStat("PercentOfRegularHealingOverHealing", stats["RegularHealingOverHealing"], Stats.GetStat("RegularHealingOverHealing")));
                    stats.Add("PercentOfCriticalHealingOverHealing", new PercentStat("PercentOfCriticalHealingOverHealing", stats["CriticalHealingOverHealing"], Stats.GetStat("CriticalHealingOverHealing")));
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
        private IEnumerable<Stat<decimal>> HealingOverTimeStatList(StatGroup sub, bool useSub = false)
        {
            var stats = StatGeneration.HealingOverTimeStats(Controller.IsHistoryBased);

            //setup per HealingOverTime "percent of" stats
            switch (useSub)
            {
                case true:
                    stats.Add("PercentOfTotalOverallHealingOverTime", new PercentStat("PercentOfTotalOverallHealingOverTime", stats["TotalOverallHealingOverTime"], sub.Stats.GetStat("TotalOverallHealingOverTime")));
                    stats.Add("PercentOfRegularHealingOverTime", new PercentStat("PercentOfRegularHealingOverTime", stats["RegularHealingOverTime"], sub.Stats.GetStat("RegularHealingOverTime")));
                    stats.Add("PercentOfCriticalHealingOverTime", new PercentStat("PercentOfCriticalHealingOverTime", stats["CriticalHealingOverTime"], sub.Stats.GetStat("CriticalHealingOverTime")));
                    break;
                case false:
                    stats.Add("PercentOfTotalOverallHealingOverTime", new PercentStat("PercentOfTotalOverallHealingOverTime", stats["TotalOverallHealingOverTime"], Stats.GetStat("TotalOverallHealingOverTime")));
                    stats.Add("PercentOfRegularHealingOverTime", new PercentStat("PercentOfRegularHealingOverTime", stats["RegularHealingOverTime"], Stats.GetStat("RegularHealingOverTime")));
                    stats.Add("PercentOfCriticalHealingOverTime", new PercentStat("PercentOfCriticalHealingOverTime", stats["CriticalHealingOverTime"], Stats.GetStat("CriticalHealingOverTime")));
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
        private IEnumerable<Stat<decimal>> HealingMitigatedStatList(StatGroup sub, bool useSub = false)
        {
            var stats = StatGeneration.HealingMitigatedStats(Controller.IsHistoryBased);

            //setup per HealingMitigated "percent of" stats
            switch (useSub)
            {
                case true:
                    stats.Add("PercentOfTotalOverallHealingMitigated", new PercentStat("PercentOfTotalOverallHealingMitigated", stats["TotalOverallHealingMitigated"], sub.Stats.GetStat("TotalOverallHealingMitigated")));
                    stats.Add("PercentOfRegularHealingMitigated", new PercentStat("PercentOfRegularHealingMitigated", stats["RegularHealingMitigated"], sub.Stats.GetStat("RegularHealingMitigated")));
                    stats.Add("PercentOfCriticalHealingMitigated", new PercentStat("PercentOfCriticalHealingMitigated", stats["CriticalHealingMitigated"], sub.Stats.GetStat("CriticalHealingMitigated")));
                    break;
                case false:
                    stats.Add("PercentOfTotalOverallHealingMitigated", new PercentStat("PercentOfTotalOverallHealingMitigated", stats["TotalOverallHealingMitigated"], Stats.GetStat("TotalOverallHealingMitigated")));
                    stats.Add("PercentOfRegularHealingMitigated", new PercentStat("PercentOfRegularHealingMitigated", stats["RegularHealingMitigated"], Stats.GetStat("RegularHealingMitigated")));
                    stats.Add("PercentOfCriticalHealingMitigated", new PercentStat("PercentOfCriticalHealingMitigated", stats["CriticalHealingMitigated"], Stats.GetStat("CriticalHealingMitigated")));
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
            var stats = StatGeneration.DamageTakenStats(Controller.IsHistoryBased);

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
        /// <param name="sub"></param>
        /// <param name="useSub"></param>
        /// <returns></returns>
        private IEnumerable<Stat<decimal>> DamageTakenOverTimeStatList(StatGroup sub, bool useSub = false)
        {
            var stats = StatGeneration.DamageTakenOverTimeStats(Controller.IsHistoryBased);

            //setup per damage taken "percent of" stats
            switch (useSub)
            {
                case true:
                    stats.Add("PercentOfTotalOverallDamageTakenOverTime", new PercentStat("PercentOfTotalOverallDamageTakenOverTime", stats["TotalOverallDamageTakenOverTime"], sub.Stats.GetStat("TotalOverallDamageTakenOverTime")));
                    stats.Add("PercentOfRegularDamageTakenOverTime", new PercentStat("PercentOfRegularDamageTakenOverTime", stats["RegularDamageTakenOverTime"], sub.Stats.GetStat("RegularDamageTakenOverTime")));
                    stats.Add("PercentOfCriticalDamageTakenOverTime", new PercentStat("PercentOfCriticalDamageTakenOverTime", stats["CriticalDamageTakenOverTime"], sub.Stats.GetStat("CriticalDamageTakenOverTime")));
                    break;
                case false:
                    stats.Add("PercentOfTotalOverallDamageTakenOverTime", new PercentStat("PercentOfTotalOverallDamageTakenOverTime", stats["TotalOverallDamageTakenOverTime"], Stats.GetStat("TotalOverallDamageTakenOverTime")));
                    stats.Add("PercentOfRegularDamageTakenOverTime", new PercentStat("PercentOfRegularDamageTakenOverTime", stats["RegularDamageTakenOverTime"], Stats.GetStat("RegularDamageTakenOverTime")));
                    stats.Add("PercentOfCriticalDamageTakenOverTime", new PercentStat("PercentOfCriticalDamageTakenOverTime", stats["CriticalDamageTakenOverTime"], Stats.GetStat("CriticalDamageTakenOverTime")));
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
    }
}
