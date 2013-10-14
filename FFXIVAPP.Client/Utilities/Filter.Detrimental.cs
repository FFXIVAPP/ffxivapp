// FFXIVAPP.Client
// Filter.Detrimental.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Text.RegularExpressions;
using FFXIVAPP.Client.Plugins.Parse.Enums;
using FFXIVAPP.Client.Plugins.Parse.Helpers;
using FFXIVAPP.Client.Plugins.Parse.Models;
using FFXIVAPP.Client.Plugins.Parse.Models.Events;
using FFXIVAPP.Client.Plugins.Parse.Models.StatGroups;
using NLog;

#endregion

namespace FFXIVAPP.Client.Utilities {
    public static partial class Filter {
        private static void ProcessDetrimental(Event e, Expressions exp) {
            var line = new Line {
                RawLine = e.RawLine
            };
            var detrimental = Regex.Match("ph", @"^\.$");
            switch (e.Subject) {
                case EventSubject.You:
                    switch (e.Direction) {
                        case EventDirection.Engaged:
                        case EventDirection.UnEngaged:
                            detrimental = exp.mDetrimentalGain;
                            if (detrimental.Success) {
                                line.Source = _lastNamePlayer;
                                line.StatusEffect = StatusEffect.DetrimentalGain;
                                UpdateDetrimentalPlayer(detrimental, line, exp, false);
                            }
                            break;
                    }
                    break;
                case EventSubject.Party:
                    switch (e.Direction) {
                        case EventDirection.Engaged:
                        case EventDirection.UnEngaged:
                            detrimental = exp.mDetrimentalGain;
                            if (detrimental.Success) {
                                line.Source = _lastNameParty;
                                line.StatusEffect = StatusEffect.DetrimentalGain;
                                UpdateDetrimentalPlayer(detrimental, line, exp);
                            }
                            break;
                    }
                    break;
                case EventSubject.Other:
                case EventSubject.NPC:
                    break;
                case EventSubject.Engaged:
                case EventSubject.UnEngaged:
                    switch (e.Direction) {
                        case EventDirection.You:
                            detrimental = exp.mDetrimentalGain;
                            if (detrimental.Success) {
                                line.Source = _lastMobName;
                                line.StatusEffect = StatusEffect.DetrimentalGain;
                                UpdateDetrimentalMonster(detrimental, line, exp, false);
                            }
                            break;
                        case EventDirection.Party:
                            detrimental = exp.mDetrimentalGain;
                            if (detrimental.Success) {
                                line.Source = _lastMobName;
                                line.StatusEffect = StatusEffect.DetrimentalGain;
                                UpdateDetrimentalMonster(detrimental, line, exp);
                            }
                            break;
                    }
                    break;
            }
            if (detrimental.Success) {
                return;
            }
            ClearLast();
            ParsingLogHelper.Log(LogManager.GetCurrentClassLogger(), "Detrimental", e, exp);
        }

        private static void UpdateDetrimentalPlayer(Match detrimental, Line line, Expressions exp, bool isParty = true) {
            _isParty = isParty;
            try {
                line.Source = isParty ? _lastNameParty : _lastNamePlayer;
                line.StatusEffectName = Convert.ToString(detrimental.Groups["status"].Value);
                line.Action = line.StatusEffectName;
                line.Target = Convert.ToString(detrimental.Groups["target"].Value);
                if (line.IsEmpty()) {
                    throw new Exception("LineIsEmpty:(Source|Target|Action)IsEmptyOrNull");
                }
                Player source;
                switch (line.StatusEffect) {
                    case StatusEffect.DetrimentalGain:
                        source = ParseControl.Instance.Timeline.GetSetPlayer(line.Source);
                        line.Amount = source.LastDamageAmount;
                        break;
                    default:
                        return;
                }
                ParseControl.Instance.Timeline.PublishTimelineEvent(TimelineEventType.MobFighting, line.Target);
                if (!DamageOverTimeHelper.PlayerActions.ContainsKey(line.StatusEffectName.ToLower())) {
                    return;
                }
                source.SetupDamageOverTimeAction(line);
            }
            catch (Exception ex) {
                ParsingLogHelper.Error(LogManager.GetCurrentClassLogger(), "Detrimental", exp.Event, ex);
            }
        }

        private static void UpdateDetrimentalMonster(Match detrimental, Line line, Expressions exp, bool isParty = true) {
            _isParty = isParty;
            try {
                line.Source = _lastMobName;
                line.StatusEffectName = Convert.ToString(detrimental.Groups["status"].Value);
                line.Action = line.StatusEffectName;
                line.Target = Convert.ToString(detrimental.Groups["target"].Value);
                if (line.IsEmpty()) {
                    throw new Exception("LineIsEmpty:(Source|Target|Action)IsEmptyOrNull");
                }
                ParseControl.Instance.Timeline.PublishTimelineEvent(TimelineEventType.MobFighting, line.Target);
                Monster source;
                switch (line.StatusEffect) {
                    case StatusEffect.DetrimentalGain:
                        source = ParseControl.Instance.Timeline.GetSetMob(line.Source);
                        line.Amount = source.LastDamageAmount;
                        break;
                    default:
                        return;
                }
                if (!DamageOverTimeHelper.MonsterActions.ContainsKey(line.StatusEffectName.ToLower())) {
                    return;
                }
                source.SetupDamageOverTimeAction(line);
            }
            catch (Exception ex) {
                ParsingLogHelper.Error(LogManager.GetCurrentClassLogger(), "Detrimental", exp.Event, ex);
            }
        }
    }
}
