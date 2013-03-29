// FFXIVAPP.Plugin.Parse
// Filter.Failed.cs
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
        private static void ProcessFailed(Event e, Expressions exp)
        {
            var line = new Line();
            var failed = Regex.Match("ph", @"^\.$");
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
                            failed = exp.pFailed;
                            switch (failed.Success)
                            {
                                case true:
                                    line.Source = _lastPlayer;
                                    if (e.Subject == EventSubject.You)
                                    {
                                        line.Source = String.IsNullOrWhiteSpace(Common.Constants.CharacterName) ? "You" : Common.Constants.CharacterName;
                                    }
                                    UpdatePlayerFailed(failed, line, exp);
                                    break;
                                case false:
                                    failed = exp.mFailedAuto;
                                    if (failed.Success)
                                    {
                                        line.Source = Convert.ToString(failed.Groups["source"].Value);
                                        if (e.Subject == EventSubject.You)
                                        {
                                            line.Source = String.IsNullOrWhiteSpace(Common.Constants.CharacterName) ? "You" : Common.Constants.CharacterName;
                                        }
                                        UpdatePlayerFailed(failed, line, exp);
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case EventSubject.Other:
                case EventSubject.NPC:
                case EventSubject.Engaged:
                case EventSubject.UnEngaged:
                    switch (e.Direction)
                    {
                        case EventDirection.Self:
                        case EventDirection.You:
                        case EventDirection.Party:
                        case EventDirection.Other:
                        case EventDirection.NPC:
                        case EventDirection.Engaged:
                        case EventDirection.UnEngaged:
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
            line.Miss = true;
            switch (failed.Groups["source"].Success)
            {
                case true:
                    line.Action = "Attack";
                    break;
                case false:
                    line.Action = _lastPlayerAction;
                    break;
            }
            line.Target = Convert.ToString(failed.Groups["target"].Value);
            if (line.IsEmpty())
            {
                return;
            }
            _lastPlayer = line.Source;
            ParseControl.Instance.Timeline.PublishTimelineEvent(TimelineEventType.MobFighting, line.Target);
            ParseControl.Instance.Timeline.GetSetMob(line.Target)
                        .SetPlayerStat(line);
            ParseControl.Instance.Timeline.GetSetPlayer(line.Source)
                        .SetAbilityStat(line);
        }
    }
}
