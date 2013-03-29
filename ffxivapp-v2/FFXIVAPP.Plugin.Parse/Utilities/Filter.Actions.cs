// FFXIVAPP.Plugin.Parse
// Filter.Actions.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Text.RegularExpressions;
using FFXIVAPP.Common.Helpers;
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
        private static void ProcessActions(Event e, Expressions exp)
        {
            var line = new Line();
            var actions = Regex.Match("ph", @"^\.$");
            switch (e.Subject)
            {
                case EventSubject.You:
                case EventSubject.Party:
                    switch (e.Direction)
                    {
                        case EventDirection.Self:
                        case EventDirection.You:
                        case EventDirection.Party:
                            actions = exp.pActions;
                            if (actions.Success)
                            {
                                line.Source = Convert.ToString(actions.Groups["source"].Value);
                                if (e.Subject == EventSubject.You)
                                {
                                    line.Source = String.IsNullOrWhiteSpace(Common.Constants.CharacterName) ? "You" : Common.Constants.CharacterName;
                                }
                                _lastPlayer = line.Source;
                                UpdatePlayerActions(actions, line, exp);
                            }
                            break;
                        case EventDirection.Other:
                        case EventDirection.NPC:
                        case EventDirection.Engaged:
                        case EventDirection.UnEngaged:
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
                            actions = exp.mActions;
                            if (actions.Success)
                            {
                                _lastMob = StringHelper.TitleCase(Convert.ToString(actions.Groups["source"].Value));
                                _lastMobAction = StringHelper.TitleCase(Convert.ToString(actions.Groups["action"].Value));
                            }
                            break;
                        case EventDirection.Other:
                        case EventDirection.NPC:
                        case EventDirection.Engaged:
                        case EventDirection.UnEngaged:
                            break;
                    }
                    break;
            }
            if (actions.Success)
            {
                return;
            }
            ClearLast(true);
            ParsingLogHelper.Log(LogManager.GetCurrentClassLogger(), "Action", e, exp);
        }

        private static void UpdatePlayerActions(Match actions, Line line, Expressions exp)
        {
            ParseControl.Instance.Timeline.GetSetPlayer(line.Source);
            _lastPlayerAction = StringHelper.TitleCase(Convert.ToString(actions.Groups["action"].Value));
        }
    }
}
