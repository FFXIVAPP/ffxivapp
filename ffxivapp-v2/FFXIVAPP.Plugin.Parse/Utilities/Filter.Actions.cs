// FFXIVAPP.Plugin.Parse
// Filter.Actions.cs
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
        private static void ProcessActions(Event e, Expressions exp)
        {
            var line = new Line();
            var actions = Regex.Match("ph", @"^\.$");
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
                            actions = exp.pActions;
                            if (actions.Success)
                            {
                                line.Source = StringHelper.TitleCase(Convert.ToString(actions.Groups["source"].Value));
                                if (e.Subject == EventSubject.You)
                                {
                                    line.Source = String.IsNullOrWhiteSpace(Common.Constants.CharacterName) ? "You" : Common.Constants.CharacterName;
                                }
                                _lastAttacker = line.Source;
                                UpdatePlayerActions(actions, line, exp);
                            }
                            break;
                            break;
                        case EventDirection.NPC:
                            break;
                        case EventDirection.Engaged:
                        case EventDirection.UnEngaged:
                            break;
                    }
                    break;
                case EventSubject.Engaged:
                case EventSubject.UnEngaged:
                    break;
            }
            if (!actions.Success)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("Unknown Action Line -> {0}:{1}", String.Format("{0:X4}", e.Code), exp.Cleaned));
            }
        }

        private static void UpdatePlayerActions(Match actions, Line line, Expressions exp)
        {
            ParseControl.Instance.Timeline.GetSetPlayer(line.Source);
            _lastAction = StringHelper.TitleCase(Convert.ToString(actions.Groups["action"].Value));
        }
    }
}
