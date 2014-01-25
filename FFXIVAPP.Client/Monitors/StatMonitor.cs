// FFXIVAPP.Client
// StatMonitor.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Linq;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Helpers.Parse;
using FFXIVAPP.Client.Models.Parse;
using FFXIVAPP.Client.Models.Parse.Events;
using FFXIVAPP.Client.Models.Parse.History;
using FFXIVAPP.Client.Models.Parse.StatGroups;
using FFXIVAPP.Client.Models.Parse.Stats;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Client.ViewModels.Parse;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Utilities;
using NLog;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Monitors
{
    [DoNotObfuscate]
    public class StatMonitor : EventMonitor
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="parseControl"> </param>
        public StatMonitor(ParseControl parseControl) : base("StatMonitor", parseControl)
        {
            IncludeSelf = false;
            Filter = (EventParser.TypeMask | EventParser.Self | EventParser.Engaged | EventParser.UnEngaged);
            if (Constants.Parse.PluginSettings.ParseYou)
            {
                Filter = FilterHelper.Enable(Filter, EventParser.You);
                Filter = FilterHelper.Enable(Filter, EventParser.Pet);
            }
            if (Constants.Parse.PluginSettings.ParseParty)
            {
                Filter = FilterHelper.Enable(Filter, EventParser.Party);
                Filter = FilterHelper.Enable(Filter, EventParser.PetParty);
            }
            if (Constants.Parse.PluginSettings.ParseAlliance)
            {
                Filter = FilterHelper.Enable(Filter, EventParser.Alliance);
                Filter = FilterHelper.Enable(Filter, EventParser.PetAlliance);
            }
            if (Constants.Parse.PluginSettings.ParseOther)
            {
                Filter = FilterHelper.Enable(Filter, EventParser.Other);
                Filter = FilterHelper.Enable(Filter, EventParser.PetOther);
            }
        }

        public void ToggleFilter(UInt64 filter)
        {
            Filter = FilterHelper.Toggle(Filter, filter);
        }

        /// <summary>
        /// </summary>
        public override void Clear()
        {
            Logging.Log(Logger, String.Format("ClearEvent : Clearing {0} Party Member Totals.", Count));
            foreach (var player in ParseControl.Timeline.Party)
            {
                var playerInstance = ParseControl.Timeline.GetSetPlayer(player.Name);
                playerInstance.StatusUpdateTimer.Stop();
                playerInstance.IsActiveTimer.Stop();
            }
            foreach (var monster in ParseControl.Timeline.Monster)
            {
                var monsterInstance = ParseControl.Timeline.GetSetMonster(monster.Name);
                monsterInstance.StatusUpdateTimer.Stop();
                //monsterInstance.IsActiveTimer.Stop();
            }
            InitializeHistory();
            base.Clear();
        }

        private void InitializeHistory()
        {
            var hasDamage = ParseControl.Timeline.Overall.Stats.GetStatValue("TotalOverallDamage") > 0;
            var hasHealing = ParseControl.Timeline.Overall.Stats.GetStatValue("TotalOverallHealing") > 0;
            var hasDamageTaken = ParseControl.Timeline.Overall.Stats.GetStatValue("TotalOverallDamageTaken") > 0;
            if (hasDamage || hasHealing || hasDamageTaken)
            {
                var currentOverallStats = ParseControl.Timeline.Overall.Stats;
                var historyItem = new ParseHistoryItem();
                var historyController = historyItem.HistoryControl = new HistoryControl();
                foreach (var stat in currentOverallStats)
                {
                    historyController.Timeline.Overall.Stats.EnsureStatValue(stat.Name, stat.Value);
                }
                historyController.Timeline.Overall.Stats.EnsureStatValue("StaticPlayerDPS", currentOverallStats.GetStatValue("DPS"));
                historyController.Timeline.Overall.Stats.EnsureStatValue("StaticPlayerDOTPS", currentOverallStats.GetStatValue("DOTPS"));
                historyController.Timeline.Overall.Stats.EnsureStatValue("StaticPlayerHPS", currentOverallStats.GetStatValue("HPS"));
                historyController.Timeline.Overall.Stats.EnsureStatValue("StaticPlayerHOHPS", currentOverallStats.GetStatValue("HOHPS"));
                historyController.Timeline.Overall.Stats.EnsureStatValue("StaticPlayerHOTPS", currentOverallStats.GetStatValue("HOTPS"));
                historyController.Timeline.Overall.Stats.EnsureStatValue("StaticPlayerHMPS", currentOverallStats.GetStatValue("HMPS"));
                historyController.Timeline.Overall.Stats.EnsureStatValue("StaticPlayerDTPS", currentOverallStats.GetStatValue("DTPS"));
                historyController.Timeline.Overall.Stats.EnsureStatValue("StaticPlayerDTOTPS", currentOverallStats.GetStatValue("DTOTPS"));
                var playerList = ParseControl.Timeline.Party.ToArray();
                foreach (var player in playerList)
                {
                    var playerInstance = historyController.Timeline.GetSetPlayer(player.Name);
                    playerInstance.Last20DamageActions = ((Player) player).Last20DamageActions.ToList();
                    playerInstance.Last20DamageTakenActions = ((Player) player).Last20DamageTakenActions.ToList();
                    playerInstance.Last20HealingActions = ((Player) player).Last20HealingActions.ToList();
                    foreach (var stat in player.Stats)
                    {
                        playerInstance.Stats.EnsureStatValue(stat.Name, stat.Value);
                    }
                    RabbitHoleCopy(ref playerInstance, player);
                }
                var monsterList = ParseControl.Timeline.Monster.ToArray();
                foreach (var monster in monsterList)
                {
                    var monsterInstance = historyController.Timeline.GetSetMonster(monster.Name);
                    monsterInstance.Last20DamageActions = ((Monster) monster).Last20DamageActions.ToList();
                    monsterInstance.Last20DamageTakenActions = ((Monster) monster).Last20DamageTakenActions.ToList();
                    monsterInstance.Last20HealingActions = ((Monster) monster).Last20HealingActions.ToList();
                    foreach (var stat in monster.Stats)
                    {
                        monsterInstance.Stats.EnsureStatValue(stat.Name, stat.Value);
                    }
                    RabbitHoleCopy(ref monsterInstance, monster);
                }
                historyItem.Start = ParseControl.StartTime;
                historyItem.End = DateTime.Now;
                historyItem.ParseLength = historyItem.End - historyItem.Start;
                var parseTimeDetails = String.Format("{0} -> {1} [{2}]", historyItem.Start, historyItem.End, historyItem.ParseLength);
                var zone = "Unknown";
                if (AppContextHelper.Instance.CurrentUser != null)
                {
                    var mapIndex = AppContextHelper.Instance.CurrentUser.MapIndex;
                    zone = ZoneHelper.GetMapInfo(mapIndex)
                                     .English;
                    switch (Settings.Default.GameLanguage)
                    {
                        case "French":
                            zone = ZoneHelper.GetMapInfo(mapIndex)
                                             .French;
                            break;
                        case "German":
                            zone = ZoneHelper.GetMapInfo(mapIndex)
                                             .German;
                            break;
                        case "Japanese":
                            zone = ZoneHelper.GetMapInfo(mapIndex)
                                             .Japanese;
                            break;
                    }
                }
                var monsterName = "NULL";
                try
                {
                    StatGroup biggestMonster = null;
                    foreach (var monster in ParseControl.Timeline.Monster)
                    {
                        if (biggestMonster == null)
                        {
                            biggestMonster = monster;
                        }
                        else
                        {
                            if (monster.Stats.GetStatValue("TotalOverallDamage") > biggestMonster.Stats.GetStatValue("TotalOverallDamage"))
                            {
                                biggestMonster = monster;
                            }
                        }
                    }
                    if (biggestMonster != null)
                    {
                        monsterName = biggestMonster.Name;
                    }
                }
                catch (Exception ex)
                {
                }
                foreach (var oStat in currentOverallStats)
                {
                    historyController.Timeline.Overall.Stats.EnsureStatValue(oStat.Name, oStat.Value);
                }
                historyItem.Name = String.Format("{0} [{1}] {2}", zone, monsterName, parseTimeDetails);
                DispatcherHelper.Invoke(() => MainViewModel.Instance.ParseHistory.Insert(1, historyItem));
            }
        }

        private void RabbitHoleCopy(ref HistoryGroup parent, StatGroup statGroup)
        {
            if (statGroup.Stats != null)
            {
                foreach (var stat in statGroup.Stats)
                {
                    parent.Stats.EnsureStatValue(stat.Name, stat.Value);
                }
            }
            if (!statGroup.Children.Any())
            {
                return;
            }
            foreach (var group in statGroup.Children)
            {
                var newParent = parent.GetGroup(group.Name);
                foreach (var stat in group.Stats)
                {
                    newParent.Stats.EnsureStatValue(stat.Name, stat.Value);
                }
                RabbitHoleCopy(ref newParent, group);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="e"> </param>
        protected override void HandleEvent(Event e)
        {
            #region Clean Monster Names

            #endregion

            Utilities.Parse.Filter.Process(e);
        }

        /// <summary>
        /// </summary>
        /// <param name="e"> </param>
        protected override void HandleUnknownEvent(Event e)
        {
            ParsingLogHelper.Log(Logger, "UnknownEvent", e);
        }
    }
}
