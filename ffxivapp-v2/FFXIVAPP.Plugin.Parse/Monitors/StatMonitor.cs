// FFXIVAPP.Plugin.Parse
// StatMonitor.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Linq;
using System.Text.RegularExpressions;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Plugin.Parse.Enums;
using FFXIVAPP.Plugin.Parse.Models;
using FFXIVAPP.Plugin.Parse.Models.Events;
using FFXIVAPP.Plugin.Parse.Models.LinkedStats;
using FFXIVAPP.Plugin.Parse.Models.Timelines;
using NLog;

#endregion

namespace FFXIVAPP.Plugin.Parse.Monitors
{
    public class StatMonitor : EventMonitor
    {
        internal static readonly TotalStat TotalDamage = new TotalStat("TotalDamage");
        internal static readonly TotalStat PartyDamage = new TotalStat("PartyDamage");
        internal static readonly TotalStat PartyCritDamage = new TotalStat("PartyCritDamage");
        internal static readonly TotalStat PartyHealing = new TotalStat("PartyHealing");
        internal static readonly TotalStat PartyTotalTaken = new TotalStat("PartyTotalTaken");
        internal static readonly TotalStat PartyTotalRegTaken = new TotalStat("PartyTotalRegTaken");
        internal static readonly TotalStat PartyTotalCritTaken = new TotalStat("PartyTotalCritTaken");

        private static readonly string[] removeCS = new[]
        {
            "Miser's Mistress", "Dodore's Minion"
        };

        private static readonly string[] addCS = new[]
        {
            "Uraeus"
        };

        private static readonly string[] cleanParts = new[]
        {
            "head", "eye", "skull", "left horn", "right horn", "left mandible", "right mandible", "maw", "left humerus", "right humerus", "right arm", "left arm", "left leg", "right leg", "femur", "left wart cluster", "right wart cluster", "shell", "tail", "ore cluster"
        };


        /// <summary>
        /// </summary>
        /// <param name="parseControl"> </param>
        public StatMonitor(ParseControl parseControl) : base("StatMonitor", parseControl)
        {
            IncludeSelf = false;
            Filter = ((UInt16) EventDirection.By | (UInt16) EventSubject.You | (UInt16) EventSubject.Party | (UInt16) EventType.Attack | (UInt16) EventType.Heal | (UInt16) EventDirection.On);
        }

        /// <summary>
        /// </summary>
        protected override void InitStats()
        {
            ParseControl.Timeline.Overall.Stats.Add(TotalDamage);
            ParseControl.Timeline.Overall.Stats.Add(PartyDamage);
            ParseControl.Timeline.Overall.Stats.Add(PartyCritDamage);
            ParseControl.Timeline.Overall.Stats.Add(PartyHealing);
            ParseControl.Timeline.Overall.Stats.Add(PartyTotalTaken);
            ParseControl.Timeline.Overall.Stats.Add(PartyTotalRegTaken);
            ParseControl.Timeline.Overall.Stats.Add(PartyTotalCritTaken);
            base.InitStats();
        }

        /// <summary>
        /// </summary>
        public override void Clear()
        {
            Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("ClearEvent : Clearing ${0} Party Member Totals.", Count));
            TotalDamage.Reset();
            PartyDamage.Reset();
            PartyCritDamage.Reset();
            PartyHealing.Reset();
            PartyTotalTaken.Reset();
            PartyTotalRegTaken.Reset();
            PartyTotalCritTaken.Reset();
            ParseControl.TotalA.Clear();
            ParseControl.TotalD.Clear();
            ParseControl.TotalH.Clear();
            base.Clear();
            Timeline.Clear();
        }

        /// <summary>
        /// </summary>
        /// <param name="e"> </param>
        protected override void HandleEvent(Event e)
        {
            #region Clean Mob Names

            foreach (var s in removeCS.Where(s => e.RawLine.Contains(s)))
            {
                e.RawLine = Regex.Replace(e.RawLine, s, s.Replace("'s", ""));
            }

            #endregion

            Utilities.Filter.Process(e.RawLine, e);
        }

        /// <summary>
        /// </summary>
        /// <param name="e"> </param>
        protected override void HandleUnknownEvent(Event e)
        {
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
