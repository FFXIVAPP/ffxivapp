// ParseModXIV
// StatMonitor.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
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
        private readonly TotalStat _totalDamage = new TotalStat("Overall Damage");
        private readonly TotalStat _partyDamage = new TotalStat("Reg Damage");
        private readonly TotalStat _partyCritDamage = new TotalStat("Crit Damage");
        private readonly TotalStat _partyHealing = new TotalStat("Overall Healing");
        private readonly TotalStat _partyTotalTaken = new TotalStat("Overall Taken");
        private readonly TotalStat _partyTotalRTaken = new TotalStat("Reg Taken");
        private readonly TotalStat _partyTotalCTaken = new TotalStat("Crit Taken");
        private static readonly Hashtable Offsets = GetJob();

        public readonly Dictionary<string, string> TotalD = new Dictionary<string, string>();
        public readonly Dictionary<string, string> TotalH = new Dictionary<string, string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parseModInstance"></param>
        public StatMonitor(ParseMod parseModInstance) : base("Stat Monitor", parseModInstance)
        {
            IncludeSelf = false;
            Filter = ((UInt16) EventDirection.By | (UInt16) EventSubject.You | (UInt16) EventSubject.Party |
                      (UInt16) EventType.Attack | (UInt16) EventType.Heal | (UInt16) EventDirection.On);
            ParseModInstance.Timeline.OnTimelineEvent += (src, e) => UpdatePartyList(e);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitStats()
        {
            ParseModInstance.Timeline.OverallStats.Stats.Add(_totalDamage);
            ParseModInstance.Timeline.OverallStats.Stats.Add(_partyDamage);
            ParseModInstance.Timeline.OverallStats.Stats.Add(_partyCritDamage);
            ParseModInstance.Timeline.OverallStats.Stats.Add(_partyHealing);
            ParseModInstance.Timeline.OverallStats.Stats.Add(_partyTotalTaken);
            ParseModInstance.Timeline.OverallStats.Stats.Add(_partyTotalRTaken);
            ParseModInstance.Timeline.OverallStats.Stats.Add(_partyTotalCTaken);
            ParseModInstance.Timeline.PlayerStats.Stats.AddStats(NewStatList());
            ParseModInstance.Timeline.PlayerStats.Add(new StatGroupAbilities("Abilities", _partyDamage));
            ParseModInstance.Timeline.PlayerStats.Add(new StatGroupHealing("Healing", _partyHealing));
            ParseModInstance.Timeline.PlayerStats.Add(new StatGroupDamage("Damage", _partyTotalTaken));
            base.InitStats();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private StatGroup CreatePlayerStatGroup(String name)
        {
            var playerGroup = new StatGroup(name);
            playerGroup.Stats.AddStats(NewStatList());
            var abilityGroup = new StatGroupAbilities("Abilities", _partyDamage);
            var healingGroup = new StatGroupHealing("Healing", _partyHealing);
            var damageGroup = new StatGroupDamage("Damage", _partyTotalTaken);
            playerGroup.AddGroup(abilityGroup);
            playerGroup.AddGroup(healingGroup);
            playerGroup.AddGroup(damageGroup);
            if (name == Settings.Default.CharacterName)
            {
                var playerStats = ParseModInstance.Timeline.PlayerStats;
                // Extra linkage for "You in a party" stats
                foreach (var statName in new String[] {})
                {
                    var playerStat = (TotalStat) playerStats.Stats.GetStat(statName);
                    playerStat.AddDependency(playerGroup.Stats.GetStat(statName));
                }
            }
            return playerGroup;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Clear()
        {
            //logger.Trace("Clearing ${0} party members Total Party Damage: {1} Total Party Crit Damage: {2} Total Party Healing: {3}", Count, partyDamage.Value, partyCritDamage.Value, partyHealing.Value);
            _totalDamage.Reset();
            _partyDamage.Reset();
            _partyCritDamage.Reset();
            _partyHealing.Reset();
            _partyTotalTaken.Reset();
            _partyTotalRTaken.Reset();
            _partyTotalCTaken.Reset();
            TotalD.Clear();
            TotalH.Clear();
            foreach (var g in this)
            {
                MoveToInactive(g.Name);
            }
            base.Clear();
            Timeline.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        private void MoveToInactive(String name)
        {
            if (!HasGroup(name))
            {
                return;
            }
            var dudeWhoLeft = GetGroup(name);
            var totalStat = dudeWhoLeft.Stats.GetStat("Total");
            var regularTotalStat = dudeWhoLeft.Stats.GetStat("Reg");
            var critTotalStat = dudeWhoLeft.Stats.GetStat("Crit");
            var totalHStat = dudeWhoLeft.Stats.GetStat("H Total");
            var totalDtStat = dudeWhoLeft.Stats.GetStat("DT Total");
            var totalDtrStat = dudeWhoLeft.Stats.GetStat("DT Reg");
            var totalDtcStat = dudeWhoLeft.Stats.GetStat("DT Crit");
            _totalDamage.RemoveDependency(totalStat);
            _partyDamage.RemoveDependency(regularTotalStat);
            _partyCritDamage.RemoveDependency(critTotalStat);
            _partyHealing.RemoveDependency(totalHStat);
            _partyTotalTaken.RemoveDependency(totalDtStat);
            _partyTotalRTaken.RemoveDependency(totalDtrStat);
            _partyTotalCTaken.RemoveDependency(totalDtcStat);
            ParseModInstance.Timeline.PartyStats.Remove(dudeWhoLeft);
            ParseModInstance.Timeline.InactivePartyStats.Add(dudeWhoLeft);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private StatGroup MoveToActive(String name)
        {
            if (!ParseModInstance.Timeline.InactivePartyStats.HasGroup(name))
            {
                return null;
            }
            var dudeWhoJoined = ParseModInstance.Timeline.InactivePartyStats.GetGroup(name);
            ParseModInstance.Timeline.InactivePartyStats.Remove(dudeWhoJoined);
            Add(dudeWhoJoined);
            _totalDamage.AddDependency(dudeWhoJoined.Stats.GetStat("Total"));
            _partyDamage.AddDependency(dudeWhoJoined.Stats.GetStat("Reg"));
            _partyCritDamage.AddDependency(dudeWhoJoined.Stats.GetStat("Crit"));
            _partyHealing.AddDependency(dudeWhoJoined.Stats.GetStat("H Total"));
            _partyTotalTaken.AddDependency(dudeWhoJoined.Stats.GetStat("DT Total"));
            _partyTotalTaken.AddDependency(dudeWhoJoined.Stats.GetStat("DT Reg"));
            _partyTotalTaken.AddDependency(dudeWhoJoined.Stats.GetStat("DT Crit"));
            return dudeWhoJoined;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void UpdatePartyList(TimelineEventArgs e)
        {
            switch (e.Type)
            {
                case TimelineEventType.PartyDisband:
                    //Clear();
                    break;
                case TimelineEventType.PartyJoin:
                    var whoJoined = e.Args[0] as String;
                    //logger.Info("{0} Joining Party", whoJoined);
                    if (!ParseModInstance.Timeline.PartyStats.HasGroup(whoJoined))
                    {
                        if (ParseModInstance.Timeline.InactivePartyStats.HasGroup(whoJoined))
                        {
                            MoveToActive(whoJoined);
                        }
                        else
                        {
                            ParseModInstance.Timeline.PartyStats.AddGroup(CreatePlayerStatGroup(whoJoined));
                        }
                    }
                    break;
                case TimelineEventType.PartyLeave:
                    var whoLeft = e.Args[0] as String;
                    //logger.Info("{0} Leaving Party", whoLeft);
                    //if (ParseModInstance.Timeline.PlayerInParty)
                    //{
                    //    MoveToInactive(whoLeft);
                    //}
                    //else if (whoLeft == "You")
                    //{
                    //    var playerStats = ParseModInstance.Timeline.PlayerStats;
                    //    var playerGroup = ParseModInstance.Timeline.PartyStats.GetGroup(playerStats.Name);
                    //    if (playerGroup == null)
                    //    {
                    //        logger.Warn("Got null player group from party stats using character name \"{0}\"", playerStats.Name);
                    //        return;
                    //    }
                    //    foreach (var statName in new String[] { })
                    //    {
                    //        var playerStat = (TotalStat)playerStats.Stats.GetStat(statName);
                    //        playerStat.RemoveDependency(playerGroup.Stats.GetStat(statName));
                    //    }
                    //    Clear();
                    //}
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Stat<Decimal>[] NewStatList()
        {
            var totalStat = new TotalStat("Total");
            var regularTotalStat = new TotalStat("Reg");
            var critTotalStat = new TotalStat("Crit");
            var healTotalStat = new TotalStat("H Total");
            var totalDtStat = new TotalStat("DT Total");
            var totalDtrStat = new TotalStat("DT Reg");
            var totalDtcStat = new TotalStat("DT Crit");
            _totalDamage.AddDependency(totalStat);
            _partyDamage.AddDependency(regularTotalStat);
            _partyCritDamage.AddDependency(critTotalStat);
            _partyHealing.AddDependency(healTotalStat);
            _partyTotalTaken.AddDependency(totalDtStat);
            _partyTotalRTaken.AddDependency(totalDtrStat);
            _partyTotalCTaken.AddDependency(totalDtcStat);
            var hitStat = new TotalStat("Hit");
            var cHitStat = new TotalStat("C Hit");
            var dtHitStat = new TotalStat("DT Hit");
            var dtcHitStat = new TotalStat("DT C Hit");
            var missStat = new TotalStat("Miss");
            var accuracyStat = new AccuracyStat("Acc", hitStat, missStat);
            var evadeStat = new TotalStat("Evade");
            var damagePctStat = new PercentStat("% of Dmg", totalStat, _partyDamage);
            var cDamagePctStat = new PercentStat("% of Crit", critTotalStat, _partyCritDamage);
            var damageDtPctStat = new PercentStat("% of DT Dmg", totalDtStat, _partyTotalTaken);
            var cDamageDtPctStat = new PercentStat("% of DT C Dmg", totalDtcStat, _partyTotalCTaken);
            var critPctStat = new PercentStat("Crit %", cHitStat, hitStat);
            var healingPctStat = new PercentStat("% of Heal", healTotalStat, _partyHealing);
            var minStat = new MinStat("Low", regularTotalStat);
            var maxStat = new MaxStat("High", regularTotalStat);
            var minCStat = new MinStat("C Low", critTotalStat);
            var maxCStat = new MaxStat("C High", critTotalStat);
            var minHStat = new MinStat("Heal Low", healTotalStat);
            var maxHStat = new MaxStat("Heal High", healTotalStat);
            var minDtStat = new MinStat("DT Low", totalDtStat);
            var maxDtStat = new MaxStat("DT High", totalDtStat);
            var minDtcStat = new MinStat("DT C Low", totalDtcStat);
            var maxDtcStat = new MaxStat("DT C High", totalDtcStat);
            var avgDamageStat = new AverageStat("Avg", totalStat);
            var avgCDamageStat = new AverageStat("C Avg", critTotalStat);
            var avgDtDamageStat = new AverageStat("DT Avg", totalDtStat);
            var avgDtcDamageStat = new AverageStat("DT C Avg", totalDtcStat);
            var avgHealingStat = new AverageStat("Heal Avg", healTotalStat);
            var dpsStat = new PerSecondAverageStat("DPS", totalStat);
            var hpsStat = new PerSecondAverageStat("HPS", healTotalStat);
            var dtpsStat = new PerSecondAverageStat("DTPS", totalDtStat);
            return new Stat<decimal>[]
                   {
                       totalStat, regularTotalStat, critTotalStat, healTotalStat, hitStat, cHitStat, missStat, accuracyStat
                       , evadeStat, damagePctStat, cDamagePctStat, critPctStat, healingPctStat, minStat, maxStat, minCStat,
                       maxCStat, minHStat, maxHStat, avgDamageStat, avgCDamageStat, avgHealingStat, dpsStat, hpsStat,
                       totalDtStat, totalDtrStat, totalDtcStat, dtHitStat, dtcHitStat, damageDtPctStat, cDamageDtPctStat,
                       minDtStat, maxDtStat, minDtcStat, maxDtcStat, avgDtDamageStat, avgDtcDamageStat, dtpsStat
                   };
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
                            Logger.Trace("No match for Damage or Counter RegEx on line {0}", e.RawLine);
                            //ChatWorkerDelegate.XmlWriteLog.AddChatLine(new string[] { cleaned, mCode, mTimeStamp, "#FFFFFF" });
                            return;
                        }
                        //logger.Trace("Matched ResistsOrEvades regex");
                    }
                    var didHit = (Convert.ToString(mReg.Groups["didHit"].Value) == "hits");
                    var resist = !(String.IsNullOrWhiteSpace(mReg.Groups["resist"].Value)) &&
                                 mReg.Groups["resist"].Value != "evades";
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
                    StatGroup target;
                    if (Regex.IsMatch(whoHit, @"^[Yy]our?$"))
                    {
                        whoHit = Settings.Default.CharacterName;
                        target = ParseModInstance.Timeline.PlayerInParty
                                     ? ParseModInstance.Timeline.PartyStats.GetGroup(Settings.Default.CharacterName)
                                     : ParseModInstance.Timeline.PlayerStats;
                        if (target == null && ParseModInstance.Timeline.PlayerInParty)
                        {
                            target = CreatePlayerStatGroup(Settings.Default.CharacterName);
                            ParseModInstance.Timeline.PartyStats.AddGroup(target);
                        }
                    }
                    else
                    {
                        if (ParseModInstance.Timeline.PartyStats.HasGroup(whoHit))
                        {
                            target = ParseModInstance.Timeline.PartyStats.GetGroup(whoHit);
                        }
                        else
                        {
                            //logger.Warn("Adding party member {0} to active list because of damage event (no party join event received)", whoHit);
                            if (ParseModInstance.Timeline.InactivePartyStats.HasGroup(whoHit))
                            {
                                target = MoveToActive(whoHit);
                            }
                            else
                            {
                                target = CreatePlayerStatGroup(whoHit);
                                ParseModInstance.Timeline.PartyStats.AddGroup(target);
                            }
                        }
                    }
                    if (target == null)
                    {
                        return;
                    }
                    if (String.IsNullOrWhiteSpace(target.Name))
                    {
                        //logger.Error("Got blank target name : {0}", e.RawLine);
                        return;
                    }
                    ParseModInstance.Timeline.PublishTimelineEvent(TimelineEventType.MobFighting, mob);
                    if (didHit || resist)
                    {
                        target.Stats.GetStat("Total").Value += damage;
                        if (didCrit)
                        {
                            target.Stats.GetStat("Crit").Value += damage;
                            target.Stats.GetStat("C Hit").Value += 1;
                        }
                        else
                        {
                            target.Stats.GetStat("Reg").Value += damage;
                            target.Stats.GetStat("Hit").Value += 1;
                        }
                    }
                    else
                    {
                        target.Stats.GetStat("Miss").Value += 1;
                    }
                    if (resist)
                    {
                        ParseModInstance.Timeline.GetOrAddStatsForMob(whoEvaded).AddAbilityStats(ability, damage, true,
                                                                                                 false, false);
                        didHit = true;
                    }
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(mob))
                        {
                            ParseModInstance.Timeline.GetOrAddStatsForMob(mob).AddAbilityStats(ability, damage, false,
                                                                                               false, didCrit);
                        }
                    }
                    UpdateAbilities(target, damage, didHit, didCrit, ability, resist);
                    if (TotalD.ContainsKey(target.Name))
                    {
                        TotalD[target.Name] = target.GetStatValue("Total").ToString();
                    }
                    else
                    {
                        TotalD[target.Name] = target.GetStatValue("Total").ToString();
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
                                return;
                            }
                            //logger.Trace("Matched Blocks regex");
                        }
                        //logger.Trace("Matched ResistsOrEvades regex");
                    }
                    var didHit = (Convert.ToString(mReg.Groups["didHit"].Value) == "hits");
                    var resist = !(String.IsNullOrWhiteSpace(mReg.Groups["resist"].Value)) &&
                                 mReg.Groups["resist"].Value != "evades";
                    var block = !(String.IsNullOrWhiteSpace(mReg.Groups["block"].Value)) &&
                                mReg.Groups["block"].Value != "evades";
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
                    StatGroup target;
                    if (Regex.IsMatch(player, @"^[Yy]our?$"))
                    {
                        target = ParseModInstance.Timeline.PlayerInParty
                                     ? ParseModInstance.Timeline.PartyStats.GetGroup(Settings.Default.CharacterName)
                                     : ParseModInstance.Timeline.PlayerStats;
                        if (target == null && ParseModInstance.Timeline.PlayerInParty)
                        {
                            target = CreatePlayerStatGroup(Settings.Default.CharacterName);
                            ParseModInstance.Timeline.PartyStats.AddGroup(target);
                        }
                    }
                    else
                    {
                        if (ParseModInstance.Timeline.PartyStats.HasGroup(player))
                        {
                            target = ParseModInstance.Timeline.PartyStats.GetGroup(player);
                        }
                        else
                        {
                            //logger.Warn("Adding party member {0} to active list because of damage event (no party join event received)", player);
                            if (ParseModInstance.Timeline.InactivePartyStats.HasGroup(player))
                            {
                                target = MoveToActive(player);
                            }
                            else
                            {
                                target = CreatePlayerStatGroup(player);
                                ParseModInstance.Timeline.PartyStats.AddGroup(target);
                            }
                        }
                    }
                    if (target == null)
                    {
                        return;
                    }
                    if (String.IsNullOrWhiteSpace(target.Name))
                    {
                        //logger.Error("Got blank target name : {0}", e.RawLine);
                        return;
                    }
                    ParseModInstance.Timeline.PublishTimelineEvent(TimelineEventType.MobFighting, mob);
                    if (didHit || resist)
                    {
                        target.Stats.GetStat("DT Total").Value += damage;
                        if (didCrit)
                        {
                            target.Stats.GetStat("DT Crit").Value += damage;
                            target.Stats.GetStat("DT C Hit").Value += 1;
                        }
                        else
                        {
                            target.Stats.GetStat("DT Reg").Value += damage;
                            target.Stats.GetStat("DT Hit").Value += 1;
                        }
                    }
                    if (resist)
                    {
                        didHit = true;
                    }
                    UpdateDTaken(target, damage, didHit, didCrit, ability, block);
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
                        return;
                    }
                    var whoDid = Convert.ToString(hReg.Groups["whoDid"].Value);
                    var ability = Convert.ToString(hReg.Groups["ability"].Value);
                    var castOn = Convert.ToString(hReg.Groups["castOn"].Value);
                    var recLoss = Convert.ToString(hReg.Groups["recLoss"].Value);
                    var amount = hReg.Groups["amount"].Success ? Convert.ToDecimal(hReg.Groups["amount"].Value) : 0m;
                    var type = Convert.ToString(hReg.Groups["type"].Value.ToUpper());
                    StatGroup target;
                    if (Regex.IsMatch(whoDid, @"^[Yy]our?$"))
                    {
                        target = ParseModInstance.Timeline.PlayerInParty
                                     ? ParseModInstance.Timeline.PartyStats.GetGroup(Settings.Default.CharacterName)
                                     : ParseModInstance.Timeline.PlayerStats;
                        if (target == null && ParseModInstance.Timeline.PlayerInParty)
                        {
                            target = CreatePlayerStatGroup(Settings.Default.CharacterName);
                            ParseModInstance.Timeline.PartyStats.AddGroup(target);
                        }
                    }
                    else
                    {
                        if (ParseModInstance.Timeline.PartyStats.HasGroup(whoDid))
                        {
                            target = ParseModInstance.Timeline.PartyStats.GetGroup(whoDid);
                        }
                        else
                        {
                            //logger.Warn("Adding party member {0} to active list because of healing event (no party join event received)", whoDid);
                            if (ParseModInstance.Timeline.InactivePartyStats.HasGroup(whoDid))
                            {
                                target = MoveToActive(whoDid);
                            }
                            else
                            {
                                target = CreatePlayerStatGroup(whoDid);
                                ParseModInstance.Timeline.PartyStats.AddGroup(target);
                            }
                        }
                    }
                    //logger.Debug("Handling event : whoDid: {0} ability: {1} castOn: {2} recLoss: {3} amount: {4} type: {5}", whoDid, ability, castOn, recLoss, amount, type);
                    if (target == null)
                    {
                        return;
                    }
                    if (type == "HP")
                    {
                        var healing = 0m;
                        healing = Convert.ToDecimal(hReg.Groups["amount"].Value);
                        target.Stats.GetStat("H Total").Value += healing;
                        UpdateHealing(target, healing, ability);
                        //if (Regex.IsMatch(whoDid, @"^[Yy]our?$"))
                        //{
                        //    whoDid = Settings.Default.CharacterName;
                        //}
                        //if (Regex.IsMatch(castOn, @"^[Yy]our?$"))
                        //{
                        //    castOn = Settings.Default.CharacterName;
                        //}
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
                    }
                    if (TotalH.ContainsKey(target.Name))
                    {
                        TotalH[target.Name] = target.GetStatValue("H Total").ToString();
                    }
                    else
                    {
                        TotalH[target.Name] = target.GetStatValue("H Total").ToString();
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
                            Logger.Trace("No match for Damage or Resist RegEx on line {0}", e.RawLine);
                            return;
                        }
                        //logger.Trace("Matched Resists regex");
                    }
                    var didHit = (Convert.ToString(mReg.Groups["didHit"].Value) == "et") ||
                                 (Convert.ToString(mReg.Groups["didHit"].Value) == "sur");
                    var resist = !(String.IsNullOrWhiteSpace(mReg.Groups["resist"].Value)) &&
                                 mReg.Groups["resist"].Value != "evades";
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
                    StatGroup target;
                    if (whoHit == Settings.Default.CharacterName)
                    {
                        target = ParseModInstance.Timeline.PlayerInParty
                                     ? ParseModInstance.Timeline.PartyStats.GetGroup(Settings.Default.CharacterName)
                                     : ParseModInstance.Timeline.PlayerStats;
                        if (target == null && ParseModInstance.Timeline.PlayerInParty)
                        {
                            target = CreatePlayerStatGroup(Settings.Default.CharacterName);
                            ParseModInstance.Timeline.PartyStats.AddGroup(target);
                        }
                    }
                    else
                    {
                        if (ParseModInstance.Timeline.PartyStats.HasGroup(whoHit))
                        {
                            target = ParseModInstance.Timeline.PartyStats.GetGroup(whoHit);
                        }
                        else
                        {
                            //logger.Warn("Adding party member {0} to active list because of damage event (no party join event received)", whoHit);
                            if (ParseModInstance.Timeline.InactivePartyStats.HasGroup(whoHit))
                            {
                                target = MoveToActive(whoHit);
                            }
                            else
                            {
                                target = CreatePlayerStatGroup(whoHit);
                                ParseModInstance.Timeline.PartyStats.AddGroup(target);
                            }
                        }
                    }
                    if (target == null)
                    {
                        return;
                    }
                    if (String.IsNullOrWhiteSpace(target.Name))
                    {
                        //logger.Error("Got blank target name : {0}", e.RawLine);
                        return;
                    }
                    ParseModInstance.Timeline.PublishTimelineEvent(TimelineEventType.MobFighting, mob);
                    if (didHit || resist)
                    {
                        target.Stats.GetStat("Total").Value += damage;
                        if (didCrit)
                        {
                            target.Stats.GetStat("Crit").Value += damage;
                            target.Stats.GetStat("C Hit").Value += 1;
                        }
                        else
                        {
                            target.Stats.GetStat("Reg").Value += damage;
                            target.Stats.GetStat("Hit").Value += 1;
                        }
                    }
                    else
                    {
                        target.Stats.GetStat("Miss").Value += 1;
                    }

                    if (resist)
                    {
                        ParseModInstance.Timeline.GetOrAddStatsForMob(whoEvaded).AddAbilityStats(ability, damage, resist,
                                                                                                 false, false);
                    }
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(mob))
                        {
                            ParseModInstance.Timeline.GetOrAddStatsForMob(mob).AddAbilityStats(ability, damage, false,
                                                                                               false, didCrit);
                        }
                    }
                    UpdateAbilities(target, damage, didHit, didCrit, ability, resist);
                    if (TotalD.ContainsKey(target.Name))
                    {
                        TotalD[target.Name] = target.GetStatValue("Total").ToString();
                    }
                    else
                    {
                        TotalD[target.Name] = target.GetStatValue("Total").ToString();
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
                            Logger.Trace("No match for Damage Taken or Resist Taken RegEx on line {0}", e.RawLine);
                            return;
                        }
                    }
                    var didHit = (Convert.ToString(mReg.Groups["didHit"].Value) == "et") ||
                                 (Convert.ToString(mReg.Groups["didHit"].Value) == "sur");
                    var resist = !(String.IsNullOrWhiteSpace(mReg.Groups["resist"].Value)) &&
                                 mReg.Groups["resist"].Value != "evades";
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
                    if (player == "")
                    {
                        player = whoEvaded;
                    }
                    StatGroup target;
                    if (player == Settings.Default.CharacterName)
                    {
                        target = ParseModInstance.Timeline.PlayerInParty
                                     ? ParseModInstance.Timeline.PartyStats.GetGroup(Settings.Default.CharacterName)
                                     : ParseModInstance.Timeline.PlayerStats;
                        if (target == null && ParseModInstance.Timeline.PlayerInParty)
                        {
                            target = CreatePlayerStatGroup(Settings.Default.CharacterName);
                            ParseModInstance.Timeline.PartyStats.AddGroup(target);
                        }
                    }
                    else
                    {
                        if (ParseModInstance.Timeline.PartyStats.HasGroup(player))
                        {
                            target = ParseModInstance.Timeline.PartyStats.GetGroup(player);
                        }
                        else
                        {
                            //logger.Warn("Adding party member {0} to active list because of damage event (no party join event received)", player);
                            if (ParseModInstance.Timeline.InactivePartyStats.HasGroup(player))
                            {
                                target = MoveToActive(player);
                            }
                            else
                            {
                                target = CreatePlayerStatGroup(player);
                                ParseModInstance.Timeline.PartyStats.AddGroup(target);
                            }
                        }
                    }
                    if (target == null)
                    {
                        return;
                    }
                    if (String.IsNullOrWhiteSpace(target.Name))
                    {
                        //logger.Error("Got blank target name : {0}", e.RawLine);
                        return;
                    }
                    ParseModInstance.Timeline.PublishTimelineEvent(TimelineEventType.MobFighting, mob);
                    if (didHit || resist)
                    {
                        target.Stats.GetStat("DT Total").Value += damage;
                        if (didCrit)
                        {
                            target.Stats.GetStat("DT Crit").Value += damage;
                            target.Stats.GetStat("DT C Hit").Value += 1;
                        }
                        else
                        {
                            target.Stats.GetStat("DT Reg").Value += damage;
                            target.Stats.GetStat("DT Hit").Value += 1;
                        }
                    }
                    if (resist)
                    {
                        didHit = true;
                    }
                    UpdateDTaken(target, damage, didHit, didCrit, ability, false);
                }

                #endregion

                #region " HEALING BY MEMBERS "

                if (e.Type == EventType.Heal)
                {
                    var hReg = RegExpsFr.UseOnParty.Match(e.RawLine);
                    if (!hReg.Success)
                    {
                        Logger.Trace("No match for Healing on line {0}", e.RawLine);
                        return;
                    }
                    var whoDid = Convert.ToString(hReg.Groups["whoDid"].Value);
                    var ability = Convert.ToString(hReg.Groups["ability"].Value);
                    var castOn = Convert.ToString(hReg.Groups["castOn"].Value);
                    var recLoss = Convert.ToString(hReg.Groups["recLoss"].Value);
                    var amount = hReg.Groups["amount"].Success ? Convert.ToDecimal(hReg.Groups["amount"].Value) : 0m;
                    var type = Convert.ToString(hReg.Groups["type"].Value.ToUpper());
                    StatGroup target;
                    if (whoDid == Settings.Default.CharacterName)
                    {
                        target = ParseModInstance.Timeline.PlayerInParty
                                     ? ParseModInstance.Timeline.PartyStats.GetGroup(Settings.Default.CharacterName)
                                     : ParseModInstance.Timeline.PlayerStats;
                        if (target == null && ParseModInstance.Timeline.PlayerInParty)
                        {
                            target = CreatePlayerStatGroup(Settings.Default.CharacterName);
                            ParseModInstance.Timeline.PartyStats.AddGroup(target);
                        }
                    }
                    else
                    {
                        if (ParseModInstance.Timeline.PartyStats.HasGroup(whoDid))
                        {
                            target = ParseModInstance.Timeline.PartyStats.GetGroup(whoDid);
                        }
                        else
                        {
                            //logger.Warn("Adding party member {0} to active list because of healing event (no party join event received)", whoDid);
                            if (ParseModInstance.Timeline.InactivePartyStats.HasGroup(whoDid))
                            {
                                target = MoveToActive(whoDid);
                            }
                            else
                            {
                                target = CreatePlayerStatGroup(whoDid);
                                ParseModInstance.Timeline.PartyStats.AddGroup(target);
                            }
                        }
                    }
                    //logger.Debug("Handling event : whoDid: {0} ability: {1} castOn: {2} recLoss: {3} amount: {4} type: {5}", whoDid, ability, castOn, recLoss, amount, type);
                    if (target == null)
                    {
                        return;
                    }
                    if (type == "PV")
                    {
                        var healing = 0m;
                        healing = Convert.ToDecimal(hReg.Groups["amount"].Value);
                        target.Stats.GetStat("H Total").Value += healing;
                        UpdateHealing(target, amount, ability);
                    }
                    if (TotalH.ContainsKey(target.Name))
                    {
                        TotalH[target.Name] = target.GetStatValue("H Total").ToString();
                    }
                    else
                    {
                        TotalH[target.Name] = target.GetStatValue("H Total").ToString();
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
        /// <param name="characterStats"></param>
        /// <param name="damageAmount"></param>
        /// <param name="didHit"></param>
        /// <param name="didCrit"></param>
        /// <param name="ability"></param>
        /// <param name="resisted"></param>
        private void UpdateAbilities(StatGroup characterStats, Decimal damageAmount, bool didHit, bool didCrit,
                                     string ability, bool resisted)
        {
            StatGroupAbilities abilities;
            ability = ParseMod.TitleCase(ability, true);
            if (characterStats.HasGroup("Abilities"))
            {
                abilities = (StatGroupAbilities) characterStats.GetGroup("Abilities");
            }
            else
            {
                abilities = new StatGroupAbilities(characterStats.Name, _partyDamage);
                characterStats.AddGroup(abilities);
            }
            if (abilities == null)
            {
                return;
            }
            var abilityGroup = abilities.AddOrGetAbility(ability);
            if (didHit)
            {
                abilityGroup.Stats.GetStat("Total").Value += damageAmount;

                if (didCrit)
                {
                    abilityGroup.Stats.GetStat("Crit").Value += damageAmount;
                    abilityGroup.Stats.GetStat("C Hit").Value += 1;
                }
                else
                {
                    abilityGroup.Stats.GetStat("Reg").Value += damageAmount;
                    abilityGroup.Stats.GetStat("Hit").Value += 1;
                }
            }
            else
            {
                abilityGroup.Stats.GetStat("Miss").Value += 1;
            }
            if (resisted)
            {
                abilityGroup.Stats.GetStat("Resist").Value += 1;
            }
        }

        #region " UPDATE METHODS "

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characterStats"></param>
        /// <param name="damageAmount"></param>
        /// <param name="didHit"></param>
        /// <param name="didCrit"></param>
        /// <param name="ability"></param>
        /// <param name="blocked"></param>
        private void UpdateDTaken(StatGroup characterStats, Decimal damageAmount, bool didHit, bool didCrit,
                                  string ability, bool blocked)
        {
            StatGroupDamage damage;
            ability = ParseMod.TitleCase(ability, true);
            if (characterStats.HasGroup("Damage"))
            {
                damage = (StatGroupDamage) characterStats.GetGroup("Damage");
            }
            else
            {
                damage = new StatGroupDamage(characterStats.Name, _partyTotalTaken);
                characterStats.AddGroup(damage);
            }
            if (damage == null)
            {
                return;
            }
            var abilityGroup = damage.AddOrGetAbility(ability);
            if (didHit)
            {
                abilityGroup.Stats.GetStat("Total").Value += damageAmount;
                if (didCrit)
                {
                    abilityGroup.Stats.GetStat("Crit").Value += damageAmount;
                    abilityGroup.Stats.GetStat("C Hit").Value += 1;
                }
                else
                {
                    abilityGroup.Stats.GetStat("Reg").Value += damageAmount;
                    abilityGroup.Stats.GetStat("Hit").Value += 1;
                }
            }
            if (blocked)
            {
                abilityGroup.Stats.GetStat("Block").Value += 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characterStats"></param>
        /// <param name="healingAmount"></param>
        /// <param name="ability"></param>
        private void UpdateHealing(StatGroup characterStats, Decimal healingAmount, string ability)
        {
            StatGroupHealing healing;
            ability = ParseMod.TitleCase(ability, true);
            if (characterStats.HasGroup("Healing"))
            {
                healing = (StatGroupHealing) characterStats.GetGroup("Healing");
            }
            else
            {
                healing = new StatGroupHealing(characterStats.Name, _partyHealing);
                characterStats.AddGroup(healing);
            }
            if (healing == null)
            {
                return;
            }
            var healingGroup = healing.AddOrGetHealing(ability);
            healingGroup.Stats.GetStat("Total").Value += healingAmount;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="insert"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool SubmitData(string insert, string message)
        {
            var url = string.Format("http://ffxiv-app.com/battles/insert/?insert={0}&q={1}", insert,
                                    HttpUtility.UrlEncode(message));
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
            var offsets = new Hashtable
                          {
                              {"phalanx", "gladiator"}, {"aegis boon", "gladiator"}, {"riot blade", "gladiator"},
                              {"war drum", "gladiator"}, {"tempered will", "gladiator"}, {"rage of halone", "gladiator"},
                              {"goring blade", "gladiator"}, {"cover", "paladin"}, {"divine veil", "paladin"},
                              {"hallowed ground", "paladin"}, {"holy succor", "paladin"}, {"spirits within", "paladin"},
                              {"pounce", "puglist"}, {"haymaker", "puglist"}, {"fists of earth", "puglist"},
                              {"fists of fire", "puglist"}, {"aura pulse", "puglist"}, {"taunt", "puglist"},
                              {"howling fist", "puglist"}, {"simian thrash", "puglist"}, {"shoulder tackle", "monk"},
                              {"spinning heal", "monk"}, {"fists of wind", "monk"}, {"dragon kick", "monk"},
                              {"hundred fists", "monk"}, {"fracture", "marauder"}, {"berserk", "marauder"},
                              {"rampage", "marauder"}, {"path of the storm", "marauder"}, {"enduring march", "marauder"},
                              {"whirlwind", "marauder"}, {"godsbane", "marauder"}, {"vengeance", "warrior"},
                              {"antagonize", "warrior"}, {"collusion", "warrior"}, {"mighty strikes", "warrior"},
                              {"steel cyclone", "warrior"}, {"life surge", "lancer"}, {"power surge", "lancer"},
                              {"full thrust", "lancer"}, {"dread spike", "lancer"}, {"doom spike", "lancer"},
                              {"chaos thrust", "lancer"}, {"jump", "dragoon"}, {"elusive jump", "dragoon"},
                              {"dragonfire dive", "dragoon"}, {"disembowel", "dragoon"}, {"ring of talons", "dragoon"},
                              {"light shot", "archer"}, {"raging strike", "archer"}, {"shadowbind", "archer"},
                              {"swiftsong", "archer"}, {"barrage", "archer"}, {"quick nock", "archer"},
                              {"bloodletter", "archer"}, {"wide volley", "archer"}, {"battle voice", "bard"},
                              {"rain of death", "bard"}, {"ballad of magi", "bard"}, {"paeon of war", "bard"},
                              {"minuet of rigor", "bard"}, {"cleric stance", "conjurer"}, {"blissful mind", "conjurer"},
                              {"stonera", "conjurer"}, {"cura", "conjurer"}, {"shroud of saints", "conjurer"},
                              {"aerora", "conjurer"}, {"curaga", "conjurer"}, {"repose", "conjurer"},
                              {"presence of mind", "white mage"}, {"benediction", "white mage"}, {"esuna", "white mage"},
                              {"regen", "white mage"}, {"holy", "white mage"}, {"parsimony", "thaumaturgy"},
                              {"blizzard", "thaumaturgy"}, {"thundara", "thaumaturgy"}, {"blizzara", "thaumaturgy"},
                              {"excruciate", "thaumaturgy"}, {"sleep", "thaumaturgy"}, {"thundaga", "thaumaturgy"},
                              {"firaga", "thaumaturgy"}, {"convert", "black mage"}, {"burst", "black mage"},
                              {"sleepga", "black mage"}, {"flare", "black mage"}, {"freeze", "black mage"}
                          };
            return offsets;
        }

        #endregion
    }
}