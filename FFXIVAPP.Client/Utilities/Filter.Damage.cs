// FFXIVAPP.Client
// Filter.Damage.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Text.RegularExpressions;
using FFXIVAPP.Client.Enums;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Models.Parse;
using FFXIVAPP.Client.Models.Parse.Events;
using NLog;

#endregion

namespace FFXIVAPP.Client.Utilities
{
    public static partial class Filter
    {
        private static void ProcessDamage(Event e, Expressions exp)
        {
            var line = new Line
            {
                RawLine = e.RawLine
            };
            var damage = Regex.Match("ph", @"^\.$");
            switch (e.Subject)
            {
                case EventSubject.You:
                    switch (e.Direction)
                    {
                        case EventDirection.Engaged:
                        case EventDirection.UnEngaged:
                            damage = exp.pDamage;
                            switch (damage.Success)
                            {
                                case true:
                                    line.Source = _lastNamePlayer;
                                    UpdateDamagePlayer(damage, line, exp, false);
                                    break;
                                case false:
                                    damage = exp.pDamageAuto;
                                    if (damage.Success)
                                    {
                                        _autoAction = true;
                                        line.Source = _lastNamePlayer;
                                        UpdateDamagePlayer(damage, line, exp, false);
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case EventSubject.Party:
                    switch (e.Direction)
                    {
                        case EventDirection.Engaged:
                        case EventDirection.UnEngaged:
                            damage = exp.pDamage;
                            switch (damage.Success)
                            {
                                case true:
                                    line.Source = _lastNameParty;
                                    UpdateDamagePlayer(damage, line, exp);
                                    break;
                                case false:
                                    damage = exp.pDamageAuto;
                                    if (damage.Success)
                                    {
                                        _autoAction = true;
                                        line.Source = Convert.ToString(damage.Groups["source"].Value);
                                        UpdateDamagePlayer(damage, line, exp);
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
                        case EventDirection.You:
                            damage = exp.mDamage;
                            switch (damage.Success)
                            {
                                case true:
                                    line.Source = _lastMobName;
                                    line.Target = String.IsNullOrWhiteSpace(Constants.CharacterName) ? "You" : Constants.CharacterName;
                                    UpdateDamageMonster(damage, line, exp, false);
                                    break;
                                case false:
                                    damage = exp.mDamageAuto;
                                    if (damage.Success)
                                    {
                                        _autoAction = true;
                                        line.Source = Convert.ToString(damage.Groups["source"].Value);
                                        line.Target = String.IsNullOrWhiteSpace(Constants.CharacterName) ? "You" : Constants.CharacterName;
                                        UpdateDamageMonster(damage, line, exp, false);
                                    }
                                    break;
                            }
                            break;
                        case EventDirection.Party:
                            damage = exp.mDamage;
                            switch (damage.Success)
                            {
                                case true:
                                    line.Source = _lastMobName;
                                    line.Target = Convert.ToString(damage.Groups["target"].Value);
                                    UpdateDamageMonster(damage, line, exp);
                                    break;
                                case false:
                                    damage = exp.mDamageAuto;
                                    if (damage.Success)
                                    {
                                        _autoAction = true;
                                        line.Source = Convert.ToString(damage.Groups["source"].Value);
                                        line.Target = Convert.ToString(damage.Groups["target"].Value);
                                        UpdateDamageMonster(damage, line, exp);
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

        private static void UpdateDamagePlayer(Match damage, Line line, Expressions exp, bool isParty = true)
        {
            _isParty = isParty;
            try
            {
                line.Hit = true;
                switch (damage.Groups["source"].Success)
                {
                    case true:
                        line.Action = exp.Attack;
                        break;
                    case false:
                        line.Action = isParty ? _lastActionParty : _lastActionPlayer;
                        break;
                }
                line.Amount = damage.Groups["amount"].Success ? Convert.ToDecimal(damage.Groups["amount"].Value) : 0m;
                line.Block = damage.Groups["block"].Success;
                line.Crit = damage.Groups["crit"].Success;
                line.Modifier = damage.Groups["modifier"].Success ? Convert.ToDecimal(damage.Groups["modifier"].Value) / 100 : 0m;
                line.Parry = damage.Groups["parry"].Success;
                line.Target = Convert.ToString(damage.Groups["target"].Value);
                if (isParty)
                {
                    _lastNameParty = line.Source;
                    if (!_autoAction && (line.IsEmpty() || (!_isMulti && _lastEventParty.Type != EventType.Actions)))
                    {
                        ClearLast(true);
                        return;
                    }
                }
                else
                {
                    if (!_autoAction && (line.IsEmpty() || (!_isMulti && _lastEventPlayer.Type != EventType.Actions)))
                    {
                        return;
                    }
                }
                ParseControl.Instance.Timeline.PublishTimelineEvent(TimelineEventType.MobFighting, line.Target);
                ParseControl.Instance.Timeline.GetSetMob(line.Target)
                            .SetDamageTaken(line);
                ParseControl.Instance.Timeline.GetSetPlayer(line.Source)
                            .SetDamage(line);
            }
            catch (Exception ex)
            {
                ParsingLogHelper.Error(LogManager.GetCurrentClassLogger(), "Damage", exp.Event, ex);
            }
        }

        private static void UpdateDamageMonster(Match damage, Line line, Expressions exp, bool isParty = true)
        {
            _isParty = isParty;
            try
            {
                line.Hit = true;
                switch (damage.Groups["source"].Success)
                {
                    case true:
                        line.Action = exp.Attack;
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
                if (isParty)
                {
                    _lastNameParty = line.Target;
                    if (!_autoAction)
                    {
                        if (line.IsEmpty() || (!_isMulti && _lastEventParty.Type != EventType.Actions))
                        {
                            ClearLast(true);
                            return;
                        }
                    }
                }
                else
                {
                    line.Target = ParseHelper.GetPetFromPlayer(line.Target, exp);
                    _lastNamePlayer = line.Target;
                    if (!_autoAction)
                    {
                        if (line.IsEmpty() || (!_isMulti && _lastEventPlayer.Type != EventType.Actions))
                        {
                            return;
                        }
                    }
                }
                ParseControl.Instance.Timeline.PublishTimelineEvent(TimelineEventType.MobFighting, line.Source);
                ParseControl.Instance.Timeline.GetSetPlayer(line.Target)
                            .SetDamageTaken(line);
                ParseControl.Instance.Timeline.GetSetMob(line.Source)
                            .SetDamage(line);
            }
            catch (Exception ex)
            {
                ParsingLogHelper.Error(LogManager.GetCurrentClassLogger(), "Damage", exp.Event, ex);
            }
        }
    }
}
