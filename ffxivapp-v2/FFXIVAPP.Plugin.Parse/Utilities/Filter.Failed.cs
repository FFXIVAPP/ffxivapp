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
            Match failed;
            if (e.Direction == EventDirection.From)
            {
                failed = exp.pFailed;
                switch (e.Subject)
                {
                    case EventSubject.You:
                        if (failed.Success)
                        {
                            line.Source = String.IsNullOrWhiteSpace(Common.Constants.CharacterName) ? "You" : Common.Constants.CharacterName;
                        }
                        break;
                    case EventSubject.Party:
                        if (failed.Success)
                        {
                            line.Source = _lastAttacker;
                        }
                        break;
                    case EventSubject.Other:
                        if (failed.Success)
                        {
                            line.Source = _lastAttacker;
                        }
                        break;
                    case EventSubject.Engaged:
                        break;
                    case EventSubject.UnEngaged:
                        break;
                }
                if (failed.Success)
                {
                    Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("Failed Line -> {0}", exp.Cleaned));
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
                _lastAction = "";
            }
        }
    }
}
