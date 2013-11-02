// FFXIVAPP.Client
// TimelineMonitor.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Linq;
using System.Text.RegularExpressions;
using FFXIVAPP.Client.Delegates;
using FFXIVAPP.Client.Models;
using FFXIVAPP.Client.Plugins.Parse.Enums;
using FFXIVAPP.Client.Plugins.Parse.Helpers;
using FFXIVAPP.Client.Plugins.Parse.Models;
using FFXIVAPP.Client.Plugins.Parse.Models.Events;
using FFXIVAPP.Client.Plugins.Parse.Models.Fights;
using FFXIVAPP.Client.RegularExpressions;
using FFXIVAPP.Client.Utilities;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Utilities;
using NLog;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Plugins.Parse.Monitors
{
    [DoNotObfuscate]
    public class TimelineMonitor : EventMonitor
    {
        /// <summary>
        /// </summary>
        /// <param name="parseControl"> </param>
        public TimelineMonitor(ParseControl parseControl) : base("Timeline", parseControl)
        {
            Filter = (EventParser.SubjectMask | EventParser.DirectionMask | (UInt32) EventType.Loot | (UInt32) EventType.Defeats);
        }

        private Expressions Expressions { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="e"> </param>
        protected override void HandleEvent(Models.Events.Event e)
        {
            Expressions = new Expressions(e, e.RawLine);

            if (String.IsNullOrWhiteSpace(e.RawLine))
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

        /// <summary>
        /// </summary>
        private void ProcessDefeated(Models.Events.Event e)
        {
            Match matches;
            var you = Constants.CharacterName;
            switch (Constants.GameLanguage)
            {
                case "French":
                    matches = PlayerRegEx.DefeatsFr.Match(e.RawLine);
                    break;
                case "Japanese":
                    matches = PlayerRegEx.DefeatsJa.Match(e.RawLine);
                    break;
                case "German":
                    matches = PlayerRegEx.DefeatsDe.Match(e.RawLine);
                    break;
                default:
                    matches = PlayerRegEx.DefeatsEn.Match(e.RawLine);
                    break;
            }
            if (!matches.Success)
            {
                ParseControl.Timeline.PublishTimelineEvent(TimelineEventType.MobKilled, "");
                ParsingLogHelper.Log(LogManager.GetCurrentClassLogger(), "Defeat", e);
                return;
            }
            var target = matches.Groups["target"];
            var source = matches.Groups["source"];
            if (!target.Success)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("KillEvent : Got RegEx Match For Monster Defeat; No <target> Capture Group. Line: {0}", e.RawLine));
                return;
            }
            if (source.Success)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("KillEvent : Got RegEx Match For Monster Defeat; No <source> Capture Group. Line: {0}", e.RawLine));
            }
            if (ParseControl.Timeline.Party.HasGroup(target.Value) || Regex.IsMatch(target.Value, Expressions.You) || target.Value == you)
            {
                return;
            }
            var targetName = StringHelper.TitleCase(target.Value);
            var sourceName = StringHelper.TitleCase(source.Success ? source.Value : "Unknown");
            AddKillToMonster(targetName, sourceName);
        }

        /// <summary>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        private void AddKillToMonster(string target, string source)
        {
            var mobName = target.Trim();
            Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("KillEvent : {0} By : {1}", target, source));
            ParseControl.Timeline.PublishTimelineEvent(TimelineEventType.MobKilled, mobName);
            try
            {
                var monsters = MonsterWorkerDelegate.UniqueNPCEntries.ToList();
                var killEntry = new KillEntry();
                if (MonsterWorkerDelegate.CurrentUser != null)
                {
                    killEntry.MapIndex = MonsterWorkerDelegate.CurrentUser.MapIndex;
                    killEntry.Coordinate = MonsterWorkerDelegate.CurrentUser.Coordinate;
                }
                if (!String.IsNullOrWhiteSpace(mobName.Replace(" ", "")))
                {
                    if (monsters.Any(entry => String.Equals(entry.Name, mobName, StringComparison.CurrentCultureIgnoreCase) && entry.MapIndex == killEntry.MapIndex))
                    {
                        var monster = monsters.FirstOrDefault(entry => String.Equals(entry.Name, mobName, StringComparison.CurrentCultureIgnoreCase) && entry.MapIndex == killEntry.MapIndex);
                        killEntry.ModelID = monster == null ? 0 : monster.ModelID;
                    }
                }
                else
                {
                    killEntry.ModelID = 0;
                }
                DispatcherHelper.Invoke(() => KillWorkerDelegate.OnNewKill(killEntry));
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// </summary>
        private void ProcessLoot(Models.Events.Event e)
        {
            Match matches;
            switch (Constants.GameLanguage)
            {
                case "French":
                    matches = PlayerRegEx.ObtainsFr.Match(e.RawLine);
                    break;
                case "Japanese":
                    matches = PlayerRegEx.ObtainsJa.Match(e.RawLine);
                    break;
                case "German":
                    matches = PlayerRegEx.ObtainsDe.Match(e.RawLine);
                    break;
                default:
                    matches = PlayerRegEx.ObtainsEn.Match(e.RawLine);
                    break;
            }
            if (!matches.Success)
            {
                ParsingLogHelper.Log(LogManager.GetCurrentClassLogger(), "Loot", e);
                return;
            }
            var thing = StringHelper.TitleCase(matches.Groups["item"].Value);
            AttachDropToMonster(thing, e);
        }

        /// <summary>
        /// </summary>
        /// <param name="thing"> </param>
        /// <param name="e"></param>
        private void AttachDropToMonster(string thing, Models.Events.Event e)
        {
            var mobName = ParseControl.Instance.Timeline.FightingRightNow ? ParseControl.LastEngaged : "";
            if (ParseControl.Instance.Timeline.FightingRightNow)
            {
                Fight fight;
                if (ParseControl.Timeline.Fights.TryGet(ParseControl.LastEngaged, out fight))
                {
                    mobName = fight.MobName;
                    Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("DropEvent : {0} Dropped {1}", fight.MobName, thing));
                    if (mobName.Replace(" ", "") != "")
                    {
                        var mobGroup = ParseControl.Timeline.GetSetMob(mobName);
                        mobGroup.SetDrop(thing);
                    }
                }
            }
            else
            {
                ParsingLogHelper.Log(LogManager.GetCurrentClassLogger(), "Loot.NoKillInLastThreeSeconds", e);
            }
            if (LogPublisher.Parse.NeedGreedHistory.Any(item => item.ToLowerInvariant()
                                                                    .Contains(thing.ToLowerInvariant())))
            {
                return;
            }
            try
            {
                var monsters = MonsterWorkerDelegate.UniqueNPCEntries.ToList();
                var lootEntry = new LootEntry(thing);
                if (MonsterWorkerDelegate.CurrentUser != null)
                {
                    lootEntry.MapIndex = MonsterWorkerDelegate.CurrentUser.MapIndex;
                    lootEntry.Coordinate = MonsterWorkerDelegate.CurrentUser.Coordinate;
                }
                if (!String.IsNullOrWhiteSpace(mobName.Replace(" ", "")))
                {
                    if (monsters.Any(entry => String.Equals(entry.Name, mobName, StringComparison.CurrentCultureIgnoreCase) && entry.MapIndex == lootEntry.MapIndex))
                    {
                        var monster = monsters.FirstOrDefault(entry => String.Equals(entry.Name, mobName, StringComparison.CurrentCultureIgnoreCase) && entry.MapIndex == lootEntry.MapIndex);
                        lootEntry.ModelID = monster == null ? 0 : monster.ModelID;
                    }
                }
                else
                {
                    lootEntry.ModelID = 0;
                }
                if (lootEntry.ModelID > 0)
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
            //switch (Common.Constants.GameLanguage)
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
            //    Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("PartyEvent : Joined {0}", who));
            //    ParseControl.Timeline.PublishTimelineEvent(TimelineEventType.PartyJoin, who);
            //    return;
            //}
            //if (disband.Success)
            //{
            //    Logging.Log(LogManager.GetCurrentClassLogger(), "PartyEvent : Disbanned");
            //    ParseControl.Timeline.PublishTimelineEvent(TimelineEventType.PartyDisband, String.Empty);
            //    return;
            //}
            //if (!left.Success)
            //{
            //    return;
            //}
            //who = left.Groups["who"].Value;
            //Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("PartyEvent : Left {0}", who));
            //ParseControl.Timeline.PublishTimelineEvent(TimelineEventType.PartyLeave, who);
        }
    }
}
