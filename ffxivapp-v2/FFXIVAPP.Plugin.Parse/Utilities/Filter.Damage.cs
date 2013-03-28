// FFXIVAPP.Plugin.Parse
// Filter.Damage.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Text.RegularExpressions;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Plugin.Parse.Enums;
using FFXIVAPP.Plugin.Parse.Models;
using FFXIVAPP.Plugin.Parse.Models.Events;
using FFXIVAPP.Plugin.Parse.RegularExpressions;
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
                case EventSubject.Other:
                    switch (e.Direction)
                    {
                        case EventDirection.Self:
                        case EventDirection.Party:
                        case EventDirection.Other:
                        case EventDirection.NPC:
                            break;
                        case EventDirection.Engaged:
                        case EventDirection.UnEngaged:
                            damage = exp.pDamage;
                            if (damage.Success)
                            {
                                line.Source = _lastAttacker;
                                if (e.Subject == EventSubject.You)
                                {
                                    line.Source = String.IsNullOrWhiteSpace(Common.Constants.CharacterName) ? "You" : Common.Constants.CharacterName;
                                }
                                UpdatePlayerDamage(damage, line, exp);
                            }
                            break;
                    }
                    break;
                case EventSubject.NPC:
                    break;
                case EventSubject.Engaged:
                case EventSubject.UnEngaged:
                    break;
            }
            _lastAction = "";
            if (!damage.Success)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("Unknown Failed Line -> {0}:{1}", String.Format("{0:X4}", e.Code), exp.Cleaned));
            }
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
                    line.Action = String.IsNullOrWhiteSpace(_lastAction) ? "Attack" : _lastAction;
                    break;
            }
            line.Amount = Convert.ToDecimal(damage.Groups["amount"].Value);
            line.Crit = damage.Groups["crit"].Success;
            line.Target = StringHelper.TitleCase(Convert.ToString(damage.Groups["target"].Value));
            ParseControl.Instance.Timeline.PublishTimelineEvent(TimelineEventType.MobFighting, line.Target);
            ParseControl.Instance.Timeline.GetSetMob(line.Target)
                        .SetPlayerStat(line);
            ParseControl.Instance.Timeline.GetSetPlayer(line.Source)
                        .SetAbilityStat(line);
        }
    }
}