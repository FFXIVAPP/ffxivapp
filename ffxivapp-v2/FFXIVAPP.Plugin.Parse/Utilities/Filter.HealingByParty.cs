// FFXIVAPP.Plugin.Parse
// Filter.HealingByParty.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System;
using System.Text.RegularExpressions;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Plugin.Parse.Models;
using NLog;

namespace FFXIVAPP.Plugin.Parse.Utilities
{
    public static partial class Filter
    {
        private static void ProcessHealingByParty(Expressions expressions, string cleaned)
        {
            var line = new Line();
            var match = expressions.pUsed;
            switch (match.Success)
            {
                case true:
                    break;
                case false:
                    ////Logger.Warn("MatchEvent : No match for Healing on line {0}", cleaned);
                    //ChatWorkerDelegate.ParseXmlWriteUnmatchedLog.AddChatLine(new[] {cleaned, mCode, "#FFFFFF", mTimeStamp});
                    return;
            }
            line.Source = Convert.ToString(match.Groups["source"].Value);
            line.Action = Convert.ToString(match.Groups["action"].Value);
            line.Target = Convert.ToString(match.Groups["target"].Value);
            var recloss = Convert.ToString(match.Groups["recloss"].Value);
            line.Amount = match.Groups["amount"].Success ? Convert.ToDecimal(match.Groups["amount"].Value) : 0m;
            line.Type = Convert.ToString(match.Groups["type"].Value.ToUpper());
            if (Regex.IsMatch(line.Source, expressions.You))
            {
                line.Source = Common.Constants.CharacterName;
            }
            if (Regex.IsMatch(line.Target, expressions.You))
            {
                line.Target = Common.Constants.CharacterName;
            }
            if (String.IsNullOrWhiteSpace(line.Source))
            {
                return;
            }
            Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("HandlingEvent : Matched: {0}", cleaned));
            IsValid = true;
            if (line.Type == expressions.Type)
            {
                ParseControl.Instance.Timeline.GetSetPlayer(line.Source).SetHealingStat(line);
                var total = ParseControl.Instance.Timeline.Party.GetGroup(line.Source).GetStatValue("HTotal").ToString();
                switch (ParseControl.Instance.TotalH.ContainsKey(line.Source))
                {
                    case true:
                        ParseControl.Instance.TotalH[line.Source] = total;
                        break;
                    case false:
                        ParseControl.Instance.TotalH.Add(line.Source, total);
                        break;
                }
            }
        }
    }
}