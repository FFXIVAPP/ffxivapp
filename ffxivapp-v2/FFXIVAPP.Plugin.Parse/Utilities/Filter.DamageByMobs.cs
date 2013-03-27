// FFXIVAPP.Plugin.Parse
// Filter.DamageByMobs.cs
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
using NLog;

#endregion

namespace FFXIVAPP.Plugin.Parse.Utilities
{
    public static partial class Filter
    {
        private static void ProcessDamageByMobs(Expressions expressions, string cleaned)
        {
            //var line = new Line();
            //var match = expressions.mAction;
            //switch (match.Success)
            //{
            //    case true:
            //        break;
            //    case false:
            //        //Logger.Warn("MatchEvent : No match for Damage Taken on line {0}", cleaned);
            //        match = expressions.pBlock;
            //        switch (match.Success)
            //        {
            //            case true:
            //                break;
            //            case false:
            //                //Logger.Warn("MatchEvent : No match for Block on line {0}", cleaned);
            //                match = expressions.pParry;
            //                switch (match.Success)
            //                {
            //                    case true:
            //                        break;
            //                    case false:
            //                        //Logger.Warn("MatchEvent : No match for Parry on line {0}", cleaned);
            //                        match = expressions.mResist;
            //                        switch (match.Success)
            //                        {
            //                            case true:
            //                                break;
            //                            case false:
            //                                //Logger.Warn("MatchEvent : No match for Resist on line {0}", cleaned);
            //                                match = expressions.mEvade;
            //                                switch (match.Success)
            //                                {
            //                                    case true:
            //                                        break;
            //                                    case false:
            //                                        //Logger.Warn("MatchEvent : No match for Evade on line {0}", cleaned);
            //                                        //ChatWorkerDelegate.ParseXmlWriteUnmatchedLog.AddChatLine(new[] {cleaned, mCode, "#FFFFFF", mTimeStamp});
            //                                        return;
            //                                }
            //                                break;
            //                        }
            //                        break;
            //                }
            //                break;
            //        }
            //        break;
            //}
            //line.Hit = match.Groups["hit"].Success;
            //line.Miss = match.Groups["miss"].Success;
            //line.Crit = match.Groups["crit"].Success;
            //line.Counter = match.Groups["counter"].Success;
            //line.Block = match.Groups["block"].Success;
            //line.Parry = match.Groups["parry"].Success;
            //line.Resist = match.Groups["resist"].Success;
            //line.Evade = match.Groups["evade"].Success;
            //line.Partial = match.Groups["partial"].Success;
            //line.Source = StringHelper.TitleCase(Convert.ToString(match.Groups["source"].Value));
            //line.Action = Convert.ToString(match.Groups["action"].Value);
            //line.Target = Convert.ToString(match.Groups["target"].Value);
            //line.Direction = Convert.ToString(match.Groups["direction"].Value);
            //var rAttack = expressions.RAttack;
            //var attack = expressions.Attack;
            //if (line.Action.ToLower() == rAttack.ToLower())
            //{
            //    line.Action = rAttack;
            //}
            //if (line.Action.ToLower() == attack.ToLower())
            //{
            //    line.Action = attack;
            //}
            //line.Amount = match.Groups["amount"].Success ? Convert.ToDecimal(match.Groups["amount"].Value) : 0m;
            //line.Part = Convert.ToString(match.Groups["part"].Value);
            //if (Regex.IsMatch(line.Source, expressions.You))
            //{
            //    line.Source = Common.Constants.CharacterName;
            //}
            //if (Regex.IsMatch(line.Target, expressions.You))
            //{
            //    line.Target = Common.Constants.CharacterName;
            //}
            //if (String.IsNullOrWhiteSpace(line.Target))
            //{
            //    return;
            //}
            //Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("HandlingEvent : Matched: {0}", cleaned));
            //IsValid = true;
            //if (match.Groups["amount"].Success || line.Resist)
            //{
            //    line.Hit = true;
            //}
            //else
            //{
            //    line.Hit = !line.Miss;
            //}
            //if (line.Counter)
            //{
            //}
            //else
            //{
            //    ParseControl.Instance.Timeline.PublishTimelineEvent(TimelineEventType.MobFighting, line.Source);
            //    ParseControl.Instance.Timeline.GetSetPlayer(line.Target)
            //                .SetDamageStat(line);
            //    var total = ParseControl.Instance.Timeline.Party.GetGroup(line.Target)
            //                            .GetStatValue("DTTotal")
            //                            .ToString();
            //    switch (ParseControl.Instance.TotalD.ContainsKey(line.Target))
            //    {
            //        case true:
            //            ParseControl.Instance.TotalD[line.Target] = total;
            //            break;
            //        case false:
            //            ParseControl.Instance.TotalD.Add(line.Target, total);
            //            break;
            //    }
            //}
        }
    }
}
