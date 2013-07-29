// FFXIVAPP.Plugin.Parse
// Filter.Detrimental.cs
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
        private static void ProcessDetrimental(Event e, Expressions exp)
        {
            var line = new Line
            {
                RawLine = e.RawLine
            };
            var detrimental = Regex.Match("ph", @"^\.$");
            switch (e.Subject)
            {
                case EventSubject.You:
                case EventSubject.Party:
                    //switch (e.Direction)
                    //{
                    //    case EventDirection.Self:
                    //    case EventDirection.You:
                    //    case EventDirection.Party:
                    //        detrimental = exp.pDetrimentalGain;
                    //        if (detrimental.Success)
                    //        {
                    //            line.Source = Convert.ToString(detrimental.Groups["source"].Value);
                    //            if (e.Subject == EventSubject.You)
                    //            {
                    //                line.Source = String.IsNullOrWhiteSpace(Constants.CharacterName) ? "You" : Constants.CharacterName;
                    //            }
                    //            _lastPlayer = line.Source;
                    //        }
                    //        detrimental = exp.pDetrimentalLose;
                    //        if (detrimental.Success)
                    //        {
                    //            line.Source = Convert.ToString(detrimental.Groups["source"].Value);
                    //            if (e.Subject == EventSubject.You)
                    //            {
                    //                line.Source = String.IsNullOrWhiteSpace(Constants.CharacterName) ? "You" : Constants.CharacterName;
                    //            }
                    //            _lastPlayer = line.Source;
                    //        }
                    //        break;
                    //}
                    break;
                case EventSubject.Other:
                case EventSubject.NPC:
                    break;
                case EventSubject.Engaged:
                case EventSubject.UnEngaged:
                    //switch (e.Direction)
                    //{
                    //    case EventDirection.Engaged:
                    //    case EventDirection.UnEngaged:
                    //        detrimental = exp.mDetrimentalGain;
                    //        if (detrimental.Success)
                    //        {
                    //            line.Source = Convert.ToString(detrimental.Groups["source"].Value);
                    //            _lastMob = line.Source;
                    //        }
                    //        detrimental = exp.mDetrimentalLose;
                    //        if (detrimental.Success)
                    //        {
                    //            line.Source = Convert.ToString(detrimental.Groups["source"].Value);
                    //            _lastMob = line.Source;
                    //        }
                    //        break;
                    //}
                    break;
            }
            if (detrimental.Success)
            {
                return;
            }
            ClearLast();
            ParsingLogHelper.Log(LogManager.GetCurrentClassLogger(), "Detrimental", e, exp);
        }
    }
}
