// FFXIVAPP.Plugin.Parse
// Filter.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Collections;
using System.Text.RegularExpressions;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.RegularExpressions;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Plugin.Parse.Enums;
using FFXIVAPP.Plugin.Parse.Helpers;
using FFXIVAPP.Plugin.Parse.Models;
using FFXIVAPP.Plugin.Parse.Models.Events;
using FFXIVAPP.Plugin.Parse.Properties;

#endregion

namespace FFXIVAPP.Plugin.Parse.Utilities
{
    public static partial class Filter
    {
        private static readonly Hashtable Offsets = ParseHelper.GetJob();
        private static string _lastAttacker = "";
        private static string _lastAttacked = "";
        private static string _lastAction = "";
        private static string _lastDirection = "";
        private static bool IsMulti { get; set; }
        private static string MultiFlag { get; set; }
        private static bool IsValid { get; set; }

        public static void Process(string cleaned, Event e)
        {
            IsValid = false;
            //var expressions = new Expressions(cleaned);
            
            //matches = Regex.Match(cleaned, @"^( ⇒ )?(?<crit>Critical! )?((?<source>You|.+) hits? ((T|t)he )?(?<target>.+) for |((T|t)he )?(?<target>.+) takes )(?<amount>\d+) (\((?<givetake>\+|-)(?<modifier>\d+)%\) )?damage\.$");
            //matches = Regex.Match(cleaned, @"^(You miss | ⇒ The attack misses )((T|t)he )?(?<target>.+)\.$", SharedRegEx.DefaultOptions);

            var line = new Line();

            if (e.Subject == EventSubject.You)
            {
                switch (e.Type)
                {
                    case EventType.Actions:
                        var actions = Regex.Match(cleaned, @"^You use (?<action>.+)\.$");
                        if (actions.Success)
                        {
                            ParseControl.Instance.Timeline.GetSetPlayer(Common.Constants.CharacterName);
                            Logging.Log(NLog.LogManager.GetCurrentClassLogger(), String.Format("Action -> {0}", actions.Groups["action"].Value));
                        }
                        break;
                    case EventType.Damage:
                        if (e.Direction == EventDirection.From)
                        {
                            Logging.Log(NLog.LogManager.GetCurrentClassLogger(), String.Format("Damage Line -> {0}", cleaned));
                        }
                        break;
                }
            }

            #region Save Parse to XML

            if (Settings.Default.ExportXML & IsValid)
            {
                //ChatWorkerDelegate.ParseXmlWriteLog.AddChatLine(new[] {cleaned, mCode, "#FFFFFF", mTimeStamp});
            }

            #endregion
        }
    }
}
