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
        internal static readonly TotalStat TotalDamage = new TotalStat("Overall Damage");
        internal static readonly TotalStat PartyDamage = new TotalStat("Reg Damage");
        internal static readonly TotalStat PartyCritDamage = new TotalStat("Crit Damage");
        internal static readonly TotalStat PartyHealing = new TotalStat("Overall Healing");
        internal static readonly TotalStat PartyTotalTaken = new TotalStat("Overall Taken");
        internal static readonly TotalStat PartyTotalRTaken = new TotalStat("Reg Taken");
        internal static readonly TotalStat PartyTotalCTaken = new TotalStat("Crit Taken");
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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
            Logger.Trace("ClearEvent : Clearing ${0} party members totals.", Count);
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
            var cleaned = XmlCleaner.SanitizeXmlString(e.RawLine);
            Logger.Trace("ParseEvent : Parsing line of cleaned : {0}", cleaned);
            Logger.Trace("ParseEvent : Parsing line of raw : {0}", e.RawLine);
            foreach (var tmp in RegExps.Mobbies)
            {
                e.RawLine = Regex.Replace(e.RawLine, tmp, tmp.Replace("'s", ""));
            }
            switch (Settings.Default.Language)
            {
                case "English":
                    English.Parse(mCode, mTimeStamp, cleaned, e);
                    break;
                case "French":
                    French.Parse(mCode, mTimeStamp, cleaned, e);
                    break;
                case "Japanese":
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
    }
}