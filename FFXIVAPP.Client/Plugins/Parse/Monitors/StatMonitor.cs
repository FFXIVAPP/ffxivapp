// FFXIVAPP.Client
// StatMonitor.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using FFXIVAPP.Client.Delegates;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Plugins.Parse.Enums;
using FFXIVAPP.Client.Plugins.Parse.Models;
using FFXIVAPP.Client.Plugins.Parse.Models.Events;
using FFXIVAPP.Client.Plugins.Parse.Models.LinkedStats;
using FFXIVAPP.Client.Plugins.Parse.Models.StatGroups;
using FFXIVAPP.Client.Plugins.Parse.Models.Stats;
using FFXIVAPP.Client.Plugins.Parse.Models.Timelines;
using FFXIVAPP.Client.Plugins.Parse.ViewModels;
using FFXIVAPP.Client.SettingsProviders.Parse;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Utilities;
using NLog;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Plugins.Parse.Monitors
{
    [DoNotObfuscate]
    public class StatMonitor : EventMonitor
    {
        //player totals
        internal TotalStat TotalOverallDamage = new TotalStat("TotalOverallDamage");
        internal TotalStat TotalOverallHealing = new TotalStat("TotalOverallHealing");
        internal TotalStat TotalOverallDamageTaken = new TotalStat("TotalOverallDamageTaken");
        internal TotalStat TotalOverallTP = new TotalStat("TotalOverallTP");
        internal TotalStat TotalOverallMP = new TotalStat("TotalOverallMP");
        internal TotalStat RegularDamage = new TotalStat("RegularDamage");
        internal TotalStat CriticalDamage = new TotalStat("CriticalDamage");
        internal TotalStat RegularHealing = new TotalStat("RegularHealing");
        internal TotalStat CriticalHealing = new TotalStat("CriticalHealing");
        internal TotalStat RegularDamageTaken = new TotalStat("RegularDamageTaken");
        internal TotalStat CriticalDamageTaken = new TotalStat("CriticalDamageTaken");
        //monster totals
        internal TotalStat TotalOverallDamageMonster = new TotalStat("TotalOverallDamageMonster");
        internal TotalStat TotalOverallHealingMonster = new TotalStat("TotalOverallHealingMonster");
        internal TotalStat TotalOverallDamageTakenMonster = new TotalStat("TotalOverallDamageTakenMonster");
        internal TotalStat TotalOverallTPMonster = new TotalStat("TotalOverallTPMonster");
        internal TotalStat TotalOverallMPMonster = new TotalStat("TotalOverallMPMonster");
        internal TotalStat RegularDamageMonster = new TotalStat("RegularDamageMonster");
        internal TotalStat CriticalDamageMonster = new TotalStat("CriticalDamageMonster");
        internal TotalStat RegularHealingMonster = new TotalStat("RegularHealingMonster");
        internal TotalStat CriticalHealingMonster = new TotalStat("CriticalHealingMonster");
        internal TotalStat RegularDamageTakenMonster = new TotalStat("RegularDamageTakenMonster");
        internal TotalStat CriticalDamageTakenMonster = new TotalStat("CriticalDamageTakenMonster");

        /// <summary>
        /// </summary>
        /// <param name="parseControl"> </param>
        public StatMonitor(ParseControl parseControl) : base("StatMonitor", parseControl)
        {
            IncludeSelf = false;
            if (!parseControl.IsHistoryBased)
            {
                Filter = (EventParser.TypeMask | (UInt32)EventSubject.You | (UInt32)EventSubject.Party | (UInt32)EventSubject.Engaged | (UInt32)EventSubject.UnEngaged | (UInt32)EventDirection.Self | (UInt32)EventDirection.You | (UInt32)EventDirection.Party | (UInt32)EventDirection.Engaged | (UInt32)EventDirection.UnEngaged);
            }
        }

        /// <summary>
        /// </summary>
        protected override void InitStats()
        {
            ParseControl.Timeline.Overall.Stats.Add(TotalOverallDamage);
            ParseControl.Timeline.Overall.Stats.Add(TotalOverallHealing);
            ParseControl.Timeline.Overall.Stats.Add(TotalOverallDamageTaken);
            ParseControl.Timeline.Overall.Stats.Add(TotalOverallTP);
            ParseControl.Timeline.Overall.Stats.Add(TotalOverallMP);
            ParseControl.Timeline.Overall.Stats.Add(RegularDamage);
            ParseControl.Timeline.Overall.Stats.Add(CriticalDamage);
            ParseControl.Timeline.Overall.Stats.Add(RegularHealing);
            ParseControl.Timeline.Overall.Stats.Add(CriticalHealing);
            ParseControl.Timeline.Overall.Stats.Add(RegularDamageTaken);
            ParseControl.Timeline.Overall.Stats.Add(CriticalDamageTaken);
            ParseControl.Timeline.Overall.Stats.Add(TotalOverallDamageMonster);
            ParseControl.Timeline.Overall.Stats.Add(TotalOverallHealingMonster);
            ParseControl.Timeline.Overall.Stats.Add(TotalOverallDamageTakenMonster);
            ParseControl.Timeline.Overall.Stats.Add(TotalOverallTPMonster);
            ParseControl.Timeline.Overall.Stats.Add(TotalOverallMPMonster);
            ParseControl.Timeline.Overall.Stats.Add(RegularDamageMonster);
            ParseControl.Timeline.Overall.Stats.Add(CriticalDamageMonster);
            ParseControl.Timeline.Overall.Stats.Add(RegularHealingMonster);
            ParseControl.Timeline.Overall.Stats.Add(CriticalHealingMonster);
            ParseControl.Timeline.Overall.Stats.Add(RegularDamageTakenMonster);
            ParseControl.Timeline.Overall.Stats.Add(CriticalDamageTakenMonster);
            base.InitStats();
        }

        /// <summary>
        /// </summary>
        public override void Clear()
        {
            Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("ClearEvent : Clearing ${0} Party Member Totals.", Count));
            foreach (var player in ParseControl.Timeline.Party)
            {
                var playerInstance = ParseControl.Timeline.GetSetPlayer(player.Name);
                playerInstance.LineHistory.Clear();
                playerInstance.StatusUpdateTimer.Stop();
                playerInstance.LastDamageAmountByAction.Clear();
                playerInstance.StatusEntriesSelf.Clear();
                playerInstance.StatusEntriesMonster.Clear();
            }
            foreach (var monster in ParseControl.Timeline.Monster)
            {
                var monsterInstance = ParseControl.Timeline.GetSetMob(monster.Name);
                monsterInstance.LineHistory.Clear();
                //monsterInstance.StatusUpdateTimer.Stop();
                monsterInstance.LastDamageAmountByAction.Clear();
                //monsterInstance.StatusEntriesSelf.Clear();
                //monsterInstance.StatusEntriesMonster.Clear();
            }
            if (Settings.Default.EnableStoreHistoryReset)
            {
                InitializeHistory();
            }
            TotalOverallDamage.Reset();
            TotalOverallHealing.Reset();
            TotalOverallDamageTaken.Reset();
            TotalOverallTP.Reset();
            TotalOverallMP.Reset();
            RegularDamage.Reset();
            CriticalDamage.Reset();
            RegularHealing.Reset();
            CriticalHealing.Reset();
            RegularDamageTaken.Reset();
            CriticalDamageTaken.Reset();
            TotalOverallDamageMonster.Reset();
            TotalOverallHealingMonster.Reset();
            TotalOverallDamageTakenMonster.Reset();
            TotalOverallTPMonster.Reset();
            TotalOverallMPMonster.Reset();
            RegularDamageMonster.Reset();
            CriticalDamageMonster.Reset();
            RegularHealingMonster.Reset();
            CriticalHealingMonster.Reset();
            RegularDamageTakenMonster.Reset();
            CriticalDamageTakenMonster.Reset();
            base.Clear();
        }

        private void InitializeHistory()
        {
            var hasDamage = ParseControl.Timeline.Overall.Stats.GetStatValue("TotalOverallDamage") > 0;
            if (!hasDamage)
            {
                return;
            }
            var historyItem = new ParseHistoryItem();
            var controller = historyItem.HistoryControl.Controller;
            var oStatList = ParseControl.Timeline.Overall.Stats.ToArray();
            foreach (var oStat in oStatList)
            {
                controller.Timeline.Overall.Stats.SetOrAddStat(oStat.Name, oStat.Value);
            }
            var playerList = ParseControl.Timeline.Party.ToArray();
            foreach (var player in playerList)
            {
                var playerInstance = new Player(player.Name, controller);
                RabbitHoleCopy(playerInstance, player, player.Stats);
                //var playerInstance = new Player(player.Name, controller);
                //var playerGroups = new StatGroup[player.Children.Count];
                //player.CopyTo(playerGroups, 0);
                //var playerStats = new Stat<decimal>[player.Stats.Count];
                //player.Stats.CopyTo(playerStats,0);
                //foreach (var playerGroup in playerGroups)
                //{
                //    playerInstance.AddGroup(playerGroup);
                //}
                //foreach (var playerStat in playerStats)
                //{
                //    playerInstance.Stats.SetOrAddStat(playerStat.Name, playerStat.Value);
                //}
                controller.Timeline.Party.Add(playerInstance);
            }
            var monsterList = ParseControl.Timeline.Monster.ToArray();
            foreach (var monster in monsterList)
            {
                var monsterInstance = new Player(monster.Name, controller);
                //var monsterGroups = new StatGroup[monster.Children.Count];
                //monster.CopyTo(monsterGroups, 0);
                //var monsterStats = new Stat<decimal>[monster.Stats.Count];
                //monster.Stats.CopyTo(monsterStats, 0);
                //foreach (var monsterGroup in monsterGroups)
                //{
                //    monsterInstance.AddGroup(monsterGroup);
                //}
                //foreach (var monsterStat in monsterStats)
                //{
                //    monsterInstance.Stats.SetOrAddStat(monsterStat.Name, monsterStat.Value);
                //}
                controller.Timeline.Monster.Add(monsterInstance);
            }
            historyItem.Start = ParseControl.StartTime;
            historyItem.End = DateTime.Now;
            historyItem.ParseLength = historyItem.End - historyItem.Start;
            var parseTimeDetails = String.Format("{0} -> {1} [{2}]", historyItem.Start, historyItem.End, historyItem.ParseLength);
            var zone = "Unknown";
            if (MonsterWorkerDelegate.CurrentUser != null)
            {
                var mapIndex = MonsterWorkerDelegate.CurrentUser.MapIndex;
                //switch (Settings.Default.GameLanguage)
                //{
                //    case "French":
                //        zone = ZoneHelper.GetMapInfo(ParseControl.Instance.Timeline.ZoneID)
                //                     .French;
                //        break;
                //    case "German":
                //        zone = ZoneHelper.GetMapInfo(ParseControl.Instance.Timeline.ZoneID)
                //                     .German;
                //        break;
                //    case "Japanese":
                //        zone = ZoneHelper.GetMapInfo(ParseControl.Instance.Timeline.ZoneID)
                //                     .Japanese;
                //        break;
                //}
                zone = ZoneHelper.GetMapInfo(mapIndex)
                                 .English;
            }
            historyItem.Name = String.Format("{0} {1}", zone, parseTimeDetails);
            DispatcherHelper.Invoke(() => HistoryViewModel.Instance.ParseHistory.Insert(0, historyItem));
        }

        private void RabbitHoleCopy(StatGroup parent, StatGroup statGroup, IEnumerable<Stat<decimal>> stats)
        {
            if (statGroup.Children.Any())
            {
                foreach (var group in statGroup)
                {
                    RabbitHoleCopy(statGroup, group, group.Stats);
                }
            }
            if (stats == null)
            {
                return;
            }
            foreach (var stat in stats)
            {
                parent.Stats.SetOrAddStat(stat.Name, stat.Value);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="e"> </param>
        protected override void HandleEvent(Models.Events.Event e)
        {
            #region Clean Mob Names

            #endregion

            Utilities.Filter.Process(e.RawLine, e);
        }

        /// <summary>
        /// </summary>
        /// <param name="e"> </param>
        protected override void HandleUnknownEvent(Models.Events.Event e)
        {
            var line = e.RawLine;
        }
    }
}
