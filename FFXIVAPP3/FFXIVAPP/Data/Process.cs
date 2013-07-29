// FFXIVAPP
// Process.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using FFXIVAPP.Classes;
using FFXIVAPP.Classes.Helpers;
using FFXIVAPP.Classes.RegExs;
using FFXIVAPP.Models;
using NLog;

namespace FFXIVAPP.Data
{
    internal static class Process
    {
        //static SqlCeConnection conn = null;
        private static readonly Hashtable Offsets = ParseHelper.GetJob();
        private static string _lastAttacker = "";
        private static string _lastAttacked = "";
        private static string _lastAction = "";
        private static string _lastDirection = "";
        private static bool _multi;
        private static string _multiFlag;
        private static bool _valid;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Parse(string mCode, string mTimeStamp, string cleaned, Event e)
        {
            _valid = false;
            Match mReg;
            var json = "";
            var q = "";
            var d = new ParseHelper.LineData();

            #region Player Matches

            var pAction = Regex.Match("ph", @"^\.$");
            var pUsed = Regex.Match("ph", @"^\.$");
            var pAdditional = Regex.Match("ph", @"^\.$");
            var pCounter = Regex.Match("ph", @"^\.$");
            var pBlock = Regex.Match("ph", @"^\.$");
            var pParry = Regex.Match("ph", @"^\.$");
            var pResist = Regex.Match("ph", @"^\.$");
            var pEvade = Regex.Match("ph", @"^\.$");
            var pMultiFlag = Regex.Match("ph", @"^\.$");
            var pMultiFlagAbility = new string[] {};
            var pMulti = Regex.Match("ph", @"^\.$");

            #endregion

            #region Monster Matches

            var mAction = Regex.Match("ph", @"^\.$");
            var mUsed = Regex.Match("ph", @"^\.$");
            var mAdditional = Regex.Match("ph", @"^\.$");
            var mCounter = Regex.Match("ph", @"^\.$");
            var mBlock = Regex.Match("ph", @"^\.$");
            var mParry = Regex.Match("ph", @"^\.$");
            var mResist = Regex.Match("ph", @"^\.$");
            var mEvade = Regex.Match("ph", @"^\.$");

            #endregion

            #region Setup Language Variables

            var counter = "";
            var added = "";
            var type = "";
            var attack = "";
            var rattack = "";
            var you = "";

            switch (Settings.Default.Language)
            {
                case "English":
                    pAction = Player.ActionEn.Match(cleaned);
                    pUsed = Player.UsedEn.Match(cleaned);
                    pAdditional = Player.AdditionalEn.Match(cleaned);
                    pCounter = Player.CounterEn.Match(cleaned);
                    pBlock = Player.BlockEn.Match(cleaned);
                    pParry = Player.ParryEn.Match(cleaned);
                    pResist = Player.ResistEn.Match(cleaned);
                    pEvade = Player.EvadeEn.Match(cleaned);
                    pMultiFlag = Player.MultiFlagEn.Match(cleaned);
                    pMultiFlagAbility = ParseHelper.MultiEn;
                    pMulti = Player.MultiEn.Match(cleaned);
                    mAction = Monster.ActionEn.Match(cleaned);
                    mResist = Monster.ResistEn.Match(cleaned);
                    mEvade = Monster.EvadeEn.Match(cleaned);
                    counter = "Counter";
                    added = "Additional Effect";
                    type = "HP";
                    rattack = "Ranged Attack";
                    attack = "Attack";
                    you = @"^[Yy]our?$";
                    break;
                case "French":
                    pAction = Player.ActionFr.Match(cleaned);
                    pUsed = Player.UsedFr.Match(cleaned);
                    pAdditional = Player.AdditionalFr.Match(cleaned);
                    pCounter = Player.CounterFr.Match(cleaned);
                    pBlock = Player.BlockFr.Match(cleaned);
                    pParry = Player.ParryFr.Match(cleaned);
                    pResist = Player.ResistFr.Match(cleaned);
                    pEvade = Player.EvadeFr.Match(cleaned);
                    pMultiFlag = Player.MultiFlagFr.Match(cleaned);
                    pMultiFlagAbility = ParseHelper.MultiFr;
                    pMulti = Player.MultiFr.Match(cleaned);
                    mAction = Monster.ActionFr.Match(cleaned);
                    mResist = Monster.ResistFr.Match(cleaned);
                    mEvade = Monster.EvadeFr.Match(cleaned);
                    counter = "Contre";
                    added = "Effet Supplémentaire";
                    type = "PV";
                    rattack = "D'Attaque À Distance";
                    attack = "Attaque";
                    you = @"^[Vv]ous$";
                    break;
                case "Japanese":
                    pAction = Player.ActionJa.Match(cleaned);
                    pUsed = Player.UsedJa.Match(cleaned);
                    pAdditional = Player.AdditionalJa.Match(cleaned);
                    pCounter = Player.CounterJa.Match(cleaned);
                    pBlock = Player.BlockJa.Match(cleaned);
                    pParry = Player.ParryJa.Match(cleaned);
                    pResist = Player.ResistJa.Match(cleaned);
                    pEvade = Player.EvadeJa.Match(cleaned);
                    pMultiFlag = Player.MultiFlagJa.Match(cleaned);
                    pMultiFlagAbility = ParseHelper.MultiJa;
                    pMulti = Player.MultiJa.Match(cleaned);
                    mAction = Monster.ActionJa.Match(cleaned);
                    mResist = Monster.ResistJa.Match(cleaned);
                    mEvade = Monster.EvadeJa.Match(cleaned);
                    counter = "カウンター";
                    added = "追加効果";
                    type = "ＨＰ";
                    rattack = "Ranged Attack";
                    attack = "Attack";
                    you = @"^\.$";
                    break;
                case "German":
                    pAction = Player.ActionDe.Match(cleaned);
                    pUsed = Player.UsedDe.Match(cleaned);
                    pAdditional = Player.AdditionalDe.Match(cleaned);
                    pCounter = Player.CounterDe.Match(cleaned);
                    pBlock = Player.BlockDe.Match(cleaned);
                    pParry = Player.ParryDe.Match(cleaned);
                    pResist = Player.ResistDe.Match(cleaned);
                    pEvade = Player.EvadeDe.Match(cleaned);
                    pMultiFlag = Player.MultiFlagDe.Match(cleaned);
                    pMultiFlagAbility = ParseHelper.MultiDe;
                    pMulti = Player.MultiDe.Match(cleaned);
                    mAction = Monster.ActionDe.Match(cleaned);
                    mResist = Monster.ResistDe.Match(cleaned);
                    mEvade = Monster.EvadeDe.Match(cleaned);
                    counter = "Counter";
                    added = "Zusatzefeckt";
                    type = "HP";
                    rattack = "Ranged Attack";
                    attack = "Attack";
                    you = @"^[Dd]u$";
                    break;
            }

            #endregion

            #region Damage to Mobs

            if (e.Type == EventType.Attack && e.Direction == EventDirection.By)
            {
                mReg = pMultiFlag;
                switch (pMultiFlag.Success)
                {
                    case true:
                        if (pMultiFlagAbility.Any(s => cleaned.ToLower().Contains(s.ToLower())))
                        {
                            _lastAttacked = FFXIV.TitleCase((Convert.ToString(mReg.Groups["target"].Value)));
                            _lastAttacker = Convert.ToString(mReg.Groups["source"].Value);
                            _lastAction = Convert.ToString(mReg.Groups["action"].Value);
                            _lastDirection = FFXIV.TitleCase(Convert.ToString(mReg.Groups["direction"].Value));
                            _multiFlag = cleaned;
                        }
                        break;
                }
                mReg = pMulti;
                switch (pMulti.Success)
                {
                    case true:
                        _multi = true;
                        break;
                    case false:
                        mReg = pAction;
                        switch (pAction.Success)
                        {
                            case true:
                                break;
                            case false:
                                //Logger.Warn("MatchEvent : No match for Damage on line {0}", cleaned);
                                mReg = mParry;
                                switch (mReg.Success)
                                {
                                    case true:
                                        break;
                                    case false:
                                        //Logger.Warn("MatchEvent : No match for Parry on line {0}", cleaned);
                                        mReg = pResist;
                                        switch (pResist.Success)
                                        {
                                            case true:
                                                break;
                                            case false:
                                                //Logger.Warn("MatchEvent : No match for Resist on line {0}", cleaned);
                                                mReg = pEvade;
                                                switch (pEvade.Success)
                                                {
                                                    case true:
                                                        break;
                                                    case false:
                                                        //Logger.Warn("MatchEvent : No match for Evade on line {0}", cleaned);
                                                        mReg = pAdditional;
                                                        switch (pAdditional.Success)
                                                        {
                                                            case true:
                                                                if (!String.IsNullOrWhiteSpace(_lastAttacked) && !String.IsNullOrWhiteSpace(_lastAttacker))
                                                                {
                                                                    var amount = pAdditional.Groups["amount"].Success ? Convert.ToDecimal(pAdditional.Groups["amount"].Value) : 0m;
                                                                    d.Amount = amount;
                                                                    d.Action = added;
                                                                    d.Source = _lastAttacker;
                                                                    d.Target = _lastAttacked;
                                                                    d.Hit = true;
                                                                    FFXIV.Instance.Timeline.GetSetPlayer(_lastAttacker).AddAbilityStats(d);
                                                                }
                                                                return;
                                                            case false:
                                                                //Logger.Warn("MatchEvent : No match for Additional on line {0}", cleaned);
                                                                ChatWorkerDelegate.ParseXmlWriteUnmatchedLog.AddChatLine(new[] {cleaned, mCode, "#FFFFFF", mTimeStamp});
                                                                return;
                                                        }
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                }
                                break;
                        }
                        break;
                }
                d.Hit = mReg.Groups["hit"].Success;
                d.Miss = mReg.Groups["miss"].Success;
                d.Crit = mReg.Groups["crit"].Success;
                d.Counter = mReg.Groups["counter"].Success;
                d.Block = mReg.Groups["block"].Success;
                d.Parry = mReg.Groups["parry"].Success;
                d.Resist = mReg.Groups["resist"].Success;
                d.Evade = mReg.Groups["evade"].Success;
                d.Partial = mReg.Groups["partial"].Success;
                d.Direction = FFXIV.TitleCase(Convert.ToString(mReg.Groups["direction"].Value));
                d.Source = Convert.ToString(mReg.Groups["source"].Value);
                d.Action = Convert.ToString(mReg.Groups["action"].Value);
                if (d.Action.ToLower() == rattack.ToLower())
                {
                    d.Action = rattack;
                }
                if (d.Action.ToLower() == attack.ToLower())
                {
                    d.Action = attack;
                }
                d.Target = FFXIV.TitleCase((Convert.ToString(mReg.Groups["target"].Value)));
                var whoevaded = FFXIV.TitleCase(Convert.ToString(mReg.Groups["whoevaded"]));
                d.Amount = mReg.Groups["amount"].Success ? Convert.ToDecimal(mReg.Groups["amount"].Value) : 0m;
                d.Part = FFXIV.TitleCase((Convert.ToString(mReg.Groups["part"].Value)));
                if (_multi)
                {
                    if (!String.IsNullOrWhiteSpace(_multiFlag))
                    {
                        if (Settings.Default.Parse_SaveLog && App.MArgs == null)
                        {
                            ChatWorkerDelegate.ParseXmlWriteLog.AddChatLine(new[] {cleaned, mCode, "#FFFFFF", mTimeStamp});
                        }
                        _multiFlag = "";
                    }
                    d.Target = _lastAttacked;
                    d.Source = _lastAttacker;
                    d.Action = _lastAction;
                    d.Direction = _lastDirection;
                    d.Hit = true;
                    _multi = false;
                }
                if (Regex.IsMatch(d.Source, you))
                {
                    d.Source = Settings.Default.CharacterName;
                }
                d.Target = (String.IsNullOrWhiteSpace(d.Target)) ? whoevaded : d.Target;
                if (String.IsNullOrWhiteSpace(d.Source) || String.IsNullOrWhiteSpace(d.Target))
                {
                    return;
                }
                Logger.Trace("HandlingEvent : Matched: {0}", cleaned);
                _valid = true;
                FFXIV.Instance.Timeline.PublishTimelineEvent(TimelineEventType.MobFighting, d.Target);
                if (mReg.Groups["amount"].Success || d.Resist)
                {
                    d.Hit = true;
                }
                else
                {
                    d.Hit = !d.Miss;
                }
                if (d.Counter)
                {
                }
                else
                {
                    FFXIV.Instance.Timeline.GetSetMob(d.Target).GetSetPlayer(d);
                    FFXIV.Instance.Timeline.GetSetPlayer(d.Source).AddAbilityStats(d);
                    _lastAttacker = d.Source;
                    _lastAttacked = d.Target;
                    var total = FFXIV.Instance.Timeline.Party.GetGroup(d.Source).GetStatValue("Total").ToString();
                    var dps = FFXIV.Instance.Timeline.Party.GetGroup(d.Source).GetStatValue("DPS").ToString();
                    switch (FFXIV.Instance.TotalA.ContainsKey(d.Source))
                    {
                        case true:
                            FFXIV.Instance.TotalA[d.Source] = total;
                            break;
                        case false:
                            FFXIV.Instance.TotalA.Add(d.Source, total);
                            break;
                    }
                    switch (FFXIV.Instance.TotalDPS.ContainsKey(d.Source))
                    {
                        case true:
                            FFXIV.Instance.TotalDPS[d.Source] = dps;
                            break;
                        case false:
                            FFXIV.Instance.TotalDPS.Add(d.Source, dps);
                            break;
                    }
                }

                #region JSON Constructor

                if (Settings.Default.UploadParse && App.MArgs == null)
                {
                    if (Settings.Default.CICUID == "" || FFXIV.Desc == "" || d.Source != Settings.Default.CharacterName)
                    {
                        return;
                    }
                    var critc = (d.Crit) ? "1" : "0";
                    var counterc = (d.Counter) ? "1" : "0";
                    var blockc = (d.Block) ? "1" : "0";
                    var parryc = (d.Parry) ? "1" : "0";
                    var resistc = (d.Resist) ? "1" : "0";
                    var evadec = (d.Evade) ? "1" : "0";
                    var partialc = (d.Partial) ? "1" : "0";
                    try
                    {
                        d.Job = Offsets[d.Action.ToLower()].ToString();
                    }
                    catch
                    {
                        d.Job = "Unknown";
                    }
                    json = "{\"uid\":\"" + FFXIV.UID + "\",\"cicuid\":\"" + Settings.Default.CICUID + "\",\"server\":\"" + Settings.Default.Server + "\",\"crit\":\"" + critc + "\",\"counter\":\"" + counterc + "\",\"block\":\"" + blockc + "\",\"parry\":\"" + parryc + "\",\"resist\":\"" + resistc + "\",\"evade\":\"" + evadec + "\",\"partial\":\"" + partialc + "\",\"player\":\"" + d.Source + "\",\"job\":\"" + FFXIV.TitleCase(d.Job) + "\",\"target\":\"" + d.Target + "\",\"action\":\"" + d.Action + "\",\"direction\":\"" + d.Direction + "\",\"body_part\":\"" + d.Part + "\",\"amount\":\"" + d.Amount.ToString(CultureInfo.InvariantCulture) + "\",\"parse_desc\":\"" + FFXIV.Desc + "\"}";
                    q = "p";
                }

                #endregion
            }

            #endregion

            #region Damage by Mobs

            if (e.Type == EventType.Attack && e.Direction == EventDirection.On)
            {
                mReg = mAction;
                switch (mAction.Success)
                {
                    case true:
                        break;
                    case false:
                        //Logger.Warn("MatchEvent : No match for Damage Taken on line {0}", cleaned);
                        mReg = pBlock;
                        switch (pBlock.Success)
                        {
                            case true:
                                break;
                            case false:
                                //Logger.Warn("MatchEvent : No match for Block on line {0}", cleaned);
                                mReg = pParry;
                                switch (pParry.Success)
                                {
                                    case true:
                                        break;
                                    case false:
                                        //Logger.Warn("MatchEvent : No match for Parry on line {0}", cleaned);
                                        mReg = mResist;
                                        switch (mResist.Success)
                                        {
                                            case true:
                                                break;
                                            case false:
                                                //Logger.Warn("MatchEvent : No match for Resist on line {0}", cleaned);
                                                mReg = mEvade;
                                                switch (mReg.Success)
                                                {
                                                    case true:
                                                        break;
                                                    case false:
                                                        //Logger.Warn("MatchEvent : No match for Evade on line {0}", cleaned);
                                                        ChatWorkerDelegate.ParseXmlWriteUnmatchedLog.AddChatLine(new[] {cleaned, mCode, "#FFFFFF", mTimeStamp});
                                                        return;
                                                }
                                                break;
                                        }
                                        break;
                                }
                                break;
                        }
                        break;
                }
                d.Hit = mReg.Groups["hit"].Success;
                d.Miss = mReg.Groups["miss"].Success;
                d.Crit = mReg.Groups["crit"].Success;
                d.Counter = mReg.Groups["counter"].Success;
                d.Block = mReg.Groups["block"].Success;
                d.Parry = mReg.Groups["parry"].Success;
                d.Resist = mReg.Groups["resist"].Success;
                d.Evade = mReg.Groups["evade"].Success;
                d.Partial = mReg.Groups["partial"].Success;
                d.Source = FFXIV.TitleCase(Convert.ToString(mReg.Groups["source"].Value));
                d.Action = Convert.ToString(mReg.Groups["action"].Value);
                d.Target = Convert.ToString(mReg.Groups["target"].Value);
                d.Direction = FFXIV.TitleCase(Convert.ToString(mReg.Groups["direction"].Value));
                if (d.Action.ToLower() == rattack.ToLower())
                {
                    d.Action = rattack;
                }
                if (d.Action.ToLower() == attack.ToLower())
                {
                    d.Action = attack;
                }
                d.Amount = mReg.Groups["amount"].Success ? Convert.ToDecimal(mReg.Groups["amount"].Value) : 0m;
                d.Part = FFXIV.TitleCase((Convert.ToString(mReg.Groups["part"].Value)));
                if (Regex.IsMatch(d.Source, you))
                {
                    d.Source = Settings.Default.CharacterName;
                }
                if (Regex.IsMatch(d.Target, you))
                {
                    d.Target = Settings.Default.CharacterName;
                }
                if (String.IsNullOrWhiteSpace(d.Target))
                {
                    return;
                }
                Logger.Trace("HandlingEvent : Matched: {0}", cleaned);
                _valid = true;
                if (mReg.Groups["amount"].Success || d.Resist)
                {
                    d.Hit = true;
                }
                else
                {
                    d.Hit = !d.Miss;
                }
                if (d.Counter)
                {
                }
                else
                {
                    FFXIV.Instance.Timeline.PublishTimelineEvent(TimelineEventType.MobFighting, d.Source);
                    FFXIV.Instance.Timeline.GetSetPlayer(d.Target).AddDamageStats(d);
                    var total = FFXIV.Instance.Timeline.Party.GetGroup(d.Target).GetStatValue("DT Total").ToString();
                    switch (FFXIV.Instance.TotalD.ContainsKey(d.Target))
                    {
                        case true:
                            FFXIV.Instance.TotalD[d.Target] = total;
                            break;
                        case false:
                            FFXIV.Instance.TotalD.Add(d.Target, total);
                            break;
                    }
                }

                #region JSON Constructor

                if (Settings.Default.UploadParse && App.MArgs == null)
                {
                    if (Settings.Default.CICUID == "" || FFXIV.Desc == "" || d.Target != Settings.Default.CharacterName)
                    {
                        return;
                    }
                    var critc = (d.Crit) ? "1" : "0";
                    var counterc = (d.Counter) ? "1" : "0";
                    var blockc = (d.Block) ? "1" : "0";
                    var parryc = (d.Parry) ? "1" : "0";
                    var resistc = (d.Resist) ? "1" : "0";
                    var evadec = (d.Evade) ? "1" : "0";
                    var partialc = (d.Partial) ? "1" : "0";
                    json = "{\"uid\":\"" + FFXIV.UID + "\",\"cicuid\":\"" + Settings.Default.CICUID + "\",\"server\":\"" + Settings.Default.Server + "\",\"crit\":\"" + critc + "\",\"counter\":\"" + counterc + "\",\"block\":\"" + blockc + "\",\"parry\":\"" + parryc + "\",\"resist\":\"" + resistc + "\",\"evade\":\"" + evadec + "\",\"partial\":\"" + partialc + "\",\"monster\":\"" + d.Source + "\",\"target\":\"" + d.Target + "\",\"action\":\"" + d.Action + "\",\"direction\":\"" + d.Direction + "\",\"body_part\":\"" + d.Part + "\",\"amount\":\"" + d.Amount.ToString(CultureInfo.InvariantCulture) + "\",\"parse_desc\":\"" + FFXIV.Desc + "\"}";
                    q = "m";
                }

                #endregion
            }

            #endregion

            #region Healing by Members

            if (e.Type == EventType.Heal)
            {
                mReg = pUsed;
                switch (pUsed.Success)
                {
                    case true:
                        break;
                    case false:
                        ////Logger.Warn("MatchEvent : No match for Healing on line {0}", cleaned);
                        ChatWorkerDelegate.ParseXmlWriteUnmatchedLog.AddChatLine(new[] {cleaned, mCode, "#FFFFFF", mTimeStamp});
                        return;
                }
                d.Source = Convert.ToString(mReg.Groups["source"].Value);
                d.Action = Convert.ToString(mReg.Groups["action"].Value);
                d.Target = Convert.ToString(mReg.Groups["target"].Value);
                var recloss = Convert.ToString(mReg.Groups["recloss"].Value);
                d.Amount = mReg.Groups["amount"].Success ? Convert.ToDecimal(mReg.Groups["amount"].Value) : 0m;
                d.Type = Convert.ToString(mReg.Groups["type"].Value.ToUpper());
                if (Regex.IsMatch(d.Source, you))
                {
                    d.Source = Settings.Default.CharacterName;
                }
                if (Regex.IsMatch(d.Target, you))
                {
                    d.Target = Settings.Default.CharacterName;
                }
                if (String.IsNullOrWhiteSpace(d.Source))
                {
                    return;
                }
                Logger.Trace("HandlingEvent : Matched: {0}", cleaned);
                _valid = true;
                if (d.Type == type)
                {
                    FFXIV.Instance.Timeline.GetSetPlayer(d.Source).AddHealingStats(d);
                    var total = FFXIV.Instance.Timeline.Party.GetGroup(d.Source).GetStatValue("H Total").ToString();
                    switch (FFXIV.Instance.TotalH.ContainsKey(d.Source))
                    {
                        case true:
                            FFXIV.Instance.TotalH[d.Source] = total;
                            break;
                        case false:
                            FFXIV.Instance.TotalH.Add(d.Source, total);
                            break;
                    }

                    #region JSON Constructor

                    if (Settings.Default.UploadParse && App.MArgs == null && d.Source == Settings.Default.CharacterName)
                    {
                        if (Settings.Default.CICUID == "" || FFXIV.Desc == "")
                        {
                            return;
                        }
                        json = "{\"uid\":\"" + FFXIV.UID + "\",\"cicuid\":\"" + Settings.Default.CICUID + "\",\"server\":\"" + Settings.Default.Server + "\",\"caster\":\"" + d.Source + "\",\"target\":\"" + d.Target + "\",\"action\":\"" + d.Action + "\",\"amount\":\"" + d.Amount + "\",\"type\":\"" + d.Type + "\",\"parse_desc\":\"" + FFXIV.Desc + "\"}";
                        q = "h";
                    }

                    #endregion
                }
            }

            #endregion

            #region Save Parse to XML

            if (Settings.Default.Parse_SaveLog && App.MArgs == null & _valid)
            {
                ChatWorkerDelegate.ParseXmlWriteLog.AddChatLine(new[] {cleaned, mCode, "#FFFFFF", mTimeStamp});
            }

            #endregion

            #region Upload Parse

            if (!String.IsNullOrWhiteSpace(q) && !String.IsNullOrWhiteSpace(json))
            {
                Func<bool> sendJson = () => FFXIV.SubmitData(q, json);
                sendJson.BeginInvoke(result =>
                {
                    if (!sendJson.EndInvoke(result))
                    {
                    }
                }, null);
            }

            #endregion
        }
    }
}