// FFXIVAPP.Plugin.Parse
// Filter.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

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
            var expressions = new Expressions(cleaned);

            #region Damage to Mobs

            if (e.Type == EventType.Attack && e.Direction == EventDirection.By)
            {
                ProcessDamageToMobs(expressions, cleaned);
            }

            #endregion

            #region Damage by Mobs

            if (e.Type == EventType.Attack && e.Direction == EventDirection.On)
            {
                ProcessDamageByMobs(expressions, cleaned);
            }

            #endregion

            #region Healing by Party

            if (e.Type == EventType.Heal)
            {
                ProcessHealingByParty(expressions, cleaned);
            }

            #endregion

            #region Save Parse to XML

            if (Settings.Default.ExportXML & IsValid)
            {
                //ChatWorkerDelegate.ParseXmlWriteLog.AddChatLine(new[] {cleaned, mCode, "#FFFFFF", mTimeStamp});
            }

            #endregion
        }
    }
}
