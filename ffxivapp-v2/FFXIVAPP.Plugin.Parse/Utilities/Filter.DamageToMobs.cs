// FFXIVAPP.Plugin.Parse
// Filter.DamageToMobs.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Linq;
using System.Text.RegularExpressions;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Plugin.Parse.Enums;
using FFXIVAPP.Plugin.Parse.Models;
using FFXIVAPP.Plugin.Parse.Properties;
using NLog;

#endregion

namespace FFXIVAPP.Plugin.Parse.Utilities
{
    public static partial class Filter
    {
        private static void ProcessDamageToMobs(Expressions expressions, string cleaned)
        {
            //var line = new Line();
            //var match = expressions.pMultiFlag;
            //switch (match.Success)
            //{
            //    case true:
            //        if (expressions.pMultiFlagAbility.Any(s => cleaned.ToLower()
            //                                                          .Contains(s.ToLower())))
            //        {
            //            _lastAttacked = StringHelper.TitleCase((Convert.ToString(match.Groups["target"].Value)));
            //            _lastAttacker = Convert.ToString(match.Groups["source"].Value);
            //            _lastAction = Convert.ToString(match.Groups["action"].Value);
            //            _lastDirection = StringHelper.TitleCase(Convert.ToString(match.Groups["direction"].Value));
            //            MultiFlag = cleaned;
            //        }
            //        break;
            //}
            //match = expressions.pMulti;
            //switch (match.Success)
            //{
            //    case true:
            //        IsMulti = true;
            //        break;
            //    case false:
            //        match = expressions.pAction;
            //        switch (match.Success)
            //        {
            //            case true:
            //                break;
            //            case false:
            //                //Logger.Warn("MatchEvent : No match for Damage on line {0}", cleaned);
            //                match = expressions.mParry;
            //                switch (match.Success)
            //                {
            //                    case true:
            //                        break;
            //                    case false:
            //                        //Logger.Warn("MatchEvent : No match for Parry on line {0}", cleaned);
            //                        match = expressions.pResist;
            //                        switch (match.Success)
            //                        {
            //                            case true:
            //                                break;
            //                            case false:
            //                                //Logger.Warn("MatchEvent : No match for Resist on line {0}", cleaned);
            //                                match = expressions.pEvade;
            //                                switch (match.Success)
            //                                {
            //                                    case true:
            //                                        break;
            //                                    case false:
            //                                        //Logger.Warn("MatchEvent : No match for Evade on line {0}", cleaned);
            //                                        match = expressions.pAdditional;
            //                                        switch (match.Success)
            //                                        {
            //                                            case true:
            //                                                if (!String.IsNullOrWhiteSpace(_lastAttacked) && !String.IsNullOrWhiteSpace(_lastAttacker))
            //                                                {
            //                                                    var amount = match.Groups["amount"].Success ? Convert.ToDecimal(match.Groups["amount"].Value) : 0m;
            //                                                    line.Amount = amount;
            //                                                    line.Action = expressions.Added;
            //                                                    line.Source = _lastAttacker;
            //                                                    line.Target = _lastAttacked;
            //                                                    line.Hit = true;
            //                                                    ParseControl.Instance.Timeline.GetSetPlayer(_lastAttacker)
            //                                                                .SetAbilityStat(line);
            //                                                }
            //                                                return;
            //                                            case false:
            //                                                //Logger.Warn("MatchEvent : No match for Additional on line {0}", cleaned);
            //                                                //ChatWorkerDelegate.ParseXmlWriteUnmatchedLog.AddChatLine(new[] {cleaned, mCode, "#FFFFFF", mTimeStamp});
            //                                                return;
            //                                        }
            //                                        break;
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
            //line.Direction = Convert.ToString(match.Groups["direction"].Value);
            //line.Source = Convert.ToString(match.Groups["source"].Value);
            //line.Action = Convert.ToString(match.Groups["action"].Value);
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
            //line.Target = StringHelper.TitleCase((Convert.ToString(match.Groups["target"].Value)));
            //var whoevaded = StringHelper.TitleCase(Convert.ToString(match.Groups["whoevaded"]));
            //line.Amount = match.Groups["amount"].Success ? Convert.ToDecimal(match.Groups["amount"].Value) : 0m;
            //line.Part = Convert.ToString(match.Groups["part"].Value);
            //if (IsMulti)
            //{
            //    if (!String.IsNullOrWhiteSpace(MultiFlag))
            //    {
            //        if (Settings.Default.ExportXML)
            //        {
            //            //ChatWorkerDelegate.ParseXmlWriteLog.AddChatLine(new[] {cleaned, mCode, "#FFFFFF", mTimeStamp});
            //        }
            //        MultiFlag = "";
            //    }
            //    line.Target = _lastAttacked;
            //    line.Source = _lastAttacker;
            //    line.Action = _lastAction;
            //    line.Direction = _lastDirection;
            //    line.Hit = true;
            //    IsMulti = false;
            //}
            //if (Regex.IsMatch(line.Source, expressions.You))
            //{
            //    line.Source = Common.Constants.CharacterName;
            //}
            //line.Target = (String.IsNullOrWhiteSpace(line.Target)) ? whoevaded : line.Target;
            //if (String.IsNullOrWhiteSpace(line.Source) || String.IsNullOrWhiteSpace(line.Target))
            //{
            //    return;
            //}
            //Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("HandlingEvent : Matched: {0}", cleaned));
            //IsValid = true;
            //ParseControl.Instance.Timeline.PublishTimelineEvent(TimelineEventType.MobFighting, line.Target);
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
            //    ParseControl.Instance.Timeline.GetSetMob(line.Target)
            //                .SetPlayerStat(line);
            //    ParseControl.Instance.Timeline.GetSetPlayer(line.Source)
            //                .SetAbilityStat(line);
            //    _lastAttacker = line.Source;
            //    _lastAttacked = line.Target;
            //    var total = ParseControl.Instance.Timeline.Party.GetGroup(line.Source)
            //                            .GetStatValue("Total")
            //                            .ToString();
            //    var dps = ParseControl.Instance.Timeline.Party.GetGroup(line.Source)
            //                          .GetStatValue("DPS")
            //                          .ToString();
            //    switch (ParseControl.Instance.TotalA.ContainsKey(line.Source))
            //    {
            //        case true:
            //            ParseControl.Instance.TotalA[line.Source] = total;
            //            break;
            //        case false:
            //            ParseControl.Instance.TotalA.Add(line.Source, total);
            //            break;
            //    }
            //    switch (ParseControl.Instance.TotalDPS.ContainsKey(line.Source))
            //    {
            //        case true:
            //            ParseControl.Instance.TotalDPS[line.Source] = dps;
            //            break;
            //        case false:
            //            ParseControl.Instance.TotalDPS.Add(line.Source, dps);
            //            break;
            //    }
            //}
        }
    }
}
