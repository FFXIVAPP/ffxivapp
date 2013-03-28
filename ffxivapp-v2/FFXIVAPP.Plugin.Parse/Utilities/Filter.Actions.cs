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
            var actions = exp.pActions;
            switch (e.Subject)
            {
                case EventSubject.You:
                    actions = exp.pActions;
                    if (actions.Success)
                    {
                        line.Source = String.IsNullOrWhiteSpace(Common.Constants.CharacterName) ? "You" : Common.Constants.CharacterName;
                        UpdatePlayerActions(actions, line, exp);
                    }
                    break;
                case EventSubject.Party:
                case EventSubject.Other:
                    actions = exp.pActions;
                    if (actions.Success)
                    {
                        line.Source = StringHelper.TitleCase(Convert.ToString(actions.Groups["source"].Value));
                        _lastAttacker = line.Source;
                        UpdatePlayerActions(actions, line, exp);
                    }
                    break;
                case EventSubject.Engaged:
                case EventSubject.UnEngaged:
                    actions = exp.mActions;
                    if (actions.Success)
                    {
                        _lastAttacker = line.Source;
                        _lastAction = StringHelper.TitleCase(Convert.ToString(actions.Groups["action"].Value));
                    }
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
