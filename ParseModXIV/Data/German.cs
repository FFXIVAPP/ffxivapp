// ParseModXIV
// French.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Collections;
using System.Text.RegularExpressions;
using NLog;
using ParseModXIV.Classes;
using ParseModXIV.Model;
using ParseModXIV.Monitors;

namespace ParseModXIV.Data
{
    internal static class German
    {
        private static readonly Hashtable Offsets = GetJob();
        private static string _lastAttacker = "";
        private static string _lastAttacked = "";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mCode"></param>
        /// <param name="mTimeStamp"></param>
        /// <param name="cleaned"></param>
        /// <param name="e"></param>
        public static void Parse(string mCode, string mTimeStamp, string cleaned, Event e)
        {
            #region " DAMAGE TO MOBS "

            if (e.Type == EventType.Attack && e.Direction == EventDirection.By)
            {
                var mReg = RegExpsDe.Damage.Match(cleaned);
                if (!mReg.Success)
                {
                    Logger.Warn("MatchEvent : No match for Damage on line {0}", cleaned);
                    mReg = RegExpsDe.Resists.Match(cleaned);
                    if (!mReg.Success)
                    {
                        Logger.Warn("MatchEvent : No match for Resists or Evades on line {0}", cleaned);
                        mReg = RegExpsDe.Additional.Match(cleaned);
                        if (!mReg.Success)
                        {
                            Logger.Warn("MatchEvent : No match for Additional on line {0}", cleaned);
                            //ChatWorkerDelegate.XmlWriteLog.AddChatLine(new string[] { cleaned, mCode, "#FFFFFF", mTimeStamp });
                            ChatWorkerDelegate.XmlWriteUnmatchedLog.AddChatLine(new[] { cleaned, mCode, "#FFFFFF", mTimeStamp });
                            return;
                        }
                        if (!String.IsNullOrWhiteSpace(_lastAttacked) && !String.IsNullOrWhiteSpace(_lastAttacker))
                        {
                            var added = mReg.Groups["amount"].Success ? Convert.ToDecimal(mReg.Groups["amount"].Value) : 0m;
                            ParseMod.Instance.Timeline.GetOrAddStatsForParty(_lastAttacker, StatMonitor.PartyDamage, StatMonitor.PartyHealing, StatMonitor.PartyTotalTaken).AddAbilityStats(_lastAttacked, "Effet Supplémentaire", added, true, false, false);
                        }
                        return;
                    }
                }
                var didHit = (Convert.ToString(mReg.Groups["didHit"].Value) == "et") || (Convert.ToString(mReg.Groups["didHit"].Value) == "sur");
                var resist = !(String.IsNullOrWhiteSpace(mReg.Groups["resist"].Value));
                var direction = ParseMod.TitleCase(Convert.ToString(mReg.Groups["direction"].Value), true);
                var whoHit = Convert.ToString(mReg.Groups["whoHit"].Value);
                var ability = Convert.ToString(mReg.Groups["ability"].Value);
                var mob = ParseMod.TitleCase((Convert.ToString(mReg.Groups["mob"].Value)), true);
                var whoEvaded = ParseMod.TitleCase(Convert.ToString(mReg.Groups["whoEvaded"]), true);
                var damage = mReg.Groups["amount"].Success ? Convert.ToDecimal(mReg.Groups["amount"].Value) : 0m;
                var didCrit = !(String.IsNullOrWhiteSpace(Convert.ToString(mReg.Groups["crit"].Value)));
                mob = (String.IsNullOrWhiteSpace(mob)) ? whoEvaded : mob;
                if (String.IsNullOrWhiteSpace(whoHit) || String.IsNullOrWhiteSpace(mob))
                {
                    return;
                }
                Logger.Trace("HandlingEvent : Who: {0} Monster: {1} Ability: {2} Damage: {3} Hit: {4} Crit: {5} Resist: {6}", whoHit, mob, ability, damage, didHit, didCrit, resist);
                ParseMod.Instance.Timeline.PublishTimelineEvent(TimelineEventType.MobFighting, mob);
                if (resist)
                {
                    ParseMod.Instance.Timeline.GetOrAddStatsForMob(whoEvaded).AddAbilityStats(ability, damage, true, false, false);
                    didHit = true;
                }
                else
                {
                    ParseMod.Instance.Timeline.GetOrAddStatsForMob(mob).AddAbilityStats(ability, damage, false, false, didCrit);
                }
                ParseMod.Instance.Timeline.GetOrAddStatsForParty(whoHit, StatMonitor.PartyDamage, StatMonitor.PartyHealing, StatMonitor.PartyTotalTaken).AddAbilityStats(mob, ability, damage, didHit, didCrit, resist);
                _lastAttacker = whoHit;
                _lastAttacked = mob;
                if (ParseMod.Instance.TotalA.ContainsKey(whoHit))
                {
                    ParseMod.Instance.TotalA[whoHit] = ParseMod.Instance.Timeline.Party.GetGroup(whoHit).GetStatValue("Total").ToString();
                }
                else
                {
                    ParseMod.Instance.TotalA[whoHit] = ParseMod.Instance.Timeline.Party.GetGroup(whoHit).GetStatValue("Total").ToString();
                }
            }

            #endregion

            #region " DAMAGE BY MOBS "

            if (e.Type == EventType.Attack && e.Direction == EventDirection.On)
            {
                var mReg = RegExpsDe.DamageToPlayer.Match(cleaned);
                if (!mReg.Success)
                {
                    Logger.Warn("MatchEvent : No match for Damage Taken on line {0}", cleaned);
                    mReg = RegExpsDe.Resists.Match(cleaned);
                    if (!mReg.Success)
                    {
                        Logger.Warn("MatchEvent : No match for Resists or Evades on line {0}", cleaned);
                        mReg = RegExpsDe.Blocks.Match(cleaned);
                        if (!mReg.Success)
                        {
                            Logger.Warn("MatchEvent : No match for Blocks on line {0}", cleaned);
                            //ChatWorkerDelegate.XmlWriteLog.AddChatLine(new string[] { cleaned, mCode, "#FFFFFF", mTimeStamp });
                            ChatWorkerDelegate.XmlWriteUnmatchedLog.AddChatLine(new[] { cleaned, mCode, "#FFFFFF", mTimeStamp });
                            return;
                        }
                    }
                }
                var didHit = (Convert.ToString(mReg.Groups["didHit"].Value) == "et") || (Convert.ToString(mReg.Groups["didHit"].Value) == "sur");
                var resist = !(String.IsNullOrWhiteSpace(mReg.Groups["resist"].Value)) && mReg.Groups["resist"].Value != "evades";
                var block = !(String.IsNullOrWhiteSpace(mReg.Groups["block"].Value));
                var mob = ParseMod.TitleCase(Convert.ToString(mReg.Groups["whoHit"].Value), true);
                var whoEvaded = ParseMod.TitleCase(Convert.ToString(mReg.Groups["whoEvaded"]), true);
                var ability = Convert.ToString(mReg.Groups["ability"].Value);
                var player = Convert.ToString(mReg.Groups["player"].Value);
                var direction = ParseMod.TitleCase(Convert.ToString(mReg.Groups["direction"].Value), true);
                var damage = mReg.Groups["amount"].Success ? Convert.ToDecimal(mReg.Groups["amount"].Value) : 0m;
                var didCrit = !(String.IsNullOrWhiteSpace(Convert.ToString(mReg.Groups["crit"].Value)));
                if (String.IsNullOrWhiteSpace(player))
                {
                    return;
                }
                Logger.Trace("HandlingEvent : Who: {0} Monster: {1} Ability: {2} Damage: {3} Hit: {4} Crit: {5} Resist: {6}", player, mob, ability, damage, didHit, didCrit, resist);
                ParseMod.Instance.Timeline.PublishTimelineEvent(TimelineEventType.MobFighting, mob);
                ParseMod.Instance.Timeline.GetOrAddStatsForParty(player, StatMonitor.PartyDamage, StatMonitor.PartyHealing, StatMonitor.PartyTotalTaken).AddDamageStats(mob, ability, damage, didHit, didCrit, block);
                if (ParseMod.Instance.TotalD.ContainsKey(player))
                {
                    ParseMod.Instance.TotalD[player] = ParseMod.Instance.Timeline.Party.GetGroup(player).GetStatValue("DT Total").ToString();
                }
                else
                {
                    ParseMod.Instance.TotalD[player] = ParseMod.Instance.Timeline.Party.GetGroup(player).GetStatValue("DT Total").ToString();
                }
            }

            #endregion

            #region " HEALING BY MEMBERS "

            if (e.Type == EventType.Heal)
            {
                var hReg = RegExpsDe.UseOnParty.Match(cleaned);
                if (!hReg.Success)
                {
                    Logger.Warn("MatchEvent : No match for Healing on line {0}", cleaned);
                    ChatWorkerDelegate.XmlWriteUnmatchedLog.AddChatLine(new[] { cleaned, mCode, "#FFFFFF", mTimeStamp });
                    return;
                }
                var whoDid = Convert.ToString(hReg.Groups["whoDid"].Value);
                var ability = Convert.ToString(hReg.Groups["ability"].Value);
                var castOn = Convert.ToString(hReg.Groups["castOn"].Value);
                var recLoss = Convert.ToString(hReg.Groups["recLoss"].Value);
                var amount = hReg.Groups["amount"].Success ? Convert.ToDecimal(hReg.Groups["amount"].Value) : 0m;
                var type = Convert.ToString(hReg.Groups["type"].Value.ToUpper());
                if (String.IsNullOrWhiteSpace(whoDid))
                {
                    return;
                }
                Logger.Trace("HandlingEvent : Caster: {0} Ability: {1} Target: {2} Rec/Loss: {3} Amount: {4} Type: {5}", whoDid, ability, castOn, recLoss, amount, type);
                if (type == "PV")
                {
                    ParseMod.Instance.Timeline.GetOrAddStatsForParty(whoDid, StatMonitor.PartyDamage, StatMonitor.PartyHealing, StatMonitor.PartyTotalTaken).AddHealingStats(ability, castOn, amount);

                    if (ParseMod.Instance.TotalH.ContainsKey(whoDid))
                    {
                        ParseMod.Instance.TotalH[whoDid] = ParseMod.Instance.Timeline.Party.GetGroup(whoDid).GetStatValue("H Total").ToString();
                    }
                    else
                    {
                        ParseMod.Instance.TotalH[whoDid] = ParseMod.Instance.Timeline.Party.GetGroup(whoDid).GetStatValue("H Total").ToString();
                    }
                }
            }

            #endregion
        }

        #region " JOB INFO "

        private static Hashtable GetJob()
        {
            var offsets = new Hashtable { { "phalanx", "gladiator" }, { "aegis boon", "gladiator" }, { "riot blade", "gladiator" }, { "war drum", "gladiator" }, { "tempered will", "gladiator" }, { "rage of halone", "gladiator" }, { "goring blade", "gladiator" }, { "cover", "paladin" }, { "divine veil", "paladin" }, { "hallowed ground", "paladin" }, { "holy succor", "paladin" }, { "spirits within", "paladin" }, { "pounce", "puglist" }, { "haymaker", "puglist" }, { "fists of earth", "puglist" }, { "fists of fire", "puglist" }, { "aura pulse", "puglist" }, { "taunt", "puglist" }, { "howling fist", "puglist" }, { "simian thrash", "puglist" }, { "shoulder tackle", "monk" }, { "spinning heal", "monk" }, { "fists of wind", "monk" }, { "dragon kick", "monk" }, { "hundred fists", "monk" }, { "fracture", "marauder" }, { "berserk", "marauder" }, { "rampage", "marauder" }, { "path of the storm", "marauder" }, { "enduring march", "marauder" }, { "whirlwind", "marauder" }, { "godsbane", "marauder" }, { "vengeance", "warrior" }, { "antagonize", "warrior" }, { "collusion", "warrior" }, { "mighty strikes", "warrior" }, { "steel cyclone", "warrior" }, { "life surge", "lancer" }, { "power surge", "lancer" }, { "full thrust", "lancer" }, { "dread spike", "lancer" }, { "doom spike", "lancer" }, { "chaos thrust", "lancer" }, { "jump", "dragoon" }, { "elusive jump", "dragoon" }, { "dragonfire dive", "dragoon" }, { "disembowel", "dragoon" }, { "ring of talons", "dragoon" }, { "light shot", "archer" }, { "raging strike", "archer" }, { "shadowbind", "archer" }, { "swiftsong", "archer" }, { "barrage", "archer" }, { "quick nock", "archer" }, { "bloodletter", "archer" }, { "wide volley", "archer" }, { "battle voice", "bard" }, { "rain of death", "bard" }, { "ballad of magi", "bard" }, { "paeon of war", "bard" }, { "minuet of rigor", "bard" }, { "cleric stance", "conjurer" }, { "blissful mind", "conjurer" }, { "stonera", "conjurer" }, { "cura", "conjurer" }, { "shroud of saints", "conjurer" }, { "aerora", "conjurer" }, { "curaga", "conjurer" }, { "repose", "conjurer" }, { "presence of mind", "white mage" }, { "benediction", "white mage" }, { "esuna", "white mage" }, { "regen", "white mage" }, { "holy", "white mage" }, { "parsimony", "thaumaturgy" }, { "blizzard", "thaumaturgy" }, { "thundara", "thaumaturgy" }, { "blizzara", "thaumaturgy" }, { "excruciate", "thaumaturgy" }, { "sleep", "thaumaturgy" }, { "thundaga", "thaumaturgy" }, { "firaga", "thaumaturgy" }, { "convert", "black mage" }, { "burst", "black mage" }, { "sleepga", "black mage" }, { "flare", "black mage" }, { "freeze", "black mage" } };
            return offsets;
        }

        #endregion
    }
}