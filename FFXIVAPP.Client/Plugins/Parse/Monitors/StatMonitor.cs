// FFXIVAPP.Client
// StatMonitor.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Linq;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Plugins.Parse.Helpers;
using FFXIVAPP.Client.Plugins.Parse.Models;
using FFXIVAPP.Client.Plugins.Parse.Models.Events;
using FFXIVAPP.Client.Plugins.Parse.Models.Stats;
using FFXIVAPP.Client.Plugins.Parse.ViewModels;
using FFXIVAPP.Client.Plugins.Parse.Views;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.Utilities;
using NLog;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Plugins.Parse.Monitors
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
            if (parseControl.IsHistoryBased)
            {
                return;
            }
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
                playerInstance.LineHistory.Clear();
                playerInstance.StatusUpdateTimer.Stop();
                playerInstance.StatusEntriesSelf.Clear();
                playerInstance.StatusEntriesPlayers.Clear();
                playerInstance.StatusEntriesMonsters.Clear();
            }
            foreach (var monster in ParseControl.Timeline.Monster)
            {
                var monsterInstance = ParseControl.Timeline.GetSetMonster(monster.Name);
                monsterInstance.LineHistory.Clear();
                monsterInstance.StatusUpdateTimer.Stop();
                monsterInstance.StatusEntriesSelf.Clear();
                monsterInstance.StatusEntriesPlayers.Clear();
                monsterInstance.StatusEntriesMonsters.Clear();
            }
            InitializeHistory();
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
            var oStats = ParseControl.Timeline.Overall.Stats;
            controller.Timeline.Overall.Stats.SetOrAddStat("StaticPlayerDPS", oStats.GetStatValue("DPS"));
            controller.Timeline.Overall.Stats.SetOrAddStat("StaticPlayerHPS", oStats.GetStatValue("HPS"));
            controller.Timeline.Overall.Stats.SetOrAddStat("StaticPlayerDTPS", oStats.GetStatValue("DTPS"));
            var playerList = ParseControl.Timeline.Party.ToArray();
            foreach (var player in playerList)
            {
                var playerInstance = controller.Timeline.GetSetPlayer(player.Name);
                foreach (var stat in player.Stats)
                {
                    playerInstance.Stats.SetOrAddStat(stat.Name, stat.Value);
                }
                RabbitHoleCopy(playerInstance, player);
            }
            var monsterList = ParseControl.Timeline.Monster.ToArray();
            foreach (var monster in monsterList)
            {
                var monsterInstance = controller.Timeline.GetSetMonster(monster.Name);
                foreach (var stat in monster.Stats)
                {
                    monsterInstance.Stats.SetOrAddStat(stat.Name, stat.Value);
                }
                RabbitHoleCopy(monsterInstance, monster);
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
            historyItem.Name = String.Format("{0} [{1}] {2}", zone, monsterName, parseTimeDetails);
            DispatcherHelper.Invoke(delegate
            {
                HistoryViewModel.Instance.ParseHistory.Insert(0, historyItem);
                if (Constants.Parse.PluginSettings.AutoLoadLastParseFromHistory)
                {
                    HistoryView.View.HistoryList.SelectedIndex = 0;
                }
            });
        }

        private void RabbitHoleCopy(StatGroup parent, StatGroup statGroup)
        {
            if (statGroup.Stats != null)
            {
                var newStats = new Stat<decimal>[statGroup.Stats.Count];
                statGroup.Stats.CopyTo(newStats, 0);
                parent.Stats.AddStats(newStats);
            }
            if (!statGroup.Children.Any())
            {
                return;
            }
            foreach (var group in statGroup.Children)
            {
                var newParent = parent.GetGroup(@group.Name);
                RabbitHoleCopy(newParent, @group);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="e"> </param>
        protected override void HandleEvent(Event e)
        {
            #region Clean Monster Names

            #endregion

            Utilities.Filter.Process(e);
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
