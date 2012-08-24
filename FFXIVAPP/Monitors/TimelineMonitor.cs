// FFXIVAPP
// TimelineMonitor.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Text.RegularExpressions;
using FFXIVAPP.Classes;
using FFXIVAPP.Classes.RegExs;
using FFXIVAPP.Models;
using NLog;

namespace FFXIVAPP.Monitors
{
    public class TimelineMonitor : EventMonitor
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// </summary>
        /// <param name="ffxivInstance"> </param>
        public TimelineMonitor(FFXIV ffxivInstance) : base("Timeline", ffxivInstance)
        {
            Filter = ((UInt16) EventType.Notice | (UInt16) EventSubject.You | (UInt16) EventSubject.Party | (UInt16) EventDirection.On);
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
        private void CheckFight(String line)
        {
            Match matches = null;
            var you = Settings.Default.CharacterName;
            switch (Settings.Default.Language)
            {
                case "English":
                    matches = Player.DefeatsEn.Match(line);
                    break;
                case "French":
                    matches = Player.DefeatsFr.Match(line);
                    break;
                case "Japanese":
                    matches = Player.DefeatsJa.Match(line);
                    break;
                case "German":
                    matches = Player.DefeatsDe.Match(line);
                    break;
            }
            if (matches == null || !matches.Success)
            {
                return;
            }
            var target = matches.Groups["target"];
            var source = matches.Groups["source"];
            if (!target.Success)
            {
                Logger.Debug("KillEvent : Got regex match for mob defeat, but no <target> capture group.  Raw Line: {0}", line);
                return;
            }
            if (FFXIVInstance.Timeline.Party.HasGroup(target.Value))
            {
                return;
            }
            if (Regex.IsMatch(target.Value, @"^[Yy]our?$") || target.Value == you)
            {
                return;
            }
            var t = FFXIV.TitleCase(target.Value);
            var s = FFXIV.TitleCase(source.Value);
            Logger.Debug("KillEvent : {0} by : {1}", t, s);
            FFXIVInstance.Timeline.PublishTimelineEvent(TimelineEventType.MobKilled, t);
        }

        /// <summary>
        /// </summary>
        /// <param name="line"> </param>
        private void CheckDrops(String line)
        {
            var matches = Regex.Match("ph", @"^\.$");
            switch (Settings.Default.Language)
            {
                case "English":
                    matches = Player.ObtainsEn.Match(line);
                    break;
                case "French":
                    matches = Player.ObtainsFr.Match(line);
                    break;
                case "Japanese":
                    matches = Player.ObtainsJa.Match(line);
                    break;
                case "German":
                    matches = Player.ObtainsDe.Match(line);
                    break;
            }
            if (matches.Success)
            {
                var thing = FFXIV.TitleCase(matches.Groups["item"].Value).Replace("」", "");
                AddDrop(thing);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="thing"> </param>
        private void AddDrop(String thing)
        {
            Fight fight;
            if (FFXIVInstance.Timeline.Fights.TryGetLastOrCurrent(FFXIV.LastKilled, out fight))
            {
                Logger.Debug("DropEvent : {0} dropped {1}", fight.MobName, thing);
                if (fight.MobName.Replace(" ", "") == "")
                {
                    return;
                }
                var mobGroup = FFXIVInstance.Timeline.GetSetMob(fight.MobName);
                mobGroup.AddDropStats(thing);
            }
            else
            {
                Logger.Debug("DropEvent : Got loot drop (\"{0}\"), but no current or last fight info. Adding to last killed.", thing);
                if (FFXIV.LastKilled.Replace(" ", "") == "")
                {
                    return;
                }
                var mobGroup = FFXIVInstance.Timeline.GetSetMob(FFXIV.LastKilled);
                mobGroup.AddDropStats(thing);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="line"> </param>
        private void CheckParty(String line)
        {
            var join = Regex.Match("ph", @"^\.$");
            var disband = Regex.Match("ph", @"^\.$");
            var left = Regex.Match("ph", @"^\.$");
            switch (Settings.Default.Language)
            {
                case "English":
                    join = Player.JoinEn.Match(line);
                    disband = Player.DisbandEn.Match(line);
                    left = Player.LeftEn.Match(line);
                    break;
                case "French":
                    join = Player.JoinFr.Match(line);
                    disband = Player.DisbandFr.Match(line);
                    left = Player.LeftFr.Match(line);
                    break;
                case "Japanese":
                    join = Player.JoinJa.Match(line);
                    disband = Player.DisbandJa.Match(line);
                    left = Player.LeftJa.Match(line);
                    break;
                case "German":
                    join = Player.JoinDe.Match(line);
                    disband = Player.DisbandDe.Match(line);
                    left = Player.LeftDe.Match(line);
                    break;
            }
            if (join.Success)
            {
                var who = join.Groups["who"].Value;
                Logger.Debug("PartyEvent : Joined {0}", who);
                FFXIVInstance.Timeline.PublishTimelineEvent(TimelineEventType.PartyJoin, who);
                return;
            }
            if (disband.Success)
            {
                Logger.Debug("PartyEvent : Disbanned");
                FFXIVInstance.Timeline.PublishTimelineEvent(TimelineEventType.PartyDisband, String.Empty);
                return;
            }
            if (left.Success)
            {
                var who = left.Groups["who"].Value;
                Logger.Debug("PartyEvent : Left {0}", who);
                FFXIVInstance.Timeline.PublishTimelineEvent(TimelineEventType.PartyLeave, who);
            }
        }
    }
}