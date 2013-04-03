// FFXIVAPP.Plugin.Parse
// Filter.Damage.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Text.RegularExpressions;
using FFXIVAPP.Plugin.Parse.Enums;
using FFXIVAPP.Plugin.Parse.Helpers;
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
                        case EventDirection.Engaged:
                        case EventDirection.UnEngaged:
                            damage = exp.pDamage;
                            switch (damage.Success)
                            {
                                case true:
                                    line.Source = _lastPlayer;
                                    if (e.Subject == EventSubject.You)
                                    {
                                        line.Source = String.IsNullOrWhiteSpace(Common.Constants.CharacterName) ? "You" : Common.Constants.CharacterName;
                                    }
                                    UpdatePlayerDamage(damage, line, exp);
                                    break;
                                case false:
                                    damage = exp.pDamageAuto;
                                    if (damage.Success)
                                    {
                                        _autoAction = true;
                                        line.Source = Convert.ToString(damage.Groups["source"].Value);
                                        if (e.Subject == EventSubject.You)
                                        {
                                            line.Source = String.IsNullOrWhiteSpace(Common.Constants.CharacterName) ? "You" : Common.Constants.CharacterName;
                                        }
                                        UpdatePlayerDamage(damage, line, exp);
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case EventSubject.Engaged:
                case EventSubject.UnEngaged:
                    switch (e.Direction)
                    {
                        case EventDirection.Self:
                        case EventDirection.You:
                        case EventDirection.Party:
                            damage = exp.mDamage;
                            switch (damage.Success)
                            {
                                case true:
                                    line.Source = _lastMob;
                                    line.Target = Convert.ToString(damage.Groups["target"].Value);
                                    if (line.Target.ToLower() == "you")
                                    {
                                        line.Target = String.IsNullOrWhiteSpace(Common.Constants.CharacterName) ? "You" : Common.Constants.CharacterName;
                                    }
                                    UpdateMonsterDamage(damage, line, exp);
                                    break;
                                case false:
                                    damage = exp.mDamageAuto;
                                    if (damage.Success)
                                    {
                                        _autoAction = true;
                                        line.Source = Convert.ToString(damage.Groups["source"].Value);
                                        line.Target = Convert.ToString(damage.Groups["target"].Value);
                                        if (line.Target.ToLower() == "you")
                                        {
                                            line.Target = String.IsNullOrWhiteSpace(Common.Constants.CharacterName) ? "You" : Common.Constants.CharacterName;
                                        }
                                        UpdateMonsterDamage(damage, line, exp);
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
            }
            if (damage.Success)
            {
                return;
            }
            ClearLast();
            ParsingLogHelper.Log(LogManager.GetCurrentClassLogger(), "Damage", e, exp);
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
                    line.Action = _lastPlayerAction;
                    break;
            }
            line.Amount = damage.Groups["amount"].Success ? Convert.ToDecimal(damage.Groups["amount"].Value) : 0m;
            line.Block = damage.Groups["block"].Success;
            line.Crit = damage.Groups["crit"].Success;
            line.Modifier = damage.Groups["modifier"].Success ? Convert.ToDecimal(damage.Groups["modifier"].Value) / 100 : 0m;
            line.Parry = damage.Groups["parry"].Success;
            line.Target = Convert.ToString(damage.Groups["target"].Value);
            if (!_autoAction)
            {
                if (line.IsEmpty() || (!_isMulti && _lastEvent.Type != EventType.Actions))
                {
                    ClearLast(true);
                    return;
                }
            }
            _lastPlayer = line.Source;
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
                    line.Action = _lastMobAction;
                    break;
            }
            line.Amount = damage.Groups["amount"].Success ? Convert.ToDecimal(damage.Groups["amount"].Value) : 0m;
            line.Block = damage.Groups["block"].Success;
            line.Crit = damage.Groups["crit"].Success;
            line.Modifier = damage.Groups["modifier"].Success ? Convert.ToDecimal(damage.Groups["modifier"].Value) / 100 : 0m;
            line.Parry = damage.Groups["parry"].Success;
            if (!_autoAction)
            {
                if (line.IsEmpty() || (!_isMulti && _lastEvent.Type != EventType.Actions))
                {
                    ClearLast(true);
                    return;
                }
            }
            _lastPlayer = line.Target;
            ParseControl.Instance.Timeline.PublishTimelineEvent(TimelineEventType.MobFighting, line.Source);
            ParseControl.Instance.Timeline.GetSetPlayer(line.Target)
                        .SetDamageStat(line);
        }
    }
}
