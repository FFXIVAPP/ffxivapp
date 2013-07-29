// FFXIVAPP.Plugin.Parse
// Filter.Beneficial.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

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
        private static void ProcessBeneficial(Event e, Expressions exp)
        {
            var line = new Line
            {
                RawLine = e.RawLine
            };
            var beneficial = Regex.Match("ph", @"^\.$");
            switch (e.Subject)
            {
                case EventSubject.You:
                case EventSubject.Party:
                    //switch (e.Direction)
                    //{
                    //    case EventDirection.Self:
                    //    case EventDirection.You:
                    //    case EventDirection.Party:
                    //        beneficial = exp.pBeneficialGain;
                    //        if (beneficial.Success)
                    //        {
                    //            line.Source = Convert.ToString(beneficial.Groups["source"].Value);
                    //            if (e.Subject == EventSubject.You)
                    //            {
                    //                line.Source = String.IsNullOrWhiteSpace(Constants.CharacterName) ? "You" : Constants.CharacterName;
                    //            }
                    //            _lastPlayer = line.Source;
                    //        }
                    //        beneficial = exp.pBeneficialLose;
                    //        if (beneficial.Success)
                    //        {
                    //            line.Source = Convert.ToString(beneficial.Groups["source"].Value);
                    //            if (e.Subject == EventSubject.You)
                    //            {
                    //                line.Source = String.IsNullOrWhiteSpace(Constants.CharacterName) ? "You" : Constants.CharacterName;
                    //            }
                    //            _lastPlayer = line.Source;
                    //        }
                    //        break;
                    //}
                    //break;
                case EventSubject.Other:
                case EventSubject.NPC:
                    break;
                case EventSubject.Engaged:
                case EventSubject.UnEngaged:
                    //switch (e.Direction)
                    //{
                    //    case EventDirection.Engaged:
                    //    case EventDirection.UnEngaged:
                    //        beneficial = exp.mBeneficialGain;
                    //        if (beneficial.Success)
                    //        {
                    //            line.Source = Convert.ToString(beneficial.Groups["source"].Value);
                    //            _lastMob = line.Source;
                    //        }
                    //        beneficial = exp.mBeneficialLose;
                    //        if (beneficial.Success)
                    //        {
                    //            line.Source = Convert.ToString(beneficial.Groups["source"].Value);
                    //            _lastMob = line.Source;
                    //        }
                    //        break;
                    //}
                    break;
            }
            if (beneficial.Success)
            {
                return;
            }
            ClearLast();
            ParsingLogHelper.Log(LogManager.GetCurrentClassLogger(), "Beneficial", e, exp);
        }
    }
}
