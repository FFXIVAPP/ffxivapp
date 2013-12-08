// FFXIVAPP.Client
// Player.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Timers;
using FFXIVAPP.Client.Delegates;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Plugins.Parse.Helpers;
using FFXIVAPP.Client.Plugins.Parse.Models.LinkedStats;
using FFXIVAPP.Client.Plugins.Parse.Models.Stats;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Utilities;
using NLog;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Plugins.Parse.Models.StatGroups
{
    [DoNotObfuscate]
    public partial class Player : StatGroup
    {
        private static readonly IList<string> LD = new[]
        {
            "Counter", "Block", "Parry", "Resist", "Evade"
        };

        public readonly Timer StatusUpdateTimer = new Timer(1000);
        public Dictionary<string, decimal> LastDamageAmountByAction = new Dictionary<string, decimal>();

        public List<StatusEntry> StatusEntriesMonster = new List<StatusEntry>();
        public List<StatusEntry> StatusEntriesSelf = new List<StatusEntry>();

        public Player(string name, ParseControl parseControl) : base(name)
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

        private static ParseControl Controller { get; set; }

        public uint ID { get; set; }

        public static ActorEntity NPCEntry { get; set; }

        public List<LineHistory> LineHistory { get; set; }

        private void StatusUpdateTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            StatusEntriesSelf.Clear();
            StatusEntriesMonster.Clear();
            var pcEntries = PCWorkerDelegate.NPCEntries.ToList();
            var monsterEntries = MonsterWorkerDelegate.NPCEntries.ToList();
            if (pcEntries.Any())
            {
                try
                {
                    if (Regex.IsMatch(Name, @"^(([Dd](ich|ie|u))|You|Vous)$") || String.Equals(Name, Settings.Default.CharacterName, Constants.InvariantComparer))
                    {
                        NPCEntry = AppContextHelper.Instance.CurrentUser;
                    }
                    else
                    {
                        NPCEntry = pcEntries.First(p => String.Equals(p.Name, Name, Constants.InvariantComparer));
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
                                    StatusEntriesMonster.Add(statusEntry);
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
                    Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
                }
            }
            if (!StatusEntriesMonster.Any())
            {
                return;
            }
            foreach (var statusEntry in StatusEntriesMonster)
            {
                try
                {
                    var statusInfo = StatusEffectHelper.StatusInfo(statusEntry.StatusID);
                    var statusKey = "";
                    switch (Settings.Default.GameLanguage)
                    {
                        case "English":
                            statusKey = statusInfo.Name.English;
                            break;
                        case "French":
                            statusKey = statusInfo.Name.French;
                            break;
                        case "German":
                            statusKey = statusInfo.Name.German;
                            break;
                        case "Japanese":
                            statusKey = statusInfo.Name.Japanese;
                            break;
                    }
                    if (String.IsNullOrWhiteSpace(statusKey))
                    {
                        continue;
                    }
                    decimal amount = 0;
                    var key = statusKey;
                    foreach (var lastDamageAmountByAction in LastDamageAmountByAction.Where(d => String.Equals(d.Key, key, Constants.InvariantComparer)))
                    {
                        amount = lastDamageAmountByAction.Value;
                    }
                    DamageOverTimeAction actionData = null;
                    foreach (var damageOverTimeAction in DamageOverTimeHelper.PlayerActions.Where(d => String.Equals(d.Key, key, Constants.InvariantComparer)))
                    {
                        actionData = damageOverTimeAction.Value;
                    }
                    if (actionData == null)
                    {
                        continue;
                    }
                    if (actionData.ZeroBaseDamageDOT)
                    {
                        amount = 100;
                    }
                    amount = (amount == 0) ? 100 : amount;
                    statusKey = String.Format("{0} [•]", statusKey);
                    var tickDamage = Math.Ceiling(((amount / actionData.ActionPotency) * actionData.DamageOverTimePotency) / 3);
                    if (amount > 300)
                    {
                        tickDamage = Math.Ceiling(tickDamage / ((decimal) actionData.Duration / 3));
                    }
                    var line = new Line
                    {
                        Action = statusKey,
                        Source = Name,
                        Target = statusEntry.TargetName,
                        Amount = tickDamage
                    };
                    DispatcherHelper.Invoke(delegate
                    {
                        line.Hit = true;
                        //var currentCritRate = ParseControl.Instance.Timeline.GetSetPlayer(line.Source)
                        //                                  .Stats.GetStatValue("DamageCritPerecent");
                        //var randomizer = new Random();
                        //var critChance = randomizer.Next(0, 100) / 100;
                        //if (critChance > (currentCritRate - 0.1m) && critChance < (currentCritRate + 0.1m))
                        //{
                        //    line.Crit = true;
                        //    line.Amount += line.Amount * 0.5m;
                        //}
                        ParseControl.Instance.Timeline.GetSetPlayer(line.Source)
                                    .SetDamage(line, true);
                        ParseControl.Instance.Timeline.GetSetMob(line.Target)
                                    .SetDamageTaken(line, true);
                    });
                }
                catch (Exception ex)
                {
                    Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
                }
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
            var oStats = Controller.Timeline.Overall.Stats.ToDictionary(o => o.Name);
            ((TotalStat) oStats["TotalOverallDamage"]).AddDependency(stats["TotalOverallDamage"]);
            ((TotalStat) oStats["RegularDamage"]).AddDependency(stats["RegularDamage"]);
            ((TotalStat) oStats["CriticalDamage"]).AddDependency(stats["CriticalDamage"]);
            ((TotalStat) oStats["TotalOverallHealing"]).AddDependency(stats["TotalOverallHealing"]);
            ((TotalStat) oStats["RegularHealing"]).AddDependency(stats["RegularHealing"]);
            ((TotalStat) oStats["CriticalHealing"]).AddDependency(stats["CriticalHealing"]);
            ((TotalStat) oStats["TotalOverallDamageTaken"]).AddDependency(stats["TotalOverallDamageTaken"]);
            ((TotalStat) oStats["RegularDamageTaken"]).AddDependency(stats["RegularDamageTaken"]);
            ((TotalStat) oStats["CriticalDamageTaken"]).AddDependency(stats["CriticalDamageTaken"]);

            //setup global "percent of" stats
            stats.Add("PercentOfTotalOverallDamage", new PercentStat("PercentOfTotalOverallDamage", stats["TotalOverallDamage"], ((TotalStat) oStats["TotalOverallDamage"])));
            stats.Add("PercentOfRegularDamage", new PercentStat("PercentOfRegularDamage", stats["RegularDamage"], ((TotalStat) oStats["RegularDamage"])));
            stats.Add("PercentOfCriticalDamage", new PercentStat("PercentOfCriticalDamage", stats["CriticalDamage"], ((TotalStat) oStats["CriticalDamage"])));
            stats.Add("PercentOfTotalOverallHealing", new PercentStat("PercentOfTotalOverallHealing", stats["TotalOverallHealing"], ((TotalStat) oStats["TotalOverallHealing"])));
            stats.Add("PercentOfRegularHealing", new PercentStat("PercentOfRegularHealing", stats["RegularHealing"], ((TotalStat) oStats["RegularHealing"])));
            stats.Add("PercentOfCriticalHealing", new PercentStat("PercentOfCriticalHealing", stats["CriticalHealing"], ((TotalStat) oStats["CriticalHealing"])));
            stats.Add("PercentOfTotalOverallDamageTaken", new PercentStat("PercentOfTotalOverallDamageTaken", stats["TotalOverallDamageTaken"], ((TotalStat) oStats["TotalOverallDamageTaken"])));
            stats.Add("PercentOfRegularDamageTaken", new PercentStat("PercentOfRegularDamageTaken", stats["RegularDamageTaken"], ((TotalStat) oStats["RegularDamageTaken"])));
            stats.Add("PercentOfCriticalDamageTaken", new PercentStat("PercentOfCriticalDamageTaken", stats["CriticalDamageTaken"], ((TotalStat) oStats["CriticalDamageTaken"])));

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
