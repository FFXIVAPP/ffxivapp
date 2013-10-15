// FFXIVAPP.Client
// Filter.Failed.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Text.RegularExpressions;
using FFXIVAPP.Client.Plugins.Parse.Enums;
using FFXIVAPP.Client.Plugins.Parse.Helpers;
using FFXIVAPP.Client.Plugins.Parse.Models;
using NLog;

#endregion

namespace FFXIVAPP.Client.Plugins.Parse.Utilities
{
    public static partial class Filter
    {
        private static void ProcessFailed(Models.Events.Event e, Expressions exp)
        {
            var line = new Line
            {
                RawLine = e.RawLine
            };
            var failed = Regex.Match("ph", @"^\.$");
            switch (e.Subject)
            {
                case EventSubject.You:
                    switch (e.Direction)
                    {
                        case EventDirection.Engaged:
                        case EventDirection.UnEngaged:
                            failed = exp.pFailed;
                            switch (failed.Success)
                            {
                                case true:
                                    line.Source = _lastNamePlayer;
                                    UpdateFailedPlayer(failed, line, exp, false);
                                    break;
                                case false:
                                    failed = exp.pFailedAuto;
                                    if (failed.Success)
                                    {
                                        _autoAction = true;
                                        line.Source = _lastNamePlayer;
                                        UpdateFailedPlayer(failed, line, exp, false);
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
                            failed = exp.pFailed;
                            switch (failed.Success)
                            {
                                case true:
                                    line.Source = _lastNameParty;
                                    UpdateFailedPlayer(failed, line, exp);
                                    break;
                                case false:
                                    failed = exp.pFailedAuto;
                                    if (failed.Success)
                                    {
                                        _autoAction = true;
                                        line.Source = Convert.ToString(failed.Groups["source"].Value);
                                        UpdateFailedPlayer(failed, line, exp);
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
                            failed = exp.mFailed;
                            switch (failed.Success)
                            {
                                case true:
                                    line.Source = _lastMobName;
                                    UpdateFailedMonster(failed, line, exp, false);
                                    break;
                                case false:
                                    failed = exp.mFailedAuto;
                                    if (failed.Success)
                                    {
                                        _autoAction = true;
                                        line.Source = Convert.ToString(failed.Groups["source"].Value);
                                        UpdateFailedMonster(failed, line, exp, false);
                                    }
                                    break;
                            }
                            break;
                        case EventDirection.Party:
                            failed = exp.mFailed;
                            switch (failed.Success)
                            {
                                case true:
                                    line.Source = _lastMobName;
                                    UpdateFailedMonster(failed, line, exp);
                                    break;
                                case false:
                                    failed = exp.mFailedAuto;
                                    if (failed.Success)
                                    {
                                        _autoAction = true;
                                        line.Source = Convert.ToString(failed.Groups["source"].Value);
                                        UpdateFailedMonster(failed, line, exp);
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
            }
            if (failed.Success)
            {
                return;
            }
            ClearLast();
            ParsingLogHelper.Log(LogManager.GetCurrentClassLogger(), "Failed", e, exp);
        }

        private static void UpdateFailedPlayer(Match failed, Line line, Expressions exp, bool isParty = true)
        {
            _isParty = isParty;
            try
            {
                line.Miss = true;
                switch (failed.Groups["source"].Success)
                {
                    case true:
                        line.Action = exp.Attack;
                        break;
                    case false:
                        line.Action = isParty ? _lastActionParty : _lastActionPlayer;
                        break;
                }
                line.Target = failed.Groups["target"].Success ? Convert.ToString(failed.Groups["target"].Value) : _lastMobName;
                if (isParty)
                {
                    _lastNameParty = line.Source;
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
                    line.Target = ParseHelper.GetPetFromPlayer(line.Source, exp);
                    _lastNamePlayer = line.Source;
                    if (!_autoAction)
                    {
                        if (line.IsEmpty() || (!_isMulti && _lastEventPlayer.Type != EventType.Actions))
                        {
                            return;
                        }
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
                ParsingLogHelper.Error(LogManager.GetCurrentClassLogger(), "Failed", exp.Event, ex);
            }
        }

        private static void UpdateFailedMonster(Match failed, Line line, Expressions exp, bool isParty = true)
        {
            _isParty = isParty;
            try
            {
                line.Miss = true;
                switch (failed.Groups["source"].Success)
                {
                    case true:
                        line.Action = exp.Attack;
                        break;
                    case false:
                        line.Action = _lastMobAction;
                        break;
                }
                line.Target = failed.Groups["target"].Success ? Convert.ToString(failed.Groups["target"].Value) : isParty ? _lastNameParty : _lastNamePlayer;
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
                ParsingLogHelper.Error(LogManager.GetCurrentClassLogger(), "Failed", exp.Event, ex);
            }
        }
    }
}
