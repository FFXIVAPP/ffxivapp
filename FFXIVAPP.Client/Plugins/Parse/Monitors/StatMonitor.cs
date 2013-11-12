// FFXIVAPP.Client
// StatMonitor.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Linq;
using System.Text.RegularExpressions;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Plugins.Parse.Enums;
using FFXIVAPP.Client.Plugins.Parse.Models;
using FFXIVAPP.Client.Plugins.Parse.Models.Events;
using FFXIVAPP.Client.Plugins.Parse.Models.LinkedStats;
using FFXIVAPP.Client.Plugins.Parse.Models.Timelines;
using FFXIVAPP.Client.Plugins.Parse.ViewModels;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Utilities;
using NLog;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Plugins.Parse.Monitors
{
    [DoNotObfuscate]
    public class StatMonitor : EventMonitor
    {
        //player totals
        internal static readonly TotalStat TotalOverallDamage = new TotalStat("TotalOverallDamage");
        internal static readonly TotalStat TotalOverallHealing = new TotalStat("TotalOverallHealing");
        internal static readonly TotalStat TotalOverallDamageTaken = new TotalStat("TotalOverallDamageTaken");
        internal static readonly TotalStat TotalOverallTP = new TotalStat("TotalOverallTP");
        internal static readonly TotalStat TotalOverallMP = new TotalStat("TotalOverallMP");
        internal static readonly TotalStat RegularDamage = new TotalStat("RegularDamage");
        internal static readonly TotalStat CriticalDamage = new TotalStat("CriticalDamage");
        internal static readonly TotalStat RegularHealing = new TotalStat("RegularHealing");
        internal static readonly TotalStat CriticalHealing = new TotalStat("CriticalHealing");
        internal static readonly TotalStat RegularDamageTaken = new TotalStat("RegularDamageTaken");
        internal static readonly TotalStat CriticalDamageTaken = new TotalStat("CriticalDamageTaken");
        //monster totals
        internal static readonly TotalStat TotalOverallDamageMonster = new TotalStat("TotalOverallDamageMonster");
        internal static readonly TotalStat TotalOverallHealingMonster = new TotalStat("TotalOverallHealingMonster");
        internal static readonly TotalStat TotalOverallDamageTakenMonster = new TotalStat("TotalOverallDamageTakenMonster");
        internal static readonly TotalStat TotalOverallTPMonster = new TotalStat("TotalOverallTPMonster");
        internal static readonly TotalStat TotalOverallMPMonster = new TotalStat("TotalOverallMPMonster");
        internal static readonly TotalStat RegularDamageMonster = new TotalStat("RegularDamageMonster");
        internal static readonly TotalStat CriticalDamageMonster = new TotalStat("CriticalDamageMonster");
        internal static readonly TotalStat RegularHealingMonster = new TotalStat("RegularHealingMonster");
        internal static readonly TotalStat CriticalHealingMonster = new TotalStat("CriticalHealingMonster");
        internal static readonly TotalStat RegularDamageTakenMonster = new TotalStat("RegularDamageTakenMonster");
        internal static readonly TotalStat CriticalDamageTakenMonster = new TotalStat("CriticalDamageTakenMonster");

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
            Filter = (EventParser.TypeMask | (UInt32) EventSubject.You | (UInt32) EventSubject.Party | (UInt32) EventSubject.Engaged | (UInt32) EventSubject.UnEngaged | (UInt32) EventDirection.Self | (UInt32) EventDirection.You | (UInt32) EventDirection.Party | (UInt32) EventDirection.Engaged | (UInt32) EventDirection.UnEngaged);
        }

        /// <summary>
        /// </summary>
        protected override void InitStats()
        {
            ParseControl.Timeline.Overall.Stats.Add(TotalOverallDamage);
            ParseControl.Timeline.Overall.Stats.Add(TotalOverallHealing);
            ParseControl.Timeline.Overall.Stats.Add(TotalOverallDamageTaken);
            ParseControl.Timeline.Overall.Stats.Add(TotalOverallTP);
            ParseControl.Timeline.Overall.Stats.Add(TotalOverallMP);
            ParseControl.Timeline.Overall.Stats.Add(RegularDamage);
            ParseControl.Timeline.Overall.Stats.Add(CriticalDamage);
            ParseControl.Timeline.Overall.Stats.Add(RegularHealing);
            ParseControl.Timeline.Overall.Stats.Add(CriticalHealing);
            ParseControl.Timeline.Overall.Stats.Add(RegularDamageTaken);
            ParseControl.Timeline.Overall.Stats.Add(CriticalDamageTaken);
            ParseControl.Timeline.Overall.Stats.Add(TotalOverallDamageMonster);
            ParseControl.Timeline.Overall.Stats.Add(TotalOverallHealingMonster);
            ParseControl.Timeline.Overall.Stats.Add(TotalOverallDamageTakenMonster);
            ParseControl.Timeline.Overall.Stats.Add(TotalOverallTPMonster);
            ParseControl.Timeline.Overall.Stats.Add(TotalOverallMPMonster);
            ParseControl.Timeline.Overall.Stats.Add(RegularDamageMonster);
            ParseControl.Timeline.Overall.Stats.Add(CriticalDamageMonster);
            ParseControl.Timeline.Overall.Stats.Add(RegularHealingMonster);
            ParseControl.Timeline.Overall.Stats.Add(CriticalHealingMonster);
            ParseControl.Timeline.Overall.Stats.Add(RegularDamageTakenMonster);
            ParseControl.Timeline.Overall.Stats.Add(CriticalDamageTakenMonster);
            base.InitStats();
        }

        /// <summary>
        /// </summary>
        public override void Clear()
        {
            Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("ClearEvent : Clearing ${0} Party Member Totals.", Count));
            foreach (var player in ParseControl.Instance.Timeline.Party)
            {
                var playerInstance = ParseControl.Instance.Timeline.GetSetPlayer(player.Name);
                playerInstance.StatusUpdateTimer.Stop();
                playerInstance.LastDamageAmountByAction.Clear();
                playerInstance.StatusEntriesSelf.Clear();
                playerInstance.StatusEntriesMonster.Clear();
            }
            foreach (var monster in ParseControl.Instance.Timeline.Monster)
            {
                var monsterInstance = ParseControl.Instance.Timeline.GetSetMob(monster.Name);
                //monsterInstance.StatusUpdateTimer.Stop();
                monsterInstance.LastDamageAmountByAction.Clear();
                //monsterInstance.StatusEntriesSelf.Clear();
                //monsterInstance.StatusEntriesMonster.Clear();
            }
            var hasDamage = ParseControl.Instance.Timeline.Overall.Stats.GetStatValue("TotalOverallDamage") > 0;
            if (hasDamage)
            {
                var historyItem = new ParseHistoryItem();
                foreach (var stat in ParseControl.Instance.Timeline.Overall.Stats)
                {
                    historyItem.HistoryControl.Timeline.Overall.Stats.Add(stat);
                }
                foreach (var player in ParseControl.Instance.Timeline.Party)
                {
                    historyItem.HistoryControl.Timeline.Party.Add(player);
                }
                foreach (var monster in ParseControl.Instance.Timeline.Monster)
                {
                    historyItem.HistoryControl.Timeline.Monster.Add(monster);
                }
                historyItem.Start = ParseControl.Instance.Timeline.ParseStarted;
                historyItem.End = DateTime.Now;
                historyItem.ParseLength = historyItem.End - historyItem.Start;
                var parseTimeDetails = String.Format("{0} -> {1} [{2}]", historyItem.Start, historyItem.End, historyItem.ParseLength);
                var zone = ZoneHelper.GetMapInfo(ParseControl.Instance.Timeline.ZoneID)
                                     .English;
                //switch (Settings.Default.GameLanguage)
                //{
                //    case "French":
                //        zone = ZoneHelper.GetMapInfo(ParseControl.Instance.Timeline.ZoneID)
                //                     .French;
                //        break;
                //    case "German":
                //        zone = ZoneHelper.GetMapInfo(ParseControl.Instance.Timeline.ZoneID)
                //                     .German;
                //        break;
                //    case "Japanese":
                //        zone = ZoneHelper.GetMapInfo(ParseControl.Instance.Timeline.ZoneID)
                //                     .Japanese;
                //        break;
                //}
                historyItem.Name = String.Format("{0} {1}", zone, parseTimeDetails);
                DispatcherHelper.Invoke(() => HistoryViewModel.Instance.ParseHistory.Add(historyItem));
            }
            TotalOverallDamage.Reset();
            TotalOverallHealing.Reset();
            TotalOverallDamageTaken.Reset();
            TotalOverallTP.Reset();
            TotalOverallMP.Reset();
            RegularDamage.Reset();
            CriticalDamage.Reset();
            RegularHealing.Reset();
            CriticalHealing.Reset();
            RegularDamageTaken.Reset();
            CriticalDamageTaken.Reset();
            TotalOverallDamageMonster.Reset();
            TotalOverallHealingMonster.Reset();
            TotalOverallDamageTakenMonster.Reset();
            TotalOverallTPMonster.Reset();
            TotalOverallMPMonster.Reset();
            RegularDamageMonster.Reset();
            CriticalDamageMonster.Reset();
            RegularHealingMonster.Reset();
            CriticalHealingMonster.Reset();
            RegularDamageTakenMonster.Reset();
            CriticalDamageTakenMonster.Reset();
            ParseControl.TotalA.Clear();
            ParseControl.TotalD.Clear();
            ParseControl.TotalH.Clear();
            base.Clear();
            Timeline.Clear();
        }

        /// <summary>
        /// </summary>
        /// <param name="e"> </param>
        protected override void HandleEvent(Models.Events.Event e)
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
        protected override void HandleUnknownEvent(Models.Events.Event e)
        {
            var line = e.RawLine;
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
