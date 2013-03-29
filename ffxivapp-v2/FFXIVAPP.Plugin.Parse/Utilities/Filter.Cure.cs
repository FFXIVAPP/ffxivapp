// FFXIVAPP.Plugin.Parse
// Filter.Cure.cs
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
        private static void ProcessCure(Event e, Expressions exp)
        {
            var line = new Line();
            var cure = Regex.Match("ph", @"^\.$");
            switch (e.Subject)
            {
                case EventSubject.You:
                case EventSubject.Party:
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
                case EventSubject.Other:
                case EventSubject.NPC:
                case EventSubject.Engaged:
                case EventSubject.UnEngaged:
                    break;
            }
            _lastPlayerAction = "";
            _lastMobAction = "";
            if (cure.Success)
            {
                return;
            }
            var data = String.Format("Unknown Cure Line -> [Subject:{0}][Direction:{1}] {2}:{3}", e.Subject, e.Direction, String.Format("{0:X4}", e.Code), exp.Cleaned);
            Logging.Log(LogManager.GetCurrentClassLogger(), data);
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
            ParseControl.Instance.Timeline.GetSetPlayer(line.Source)
                        .SetHealingStat(line);
        }
    }
}
