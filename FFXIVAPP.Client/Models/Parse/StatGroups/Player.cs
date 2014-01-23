// FFXIVAPP.Client
// Player.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Timers;
using FFXIVAPP.Client.Delegates;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Models.Parse.LinkedStats;
using FFXIVAPP.Client.Models.Parse.Stats;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Core.Memory;
using NLog;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models.Parse.StatGroups
{
    [DoNotObfuscate]
    public partial class Player : StatGroup
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        private static readonly IList<string> LD = new[]
        {
            "Counter", "Block", "Parry", "Resist", "Evade"
        };

        public readonly Timer IsActiveTimer = new Timer(1000);
        public readonly Timer StatusUpdateTimer = new Timer(1000);

        public List<StatusEntry> StatusEntriesMonsters = new List<StatusEntry>();
        public List<StatusEntry> StatusEntriesPlayers = new List<StatusEntry>();
        public List<StatusEntry> StatusEntriesSelf = new List<StatusEntry>();

        public Player(string name, ParseControl parseControl) : base(name)
        {
            Controller = parseControl;
            ID = 0;
            InitStats();
            LineHistory = new List<LineHistory>();
            Last20DamageActions = new List<LineHistory>();
            Last20DamageTakenActions = new List<LineHistory>();
            Last20HealingActions = new List<LineHistory>();
            if (Controller.IsHistoryBased)
            {
                return;
            }
            StatusUpdateTimer.Elapsed += StatusUpdateTimerOnElapsed;
            IsActiveTimer.Elapsed += IsActiveTimerOnElapsed;
            LastActionTime = DateTime.Now;
            StatusUpdateTimer.Start();
            IsActiveTimer.Start();
        }

        public DateTime LastActionTime { get; set; }
        public double TotalInActiveTime { get; set; }

        public bool StatusUpdateTimerProcessing { get; set; }

        private static ParseControl Controller { get; set; }

        public uint ID { get; set; }

        public ActorEntity NPCEntry { get; set; }

        public List<LineHistory> LineHistory { get; set; }
        public List<LineHistory> Last20DamageActions { get; set; }
        public List<LineHistory> Last20DamageTakenActions { get; set; }
        public List<LineHistory> Last20HealingActions { get; set; }

        private void StatusUpdateTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (StatusUpdateTimerProcessing)
            {
                return;
            }
            StatusUpdateTimerProcessing = true;
            var monsterEntries = MonsterWorkerDelegate.GetNPCEntities();
            var pcEntries = PCWorkerDelegate.GetNPCEntities();
            StatusEntriesSelf.Clear();
            StatusEntriesPlayers.Clear();
            StatusEntriesMonsters.Clear();
            if (pcEntries.Any())
            {
                try
                {
                    var cleanedName = Regex.Replace(Name, @"\[[\w]+\]", "")
                                           .Trim();
                    var isYou = Regex.IsMatch(cleanedName, @"^(([Dd](ich|ie|u))|You|Vous)$") || String.Equals(cleanedName, Settings.Default.CharacterName, Constants.InvariantComparer);
                    var isPet = false;
                    try
                    {
                        NPCEntry = isYou ? AppContextHelper.Instance.CurrentUser : null;
                        if (!isYou)
                        {
                            try
                            {
                                NPCEntry = pcEntries.First(p => String.Equals(p.Name, cleanedName, Constants.InvariantComparer));
                            }
                            catch (Exception ex)
                            {
                                isPet = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    if (NPCEntry != null)
                    {
                        ID = NPCEntry.ID;
                        if (ID > 0)
                        {
                            StatusEntriesSelf = NPCEntry.StatusEntries;
                            try
                            {
                                foreach (var statusEntry in
                                    monsterEntries.SelectMany(monster => monster.StatusEntries)
                                                  .Where(statusEntry => statusEntry.CasterID == ID))
                                {
                                    StatusEntriesMonsters.Add(statusEntry);
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                            try
                            {
                                foreach (var statusEntry in
                                    pcEntries.SelectMany(pc => pc.StatusEntries)
                                             .Where(statusEntry => statusEntry.CasterID == ID))
                                {
                                    StatusEntriesPlayers.Add(statusEntry);
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            if (!StatusEntriesMonsters.Any() && !StatusEntriesPlayers.Any())
            {
                StatusUpdateTimerProcessing = false;
                return;
            }
            if (StatusEntriesMonsters.Any())
            {
                ProcessDamageOverTime(StatusEntriesMonsters);
            }
            if (StatusEntriesPlayers.Any())
            {
                ProcessHealingOverTime(StatusEntriesPlayers);
                ProcessBuffs(StatusEntriesPlayers);
            }
            StatusUpdateTimerProcessing = false;
        }

        private void IsActiveTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            try
            {
                if (!Controller.Timeline.FightingRightNow)
                {
                    return;
                }
                Stats.GetStat("TotalParserTime")
                     .Value = Convert.ToDecimal(Controller.EndTime.Subtract(Controller.StartTime)
                                                          .TotalSeconds);
                var parserTime = Stats.GetStat("TotalParserTime");
                var activeTime = Stats.GetStat("TotalActiveTime");
                var inactiveTime = DateTime.Now.Subtract(LastActionTime)
                                           .TotalSeconds;
                if (inactiveTime > 5)
                {
                    TotalInActiveTime++;
                }
                TotalInActiveTime = TotalInActiveTime > (double) parserTime.Value ? (double) parserTime.Value : TotalInActiveTime;
                activeTime.Value = parserTime.Value - (decimal) TotalInActiveTime;
            }
            catch (Exception ex)
            {
            }
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

            stats.Add("TotalActiveTime", new TotalStat("TotalActiveTime"));
            stats.Add("TotalParserTime", new TotalStat("TotalParserTime"));

            //setup player ability stats
            foreach (var damageStat in StatGeneration.DamageStats(Controller.IsHistoryBased))
            {
                stats.Add(damageStat.Key, damageStat.Value);
            }

            foreach (var damageStat in StatGeneration.DamageOverTimeStats(Controller.IsHistoryBased))
            {
                stats.Add(damageStat.Key, damageStat.Value);
            }

            //setup player healing stats
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

            //setup player damage taken stats
            foreach (var damageTakenStat in StatGeneration.DamageTakenStats(Controller.IsHistoryBased))
            {
                stats.Add(damageTakenStat.Key, damageTakenStat.Value);
            }

            foreach (var damageTakenStat in StatGeneration.DamageTakenOverTimeStats(Controller.IsHistoryBased))
            {
                stats.Add(damageTakenStat.Key, damageTakenStat.Value);
            }

            //setup player buff stats
            foreach (var buffStat in StatGeneration.BuffStats(Controller.IsHistoryBased))
            {
                stats.Add(buffStat.Key, buffStat.Value);
            }

            //setup combined stats
            foreach (var combinedStat in StatGeneration.CombinedStats(Controller.IsHistoryBased))
            {
                stats.Add(combinedStat.Key, combinedStat.Value);
            }

            //link to main party stats
            var oStats = Controller.Timeline.Overall.Stats.ToDictionary(o => o.Name);

            #region Damage

            ((TotalStat) oStats["TotalOverallDamage"]).AddDependency(stats["TotalOverallDamage"]);
            ((TotalStat) oStats["RegularDamage"]).AddDependency(stats["RegularDamage"]);
            ((TotalStat) oStats["CriticalDamage"]).AddDependency(stats["CriticalDamage"]);

            ((TotalStat) oStats["TotalOverallDamageOverTime"]).AddDependency(stats["TotalOverallDamageOverTime"]);
            ((TotalStat) oStats["RegularDamageOverTime"]).AddDependency(stats["RegularDamageOverTime"]);
            ((TotalStat) oStats["CriticalDamageOverTime"]).AddDependency(stats["CriticalDamageOverTime"]);

            #endregion

            #region Healing

            ((TotalStat) oStats["TotalOverallHealing"]).AddDependency(stats["TotalOverallHealing"]);
            ((TotalStat) oStats["RegularHealing"]).AddDependency(stats["RegularHealing"]);
            ((TotalStat) oStats["CriticalHealing"]).AddDependency(stats["CriticalHealing"]);

            ((TotalStat) oStats["TotalOverallHealingOverHealing"]).AddDependency(stats["TotalOverallHealingOverHealing"]);
            ((TotalStat) oStats["RegularHealingOverHealing"]).AddDependency(stats["RegularHealingOverHealing"]);
            ((TotalStat) oStats["CriticalHealingOverHealing"]).AddDependency(stats["CriticalHealingOverHealing"]);

            ((TotalStat) oStats["TotalOverallHealingOverTime"]).AddDependency(stats["TotalOverallHealingOverTime"]);
            ((TotalStat) oStats["RegularHealingOverTime"]).AddDependency(stats["RegularHealingOverTime"]);
            ((TotalStat) oStats["CriticalHealingOverTime"]).AddDependency(stats["CriticalHealingOverTime"]);

            ((TotalStat) oStats["TotalOverallHealingMitigated"]).AddDependency(stats["TotalOverallHealingMitigated"]);
            ((TotalStat) oStats["RegularHealingMitigated"]).AddDependency(stats["RegularHealingMitigated"]);
            ((TotalStat) oStats["CriticalHealingMitigated"]).AddDependency(stats["CriticalHealingMitigated"]);

            #endregion

            #region Damage Taken

            ((TotalStat) oStats["TotalOverallDamageTaken"]).AddDependency(stats["TotalOverallDamageTaken"]);
            ((TotalStat) oStats["RegularDamageTaken"]).AddDependency(stats["RegularDamageTaken"]);
            ((TotalStat) oStats["CriticalDamageTaken"]).AddDependency(stats["CriticalDamageTaken"]);

            ((TotalStat) oStats["TotalOverallDamageTakenOverTime"]).AddDependency(stats["TotalOverallDamageTakenOverTime"]);
            ((TotalStat) oStats["RegularDamageTakenOverTime"]).AddDependency(stats["RegularDamageTakenOverTime"]);
            ((TotalStat) oStats["CriticalDamageTakenOverTime"]).AddDependency(stats["CriticalDamageTakenOverTime"]);

            #endregion

            #region Global Percent Of Total Stats

            // damage
            stats.Add("PercentOfTotalOverallDamage", new PercentStat("PercentOfTotalOverallDamage", stats["TotalOverallDamage"], ((TotalStat) oStats["TotalOverallDamage"])));
            stats.Add("PercentOfRegularDamage", new PercentStat("PercentOfRegularDamage", stats["RegularDamage"], ((TotalStat) oStats["RegularDamage"])));
            stats.Add("PercentOfCriticalDamage", new PercentStat("PercentOfCriticalDamage", stats["CriticalDamage"], ((TotalStat) oStats["CriticalDamage"])));

            stats.Add("PercentOfTotalOverallDamageOverTime", new PercentStat("PercentOfTotalOverallDamageOverTime", stats["TotalOverallDamageOverTime"], ((TotalStat) oStats["TotalOverallDamageOverTime"])));
            stats.Add("PercentOfRegularDamageOverTime", new PercentStat("PercentOfRegularDamageOverTime", stats["RegularDamageOverTime"], ((TotalStat) oStats["RegularDamageOverTime"])));
            stats.Add("PercentOfCriticalDamageOverTime", new PercentStat("PercentOfCriticalDamageOverTime", stats["CriticalDamageOverTime"], ((TotalStat) oStats["CriticalDamageOverTime"])));

            // healing
            stats.Add("PercentOfTotalOverallHealing", new PercentStat("PercentOfTotalOverallHealing", stats["TotalOverallHealing"], ((TotalStat) oStats["TotalOverallHealing"])));
            stats.Add("PercentOfRegularHealing", new PercentStat("PercentOfRegularHealing", stats["RegularHealing"], ((TotalStat) oStats["RegularHealing"])));
            stats.Add("PercentOfCriticalHealing", new PercentStat("PercentOfCriticalHealing", stats["CriticalHealing"], ((TotalStat) oStats["CriticalHealing"])));

            stats.Add("PercentOfTotalOverallHealingOverHealing", new PercentStat("PercentOfTotalOverallHealingOverHealing", stats["TotalOverallHealingOverHealing"], ((TotalStat) oStats["TotalOverallHealingOverHealing"])));
            stats.Add("PercentOfRegularHealingOverHealing", new PercentStat("PercentOfRegularHealingOverHealing", stats["RegularHealingOverHealing"], ((TotalStat) oStats["RegularHealingOverHealing"])));
            stats.Add("PercentOfCriticalHealingOverHealing", new PercentStat("PercentOfCriticalHealingOverHealing", stats["CriticalHealingOverHealing"], ((TotalStat) oStats["CriticalHealingOverHealing"])));

            stats.Add("PercentOfTotalOverallHealingOverTime", new PercentStat("PercentOfTotalOverallHealingOverTime", stats["TotalOverallHealingOverTime"], ((TotalStat) oStats["TotalOverallHealingOverTime"])));
            stats.Add("PercentOfRegularHealingOverTime", new PercentStat("PercentOfRegularHealingOverTime", stats["RegularHealingOverTime"], ((TotalStat) oStats["RegularHealingOverTime"])));
            stats.Add("PercentOfCriticalHealingOverTime", new PercentStat("PercentOfCriticalHealingOverTime", stats["CriticalHealingOverTime"], ((TotalStat) oStats["CriticalHealingOverTime"])));

            stats.Add("PercentOfTotalOverallHealingMitigated", new PercentStat("PercentOfTotalOverallHealingMitigated", stats["TotalOverallHealingMitigated"], ((TotalStat) oStats["TotalOverallHealingMitigated"])));
            stats.Add("PercentOfRegularHealingMitigated", new PercentStat("PercentOfRegularHealingMitigated", stats["RegularHealingMitigated"], ((TotalStat) oStats["RegularHealingMitigated"])));
            stats.Add("PercentOfCriticalHealingMitigated", new PercentStat("PercentOfCriticalHealingMitigated", stats["CriticalHealingMitigated"], ((TotalStat) oStats["CriticalHealingMitigated"])));

            // damage taken
            stats.Add("PercentOfTotalOverallDamageTaken", new PercentStat("PercentOfTotalOverallDamageTaken", stats["TotalOverallDamageTaken"], ((TotalStat) oStats["TotalOverallDamageTaken"])));
            stats.Add("PercentOfRegularDamageTaken", new PercentStat("PercentOfRegularDamageTaken", stats["RegularDamageTaken"], ((TotalStat) oStats["RegularDamageTaken"])));
            stats.Add("PercentOfCriticalDamageTaken", new PercentStat("PercentOfCriticalDamageTaken", stats["CriticalDamageTaken"], ((TotalStat) oStats["CriticalDamageTaken"])));

            stats.Add("PercentOfTotalOverallDamageTakenOverTime", new PercentStat("PercentOfTotalOverallDamageTakenOverTime", stats["TotalOverallDamageTakenOverTime"], ((TotalStat) oStats["TotalOverallDamageTakenOverTime"])));
            stats.Add("PercentOfRegularDamageTakenOverTime", new PercentStat("PercentOfRegularDamageTakenOverTime", stats["RegularDamageTakenOverTime"], ((TotalStat) oStats["RegularDamageTakenOverTime"])));
            stats.Add("PercentOfCriticalDamageTakenOverTime", new PercentStat("PercentOfCriticalDamageTakenOverTime", stats["CriticalDamageTakenOverTime"], ((TotalStat) oStats["CriticalDamageTakenOverTime"])));

            #endregion

            #region Player Combined

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

            ((TotalStat) oStats["CombinedTotalOverallDamage"]).AddDependency(stats["CombinedTotalOverallDamage"]);
            ((TotalStat) oStats["CombinedRegularDamage"]).AddDependency(stats["CombinedRegularDamage"]);
            ((TotalStat) oStats["CombinedCriticalDamage"]).AddDependency(stats["CombinedCriticalDamage"]);

            ((TotalStat) oStats["CombinedTotalOverallHealing"]).AddDependency(stats["CombinedTotalOverallHealing"]);
            ((TotalStat) oStats["CombinedRegularHealing"]).AddDependency(stats["CombinedRegularHealing"]);
            ((TotalStat) oStats["CombinedCriticalHealing"]).AddDependency(stats["CombinedCriticalHealing"]);

            ((TotalStat) oStats["CombinedTotalOverallDamageTaken"]).AddDependency(stats["CombinedTotalOverallDamageTaken"]);
            ((TotalStat) oStats["CombinedRegularDamageTaken"]).AddDependency(stats["CombinedRegularDamageTaken"]);
            ((TotalStat) oStats["CombinedCriticalDamageTaken"]).AddDependency(stats["CombinedCriticalDamageTaken"]);

            #endregion

            stats.Add("ActivePercent", new PercentStat("ActivePercent", stats["TotalActiveTime"], stats["TotalParserTime"]));

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
        /// <param name="sub"></param>
        /// <param name="useSub"></param>
        /// <returns></returns>
        private IEnumerable<Stat<decimal>> BuffStatList(StatGroup sub, bool useSub = false)
        {
            var stats = StatGeneration.BuffStats();

            //setup per damage taken "percent of" stats
            switch (useSub)
            {
                case true:
                    break;
                case false:
                    break;
            }

            return stats.Select(s => s.Value)
                        .ToList();
        }
    }
}
