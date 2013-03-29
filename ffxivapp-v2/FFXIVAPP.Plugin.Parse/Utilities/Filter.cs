// FFXIVAPP.Plugin.Parse
// Filter.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.Collections;
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
        private static Event _lastEvent;
        private static string _lastPlayer = "";
        private static string _lastPlayerAction = "";
        private static string _lastMob = "";
        private static string _lastMobAction = "";
        private static bool IsMulti { get; set; }
        private static string MultiFlag { get; set; }
        private static bool IsValid { get; set; }

        public static void Process(string cleaned, Event e)
        {
            _lastEvent = _lastEvent ?? e;

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
                    ProcessItems(e, expressions);
                    break;
                case EventType.Cure:
                    ProcessCure(e, expressions);
                    break;
                case EventType.Benficial:
                    ProcessBeneficial(e, expressions);
                    break;
                case EventType.Detrimental:
                    ProcessDetrimental(e, expressions);
                    break;
            }

            _lastEvent = e;

            #region Save Parse to XML

            if (Settings.Default.ExportXML & IsValid)
            {
                //ChatWorkerDelegate.ParseXmlWriteLog.AddChatLine(new[] {cleaned, mCode, "#FFFFFF", mTimeStamp});
            }

            #endregion
        }

        private static void ClearLast(bool clearNames = false)
        {
            _lastPlayerAction = "";
            _lastMobAction = "";
            if (!clearNames)
            {
                return;
            }
            _lastPlayer = "";
            _lastMob = "";
        }
    }
}
