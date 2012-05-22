// ParseModXIV
// StatMonitor.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Text.RegularExpressions;
using AppModXIV.Classes;
using NLog;
using ParseModXIV.Classes;
using ParseModXIV.Data;
using ParseModXIV.Model;
using ParseModXIV.Stats;

namespace ParseModXIV.Monitors
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
        static readonly string[] Mobbies = new[] { "Miser's Mistress", "Dodore's Minion" };
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parseModInstance"></param>
        public StatMonitor(ParseMod parseModInstance) : base("StatMonitor", parseModInstance)
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
            Logger.Debug("ClearEvent : Clearing ${0} party members totals.", Count);
            TotalDamage.Reset();
            PartyDamage.Reset();
            PartyCritDamage.Reset();
            PartyHealing.Reset();
            PartyTotalTaken.Reset();
            PartyTotalRTaken.Reset();
            PartyTotalCTaken.Reset();
            ParseModInstance.TotalA.Clear();
            ParseModInstance.TotalD.Clear();
            ParseModInstance.TotalH.Clear();
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
            foreach (var tmp in Mobbies)
            {
                e.RawLine = Regex.Replace(e.RawLine, tmp, tmp.Replace("'s", ""));
            }
            var cleaned = XmlCleaner.SanitizeXmlString(e.RawLine);
            switch (Settings.Default.Language)
            {
                case "English":
                    English.Parse(mCode, mTimeStamp, cleaned, e);
                    break;
                case "French":
                    French.Parse(mCode, mTimeStamp, cleaned, e);
                    break;
                case "Japanese":
                    Japanese.Parse(mCode, mTimeStamp, cleaned, e);
                    break;
                case "German":
                    break;
            }

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
        /// <param name="originalText"></param>
        /// <returns></returns>
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