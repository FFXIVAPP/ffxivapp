// FFXIVAPP.Plugin.Parse
// Filter.Failed.cs
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
                            if (failed.Success)
                            {
                                line.Source = _lastPlayer;
                                if (e.Subject == EventSubject.You)
                                {
                                    line.Source = String.IsNullOrWhiteSpace(Common.Constants.CharacterName) ? "You" : Common.Constants.CharacterName;
                                }
                                UpdatePlayerFailed(failed, line, exp);
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
            if (failed.Success)
            {
                return;
            }
            var data = String.Format("Unknown Failed Line -> [Subject:{0}][Direction:{1}] {2}:{3}", e.Subject, e.Direction, String.Format("{0:X4}", e.Code), exp.Cleaned);
            Logging.Log(LogManager.GetCurrentClassLogger(), data);
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
                    line.Action = String.IsNullOrWhiteSpace(_lastPlayerAction) ? "Attack" : _lastPlayerAction;
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
