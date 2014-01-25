// FFXIVAPP.Client
// TimelineMonitor.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Linq;
using System.Text.RegularExpressions;
using FFXIVAPP.Client.Delegates;
using FFXIVAPP.Client.Enums.Parse;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Helpers.Parse;
using FFXIVAPP.Client.Models;
using FFXIVAPP.Client.Models.Parse;
using FFXIVAPP.Client.Models.Parse.Events;
using FFXIVAPP.Client.Models.Parse.Fights;
using FFXIVAPP.Client.RegularExpressions;
using FFXIVAPP.Client.Utilities;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Helpers;
using NLog;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Monitors
{
    [DoNotObfuscate]
    public class TimelineMonitor : EventMonitor
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="parseControl"> </param>
        public TimelineMonitor(ParseControl parseControl) : base("Timeline", parseControl)
        {
            Filter = (EventParser.SubjectMask | EventParser.DirectionMask | (UInt64) EventType.Loot | (UInt64) EventType.Defeats);
        }

        private Expressions Expressions { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="e"> </param>
        protected override void HandleEvent(Event e)
        {
            Expressions = new Expressions(e);

            if (String.IsNullOrWhiteSpace(e.ChatLogEntry.Line))
            {
                return;
            }
            switch (e.Type)
            {
                case EventType.Defeats:
                    ProcessDefeated(e);
                    break;
                case EventType.Loot:
                    ProcessLoot(e);
                    break;
                default:
                    //ProcessParty(e);
                    break;
            }
        }

        protected override void HandleUnknownEvent(Event e)
        {
            ParsingLogHelper.Log(Logger, "UnknownEvent", e);
        }

        /// <summary>
        /// </summary>
        private void ProcessDefeated(Event e)
        {
            Match matches;
            var you = Constants.CharacterName;
            switch (Constants.GameLanguage)
            {
                case "French":
                    matches = PlayerRegEx.DefeatsFr.Match(e.ChatLogEntry.Line);
                    break;
                case "Japanese":
                    matches = PlayerRegEx.DefeatsJa.Match(e.ChatLogEntry.Line);
                    break;
                case "German":
                    matches = PlayerRegEx.DefeatsDe.Match(e.ChatLogEntry.Line);
                    break;
                default:
                    matches = PlayerRegEx.DefeatsEn.Match(e.ChatLogEntry.Line);
                    break;
            }
            if (!matches.Success)
            {
                ParseControl.Timeline.PublishTimelineEvent(TimelineEventType.PartyMonsterKilled, "");
                ParsingLogHelper.Log(Logger, "Defeat", e);
                return;
            }
            var target = matches.Groups["target"];
            var source = matches.Groups["source"];
            if (!target.Success)
            {
                return;
            }
            if (ParseControl.Timeline.Party.HasGroup(target.Value) || Regex.IsMatch(target.Value, Expressions.You) || target.Value == you)
            {
                return;
            }
            var targetName = StringHelper.TitleCase(target.Value);
            var sourceName = StringHelper.TitleCase(source.Success ? source.Value : "Unknown");
            AddKillToPartyMonster(targetName, sourceName);
        }

        /// <summary>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        private void AddKillToPartyMonster(string target, string source)
        {
            var monsterName = target.Trim();
            ParseControl.Timeline.PublishTimelineEvent(TimelineEventType.PartyMonsterKilled, monsterName);
            try
            {
                var monsters = MonsterWorkerDelegate.GetUniqueNPCEntities();
                var killEntry = new KillEntry();
                if (AppContextHelper.Instance.CurrentUser != null)
                {
                    var currentUser = AppContextHelper.Instance.CurrentUser;
                    killEntry.MapIndex = currentUser.MapIndex;
                    killEntry.Coordinate = new Coordinate
                    {
                        X = currentUser.X,
                        Z = currentUser.Z,
                        Y = currentUser.Y,
                    };
                }
                if (!String.IsNullOrWhiteSpace(monsterName.Replace(" ", "")))
                {
                    if (monsters.Any(entry => String.Equals(entry.Name, monsterName, Constants.InvariantComparer) && entry.MapIndex == killEntry.MapIndex))
                    {
                        var monster = monsters.FirstOrDefault(entry => String.Equals(entry.Name, monsterName, Constants.InvariantComparer) && entry.MapIndex == killEntry.MapIndex);
                        killEntry.ModelID = monster == null ? 0 : monster.ModelID;
                    }
                }
                else
                {
                    killEntry.ModelID = 0;
                }
                var mapInfo = ZoneHelper.GetMapInfo(killEntry.MapIndex);
                if (killEntry.ModelID > 0 && !mapInfo.IsDungeonInstance)
                {
                    DispatcherHelper.Invoke(() => KillWorkerDelegate.OnNewKill(killEntry));
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// </summary>
        private void ProcessLoot(Event e)
        {
            Match matches;
            switch (Constants.GameLanguage)
            {
                case "French":
                    matches = PlayerRegEx.ObtainsFr.Match(e.ChatLogEntry.Line);
                    break;
                case "Japanese":
                    matches = PlayerRegEx.ObtainsJa.Match(e.ChatLogEntry.Line);
                    break;
                case "German":
                    matches = PlayerRegEx.ObtainsDe.Match(e.ChatLogEntry.Line);
                    break;
                default:
                    matches = PlayerRegEx.ObtainsEn.Match(e.ChatLogEntry.Line);
                    break;
            }
            if (!matches.Success)
            {
                ParsingLogHelper.Log(Logger, "Loot", e);
                return;
            }
            var thing = StringHelper.TitleCase(matches.Groups["item"].Value);
            AttachDropToPartyMonster(thing, e);
        }

        /// <summary>
        /// </summary>
        /// <param name="thing"> </param>
        /// <param name="e"></param>
        private void AttachDropToPartyMonster(string thing, Event e)
        {
            var monsterName = ParseControl.Timeline.FightingRightNow ? ParseControl.Timeline.LastEngaged : "";
            if (ParseControl.Instance.Timeline.FightingRightNow)
            {
                Fight fight;
                if (ParseControl.Timeline.Fights.TryGet(ParseControl.Timeline.LastEngaged, out fight))
                {
                    monsterName = fight.MonsterName;
                    if (monsterName.Replace(" ", "") != "")
                    {
                        var monsterGroup = ParseControl.Timeline.GetSetMonster(monsterName);
                        monsterGroup.SetDrop(thing);
                    }
                }
            }
            else
            {
                ParsingLogHelper.Log(Logger, "Loot.NoKillInLastThreeSeconds", e);
            }
            if (LogPublisher.Parse.NeedGreedHistory.Any(item => item.ToLowerInvariant()
                                                                    .Contains(thing.ToLowerInvariant())))
            {
                return;
            }
            try
            {
                var monsters = MonsterWorkerDelegate.GetUniqueNPCEntities();
                var lootEntry = new LootEntry(thing);
                if (AppContextHelper.Instance.CurrentUser != null)
                {
                    var currentUser = AppContextHelper.Instance.CurrentUser;
                    lootEntry.MapIndex = currentUser.MapIndex;
                    lootEntry.Coordinate = new Coordinate
                    {
                        X = currentUser.X,
                        Z = currentUser.Z,
                        Y = currentUser.Y,
                    };
                }
                if (!String.IsNullOrWhiteSpace(monsterName.Replace(" ", "")))
                {
                    if (monsters.Any(entry => String.Equals(entry.Name, monsterName, Constants.InvariantComparer) && entry.MapIndex == lootEntry.MapIndex))
                    {
                        var monster = monsters.FirstOrDefault(entry => String.Equals(entry.Name, monsterName, Constants.InvariantComparer) && entry.MapIndex == lootEntry.MapIndex);
                        lootEntry.ModelID = monster == null ? 0 : monster.ModelID;
                    }
                }
                else
                {
                    lootEntry.ModelID = 0;
                }
                var mapInfo = ZoneHelper.GetMapInfo(lootEntry.MapIndex);
                if (lootEntry.ModelID > 0 && !mapInfo.IsDungeonInstance)
                {
                    DispatcherHelper.Invoke(() => LootWorkerDelegate.OnNewLoot(lootEntry));
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="line"> </param>
        private void Processparty(string line)
        {
            //var join = Regex.Match("ph", @"^\.$");
            //var disband = Regex.Match("ph", @"^\.$");
            //var left = Regex.Match("ph", @"^\.$");
            //switch (Common.Constants.Client.GameLanguage)
            //{
            //    case "French":
            //        join = PlayerRegEx.JoinFr.Match(line);
            //        disband = PlayerRegEx.DisbandFr.Match(line);
            //        left = PlayerRegEx.LeftFr.Match(line);
            //        break;
            //    case "Japanese":
            //        join = PlayerRegEx.JoinJa.Match(line);
            //        disband = PlayerRegEx.DisbandJa.Match(line);
            //        left = PlayerRegEx.LeftJa.Match(line);
            //        break;
            //    case "German":
            //        join = PlayerRegEx.JoinDe.Match(line);
            //        disband = PlayerRegEx.DisbandDe.Match(line);
            //        left = PlayerRegEx.LeftDe.Match(line);
            //        break;
            //    default:
            //        join = PlayerRegEx.JoinEn.Match(line);
            //        disband = PlayerRegEx.DisbandEn.Match(line);
            //        left = PlayerRegEx.LeftEn.Match(line);
            //        break;
            //}
            //string who;
            //if (join.Success)
            //{
            //    who = @join.Groups["who"].Value;
            //    ParseControl.Timeline.PublishTimelineEvent(TimelineEventType.PartyJoin, who);
            //    return;
            //}
            //if (disband.Success)
            //{
            //    ParseControl.Timeline.PublishTimelineEvent(TimelineEventType.PartyDisband, String.Empty);
            //    return;
            //}
            //if (!left.Success)
            //{
            //    return;
            //}
            //who = left.Groups["who"].Value;
            //ParseControl.Timeline.PublishTimelineEvent(TimelineEventType.PartyLeave, who);
        }
    }
}
