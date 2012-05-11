// ParseModXIV
// StatMonitor.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using AppModXIV.Classes;
using ParseModXIV.Classes;
using ParseModXIV.Model;
using ParseModXIV.Stats;

namespace ParseModXIV.Monitors
{
    public class StatMonitor : EventMonitor
    {
        internal readonly TotalStat TotalDamage = new TotalStat("Overall Damage");
        internal readonly TotalStat PartyDamage = new TotalStat("Reg Damage");
        internal readonly TotalStat PartyCritDamage = new TotalStat("Crit Damage");
        internal readonly TotalStat PartyHealing = new TotalStat("Overall Healing");
        internal readonly TotalStat PartyTotalTaken = new TotalStat("Overall Taken");
        internal readonly TotalStat PartyTotalRTaken = new TotalStat("Reg Taken");
        internal readonly TotalStat PartyTotalCTaken = new TotalStat("Crit Taken");
        private static readonly Hashtable Offsets = GetJob();
        private string LastAttacker = "";
        private string LastAttacked = "";

        public readonly Dictionary<string, string> TotalA = new Dictionary<string, string>();
        public readonly Dictionary<string, string> TotalD = new Dictionary<string, string>();
        public readonly Dictionary<string, string> TotalH = new Dictionary<string, string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parseModInstance"></param>
        public StatMonitor(ParseMod parseModInstance) : base("Stat Monitor", parseModInstance)
        {
            IncludeSelf = false;
            Filter = ((UInt16) EventDirection.By | (UInt16) EventSubject.You | (UInt16) EventSubject.Party | (UInt16) EventType.Attack | (UInt16) EventType.Heal | (UInt16) EventDirection.On);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitStats()
        {
            ParseModInstance.Timeline.Overall.Stats.Add(TotalDamage);
            ParseModInstance.Timeline.Overall.Stats.Add(PartyDamage);
            ParseModInstance.Timeline.Overall.Stats.Add(PartyCritDamage);
            ParseModInstance.Timeline.Overall.Stats.Add(PartyHealing);
            ParseModInstance.Timeline.Overall.Stats.Add(PartyTotalTaken);
            ParseModInstance.Timeline.Overall.Stats.Add(PartyTotalRTaken);
            ParseModInstance.Timeline.Overall.Stats.Add(PartyTotalCTaken);
            base.InitStats();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Clear()
        {
            //logger.Trace("Clearing ${0} party members Total Party Damage: {1} Total Party Crit Damage: {2} Total Party Healing: {3}", Count, partyDamage.Value, partyCritDamage.Value, partyHealing.Value);
            TotalDamage.Reset();
            PartyDamage.Reset();
            PartyCritDamage.Reset();
            PartyHealing.Reset();
            PartyTotalTaken.Reset();
            PartyTotalRTaken.Reset();
            PartyTotalCTaken.Reset();
            TotalA.Clear();
            TotalD.Clear();
            TotalH.Clear();
            base.Clear();
            Timeline.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void HandleEvent(Event e)
        {
            var mCode = "00" + e.Code.ToString("X");
            var mTimeStamp = DateTime.Now.ToString("[HH:mm:ss] ");
            var cleaned = XmlCleaner.SanitizeXmlString(e.RawLine);
            foreach (var tmp in RegExps.Mobbies)
            {
                e.RawLine = Regex.Replace(e.RawLine, tmp, tmp.Replace("'s", ""));
            }

            #region " ENGLISH "

            if (Settings.Default.Language == "English")
            {
                #region " DAMAGE TO MOBS "

                if (e.Type == EventType.Attack && e.Direction == EventDirection.By)
                {
                    var mReg = RegExpsEn.Damage.Match(e.RawLine);
                    if (!mReg.Success)
                    {
                        mReg = RegExpsEn.ResistsOrEvades.Match(e.RawLine);
                        if (!mReg.Success)
                        {
                            mReg = RegExpsEn.Additional.Match(e.RawLine);
                            if (!mReg.Success)
                            {
                                Logger.Trace("No match for Damage or Counter RegEx on line {0}", e.RawLine);
                                //ChatWorkerDelegate.XmlWriteLog.AddChatLine(new string[] { cleaned, mCode, mTimeStamp, "#FFFFFF" });
                                ChatWorkerDelegate.XmlWriteUnmatchedLog.AddChatLine(new[] { cleaned, mCode, mTimeStamp, "#FFFFFF" });
                                return;
                            }
                            if (!String.IsNullOrWhiteSpace(LastAttacked) && !String.IsNullOrWhiteSpace(LastAttacker))
                            {
                                var added = mReg.Groups["amount"].Success ? Convert.ToDecimal(mReg.Groups["amount"].Value) : 0m;
                                ParseModInstance.Timeline.GetOrAddStatsForParty(LastAttacker, PartyDamage, PartyHealing, PartyTotalTaken).AddAbilityStats(LastAttacked, "Added Effect", added, true, false, false);
                            }
                            return;
                        }
                        //logger.Trace("Matched ResistsOrEvades regex");
                    }
                    var didHit = (Convert.ToString(mReg.Groups["didHit"].Value) == "hits");
                    var resist = !(String.IsNullOrWhiteSpace(mReg.Groups["resist"].Value)) && mReg.Groups["resist"].Value != "evades";
                    var direction = ParseMod.TitleCase(Convert.ToString(mReg.Groups["direction"].Value), true);
                    var whoHit = Convert.ToString(mReg.Groups["whoHit"].Value);
                    var ability = Convert.ToString(mReg.Groups["ability"].Value);
                    switch (ability.ToLower())
                    {
                        case "ranged attack":
                            ability = "Ranged Attack";
                            break;
                        case "attack":
                            ability = "Attack";
                            break;
                    }
                    var mob = ParseMod.TitleCase((Convert.ToString(mReg.Groups["mob"].Value)), true);
                    var whoEvaded = ParseMod.TitleCase(Convert.ToString(mReg.Groups["whoEvaded"]), true);
                    var damage = mReg.Groups["amount"].Success ? Convert.ToDecimal(mReg.Groups["amount"].Value) : 0m;
                    var didCrit = !(String.IsNullOrWhiteSpace(Convert.ToString(mReg.Groups["crit"].Value)));
                    var bodyPart = ParseMod.TitleCase((Convert.ToString(mReg.Groups["bodyPart"].Value)), true);
                    //logger.Debug("Handling event : didHit: {0} whoHit: {1} ability: {2} mob: {3}", didHit, whoHit, ability, mob);
                    if (Regex.IsMatch(whoHit, @"^[Yy]our?$"))
                    {
                        whoHit = Settings.Default.CharacterName;
                    }
                    mob = (String.IsNullOrWhiteSpace(mob)) ? whoEvaded : mob;
                    if (String.IsNullOrWhiteSpace(whoHit) || String.IsNullOrWhiteSpace(mob))
                    {
                        return;
                    }
                    ParseModInstance.Timeline.PublishTimelineEvent(TimelineEventType.MobFighting, mob);
                    if (resist)
                    {
                        ParseModInstance.Timeline.GetOrAddStatsForMob(whoEvaded).AddAbilityStats(ability, damage, true, false, false);
                        didHit = true;
                    }
                    else
                    {
                        ParseModInstance.Timeline.GetOrAddStatsForMob(mob).AddAbilityStats(ability, damage, false, false, didCrit);
                    }
                    ParseModInstance.Timeline.GetOrAddStatsForParty(whoHit, PartyDamage, PartyHealing, PartyTotalTaken).AddAbilityStats(mob, ability, damage, didHit, didCrit, resist);
                    LastAttacker = whoHit;
                    LastAttacked = mob;
                    if (TotalA.ContainsKey(whoHit))
                    {
                        TotalA[whoHit] = ParseModInstance.Timeline.Party.GetGroup(whoHit).GetStatValue("Total").ToString();
                    }
                    else
                    {
                        TotalA[whoHit] = ParseModInstance.Timeline.Party.GetGroup(whoHit).GetStatValue("Total").ToString();
                    }
                    //var critical = "0";
                    //if (didCrit) critical = "1";
                    //string job;
                    //try
                    //{
                    //    job = Offsets[ability.ToLower()].ToString();
                    //}
                    //catch
                    //{
                    //    job = "Unknown";
                    //}

                    //if (target.Name != Settings.Default.CharacterName) return;
                    //if (Settings.Default.Gui_UploadData && App.MArgs == null)
                    //{
                    //    if (Settings.Default.CICUID == "" || ParseMod.Desc == "") return;
                    //    string json = "{\"uid\":\"" + ParseMod.Uid + "\",\"cicuid\":\"" + Settings.Default.CICUID + "\",\"server\":\"" + Settings.Default.Server + "\",\"crit\":\"" + critical + "\",\"counter\":\"0\",\"block\":\"0\",\"parry\":\"0\",\"resist\":\"0\",\"evade\":\"0\",\"player\":\"" + target.Name + "\",\"job\":\"" + ParseMod.TitleCase(job, true) + "\",\"target\":\"" + mob + "\",\"ability\":\"" + ability + "\",\"direction\":\"" + direction + "\",\"body_part\":\"" + bodyPart + "\",\"amount\":\"" + damage.ToString(CultureInfo.InvariantCulture) + "\",\"parse_desc\":\"" + ParseMod.Desc + "\"}";
                    //    Func<bool> sendJson = () => SubmitData("p", json);
                    //    sendJson.BeginInvoke(result =>
                    //    {
                    //        if (!sendJson.EndInvoke(result))
                    //        {

                    //        }
                    //    }, null);
                    //}

                    ////********** Commented until I find a cheaper host.
                    //if (App.MArgs == null)
                    //{
                    //    MainWindow.BattleLog.Add(new[] { didCrit.ToString(CultureInfo.InvariantCulture), "False", target.Name, mob, ability, direction, damage.ToString(CultureInfo.InvariantCulture) });
                    //}
                }

                #endregion

                #region " DAMAGE BY MOBS "

                if (e.Type == EventType.Attack && e.Direction == EventDirection.On)
                {
                    var mReg = RegExpsEn.DamageToPlayer.Match(e.RawLine);
                    if (!mReg.Success)
                    {
                        mReg = RegExpsEn.ResistsOrEvades.Match(e.RawLine);
                        if (!mReg.Success)
                        {
                            mReg = RegExpsEn.Blocks.Match(e.RawLine);
                            if (!mReg.Success)
                            {
                                Logger.Trace("No match for Damage Taken RegEx on line {0}", e.RawLine);
                                //ChatWorkerDelegate.XmlWriteLog.AddChatLine(new string[] { cleaned, mCode, mTimeStamp, "#FFFFFF" });
                                ChatWorkerDelegate.XmlWriteUnmatchedLog.AddChatLine(new[] {cleaned, mCode, mTimeStamp, "#FFFFFF"});
                                return;
                            }
                            //logger.Trace("Matched Blocks regex");
                        }
                        //logger.Trace("Matched ResistsOrEvades regex");
                    }
                    var didHit = (Convert.ToString(mReg.Groups["didHit"].Value) == "hits");
                    var resist = !(String.IsNullOrWhiteSpace(mReg.Groups["resist"].Value)) && mReg.Groups["resist"].Value != "evades";
                    var block = !(String.IsNullOrWhiteSpace(mReg.Groups["block"].Value)) && mReg.Groups["block"].Value != "evades";
                    var mob = ParseMod.TitleCase(Convert.ToString(mReg.Groups["whoHit"].Value), true);
                    var ability = Convert.ToString(mReg.Groups["ability"].Value);
                    var player = Convert.ToString(mReg.Groups["player"].Value);
                    var whoEvaded = Convert.ToString(mReg.Groups["whoEvaded"]);
                    var direction = ParseMod.TitleCase(Convert.ToString(mReg.Groups["direction"].Value), true);
                    if (ability.ToLower() == "ranged attack")
                    {
                        ability = "Ranged Attack";
                    }
                    if (ability.ToLower() == "attack")
                    {
                        ability = "Attack";
                    }
                    var damage = mReg.Groups["amount"].Success ? Convert.ToDecimal(mReg.Groups["amount"].Value) : 0m;
                    var didCrit = !(String.IsNullOrWhiteSpace(Convert.ToString(mReg.Groups["crit"].Value)));
                    var bodyPart = ParseMod.TitleCase((Convert.ToString(mReg.Groups["bodyPart"].Value)), true);
                    //logger.Debug("Handling event : didHit: {0} hitWho: {1} ability: {2} mob: {3}", didHit, player, ability, mob);
                    if (player == "")
                    {
                        player = whoEvaded;
                    }
                    if (Regex.IsMatch(player, @"^[Yy]our?$"))
                    {
                        player = Settings.Default.CharacterName;
                    }
                    if (String.IsNullOrWhiteSpace(player))
                    {
                        //logger.Error("Got blank target name : {0}", e.RawLine);
                        return;
                    }
                    ParseModInstance.Timeline.PublishTimelineEvent(TimelineEventType.MobFighting, mob);
                    ParseModInstance.Timeline.GetOrAddStatsForParty(player, PartyDamage, PartyHealing, PartyTotalTaken).AddDamageStats(mob, ability, damage, didHit, didCrit, block);
                    if (TotalD.ContainsKey(player))
                    {
                        TotalD[player] = ParseModInstance.Timeline.Party.GetGroup(player).GetStatValue("DT Total").ToString();
                    }
                    else
                    {
                        TotalD[player] = ParseModInstance.Timeline.Party.GetGroup(player).GetStatValue("DT Total").ToString();
                    }
                    //var critical = "0";
                    //if (didCrit) critical = "1";
                    //if (target.Name != Settings.Default.CharacterName) return;
                    //if (Settings.Default.Gui_UploadData && App.MArgs == null)
                    //{
                    //    if (Settings.Default.CICUID == "" || ParseMod.Desc == "") return;
                    //    string json = "{\"uid\":\"" + ParseMod.Uid + "\",\"cicuid\":\"" + Settings.Default.CICUID + "\",\"server\":\"" + Settings.Default.Server + "\",\"crit\":\"" + critical + "\",\"counter\":\"0\",\"block\":\"0\",\"parry\":\"0\",\"resist\":\"0\",\"evade\":\"0\",\"monster\":\"" + mob + "\",\"target\":\"" + target.Name + "\",\"ability\":\"" + ability + "\",\"direction\":\"" + direction + "\",\"body_part\":\"" + bodyPart + "\",\"amount\":\"" + damage.ToString(CultureInfo.InvariantCulture) + "\",\"parse_desc\":\"" + ParseMod.Desc + "\"}";
                    //    Func<bool> sendJson = () => SubmitData("m", json);
                    //    sendJson.BeginInvoke(result =>
                    //    {
                    //        if (!sendJson.EndInvoke(result))
                    //        {

                    //        }
                    //    }, null);
                    //}

                    ////********** Commented until I find a cheaper host.
                    //if (App.MArgs == null)
                    //{
                    //    MainWindow.BattleLog.Add(new[] { didCrit.ToString(CultureInfo.InvariantCulture), "False", mob, target.Name, ability, direction, damage.ToString(CultureInfo.InvariantCulture) });
                    //}
                }

                #endregion

                #region " HEALING BY MEMBERS "

                if (e.Type == EventType.Heal)
                {
                    var hReg = RegExpsEn.UseOnParty.Match(e.RawLine);
                    if (!hReg.Success)
                    {
                        Logger.Trace("No match for Healing on line {0}", e.RawLine);
                        //ChatWorkerDelegate.XmlWriteLog.AddChatLine(new string[] { cleaned, mCode, mTimeStamp, "#FFFFFF" });
                        ChatWorkerDelegate.XmlWriteUnmatchedLog.AddChatLine(new[] {cleaned, mCode, mTimeStamp, "#FFFFFF"});
                        return;
                    }
                    var whoDid = Convert.ToString(hReg.Groups["whoDid"].Value);
                    var ability = Convert.ToString(hReg.Groups["ability"].Value);
                    var castOn = Convert.ToString(hReg.Groups["castOn"].Value);
                    var recLoss = Convert.ToString(hReg.Groups["recLoss"].Value);
                    var amount = hReg.Groups["amount"].Success ? Convert.ToDecimal(hReg.Groups["amount"].Value) : 0m;
                    var type = Convert.ToString(hReg.Groups["type"].Value.ToUpper());

                    if (Regex.IsMatch(whoDid, @"^[Yy]our?$"))
                    {
                        whoDid = Settings.Default.CharacterName;
                    }
                    if (Regex.IsMatch(castOn, @"^[Yy]our?$"))
                    {
                        castOn = Settings.Default.CharacterName;
                    }
                    //logger.Debug("Handling event : whoDid: {0} ability: {1} castOn: {2} recLoss: {3} amount: {4} type: {5}", whoDid, ability, castOn, recLoss, amount, type);
                    if (String.IsNullOrWhiteSpace(whoDid))
                    {
                        return;
                    }
                    if (type == "HP")
                    {
                        ParseModInstance.Timeline.GetOrAddStatsForParty(whoDid, PartyDamage, PartyHealing, PartyTotalTaken).AddHealingStats(ability, castOn, amount);
                        //if (whoDid != Settings.Default.CharacterName) return;
                        //if (Settings.Default.Gui_UploadData && App.MArgs == null)
                        //{
                        //    if (Settings.Default.CICUID == "" || ParseMod.Desc == "") return;
                        //    string json = "{\"uid\":\"" + ParseMod.Uid + "\",\"cicuid\":\"" + Settings.Default.CICUID + "\",\"server\":\"" + Settings.Default.Server + "\",\"caster\":\"" + whoDid + "\",\"target\":\"" + castOn + "\",\"ability\":\"" + ability + "\",\"amount\":\"" + amount + "\",\"type\":\"" + type + "\",\"parse_desc\":\"" + ParseMod.Desc + "\"}";
                        //    Func<bool> sendJson = () => SubmitData("h", json);
                        //    sendJson.BeginInvoke(result =>
                        //    {
                        //        if (!sendJson.EndInvoke(result))
                        //        {

                        //        }
                        //    }, null);
                        //}
                        if (TotalH.ContainsKey(whoDid))
                        {
                            TotalH[whoDid] = ParseModInstance.Timeline.Party.GetGroup(whoDid).GetStatValue("H Total").ToString();
                        }
                        else
                        {
                            TotalH[whoDid] = ParseModInstance.Timeline.Party.GetGroup(whoDid).GetStatValue("H Total").ToString();
                        }
                    }
                    ////********** Commented until I find a cheaper host.
                    //if (App.MArgs == null)
                    //{
                    //    MainWindow.HealingLog.Add(new[] { whoDid, castOn, ability, amount.ToString(CultureInfo.InvariantCulture), type });
                    //}
                }

                #endregion
            }

            #endregion

            #region " FRENCH "

            if (Settings.Default.Language == "French")
            {
                #region " DAMAGE TO MOBS "

                if (e.Type == EventType.Attack && e.Direction == EventDirection.By)
                {
                    var mReg = RegExpsFr.Damage.Match(e.RawLine);
                    if (!mReg.Success)
                    {
                        mReg = RegExpsFr.Resists.Match(e.RawLine);
                        if (!mReg.Success)
                        {
                            //Logger.Trace("No match for Damage or Resist RegEx on line {0}", e.RawLine);
                            ChatWorkerDelegate.XmlWriteUnmatchedLog.AddChatLine(new[] {cleaned, mCode, mTimeStamp, "#FFFFFF"});
                            return;
                        }
                        //logger.Trace("Matched Resists regex");
                    }
                    var didHit = (Convert.ToString(mReg.Groups["didHit"].Value) == "et") || (Convert.ToString(mReg.Groups["didHit"].Value) == "sur");
                    var resist = !(String.IsNullOrWhiteSpace(mReg.Groups["resist"].Value)) && mReg.Groups["resist"].Value != "evades";
                    var direction = ParseMod.TitleCase(Convert.ToString(mReg.Groups["direction"].Value), true);
                    var whoHit = Convert.ToString(mReg.Groups["whoHit"].Value);
                    var ability = Convert.ToString(mReg.Groups["ability"].Value);
                    if (ability.ToLower() == "attaque")
                    {
                        ability = "Attaque";
                    }
                    var mob = ParseMod.TitleCase((Convert.ToString(mReg.Groups["mob"].Value)), true);
                    var whoEvaded = ParseMod.TitleCase(Convert.ToString(mReg.Groups["whoEvaded"]), true);
                    var damage = mReg.Groups["amount"].Success ? Convert.ToDecimal(mReg.Groups["amount"].Value) : 0m;
                    var didCrit = !(String.IsNullOrWhiteSpace(Convert.ToString(mReg.Groups["crit"].Value)));
                    //logger.Debug("Handling event : didHit: {0} whoHit: {1} ability: {2} mob: {3}", didHit, whoHit, ability, mob);
                    if (Regex.IsMatch(whoHit, @"^[Yy]our?$"))
                    {
                        whoHit = Settings.Default.CharacterName;
                    }
                    mob = (String.IsNullOrWhiteSpace(mob)) ? whoEvaded : mob;
                    if (String.IsNullOrWhiteSpace(whoHit) || String.IsNullOrWhiteSpace(mob))
                    {
                        return;
                    }
                    ParseModInstance.Timeline.PublishTimelineEvent(TimelineEventType.MobFighting, mob);
                    if (resist)
                    {
                        ParseModInstance.Timeline.GetOrAddStatsForMob(whoEvaded).AddAbilityStats(ability, damage, true, false, false);
                        didHit = true;
                    }
                    else
                    {
                        ParseModInstance.Timeline.GetOrAddStatsForMob(mob).AddAbilityStats(ability, damage, false, false, didCrit);
                    }
                    ParseModInstance.Timeline.GetOrAddStatsForParty(whoHit, PartyDamage, PartyHealing, PartyTotalTaken).AddAbilityStats(mob, ability, damage, didHit, didCrit, resist);
                    if (TotalA.ContainsKey(whoHit))
                    {
                        TotalA[whoHit] = ParseModInstance.Timeline.Party.GetGroup(whoHit).GetStatValue("Total").ToString();
                    }
                    else
                    {
                        TotalA[whoHit] = ParseModInstance.Timeline.Party.GetGroup(whoHit).GetStatValue("Total").ToString();
                    }
                }

                #endregion

                #region " DAMAGE BY MOBS "

                if (e.Type == EventType.Attack && e.Direction == EventDirection.On)
                {
                    var mReg = RegExpsFr.DamageToPlayer.Match(e.RawLine);
                    if (!mReg.Success)
                    {
                        mReg = RegExpsFr.Resists.Match(e.RawLine);
                        if (!mReg.Success)
                        {
                            //Logger.Trace("No match for Damage Taken or Resist Taken RegEx on line {0}", e.RawLine);
                            ChatWorkerDelegate.XmlWriteUnmatchedLog.AddChatLine(new[] {cleaned, mCode, mTimeStamp, "#FFFFFF"});
                            return;
                        }
                    }
                    var didHit = (Convert.ToString(mReg.Groups["didHit"].Value) == "et") || (Convert.ToString(mReg.Groups["didHit"].Value) == "sur");
                    var resist = !(String.IsNullOrWhiteSpace(mReg.Groups["resist"].Value)) && mReg.Groups["resist"].Value != "evades";
                    var mob = ParseMod.TitleCase(Convert.ToString(mReg.Groups["whoHit"].Value), true);
                    var whoEvaded = ParseMod.TitleCase(Convert.ToString(mReg.Groups["whoEvaded"]), true);
                    var ability = Convert.ToString(mReg.Groups["ability"].Value);
                    var player = Convert.ToString(mReg.Groups["player"].Value);
                    var direction = ParseMod.TitleCase(Convert.ToString(mReg.Groups["direction"].Value), true);
                    //if (ability.ToLower() == "ranged attack") ability = "Ranged Attack";
                    if (ability.ToLower() == "attaque")
                    {
                        ability = "attaque";
                    }
                    var damage = mReg.Groups["amount"].Success ? Convert.ToDecimal(mReg.Groups["amount"].Value) : 0m;
                    var didCrit = !(String.IsNullOrWhiteSpace(Convert.ToString(mReg.Groups["crit"].Value)));
                    //logger.Debug("Handling event : didHit: {0} hitWho: {1} ability: {2} mob: {3}", didHit, player, ability, mob);
                    if (Regex.IsMatch(player, @"^[Yy]our?$"))
                    {
                        player = Settings.Default.CharacterName;
                    }
                    if (String.IsNullOrWhiteSpace(player))
                    {
                        //logger.Error("Got blank target name : {0}", e.RawLine);
                        return;
                    }
                    ParseModInstance.Timeline.PublishTimelineEvent(TimelineEventType.MobFighting, mob);
                    ParseModInstance.Timeline.GetOrAddStatsForParty(player, PartyDamage, PartyHealing, PartyTotalTaken).AddDamageStats(mob, ability, damage, didHit, didCrit, false);
                    if (TotalD.ContainsKey(player))
                    {
                        TotalD[player] = ParseModInstance.Timeline.Party.GetGroup(player).GetStatValue("DT Total").ToString();
                    }
                    else
                    {
                        TotalD[player] = ParseModInstance.Timeline.Party.GetGroup(player).GetStatValue("DT Total").ToString();
                    }
                }

                #endregion

                #region " HEALING BY MEMBERS "

                if (e.Type == EventType.Heal)
                {
                    var hReg = RegExpsFr.UseOnParty.Match(e.RawLine);
                    if (!hReg.Success)
                    {
                        //Logger.Trace("No match for Healing on line {0}", e.RawLine);
                        ChatWorkerDelegate.XmlWriteUnmatchedLog.AddChatLine(new[] {cleaned, mCode, mTimeStamp, "#FFFFFF"});
                        return;
                    }
                    var whoDid = Convert.ToString(hReg.Groups["whoDid"].Value);
                    var ability = Convert.ToString(hReg.Groups["ability"].Value);
                    var castOn = Convert.ToString(hReg.Groups["castOn"].Value);
                    var recLoss = Convert.ToString(hReg.Groups["recLoss"].Value);
                    var amount = hReg.Groups["amount"].Success ? Convert.ToDecimal(hReg.Groups["amount"].Value) : 0m;
                    var type = Convert.ToString(hReg.Groups["type"].Value.ToUpper());
                    if (Regex.IsMatch(whoDid, @"^[Yy]our?$"))
                    {
                        whoDid = Settings.Default.CharacterName;
                    }
                    if (Regex.IsMatch(castOn, @"^[Yy]our?$"))
                    {
                        castOn = Settings.Default.CharacterName;
                    }
                    //logger.Debug("Handling event : whoDid: {0} ability: {1} castOn: {2} recLoss: {3} amount: {4} type: {5}", whoDid, ability, castOn, recLoss, amount, type);
                    if (String.IsNullOrWhiteSpace(whoDid))
                    {
                        return;
                    }
                    if (type == "PV")
                    {
                        ParseModInstance.Timeline.GetOrAddStatsForParty(whoDid, PartyDamage, PartyHealing, PartyTotalTaken).AddHealingStats(ability, castOn, amount);

                        if (TotalH.ContainsKey(whoDid))
                        {
                            TotalH[whoDid] = ParseModInstance.Timeline.Party.GetGroup(whoDid).GetStatValue("H Total").ToString();
                        }
                        else
                        {
                            TotalH[whoDid] = ParseModInstance.Timeline.Party.GetGroup(whoDid).GetStatValue("H Total").ToString();
                        }
                    }
                }

                #endregion
            }

            #endregion

            #region " SAVE PARSE TO XML "

            if (Settings.Default.Gui_SaveLog && App.MArgs == null)
            {
                ChatWorkerDelegate.XmlWriteLog.AddChatLine(new[] {cleaned, mCode, "#FFFFFF", mTimeStamp});
            }

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="insert"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool SubmitData(string insert, string message)
        {
            var url = string.Format("http://ffxiv-app.com/battles/insert/?insert={0}&q={1}", insert, HttpUtility.UrlEncode(message));
            var httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";

            var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var responseText = streamReader.ReadToEnd();
                return true;
            }
        }

        #region " JOB INFO "

        private static Hashtable GetJob()
        {
            var offsets = new Hashtable {{"phalanx", "gladiator"}, {"aegis boon", "gladiator"}, {"riot blade", "gladiator"}, {"war drum", "gladiator"}, {"tempered will", "gladiator"}, {"rage of halone", "gladiator"}, {"goring blade", "gladiator"}, {"cover", "paladin"}, {"divine veil", "paladin"}, {"hallowed ground", "paladin"}, {"holy succor", "paladin"}, {"spirits within", "paladin"}, {"pounce", "puglist"}, {"haymaker", "puglist"}, {"fists of earth", "puglist"}, {"fists of fire", "puglist"}, {"aura pulse", "puglist"}, {"taunt", "puglist"}, {"howling fist", "puglist"}, {"simian thrash", "puglist"}, {"shoulder tackle", "monk"}, {"spinning heal", "monk"}, {"fists of wind", "monk"}, {"dragon kick", "monk"}, {"hundred fists", "monk"}, {"fracture", "marauder"}, {"berserk", "marauder"}, {"rampage", "marauder"}, {"path of the storm", "marauder"}, {"enduring march", "marauder"}, {"whirlwind", "marauder"}, {"godsbane", "marauder"}, {"vengeance", "warrior"}, {"antagonize", "warrior"}, {"collusion", "warrior"}, {"mighty strikes", "warrior"}, {"steel cyclone", "warrior"}, {"life surge", "lancer"}, {"power surge", "lancer"}, {"full thrust", "lancer"}, {"dread spike", "lancer"}, {"doom spike", "lancer"}, {"chaos thrust", "lancer"}, {"jump", "dragoon"}, {"elusive jump", "dragoon"}, {"dragonfire dive", "dragoon"}, {"disembowel", "dragoon"}, {"ring of talons", "dragoon"}, {"light shot", "archer"}, {"raging strike", "archer"}, {"shadowbind", "archer"}, {"swiftsong", "archer"}, {"barrage", "archer"}, {"quick nock", "archer"}, {"bloodletter", "archer"}, {"wide volley", "archer"}, {"battle voice", "bard"}, {"rain of death", "bard"}, {"ballad of magi", "bard"}, {"paeon of war", "bard"}, {"minuet of rigor", "bard"}, {"cleric stance", "conjurer"}, {"blissful mind", "conjurer"}, {"stonera", "conjurer"}, {"cura", "conjurer"}, {"shroud of saints", "conjurer"}, {"aerora", "conjurer"}, {"curaga", "conjurer"}, {"repose", "conjurer"}, {"presence of mind", "white mage"}, {"benediction", "white mage"}, {"esuna", "white mage"}, {"regen", "white mage"}, {"holy", "white mage"}, {"parsimony", "thaumaturgy"}, {"blizzard", "thaumaturgy"}, {"thundara", "thaumaturgy"}, {"blizzara", "thaumaturgy"}, {"excruciate", "thaumaturgy"}, {"sleep", "thaumaturgy"}, {"thundaga", "thaumaturgy"}, {"firaga", "thaumaturgy"}, {"convert", "black mage"}, {"burst", "black mage"}, {"sleepga", "black mage"}, {"flare", "black mage"}, {"freeze", "black mage"}};
            return offsets;
        }

        #endregion
    }
}