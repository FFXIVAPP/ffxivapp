// FFXIVAPP.Plugin.Parse
// TimelineMonitor.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Plugin.Parse.Enums;
using FFXIVAPP.Plugin.Parse.Helpers;
using FFXIVAPP.Plugin.Parse.Models;
using FFXIVAPP.Plugin.Parse.Models.Events;
using FFXIVAPP.Plugin.Parse.Models.Fights;
using FFXIVAPP.Plugin.Parse.RegularExpressions;
using NLog;

#endregion

namespace FFXIVAPP.Plugin.Parse.Monitors
{
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
        protected override void HandleEvent(Event e)
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
        private void ProcessDefeated(Event e)
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
            Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("KillEvent : {0} By : {1}", target, source));
            ParseControl.Timeline.PublishTimelineEvent(TimelineEventType.MobKilled, target);
            Plugin.PHost.ProcessDataByKey(Plugin.PName, Constants.Token, "KillEntry", new Dictionary<string, object>
            {
                {
                    "MobName", target
                }
            });
        }

        /// <summary>
        /// </summary>
        private void ProcessLoot(Event e)
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
        private void AttachDropToMonster(string thing, Event e)
        {
            var mobName = ParseControl.Instance.Timeline.FightingRightNow ? ParseControl.LastKilled : "";
            if (ParseControl.Instance.Timeline.FightingRightNow)
            {
                Fight fight;
                if (ParseControl.Timeline.Fights.TryGet(ParseControl.LastKilled, out fight))
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
            Plugin.PHost.ProcessDataByKey(Plugin.PName, Constants.Token, "LootEntry", new Dictionary<string, object>
            {
                {
                    "ItemName", thing
                },
                {
                    "MobName", mobName
                }
            });
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
