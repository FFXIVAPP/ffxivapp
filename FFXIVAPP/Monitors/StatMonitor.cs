// FFXIVAPP
// StatMonitor.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Linq;
using System.Text.RegularExpressions;
using FFXIVAPP.Classes;
using FFXIVAPP.Data;
using FFXIVAPP.Models;
using FFXIVAPP.Stats;
using NLog;

namespace FFXIVAPP.Monitors
{
    public class StatMonitor : EventMonitor
    {
        internal static readonly TotalStat TotalDamage = new TotalStat("TotalDamage");
        internal static readonly TotalStat PartyDamage = new TotalStat("PartyDamage");
        internal static readonly TotalStat PartyCritDamage = new TotalStat("PartyCritDamage");
        internal static readonly TotalStat PartyHealing = new TotalStat("PartyHealing");
        internal static readonly TotalStat PartyTotalTaken = new TotalStat("PartyTotalTaken");
        internal static readonly TotalStat PartyTotalRTaken = new TotalStat("PartyTotalRTaken");
        internal static readonly TotalStat PartyTotalCTaken = new TotalStat("PartyTotalCTaken");
        private static readonly string[] removeCS = new[] {"Miser's Mistress", "Dodore's Minion"};
        private static readonly string[] cleanParts = new[] {"head", "eye", "skull", "left horn", "right horn", "left mandible", "right mandible", "maw", "left humerus", "right humerus", "right arm", "left arm", "left leg", "right leg", "femur", "left wart cluster", "right wart cluster", "shell", "tail", "ore cluster"};
        private static readonly string[] addCS = new[] {"Uraeus"};
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// </summary>
        /// <param name="ffxivInstance"> </param>
        public StatMonitor(FFXIV ffxivInstance) : base("StatMonitor", ffxivInstance)
        {
            IncludeSelf = false;
            Filter = ((UInt16) EventDirection.By | (UInt16) EventSubject.You | (UInt16) EventSubject.Party | (UInt16) EventType.Attack | (UInt16) EventType.Heal | (UInt16) EventDirection.On);
        }

        /// <summary>
        /// </summary>
        protected override void InitStats()
        {
            FFXIVInstance.Timeline.Overall.Stats.Add(TotalDamage);
            FFXIVInstance.Timeline.Overall.Stats.Add(PartyDamage);
            FFXIVInstance.Timeline.Overall.Stats.Add(PartyCritDamage);
            FFXIVInstance.Timeline.Overall.Stats.Add(PartyHealing);
            FFXIVInstance.Timeline.Overall.Stats.Add(PartyTotalTaken);
            FFXIVInstance.Timeline.Overall.Stats.Add(PartyTotalRTaken);
            FFXIVInstance.Timeline.Overall.Stats.Add(PartyTotalCTaken);
            base.InitStats();
        }

        /// <summary>
        /// </summary>
        public override void Clear()
        {
            Logger.Debug("ClearEvent : Clearing ${0} Party Member Totals.", Count);
            TotalDamage.Reset();
            PartyDamage.Reset();
            PartyCritDamage.Reset();
            PartyHealing.Reset();
            PartyTotalTaken.Reset();
            PartyTotalRTaken.Reset();
            PartyTotalCTaken.Reset();
            FFXIVInstance.TotalA.Clear();
            FFXIVInstance.TotalD.Clear();
            FFXIVInstance.TotalH.Clear();
            base.Clear();
            Timeline.Clear();
        }

        /// <summary>
        /// </summary>
        /// <param name="e"> </param>
        protected override void HandleEvent(Event e)
        {
            var mCode = "00" + e.Code.ToString("X");
            var mTimeStamp = DateTime.Now.ToString("[HH:mm:ss] ");

            #region Clean Mob Names

            var r = removeCS.Select(t => e.RawLine.Contains(t)).ToString();
            e.RawLine = Regex.Replace(e.RawLine, r, r.Replace("'s", "") + "'s");

            #endregion

            Process.Parse(mCode, mTimeStamp, e.RawLine, e);
        }

        /// <summary>
        /// </summary>
        /// <param name="originalText"> </param>
        /// <returns> </returns>
        private static string CleanAccent(string originalText)
        {
            var strTemp = originalText;
            var regA = new Regex("[ã|à|â|ä|á|å]");
            var regAa = new Regex("[Ã|À|Â|Ä|Á|Å]");
            var regE = new Regex("[é|è|ê|ë]");
            var regEe = new Regex("[É|È|Ê|Ë]");
            var regI = new Regex("[í|ì|î|ï]");
            var regIi = new Regex("[Í|Ì|Î|Ï]");
            var regO = new Regex("[õ|ò|ó|ô|ö]");
            var regOo = new Regex("[Õ|Ó|Ò|Ô|Ö]");
            var regU = new Regex("[ù|ú|û|ü|µ]");
            var regUu = new Regex("[Ü|Ú|Ù|Û]");
            var regY = new Regex("[ý|ÿ]");
            var regYy = new Regex("[Ý]");
            var regAe = new Regex("[æ]");
            var regAeae = new Regex("[Æ]");
            var regOe = new Regex("[œ]");
            var regOeoe = new Regex("[Œ]");
            var regC = new Regex("[ç]");
            var regCc = new Regex("[Ç]");
            var regDd = new Regex("[Ð]");
            var regN = new Regex("[ñ]");
            var regNn = new Regex("[Ñ]");
            var regS = new Regex("[š]");
            var regSs = new Regex("[Š]");
            strTemp = regA.Replace(strTemp, "a");
            strTemp = regAa.Replace(strTemp, "A");
            strTemp = regE.Replace(strTemp, "e");
            strTemp = regEe.Replace(strTemp, "E");
            strTemp = regI.Replace(strTemp, "i");
            strTemp = regIi.Replace(strTemp, "I");
            strTemp = regO.Replace(strTemp, "o");
            strTemp = regOo.Replace(strTemp, "O");
            strTemp = regU.Replace(strTemp, "u");
            strTemp = regUu.Replace(strTemp, "U");
            strTemp = regY.Replace(strTemp, "y");
            strTemp = regYy.Replace(strTemp, "Y");
            strTemp = regAe.Replace(strTemp, "ae");
            strTemp = regAeae.Replace(strTemp, "AE");
            strTemp = regOe.Replace(strTemp, "oe");
            strTemp = regOeoe.Replace(strTemp, "OE");
            strTemp = regC.Replace(strTemp, "c");
            strTemp = regCc.Replace(strTemp, "C");
            strTemp = regDd.Replace(strTemp, "D");
            strTemp = regN.Replace(strTemp, "n");
            strTemp = regNn.Replace(strTemp, "N");
            strTemp = regS.Replace(strTemp, "s");
            strTemp = regSs.Replace(strTemp, "S");
            return strTemp;
        }
    }
}