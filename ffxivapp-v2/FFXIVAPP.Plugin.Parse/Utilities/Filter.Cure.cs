// FFXIVAPP.Plugin.Parse
// Filter.Cure.cs
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
        private static void ProcessCure(Event e, Expressions exp)
        {
            var line = new Line();
            var cure = Regex.Match("ph", @"^\.$");
            switch (e.Subject)
            {
                case EventSubject.You:
                case EventSubject.Party:
                    switch (e.Direction)
                    {
                        case EventDirection.Self:
                        case EventDirection.You:
                        case EventDirection.Party:
                            cure = exp.pCure;
                            if (cure.Success)
                            {
                                line.Source = _lastPlayer;
                                if (e.Subject == EventSubject.You)
                                {
                                    line.Source = String.IsNullOrWhiteSpace(Common.Constants.CharacterName) ? "You" : Common.Constants.CharacterName;
                                }
                                UpdatePlayerHealing(cure, line, exp);
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
                case EventSubject.Engaged:
                case EventSubject.UnEngaged:
                    break;
            }
            if (cure.Success)
            {
                return;
            }
            ClearLast();
            ParsingLogHelper.Log(LogManager.GetCurrentClassLogger(), "Cure", e, exp);
        }

        private static void UpdatePlayerHealing(Match cure, Line line, Expressions exp)
        {
            line.Action = _lastPlayerAction;
            line.Amount = cure.Groups["amount"].Success ? Convert.ToDecimal(cure.Groups["amount"].Value) : 0m;
            line.Crit = cure.Groups["crit"].Success;
            line.Target = Convert.ToString(cure.Groups["target"].Value);
            if (line.Target.ToLower() == "you")
            {
                line.Target = String.IsNullOrWhiteSpace(Common.Constants.CharacterName) ? "You" : Common.Constants.CharacterName;
            }
            line.Type = Convert.ToString(cure.Groups["type"].Value.ToUpper());
            if (line.IsEmpty())
            {
                return;
            }
            _lastPlayer = line.Source;
            ParseControl.Instance.Timeline.GetSetPlayer(line.Source)
                        .SetHealingStat(line);
        }
    }
}
