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
using FFXIVAPP.Plugin.Parse.ViewModels;

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
            var expressions = new Expressions(e, cleaned);

            switch (e.Type)
            {
                case EventType.Damage:
                    ProcessDamage(e, expressions);
                    break;
                case EventType.Failed:
                    ProcessFailed(e, expressions);
                    break;
                case EventType.Actions:
                    ProcessActions(e, expressions);
                    break;
                case EventType.Items:
                case EventType.Cure:
                case EventType.Benficial:
                case EventType.Detrimental:
                default:
                    _lastAction = "";
                    break;
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
