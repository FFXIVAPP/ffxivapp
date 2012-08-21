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

            switch (Settings.Default.Language)
            {
                case "English":
                    pAction = (Constants.XPlayerRegEx.ContainsKey("ActionEn")) ? Constants.XPlayerRegEx["ActionEn"].Match(cleaned) : Player.ActionEn.Match(cleaned);
                    pUsed = (Constants.XPlayerRegEx.ContainsKey("UsedEn")) ? Constants.XPlayerRegEx["UsedEn"].Match(cleaned) : Player.UsedEn.Match(cleaned);
                    pAdditional = (Constants.XPlayerRegEx.ContainsKey("AdditionalEn")) ? Constants.XPlayerRegEx["AdditionalEn"].Match(cleaned) : Player.AdditionalEn.Match(cleaned);
                    pCounter = (Constants.XPlayerRegEx.ContainsKey("CounterEn")) ? Constants.XPlayerRegEx["CounterEn"].Match(cleaned) : Player.CounterEn.Match(cleaned);
                    pBlock = (Constants.XPlayerRegEx.ContainsKey("BlockEn")) ? Constants.XPlayerRegEx["BlockEn"].Match(cleaned) : Player.BlockEn.Match(cleaned);
                    pParry = (Constants.XPlayerRegEx.ContainsKey("ParryEn")) ? Constants.XPlayerRegEx["ParryEn"].Match(cleaned) : Player.ParryEn.Match(cleaned);
                    pResist = (Constants.XPlayerRegEx.ContainsKey("ResistEn")) ? Constants.XPlayerRegEx["ResistEn"].Match(cleaned) : Player.ResistEn.Match(cleaned);
                    pEvade = (Constants.XPlayerRegEx.ContainsKey("EvadeEn")) ? Constants.XPlayerRegEx["EvadeEn"].Match(cleaned) : Player.EvadeEn.Match(cleaned);
                    pMultiFlag = (Constants.XPlayerRegEx.ContainsKey("MultiFlagEn")) ? Constants.XPlayerRegEx["MultiFlagEn"].Match(cleaned) : Player.MultiFlagEn.Match(cleaned);
                    pMultiFlagAbility = ParseHelper.MultiEn;
                    pMulti = (Constants.XPlayerRegEx.ContainsKey("MultiEn")) ? Constants.XPlayerRegEx["MultiEn"].Match(cleaned) : Player.MultiEn.Match(cleaned);
                    mAction = (Constants.XMonsterRegEx.ContainsKey("ActionEn")) ? Constants.XMonsterRegEx["ActionEn"].Match(cleaned) : Monster.ActionEn.Match(cleaned);
                    mResist = (Constants.XMonsterRegEx.ContainsKey("ResistEn")) ? Constants.XMonsterRegEx["ResistEn"].Match(cleaned) : Monster.ResistEn.Match(cleaned);
                    mEvade = (Constants.XMonsterRegEx.ContainsKey("EvadeEn")) ? Constants.XMonsterRegEx["EvadeEn"].Match(cleaned) : Monster.EvadeEn.Match(cleaned);
                    counter = "Counter";
                    added = "Additional Effect";
                    type = "HP";
                    rattack = "Ranged Attack";
                    attack = "Attack";
                    break;
                case "French":
                    pAction = (Constants.XPlayerRegEx.ContainsKey("ActionFr")) ? Constants.XPlayerRegEx["ActionFr"].Match(cleaned) : Player.ActionFr.Match(cleaned);
                    pUsed = (Constants.XPlayerRegEx.ContainsKey("UsedFr")) ? Constants.XPlayerRegEx["UsedFr"].Match(cleaned) : Player.UsedFr.Match(cleaned);
                    pAdditional = (Constants.XPlayerRegEx.ContainsKey("AdditionalFr")) ? Constants.XPlayerRegEx["AdditionalFr"].Match(cleaned) : Player.AdditionalFr.Match(cleaned);
                    pCounter = (Constants.XPlayerRegEx.ContainsKey("CounterFr")) ? Constants.XPlayerRegEx["CounterFr"].Match(cleaned) : Player.CounterFr.Match(cleaned);
                    pBlock = (Constants.XPlayerRegEx.ContainsKey("BlockFr")) ? Constants.XPlayerRegEx["BlockFr"].Match(cleaned) : Player.BlockFr.Match(cleaned);
                    pParry = (Constants.XPlayerRegEx.ContainsKey("ParryFr")) ? Constants.XPlayerRegEx["ParryFr"].Match(cleaned) : Player.ParryFr.Match(cleaned);
                    pResist = (Constants.XPlayerRegEx.ContainsKey("ResistFr")) ? Constants.XPlayerRegEx["ResistFr"].Match(cleaned) : Player.ResistFr.Match(cleaned);
                    pEvade = (Constants.XPlayerRegEx.ContainsKey("EvadeFr")) ? Constants.XPlayerRegEx["EvadeFr"].Match(cleaned) : Player.EvadeFr.Match(cleaned);
                    pMultiFlag = (Constants.XPlayerRegEx.ContainsKey("MultiFlagFr")) ? Constants.XPlayerRegEx["MultiFlagFr"].Match(cleaned) : Player.MultiFlagFr.Match(cleaned);
                    pMultiFlagAbility = ParseHelper.MultiFr;
                    pMulti = (Constants.XPlayerRegEx.ContainsKey("MultiFr")) ? Constants.XPlayerRegEx["MultiFr"].Match(cleaned) : Player.MultiFr.Match(cleaned);
                    mAction = (Constants.XMonsterRegEx.ContainsKey("ActionFr")) ? Constants.XMonsterRegEx["ActionFr"].Match(cleaned) : Monster.ActionFr.Match(cleaned);
                    mResist = (Constants.XMonsterRegEx.ContainsKey("ResistFr")) ? Constants.XMonsterRegEx["ResistFr"].Match(cleaned) : Monster.ResistFr.Match(cleaned);
                    mEvade = (Constants.XMonsterRegEx.ContainsKey("EvadeFr")) ? Constants.XMonsterRegEx["EvadeFr"].Match(cleaned) : Monster.EvadeFr.Match(cleaned);
                    counter = "Contre";
                    added = "Effet Supplémentaire";
                    type = "PV";
                    rattack = "D'Attaque À Distance";
                    attack = "Attaque";
                    break;
                case "Japanese":
                    pAction = (Constants.XPlayerRegEx.ContainsKey("ActionJa")) ? Constants.XPlayerRegEx["ActionJa"].Match(cleaned) : Player.ActionJa.Match(cleaned);
                    pUsed = (Constants.XPlayerRegEx.ContainsKey("UsedJa")) ? Constants.XPlayerRegEx["UsedJa"].Match(cleaned) : Player.UsedJa.Match(cleaned);
                    pAdditional = (Constants.XPlayerRegEx.ContainsKey("AdditionalJa")) ? Constants.XPlayerRegEx["AdditionalJa"].Match(cleaned) : Player.AdditionalJa.Match(cleaned);
                    pCounter = (Constants.XPlayerRegEx.ContainsKey("CounterJa")) ? Constants.XPlayerRegEx["CounterJa"].Match(cleaned) : Player.CounterJa.Match(cleaned);
                    pBlock = (Constants.XPlayerRegEx.ContainsKey("BlockJa")) ? Constants.XPlayerRegEx["BlockJa"].Match(cleaned) : Player.BlockJa.Match(cleaned);
                    pParry = (Constants.XPlayerRegEx.ContainsKey("ParryJa")) ? Constants.XPlayerRegEx["ParryJa"].Match(cleaned) : Player.ParryJa.Match(cleaned);
                    pResist = (Constants.XPlayerRegEx.ContainsKey("ResistJa")) ? Constants.XPlayerRegEx["ResistJa"].Match(cleaned) : Player.ResistJa.Match(cleaned);
                    pEvade = (Constants.XPlayerRegEx.ContainsKey("EvadeJa")) ? Constants.XPlayerRegEx["EvadeJa"].Match(cleaned) : Player.EvadeJa.Match(cleaned);
                    pMultiFlag = (Constants.XPlayerRegEx.ContainsKey("MultiFlagJa")) ? Constants.XPlayerRegEx["MultiFlagJa"].Match(cleaned) : Player.MultiFlagJa.Match(cleaned);
                    pMultiFlagAbility = ParseHelper.MultiJa;
                    pMulti = (Constants.XPlayerRegEx.ContainsKey("MultiJa")) ? Constants.XPlayerRegEx["MultiJa"].Match(cleaned) : Player.MultiJa.Match(cleaned);
                    mAction = (Constants.XMonsterRegEx.ContainsKey("ActionJa")) ? Constants.XMonsterRegEx["ActionJa"].Match(cleaned) : Monster.ActionJa.Match(cleaned);
                    mResist = (Constants.XMonsterRegEx.ContainsKey("ResistJa")) ? Constants.XMonsterRegEx["ResistJa"].Match(cleaned) : Monster.ResistJa.Match(cleaned);
                    mEvade = (Constants.XMonsterRegEx.ContainsKey("EvadeJa")) ? Constants.XMonsterRegEx["EvadeJa"].Match(cleaned) : Monster.EvadeJa.Match(cleaned);
                    counter = "カウンター";
                    added = "追加効果";
                    type = "ＨＰ";
                    rattack = "Ranged Attack";
                    attack = "Attack";
                    break;
                case "German":
                    return;
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
                if (Regex.IsMatch(d.Source, @"^[Yy]our?$|^Vous"))
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
                if (Regex.IsMatch(d.Source, @"^[Yy]our?$"))
                {
                    d.Source = Settings.Default.CharacterName;
                }
                if (Regex.IsMatch(d.Target, @"^[Yy]our?$"))
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
                if (Regex.IsMatch(d.Source, @"^[Yy]our?$"))
                {
                    d.Source = Settings.Default.CharacterName;
                }
                if (Regex.IsMatch(d.Target, @"^[Yy]our?$"))
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