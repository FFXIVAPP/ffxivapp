// FFXIVAPP.Plugin.Parse
// TimelineMonitor.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Text.RegularExpressions;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Plugin.Parse.Enums;
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
            Filter = EventParser.AllEvents;
        }

        /// <summary>
        /// </summary>
        /// <param name="e"> </param>
        protected override void HandleEvent(Event e)
        {
            if (String.IsNullOrWhiteSpace(e.RawLine))
            {
                return;
            }
            CheckParty(e.RawLine);
            CheckDrops(e.RawLine);
            CheckFight(e.RawLine);
        }

        /// <summary>
        /// </summary>
        /// <param name="line"> </param>
        private void CheckDrops(string line)
        {
            Match matches;
            switch (Common.Constants.GameLanguage)
            {
                case "French":
                    matches = PlayerRegEx.ObtainsFr.Match(line);
                    break;
                case "Japanese":
                    matches = PlayerRegEx.ObtainsJa.Match(line);
                    break;
                case "German":
                    matches = PlayerRegEx.ObtainsDe.Match(line);
                    break;
                default:
                    matches = PlayerRegEx.ObtainsEn.Match(line);
                    break;
            }
            if (!matches.Success)
            {
                return;
            }
            var thing = StringHelper.TitleCase(matches.Groups["item"].Value)
                                    .Replace("」", "");
            SetDrop(thing);
        }

        /// <summary>
        /// </summary>
        /// <param name="thing"> </param>
        private void SetDrop(string thing)
        {
            Fight fight;
            if (ParseControl.Timeline.Fights.TryGetLastOrCurrent(ParseControl.LastKilled, out fight))
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("DropEvent : {0} dropped {1}", fight.MobName, thing));
                if (fight.MobName.Replace(" ", "") == "")
                {
                    return;
                }
                var mobGroup = ParseControl.Timeline.GetSetMob(fight.MobName);
                mobGroup.SetDropStat(thing);
            }
            else
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("DropEvent : Got loot drop (\"{0}\"), but no current or last fight info. Adding to last killed.", thing));
                if (ParseControl.LastKilled.Replace(" ", "") == "")
                {
                    return;
                }
                var mobGroup = ParseControl.Timeline.GetSetMob(ParseControl.LastKilled);
                mobGroup.SetDropStat(thing);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="line"> </param>
        private void CheckParty(string line)
        {
            Match join;
            Match disband;
            Match left;
            switch (Common.Constants.GameLanguage)
            {
                case "French":
                    join = PlayerRegEx.JoinFr.Match(line);
                    disband = PlayerRegEx.DisbandFr.Match(line);
                    left = PlayerRegEx.LeftFr.Match(line);
                    break;
                case "Japanese":
                    join = PlayerRegEx.JoinJa.Match(line);
                    disband = PlayerRegEx.DisbandJa.Match(line);
                    left = PlayerRegEx.LeftJa.Match(line);
                    break;
                case "German":
                    join = PlayerRegEx.JoinDe.Match(line);
                    disband = PlayerRegEx.DisbandDe.Match(line);
                    left = PlayerRegEx.LeftDe.Match(line);
                    break;
                default:
                    join = PlayerRegEx.JoinEn.Match(line);
                    disband = PlayerRegEx.DisbandEn.Match(line);
                    left = PlayerRegEx.LeftEn.Match(line);
                    break;
            }
            string who;
            if (join.Success)
            {
                who = @join.Groups["who"].Value;
                Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("PartyEvent : Joined {0}", who));
                ParseControl.Timeline.PublishTimelineEvent(TimelineEventType.PartyJoin, who);
                return;
            }
            if (disband.Success)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), "PartyEvent : Disbanned");
                ParseControl.Timeline.PublishTimelineEvent(TimelineEventType.PartyDisband, String.Empty);
                return;
            }
            if (!left.Success)
            {
                return;
            }
            who = left.Groups["who"].Value;
            Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("PartyEvent : Left {0}", who));
            ParseControl.Timeline.PublishTimelineEvent(TimelineEventType.PartyLeave, who);
        }

        /// <summary>
        /// </summary>
        /// <param name="line"> </param>
        private void CheckFight(string line)
        {
            Match matches;
            var you = Common.Constants.CharacterName;
            switch (Common.Constants.GameLanguage)
            {
                case "French":
                    matches = PlayerRegEx.DefeatsFr.Match(line);
                    break;
                case "Japanese":
                    matches = PlayerRegEx.DefeatsJa.Match(line);
                    break;
                case "German":
                    matches = PlayerRegEx.DefeatsDe.Match(line);
                    break;
                default:
                    matches = PlayerRegEx.DefeatsEn.Match(line);
                    break;
            }
            if (!matches.Success)
            {
                return;
            }
            var target = matches.Groups["target"];
            var source = matches.Groups["source"];
            if (!target.Success)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("KillEvent : Got regex match for mob defeat, but no <target> capture group.  Raw Line: {0}", line));
                return;
            }
            if (ParseControl.Timeline.Party.HasGroup(target.Value))
            {
                return;
            }
            if (Regex.IsMatch(target.Value, @"^[Yy]our?$") || target.Value == you)
            {
                return;
            }
            var targetName = StringHelper.TitleCase(target.Value);
            var sourceName = StringHelper.TitleCase(source.Value);
            Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("KillEvent : {0} by : {1}", targetName, sourceName));
            ParseControl.Timeline.PublishTimelineEvent(TimelineEventType.MobKilled, targetName);
        }
    }
}
