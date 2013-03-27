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
                    if (actions.Success)
                    {
                        line.Source = String.IsNullOrWhiteSpace(Common.Constants.CharacterName) ? "You" : Common.Constants.CharacterName;
                    }
                    break;
                case EventSubject.Party:
                    if (actions.Success)
                    {
                        line.Source = StringHelper.TitleCase(Convert.ToString(actions.Groups["source"].Value));
                        _lastAttacker = line.Source;
                    }
                    break;
                case EventSubject.Other:
                    if (actions.Success)
                    {
                        line.Source = StringHelper.TitleCase(Convert.ToString(actions.Groups["source"].Value));
                        _lastAttacker = line.Source;
                    }
                    break;
                case EventSubject.Engaged:
                    break;
                case EventSubject.UnEngaged:
                    break;
            }
            if (actions.Success)
            {
                Logging.Log(NLog.LogManager.GetCurrentClassLogger(), String.Format("Action Line -> {0}", exp.Cleaned));
                ParseControl.Instance.Timeline.GetSetPlayer(line.Source);
                _lastAction = StringHelper.TitleCase(Convert.ToString(actions.Groups["action"].Value));
            }
        }
    }
}
