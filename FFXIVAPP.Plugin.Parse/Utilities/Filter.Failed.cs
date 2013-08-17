// FFXIVAPP.Plugin.Parse
// Filter.Failed.cs
// 
// © 2013 Ryan Wilson

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
        private static void ProcessFailed(Event e, Expressions exp)
        {
            var line = new Line
            {
                RawLine = e.RawLine
            };
            var failed = Regex.Match("ph", @"^\.$");
            switch (e.Subject)
            {
                case EventSubject.You:
                case EventSubject.Party:
                    switch (e.Direction)
                    {
                        case EventDirection.Engaged:
                        case EventDirection.UnEngaged:
                            failed = exp.pFailed;
                            switch (failed.Success)
                            {
                                case true:
                                    line.Source = _lastPlayer;
                                    if (e.Subject == EventSubject.You)
                                    {
                                        line.Source = String.IsNullOrWhiteSpace(Constants.CharacterName) ? "You" : Constants.CharacterName;
                                    }
                                    UpdatePlayerFailed(failed, line, exp);
                                    break;
                                case false:
                                    failed = exp.pFailedAuto;
                                    if (failed.Success)
                                    {
                                        _autoAction = true;
                                        line.Source = Convert.ToString(failed.Groups["source"].Value);
                                        if (e.Subject == EventSubject.You)
                                        {
                                            line.Source = String.IsNullOrWhiteSpace(Constants.CharacterName) ? "You" : Constants.CharacterName;
                                        }
                                        UpdatePlayerFailed(failed, line, exp);
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
                            failed = exp.mFailed;
                            switch (failed.Success)
                            {
                                case true:
                                    line.Source = _lastMob;
                                    UpdateMonsterFailed(failed, line, exp);
                                    break;
                                case false:
                                    failed = exp.mFailedAuto;
                                    if (failed.Success)
                                    {
                                        _autoAction = true;
                                        line.Source = Convert.ToString(failed.Groups["source"].Value);
                                        UpdateMonsterFailed(failed, line, exp);
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

        private static void UpdatePlayerFailed(Match failed, Line line, Expressions exp)
        {
            try
            {
                line.Miss = true;
                switch (failed.Groups["source"].Success)
                {
                    case true:
                        line.Action = exp.Attack;
                        break;
                    case false:
                        line.Action = _lastPlayerAction;
                        break;
                }
                line.Target = failed.Groups["target"].Success ? Convert.ToString(failed.Groups["target"].Value) : _lastMob;
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
                            .SetDamageTaken(line);
                ParseControl.Instance.Timeline.GetSetPlayer(line.Source)
                            .SetDamage(line);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
        }

        private static void UpdateMonsterFailed(Match failed, Line line, Expressions exp)
        {
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
                line.Target = failed.Groups["target"].Success ? Convert.ToString(failed.Groups["target"].Value) : _lastPlayer;
                if (Regex.IsMatch(line.Target.ToLower(), exp.You))
                {
                    line.Target = String.IsNullOrWhiteSpace(Constants.CharacterName) ? "You" : Constants.CharacterName;
                }
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
                            .SetDamageTaken(line);
                ParseControl.Instance.Timeline.GetSetMob(line.Source)
                            .SetDamage(line);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
        }
    }
}
