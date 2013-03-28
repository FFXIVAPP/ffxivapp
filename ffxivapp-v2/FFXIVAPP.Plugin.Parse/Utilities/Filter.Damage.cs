// FFXIVAPP.Plugin.Parse
// Filter.Damage.cs
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
using NLog;

#endregion

namespace FFXIVAPP.Plugin.Parse.Utilities
{
    public static partial class Filter
    {
        private static void ProcessDamage(Event e, Expressions exp)
        {
            var line = new Line();
            var damage = Regex.Match("ph", @"^\.$");
            switch (e.Subject)
            {
                case EventSubject.You:
                case EventSubject.Party:
                    switch (e.Direction)
                    {
                        case EventDirection.Self:
                        case EventDirection.You:
                        case EventDirection.Party:
                        case EventDirection.Other:
                        case EventDirection.NPC:
                            break;
                        case EventDirection.Engaged:
                        case EventDirection.UnEngaged:
                            damage = exp.pDamage;
                            if (damage.Success)
                            {
                                line.Source = _lastPlayer;
                                if (e.Subject == EventSubject.You)
                                {
                                    line.Source = String.IsNullOrWhiteSpace(Common.Constants.CharacterName) ? "You" : Common.Constants.CharacterName;
                                }
                                UpdatePlayerDamage(damage, line, exp);
                            }
                            break;
                    }
                    break;
                case EventSubject.Other:
                case EventSubject.NPC:
                    break;
                case EventSubject.Engaged:
                case EventSubject.UnEngaged:
                    switch (e.Direction)
                    {
                        case EventDirection.Self:
                        case EventDirection.You:
                        case EventDirection.Party:
                            damage = exp.mDamage;
                            if (damage.Success)
                            {
                                line.Source = _lastMob;
                                line.Target = StringHelper.TitleCase(Convert.ToString(damage.Groups["target"].Value));
                                if (line.Target.ToLower() == "you")
                                {
                                    line.Target = String.IsNullOrWhiteSpace(Common.Constants.CharacterName) ? "You" : Common.Constants.CharacterName;
                                }
                                UpdateMonsterDamage(damage, line, exp);
                            }
                            break;
                        case EventDirection.Other:
                        case EventDirection.NPC:
                        case EventDirection.Engaged:
                        case EventDirection.UnEngaged:
                            break;
                    }
                    break;
            }
            _lastPlayerAction = "";
            _lastMobAction = "";
            if (damage.Success)
            {
                return;
            }
            var data = String.Format("Unknown Damage Line -> [Subject:{0}][Direction:{1}] {2}:{3}", e.Subject, e.Direction, String.Format("{0:X4}", e.Code), exp.Cleaned);
            Logging.Log(LogManager.GetCurrentClassLogger(), data);
        }

        private static void UpdatePlayerDamage(Match damage, Line line, Expressions exp)
        {
            line.Hit = true;
            switch (damage.Groups["source"].Success)
            {
                case true:
                    line.Action = "Attack";
                    break;
                case false:
                    line.Action = String.IsNullOrWhiteSpace(_lastPlayerAction) ? "Attack" : _lastPlayerAction;
                    break;
            }
            line.Amount = damage.Groups["amount"].Success ? Convert.ToDecimal(damage.Groups["amount"].Value) : 0m;
            line.Crit = damage.Groups["crit"].Success;
            line.Target = StringHelper.TitleCase(Convert.ToString(damage.Groups["target"].Value));
            ParseControl.Instance.Timeline.PublishTimelineEvent(TimelineEventType.MobFighting, line.Target);
            ParseControl.Instance.Timeline.GetSetMob(line.Target)
                        .SetPlayerStat(line);
            ParseControl.Instance.Timeline.GetSetPlayer(line.Source)
                        .SetAbilityStat(line);
        }

        private static void UpdateMonsterDamage(Match damage, Line line, Expressions exp)
        {
            line.Hit = true;
            switch (damage.Groups["source"].Success)
            {
                case true:
                    line.Action = "Attack";
                    break;
                case false:
                    line.Action = String.IsNullOrWhiteSpace(_lastMobAction) ? "Attack" : _lastMobAction;
                    break;
            }
            line.Amount = damage.Groups["amount"].Success ? Convert.ToDecimal(damage.Groups["amount"].Value) : 0m;
            line.Crit = damage.Groups["crit"].Success;
            ParseControl.Instance.Timeline.PublishTimelineEvent(TimelineEventType.MobFighting, line.Source);
            ParseControl.Instance.Timeline.GetSetPlayer(line.Target)
                        .SetDamageStat(line);
        }
    }
}
