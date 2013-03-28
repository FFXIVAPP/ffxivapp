// FFXIVAPP.Plugin.Parse
// Filter.Failed.cs
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
            switch (e.Direction)
            {
                case EventDirection.From:
                    failed = exp.pFailed;
                    switch (e.Subject)
                    {
                        case EventSubject.You:
                            if (failed.Success)
                            {
                                line.Source = String.IsNullOrWhiteSpace(Common.Constants.CharacterName) ? "You" : Common.Constants.CharacterName;
                                UpdatePlayerFailed(failed, line, exp);
                            }
                            break;
                        case EventSubject.Party:
                        case EventSubject.Other:
                            if (failed.Success)
                            {
                                line.Source = _lastAttacker;
                                UpdatePlayerFailed(failed, line, exp);
                            }
                            break;
                        case EventSubject.Engaged:
                        case EventSubject.UnEngaged:
                            break;
                    }
                    _lastAction = "";
                    break;
                case EventDirection.To:
                    failed = exp.mFailed;
                    switch (e.Subject)
                    {
                        case EventSubject.You:
                        case EventSubject.Party:
                        case EventSubject.Other:
                        case EventSubject.Engaged:
                        case EventSubject.UnEngaged:
                            break;
                    }
                    _lastAction = "";
                    break;
            }
            if (!failed.Success)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("Unknown Failed Line -> {0}:{1}", String.Format("{0:X4}", e.Code), exp.Cleaned));
            }
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
                    line.Action = String.IsNullOrWhiteSpace(_lastAction) ? "Attack" : _lastAction;
                    break;
            }
            line.Target = StringHelper.TitleCase(Convert.ToString(failed.Groups["target"].Value));
            ParseControl.Instance.Timeline.PublishTimelineEvent(TimelineEventType.MobFighting, line.Target);
            ParseControl.Instance.Timeline.GetSetMob(line.Target)
                        .SetPlayerStat(line);
            ParseControl.Instance.Timeline.GetSetPlayer(line.Source)
                        .SetAbilityStat(line);
        }
    }
}