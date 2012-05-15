// ParseModXIV
// TimelineMonitor.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Text.RegularExpressions;
using ParseModXIV.Classes;
using ParseModXIV.Model;

namespace ParseModXIV.Monitors
{
    public class TimelineMonitor : EventMonitor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parseModInstance"></param>
        public TimelineMonitor(ParseMod parseModInstance) : base("Timeline", parseModInstance)
        {
            Filter = ((UInt16) EventType.Notice | (UInt16) EventSubject.You | (UInt16) EventSubject.Party | (UInt16) EventDirection.On);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
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
        /// 
        /// </summary>
        /// <param name="line"></param>
        private void CheckFight(String line)
        {
            Match matches = null;
            var you = Settings.Default.CharacterName;
            switch (Settings.Default.Language)
            {
                case "English":
                    matches = RegExpsEn.Defeated.Match(line);
                    break;
                case "French":
                    matches = RegExpsFr.Defeated.Match(line);
                    break;
            }
            if (matches == null || !matches.Success)
            {
                return;
            }
            ParseMod.DeathCount++;
            var whatDefeated = matches.Groups["whatDefeated"];
            //var whoDefeated = matches.Groups["whoDefeated"];
            if (!whatDefeated.Success)
            {
                //logger.Error("Got regex match for mob defeat, but no whatDefeated capture group.  Raw Line: {0}", line);
                return;
            }
            if (ParseModInstance.Timeline.Party.HasGroup(whatDefeated.Value))
            {
                return;
            }
            if (Regex.IsMatch(whatDefeated.Value, @"^[Yy]our?$") || whatDefeated.Value == you)
            {
                return;
            }
            var what = ParseMod.TitleCase(whatDefeated.Value, true);
            //var who = ParseMod.TitleCase(whoDefeated.Value, true);
            //logger.Trace("Mob Defeated : {0} by : {1}", what, who);
            ParseModInstance.Timeline.PublishTimelineEvent(TimelineEventType.MobKilled, what);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        private void CheckDrops(String line)
        {
            switch (Settings.Default.Language)
            {
                case "English":
                {
                    var matches = RegExpsEn.Obtains.Match(line);
                    if (matches.Success)
                    {
                        var thing = ParseMod.TitleCase(matches.Groups["item"].Value, true);
                        AddDrop(thing);
                    }
                }
                    break;
                case "French":
                {
                    var matches = RegExpsFr.Obtains.Match(line);
                    if (matches.Success)
                    {
                        var thing = ParseMod.TitleCase(matches.Groups["item"].Value, true);
                        AddDrop(thing);
                    }
                }
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thing"></param>
        private void AddDrop(String thing)
        {
            Fight fight;
            if (ParseModInstance.Timeline.Fights.TryGetLastOrCurrent(ParseMod.LastKilled, out fight))
            {
                //logger.Trace("{0} dropped {1}", fight.MobName, thing);
                if (fight.MobName.Replace(" ", "") == "")
                {
                    return;
                }
                var mobGroup = ParseModInstance.Timeline.GetOrAddStatsForMob(fight.MobName);
                mobGroup.AddDropStats(thing);
            }
            else
            {
                //logger.Warn("Got loot drop (\"{0}\"), but no current or last fight info. Adding to last killed.", thing);
                if (ParseMod.LastKilled.Replace(" ", "") == "")
                {
                    return;
                }
                var mobGroup = ParseModInstance.Timeline.GetOrAddStatsForMob(ParseMod.LastKilled);
                mobGroup.AddDropStats(thing);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        private void CheckParty(String line)
        {
            switch (Settings.Default.Language)
            {
                case "English":
                {
                    var matches = RegExpsEn.JoinParty.Match(line);
                    if (matches.Success)
                    {
                        var whoJoined = matches.Groups["whoJoined"].Value;
                        //logger.Trace("Party Join Event : {0}", whoJoined);
                        ParseModInstance.Timeline.PublishTimelineEvent(TimelineEventType.PartyJoin, whoJoined);
                    }
                    else if (RegExpsEn.DisbandParty.Match(line).Success)
                    {
                        //logger.Trace("Party Disband Event");
                        ParseModInstance.Timeline.PublishTimelineEvent(TimelineEventType.PartyDisband, String.Empty);
                    }
                    else
                    {
                        var leftParty = RegExpsEn.LeaveParty.Match(line);
                        if (leftParty.Success)
                        {
                            var whoLeft = leftParty.Groups["whoLeft"].Value;
                            //logger.Trace("Party leave event : {0}", whoLeft);
                            ParseModInstance.Timeline.PublishTimelineEvent(TimelineEventType.PartyLeave, whoLeft);
                        }
                    }
                }
                    break;
                case "French":
                {
                    var matches = RegExpsFr.JoinParty.Match(line);
                    if (matches.Success)
                    {
                        var whoJoined = matches.Groups["whoJoined"].Value;
                        //logger.Trace("Party Join Event : {0}", whoJoined);
                        ParseModInstance.Timeline.PublishTimelineEvent(TimelineEventType.PartyJoin, whoJoined);
                    }
                    else if (RegExpsFr.DisbandParty.Match(line).Success)
                    {
                        //logger.Trace("Party Disband Event");
                        ParseModInstance.Timeline.PublishTimelineEvent(TimelineEventType.PartyDisband, String.Empty);
                    }
                    else
                    {
                        var leftParty = RegExpsFr.LeaveParty.Match(line);
                        if (leftParty.Success)
                        {
                            var whoLeft = leftParty.Groups["whoLeft"].Value;
                            //logger.Trace("Party leave event : {0}", whoLeft);
                            ParseModInstance.Timeline.PublishTimelineEvent(TimelineEventType.PartyLeave, whoLeft);
                        }
                    }
                }
                    break;
            }
        }
    }
}