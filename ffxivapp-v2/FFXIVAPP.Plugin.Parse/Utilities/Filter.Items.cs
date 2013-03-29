// FFXIVAPP.Plugin.Parse
// Filter.Items.cs
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
        private static void ProcessItems(Event e, Expressions exp)
        {
            var line = new Line();
            var items = Regex.Match("ph", @"^\.$");
            switch (e.Subject)
            {
                case EventSubject.You:
                case EventSubject.Party:
                    items = exp.pItems;
                    if (items.Success)
                    {
                        line.Source = Convert.ToString(items.Groups["source"].Value);
                        if (e.Subject == EventSubject.You)
                        {
                            line.Source = String.IsNullOrWhiteSpace(Common.Constants.CharacterName) ? "You" : Common.Constants.CharacterName;
                        }
                        _lastPlayer = line.Source;
                        _lastPlayerAction = StringHelper.TitleCase(Convert.ToString(items.Groups["item"].Value));
                    }
                    break;
                case EventSubject.Other:
                case EventSubject.NPC:
                case EventSubject.Engaged:
                case EventSubject.UnEngaged:
                    break;
            }
            if (items.Success)
            {
                return;
            }
            var data = String.Format("Unknown Item Line -> [Subject:{0}][Direction:{1}] {2}:{3}", e.Subject, e.Direction, String.Format("{0:X4}", e.Code), exp.Cleaned);
            Logging.Log(LogManager.GetCurrentClassLogger(), data);
        }
    }
}
