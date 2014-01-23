// FFXIVAPP.Client
// Settings.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Xml.Linq;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.Utilities;
using NLog;
using ColorConverter = System.Windows.Media.ColorConverter;
using FontFamily = System.Drawing.FontFamily;

namespace FFXIVAPP.Client.SettingsProviders.Parse
{
    public class Settings : ApplicationSettingsBase, INotifyPropertyChanged
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        private static Settings _default;

        public static Settings Default
        {
            get { return _default ?? (_default = ((Settings) (Synchronized(new Settings())))); }
        }

        public override void Save()
        {
            DefaultSettings();
            SaveSettingsNode();
            Constants.Parse.XSettings.Save(Path.Combine(AppViewModel.Instance.PluginsSettingsPath, "FFXIVAPP.Plugin.Parse.xml"));
        }

        private void DefaultSettings()
        {
            Constants.Parse.Settings.Clear();
            Constants.Parse.Settings.Add("StoreHistoryInterval");
            Constants.Parse.Settings.Add("EnableStoreHistoryReset");
            Constants.Parse.Settings.Add("IgnoreLimitBreaks");
            Constants.Parse.Settings.Add("TrackXPSFromParseStartEvent");
            Constants.Parse.Settings.Add("ParseYou");
            Constants.Parse.Settings.Add("ParseParty");
            Constants.Parse.Settings.Add("ParseAlliance");
            Constants.Parse.Settings.Add("ParseOther");
            Constants.Parse.Settings.Add("ParseAdvanced");

            #region Basic Settings

            Constants.Parse.Settings.Add("ShowBasicTotalOverallDamage");
            Constants.Parse.Settings.Add("ShowBasicRegularDamage");
            Constants.Parse.Settings.Add("ShowBasicCriticalDamage");
            Constants.Parse.Settings.Add("ShowBasicTotalDamageActionsUsed");
            Constants.Parse.Settings.Add("ShowBasicDPS");
            Constants.Parse.Settings.Add("ShowBasicDamageRegHit");
            Constants.Parse.Settings.Add("ShowBasicDamageRegMiss");
            Constants.Parse.Settings.Add("ShowBasicDamageRegAccuracy");
            Constants.Parse.Settings.Add("ShowBasicDamageRegLow");
            Constants.Parse.Settings.Add("ShowBasicDamageRegHigh");
            Constants.Parse.Settings.Add("ShowBasicDamageRegAverage");
            Constants.Parse.Settings.Add("ShowBasicDamageRegMod");
            Constants.Parse.Settings.Add("ShowBasicDamageRegModAverage");
            Constants.Parse.Settings.Add("ShowBasicDamageCritHit");
            Constants.Parse.Settings.Add("ShowBasicDamageCritPercent");
            Constants.Parse.Settings.Add("ShowBasicDamageCritLow");
            Constants.Parse.Settings.Add("ShowBasicDamageCritHigh");
            Constants.Parse.Settings.Add("ShowBasicDamageCritAverage");
            Constants.Parse.Settings.Add("ShowBasicDamageCritMod");
            Constants.Parse.Settings.Add("ShowBasicDamageCritModAverage");
            Constants.Parse.Settings.Add("ShowBasicDamageCounter");
            Constants.Parse.Settings.Add("ShowBasicDamageCounterPercent");
            Constants.Parse.Settings.Add("ShowBasicDamageCounterMod");
            Constants.Parse.Settings.Add("ShowBasicDamageCounterModAverage");
            Constants.Parse.Settings.Add("ShowBasicDamageBlock");
            Constants.Parse.Settings.Add("ShowBasicDamageBlockPercent");
            Constants.Parse.Settings.Add("ShowBasicDamageBlockMod");
            Constants.Parse.Settings.Add("ShowBasicDamageBlockModAverage");
            Constants.Parse.Settings.Add("ShowBasicDamageParry");
            Constants.Parse.Settings.Add("ShowBasicDamageParryPercent");
            Constants.Parse.Settings.Add("ShowBasicDamageParryMod");
            Constants.Parse.Settings.Add("ShowBasicDamageParryModAverage");
            Constants.Parse.Settings.Add("ShowBasicDamageResist");
            Constants.Parse.Settings.Add("ShowBasicDamageResistPercent");
            Constants.Parse.Settings.Add("ShowBasicDamageResistMod");
            Constants.Parse.Settings.Add("ShowBasicDamageResistModAverage");
            Constants.Parse.Settings.Add("ShowBasicDamageEvade");
            Constants.Parse.Settings.Add("ShowBasicDamageEvadePercent");
            Constants.Parse.Settings.Add("ShowBasicDamageEvadeMod");
            Constants.Parse.Settings.Add("ShowBasicDamageEvadeModAverage");
            Constants.Parse.Settings.Add("ShowBasicPercentOfTotalOverallDamage");
            Constants.Parse.Settings.Add("ShowBasicPercentOfRegularDamage");
            Constants.Parse.Settings.Add("ShowBasicPercentOfCriticalDamage");

            Constants.Parse.Settings.Add("ShowBasicTotalOverallDamageOverTime");
            Constants.Parse.Settings.Add("ShowBasicRegularDamageOverTime");
            Constants.Parse.Settings.Add("ShowBasicCriticalDamageOverTime");
            Constants.Parse.Settings.Add("ShowBasicTotalDamageOverTimeActionsUsed");
            Constants.Parse.Settings.Add("ShowBasicDOTPS");
            Constants.Parse.Settings.Add("ShowBasicDamageOverTimeRegHit");
            Constants.Parse.Settings.Add("ShowBasicDamageOverTimeRegMiss");
            Constants.Parse.Settings.Add("ShowBasicDamageOverTimeRegAccuracy");
            Constants.Parse.Settings.Add("ShowBasicDamageOverTimeRegLow");
            Constants.Parse.Settings.Add("ShowBasicDamageOverTimeRegHigh");
            Constants.Parse.Settings.Add("ShowBasicDamageOverTimeRegAverage");
            Constants.Parse.Settings.Add("ShowBasicDamageOverTimeRegMod");
            Constants.Parse.Settings.Add("ShowBasicDamageOverTimeRegModAverage");
            Constants.Parse.Settings.Add("ShowBasicDamageOverTimeCritHit");
            Constants.Parse.Settings.Add("ShowBasicDamageOverTimeCritPercent");
            Constants.Parse.Settings.Add("ShowBasicDamageOverTimeCritLow");
            Constants.Parse.Settings.Add("ShowBasicDamageOverTimeCritHigh");
            Constants.Parse.Settings.Add("ShowBasicDamageOverTimeCritAverage");
            Constants.Parse.Settings.Add("ShowBasicDamageOverTimeCritMod");
            Constants.Parse.Settings.Add("ShowBasicDamageOverTimeCritModAverage");
            Constants.Parse.Settings.Add("ShowBasicPercentOfTotalOverallDamageOverTime");
            Constants.Parse.Settings.Add("ShowBasicPercentOfRegularDamageOverTime");
            Constants.Parse.Settings.Add("ShowBasicPercentOfCriticalDamageOverTime");

            Constants.Parse.Settings.Add("ShowBasicTotalOverallHealing");
            Constants.Parse.Settings.Add("ShowBasicRegularHealing");
            Constants.Parse.Settings.Add("ShowBasicCriticalHealing");
            Constants.Parse.Settings.Add("ShowBasicTotalHealingActionsUsed");
            Constants.Parse.Settings.Add("ShowBasicHPS");
            Constants.Parse.Settings.Add("ShowBasicHealingRegHit");
            Constants.Parse.Settings.Add("ShowBasicHealingRegLow");
            Constants.Parse.Settings.Add("ShowBasicHealingRegHigh");
            Constants.Parse.Settings.Add("ShowBasicHealingRegAverage");
            Constants.Parse.Settings.Add("ShowBasicHealingRegMod");
            Constants.Parse.Settings.Add("ShowBasicHealingRegModAverage");
            Constants.Parse.Settings.Add("ShowBasicHealingCritHit");
            Constants.Parse.Settings.Add("ShowBasicHealingCritPercent");
            Constants.Parse.Settings.Add("ShowBasicHealingCritLow");
            Constants.Parse.Settings.Add("ShowBasicHealingCritHigh");
            Constants.Parse.Settings.Add("ShowBasicHealingCritAverage");
            Constants.Parse.Settings.Add("ShowBasicHealingCritMod");
            Constants.Parse.Settings.Add("ShowBasicHealingCritModAverage");
            Constants.Parse.Settings.Add("ShowBasicPercentOfTotalOverallHealing");
            Constants.Parse.Settings.Add("ShowBasicPercentOfRegularHealing");
            Constants.Parse.Settings.Add("ShowBasicPercentOfCriticalHealing");

            Constants.Parse.Settings.Add("ShowBasicTotalOverallHealingOverTime");
            Constants.Parse.Settings.Add("ShowBasicRegularHealingOverTime");
            Constants.Parse.Settings.Add("ShowBasicCriticalHealingOverTime");
            Constants.Parse.Settings.Add("ShowBasicTotalHealingOverTimeActionsUsed");
            Constants.Parse.Settings.Add("ShowBasicHOTPS");
            Constants.Parse.Settings.Add("ShowBasicHealingOverTimeRegHit");
            Constants.Parse.Settings.Add("ShowBasicHealingOverTimeRegLow");
            Constants.Parse.Settings.Add("ShowBasicHealingOverTimeRegHigh");
            Constants.Parse.Settings.Add("ShowBasicHealingOverTimeRegAverage");
            Constants.Parse.Settings.Add("ShowBasicHealingOverTimeRegMod");
            Constants.Parse.Settings.Add("ShowBasicHealingOverTimeRegModAverage");
            Constants.Parse.Settings.Add("ShowBasicHealingOverTimeCritHit");
            Constants.Parse.Settings.Add("ShowBasicHealingOverTimeCritPercent");
            Constants.Parse.Settings.Add("ShowBasicHealingOverTimeCritLow");
            Constants.Parse.Settings.Add("ShowBasicHealingOverTimeCritHigh");
            Constants.Parse.Settings.Add("ShowBasicHealingOverTimeCritAverage");
            Constants.Parse.Settings.Add("ShowBasicHealingOverTimeCritMod");
            Constants.Parse.Settings.Add("ShowBasicHealingOverTimeCritModAverage");
            Constants.Parse.Settings.Add("ShowBasicPercentOfTotalOverallHealingOverTime");
            Constants.Parse.Settings.Add("ShowBasicPercentOfRegularHealingOverTime");
            Constants.Parse.Settings.Add("ShowBasicPercentOfCriticalHealingOverTime");

            Constants.Parse.Settings.Add("ShowBasicTotalOverallHealingOverHealing");
            Constants.Parse.Settings.Add("ShowBasicRegularHealingOverHealing");
            Constants.Parse.Settings.Add("ShowBasicCriticalHealingOverHealing");
            Constants.Parse.Settings.Add("ShowBasicTotalHealingOverHealingActionsUsed");
            Constants.Parse.Settings.Add("ShowBasicHOHPS");
            Constants.Parse.Settings.Add("ShowBasicHealingOverHealingRegHit");
            Constants.Parse.Settings.Add("ShowBasicHealingOverHealingRegLow");
            Constants.Parse.Settings.Add("ShowBasicHealingOverHealingRegHigh");
            Constants.Parse.Settings.Add("ShowBasicHealingOverHealingRegAverage");
            Constants.Parse.Settings.Add("ShowBasicHealingOverHealingRegMod");
            Constants.Parse.Settings.Add("ShowBasicHealingOverHealingRegModAverage");
            Constants.Parse.Settings.Add("ShowBasicHealingOverHealingCritHit");
            Constants.Parse.Settings.Add("ShowBasicHealingOverHealingCritPercent");
            Constants.Parse.Settings.Add("ShowBasicHealingOverHealingCritLow");
            Constants.Parse.Settings.Add("ShowBasicHealingOverHealingCritHigh");
            Constants.Parse.Settings.Add("ShowBasicHealingOverHealingCritAverage");
            Constants.Parse.Settings.Add("ShowBasicHealingOverHealingCritMod");
            Constants.Parse.Settings.Add("ShowBasicHealingOverHealingCritModAverage");
            Constants.Parse.Settings.Add("ShowBasicPercentOfTotalOverallHealingOverHealing");
            Constants.Parse.Settings.Add("ShowBasicPercentOfRegularHealingOverHealing");
            Constants.Parse.Settings.Add("ShowBasicPercentOfCriticalHealingOverHealing");

            Constants.Parse.Settings.Add("ShowBasicTotalOverallHealingMitigated");
            Constants.Parse.Settings.Add("ShowBasicRegularHealingMitigated");
            Constants.Parse.Settings.Add("ShowBasicCriticalHealingMitigated");
            Constants.Parse.Settings.Add("ShowBasicTotalHealingMitigatedActionsUsed");
            Constants.Parse.Settings.Add("ShowBasicHMPS");
            Constants.Parse.Settings.Add("ShowBasicHealingMitigatedRegHit");
            Constants.Parse.Settings.Add("ShowBasicHealingMitigatedRegLow");
            Constants.Parse.Settings.Add("ShowBasicHealingMitigatedRegHigh");
            Constants.Parse.Settings.Add("ShowBasicHealingMitigatedRegAverage");
            Constants.Parse.Settings.Add("ShowBasicHealingMitigatedRegMod");
            Constants.Parse.Settings.Add("ShowBasicHealingMitigatedRegModAverage");
            Constants.Parse.Settings.Add("ShowBasicHealingMitigatedCritHit");
            Constants.Parse.Settings.Add("ShowBasicHealingMitigatedCritPercent");
            Constants.Parse.Settings.Add("ShowBasicHealingMitigatedCritLow");
            Constants.Parse.Settings.Add("ShowBasicHealingMitigatedCritHigh");
            Constants.Parse.Settings.Add("ShowBasicHealingMitigatedCritAverage");
            Constants.Parse.Settings.Add("ShowBasicHealingMitigatedCritMod");
            Constants.Parse.Settings.Add("ShowBasicHealingMitigatedCritModAverage");
            Constants.Parse.Settings.Add("ShowBasicPercentOfTotalOverallHealingMitigated");
            Constants.Parse.Settings.Add("ShowBasicPercentOfRegularHealingMitigated");
            Constants.Parse.Settings.Add("ShowBasicPercentOfCriticalHealingMitigated");

            Constants.Parse.Settings.Add("ShowBasicTotalOverallDamageTaken");
            Constants.Parse.Settings.Add("ShowBasicRegularDamageTaken");
            Constants.Parse.Settings.Add("ShowBasicCriticalDamageTaken");
            Constants.Parse.Settings.Add("ShowBasicTotalDamageTakenActionsUsed");
            Constants.Parse.Settings.Add("ShowBasicDTPS");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenRegHit");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenRegMiss");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenRegAccuracy");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenRegLow");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenRegHigh");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenRegAverage");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenRegMod");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenRegModAverage");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenCritHit");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenCritPercent");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenCritLow");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenCritHigh");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenCritAverage");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenCritMod");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenCritModAverage");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenCounter");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenCounterPercent");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenCounterMod");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenCounterModAverage");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenBlock");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenBlockPercent");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenBlockMod");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenBlockModAverage");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenParry");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenParryPercent");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenParryMod");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenParryModAverage");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenResist");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenResistPercent");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenResistMod");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenResistModAverage");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenEvade");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenEvadePercent");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenEvadeMod");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenEvadeModAverage");
            Constants.Parse.Settings.Add("ShowBasicPercentOfTotalOverallDamageTaken");
            Constants.Parse.Settings.Add("ShowBasicPercentOfRegularDamageTaken");
            Constants.Parse.Settings.Add("ShowBasicPercentOfCriticalDamageTaken");

            Constants.Parse.Settings.Add("ShowBasicTotalOverallDamageTakenOverTime");
            Constants.Parse.Settings.Add("ShowBasicRegularDamageTakenOverTime");
            Constants.Parse.Settings.Add("ShowBasicCriticalDamageTakenOverTime");
            Constants.Parse.Settings.Add("ShowBasicTotalDamageTakenOverTimeActionsUsed");
            Constants.Parse.Settings.Add("ShowBasicDTOTPS");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenOverTimeRegHit");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenOverTimeRegMiss");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenOverTimeRegAccuracy");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenOverTimeRegLow");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenOverTimeRegHigh");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenOverTimeRegAverage");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenOverTimeRegMod");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenOverTimeRegModAverage");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenOverTimeCritHit");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenOverTimeCritPercent");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenOverTimeCritLow");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenOverTimeCritHigh");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenOverTimeCritAverage");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenOverTimeCritMod");
            Constants.Parse.Settings.Add("ShowBasicDamageTakenOverTimeCritModAverage");
            Constants.Parse.Settings.Add("ShowBasicPercentOfTotalOverallDamageTakenOverTime");
            Constants.Parse.Settings.Add("ShowBasicPercentOfRegularDamageTakenOverTime");
            Constants.Parse.Settings.Add("ShowBasicPercentOfCriticalDamageTakenOverTime");

            #endregion

            #region Basic Combined Settings

            Constants.Parse.Settings.Add("ShowBasicCombinedTotalOverallDamage");
            Constants.Parse.Settings.Add("ShowBasicCombinedRegularDamage");
            Constants.Parse.Settings.Add("ShowBasicCombinedCriticalDamage");
            Constants.Parse.Settings.Add("ShowBasicCombinedTotalDamageActionsUsed");
            Constants.Parse.Settings.Add("ShowBasicCombinedDPS");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageRegHit");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageRegMiss");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageRegAccuracy");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageRegLow");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageRegHigh");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageRegAverage");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageRegMod");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageRegModAverage");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageCritHit");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageCritPercent");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageCritLow");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageCritHigh");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageCritAverage");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageCritMod");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageCritModAverage");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageCounter");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageCounterPercent");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageCounterMod");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageCounterModAverage");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageBlock");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageBlockPercent");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageBlockMod");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageBlockModAverage");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageParry");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageParryPercent");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageParryMod");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageParryModAverage");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageResist");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageResistPercent");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageResistMod");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageResistModAverage");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageEvade");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageEvadePercent");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageEvadeMod");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageEvadeModAverage");
            Constants.Parse.Settings.Add("ShowBasicCombinedPercentOfTotalOverallDamage");
            Constants.Parse.Settings.Add("ShowBasicCombinedPercentOfRegularDamage");
            Constants.Parse.Settings.Add("ShowBasicCombinedPercentOfCriticalDamage");
            Constants.Parse.Settings.Add("ShowBasicCombinedTotalOverallHealing");
            Constants.Parse.Settings.Add("ShowBasicCombinedRegularHealing");
            Constants.Parse.Settings.Add("ShowBasicCombinedCriticalHealing");
            Constants.Parse.Settings.Add("ShowBasicCombinedTotalHealingActionsUsed");
            Constants.Parse.Settings.Add("ShowBasicCombinedHPS");
            Constants.Parse.Settings.Add("ShowBasicCombinedHealingRegHit");
            Constants.Parse.Settings.Add("ShowBasicCombinedHealingRegLow");
            Constants.Parse.Settings.Add("ShowBasicCombinedHealingRegHigh");
            Constants.Parse.Settings.Add("ShowBasicCombinedHealingRegAverage");
            Constants.Parse.Settings.Add("ShowBasicCombinedHealingRegMod");
            Constants.Parse.Settings.Add("ShowBasicCombinedHealingRegModAverage");
            Constants.Parse.Settings.Add("ShowBasicCombinedHealingCritHit");
            Constants.Parse.Settings.Add("ShowBasicCombinedHealingCritPercent");
            Constants.Parse.Settings.Add("ShowBasicCombinedHealingCritLow");
            Constants.Parse.Settings.Add("ShowBasicCombinedHealingCritHigh");
            Constants.Parse.Settings.Add("ShowBasicCombinedHealingCritAverage");
            Constants.Parse.Settings.Add("ShowBasicCombinedHealingCritMod");
            Constants.Parse.Settings.Add("ShowBasicCombinedHealingCritModAverage");
            Constants.Parse.Settings.Add("ShowBasicCombinedPercentOfTotalOverallHealing");
            Constants.Parse.Settings.Add("ShowBasicCombinedPercentOfRegularHealing");
            Constants.Parse.Settings.Add("ShowBasicCombinedPercentOfCriticalHealing");
            Constants.Parse.Settings.Add("ShowBasicCombinedTotalOverallDamageTaken");
            Constants.Parse.Settings.Add("ShowBasicCombinedRegularDamageTaken");
            Constants.Parse.Settings.Add("ShowBasicCombinedCriticalDamageTaken");
            Constants.Parse.Settings.Add("ShowBasicCombinedTotalDamageTakenActionsUsed");
            Constants.Parse.Settings.Add("ShowBasicCombinedDTPS");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenRegHit");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenRegMiss");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenRegAccuracy");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenRegLow");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenRegHigh");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenRegAverage");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenRegMod");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenRegModAverage");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenCritHit");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenCritPercent");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenCritLow");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenCritHigh");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenCritAverage");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenCritMod");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenCritModAverage");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenCounter");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenCounterPercent");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenCounterMod");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenCounterModAverage");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenBlock");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenBlockPercent");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenBlockMod");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenBlockModAverage");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenParry");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenParryPercent");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenParryMod");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenParryModAverage");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenResist");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenResistPercent");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenResistMod");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenResistModAverage");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenEvade");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenEvadePercent");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenEvadeMod");
            Constants.Parse.Settings.Add("ShowBasicCombinedDamageTakenEvadeModAverage");
            Constants.Parse.Settings.Add("ShowBasicCombinedPercentOfTotalOverallDamageTaken");
            Constants.Parse.Settings.Add("ShowBasicCombinedPercentOfRegularDamageTaken");
            Constants.Parse.Settings.Add("ShowBasicCombinedPercentOfCriticalDamageTaken");

            #endregion

            #region Column Settings

            Constants.Parse.Settings.Add("ShowColumnTotalOverallDamage");
            Constants.Parse.Settings.Add("ShowColumnRegularDamage");
            Constants.Parse.Settings.Add("ShowColumnCriticalDamage");
            Constants.Parse.Settings.Add("ShowColumnTotalDamageActionsUsed");
            Constants.Parse.Settings.Add("ShowColumnDPS");
            Constants.Parse.Settings.Add("ShowColumnDamageRegHit");
            Constants.Parse.Settings.Add("ShowColumnDamageRegMiss");
            Constants.Parse.Settings.Add("ShowColumnDamageRegAccuracy");
            Constants.Parse.Settings.Add("ShowColumnDamageRegLow");
            Constants.Parse.Settings.Add("ShowColumnDamageRegHigh");
            Constants.Parse.Settings.Add("ShowColumnDamageRegAverage");
            Constants.Parse.Settings.Add("ShowColumnDamageRegMod");
            Constants.Parse.Settings.Add("ShowColumnDamageRegModAverage");
            Constants.Parse.Settings.Add("ShowColumnDamageCritHit");
            Constants.Parse.Settings.Add("ShowColumnDamageCritPercent");
            Constants.Parse.Settings.Add("ShowColumnDamageCritLow");
            Constants.Parse.Settings.Add("ShowColumnDamageCritHigh");
            Constants.Parse.Settings.Add("ShowColumnDamageCritAverage");
            Constants.Parse.Settings.Add("ShowColumnDamageCritMod");
            Constants.Parse.Settings.Add("ShowColumnDamageCritModAverage");
            Constants.Parse.Settings.Add("ShowColumnDamageCounter");
            Constants.Parse.Settings.Add("ShowColumnDamageCounterPercent");
            Constants.Parse.Settings.Add("ShowColumnDamageCounterMod");
            Constants.Parse.Settings.Add("ShowColumnDamageCounterModAverage");
            Constants.Parse.Settings.Add("ShowColumnDamageBlock");
            Constants.Parse.Settings.Add("ShowColumnDamageBlockPercent");
            Constants.Parse.Settings.Add("ShowColumnDamageBlockMod");
            Constants.Parse.Settings.Add("ShowColumnDamageBlockModAverage");
            Constants.Parse.Settings.Add("ShowColumnDamageParry");
            Constants.Parse.Settings.Add("ShowColumnDamageParryPercent");
            Constants.Parse.Settings.Add("ShowColumnDamageParryMod");
            Constants.Parse.Settings.Add("ShowColumnDamageParryModAverage");
            Constants.Parse.Settings.Add("ShowColumnDamageResist");
            Constants.Parse.Settings.Add("ShowColumnDamageResistPercent");
            Constants.Parse.Settings.Add("ShowColumnDamageResistMod");
            Constants.Parse.Settings.Add("ShowColumnDamageResistModAverage");
            Constants.Parse.Settings.Add("ShowColumnDamageEvade");
            Constants.Parse.Settings.Add("ShowColumnDamageEvadePercent");
            Constants.Parse.Settings.Add("ShowColumnDamageEvadeMod");
            Constants.Parse.Settings.Add("ShowColumnDamageEvadeModAverage");
            Constants.Parse.Settings.Add("ShowColumnPercentOfTotalOverallDamage");
            Constants.Parse.Settings.Add("ShowColumnPercentOfRegularDamage");
            Constants.Parse.Settings.Add("ShowColumnPercentOfCriticalDamage");
            Constants.Parse.Settings.Add("ShowColumnTotalOverallHealing");
            Constants.Parse.Settings.Add("ShowColumnRegularHealing");
            Constants.Parse.Settings.Add("ShowColumnCriticalHealing");
            Constants.Parse.Settings.Add("ShowColumnTotalHealingActionsUsed");
            Constants.Parse.Settings.Add("ShowColumnHPS");
            Constants.Parse.Settings.Add("ShowColumnHealingRegHit");
            Constants.Parse.Settings.Add("ShowColumnHealingRegLow");
            Constants.Parse.Settings.Add("ShowColumnHealingRegHigh");
            Constants.Parse.Settings.Add("ShowColumnHealingRegAverage");
            Constants.Parse.Settings.Add("ShowColumnHealingRegMod");
            Constants.Parse.Settings.Add("ShowColumnHealingRegModAverage");
            Constants.Parse.Settings.Add("ShowColumnHealingCritHit");
            Constants.Parse.Settings.Add("ShowColumnHealingCritPercent");
            Constants.Parse.Settings.Add("ShowColumnHealingCritLow");
            Constants.Parse.Settings.Add("ShowColumnHealingCritHigh");
            Constants.Parse.Settings.Add("ShowColumnHealingCritAverage");
            Constants.Parse.Settings.Add("ShowColumnHealingCritMod");
            Constants.Parse.Settings.Add("ShowColumnHealingCritModAverage");
            Constants.Parse.Settings.Add("ShowColumnPercentOfTotalOverallHealing");
            Constants.Parse.Settings.Add("ShowColumnPercentOfRegularHealing");
            Constants.Parse.Settings.Add("ShowColumnPercentOfCriticalHealing");
            Constants.Parse.Settings.Add("ShowColumnTotalOverallDamageTaken");
            Constants.Parse.Settings.Add("ShowColumnRegularDamageTaken");
            Constants.Parse.Settings.Add("ShowColumnCriticalDamageTaken");
            Constants.Parse.Settings.Add("ShowColumnTotalDamageTakenActionsUsed");
            Constants.Parse.Settings.Add("ShowColumnDTPS");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenRegHit");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenRegMiss");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenRegAccuracy");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenRegLow");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenRegHigh");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenRegAverage");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenRegMod");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenRegModAverage");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenCritHit");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenCritPercent");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenCritLow");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenCritHigh");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenCritAverage");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenCritMod");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenCritModAverage");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenCounter");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenCounterPercent");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenCounterMod");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenCounterModAverage");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenBlock");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenBlockPercent");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenBlockMod");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenBlockModAverage");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenParry");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenParryPercent");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenParryMod");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenParryModAverage");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenResist");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenResistPercent");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenResistMod");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenResistModAverage");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenEvade");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenEvadePercent");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenEvadeMod");
            Constants.Parse.Settings.Add("ShowColumnDamageTakenEvadeModAverage");
            Constants.Parse.Settings.Add("ShowColumnPercentOfTotalOverallDamageTaken");
            Constants.Parse.Settings.Add("ShowColumnPercentOfRegularDamageTaken");
            Constants.Parse.Settings.Add("ShowColumnPercentOfCriticalDamageTaken");

            #endregion
        }

        public new void Reset()
        {
            DefaultSettings();
            foreach (var key in Constants.Parse.Settings)
            {
                var settingsProperty = Default.Properties[key];
                if (settingsProperty == null)
                {
                    continue;
                }
                var value = settingsProperty.DefaultValue.ToString();
                SetValue(key, value, CultureInfo.InvariantCulture);
            }
        }

        public static void SetValue(string key, string value, CultureInfo cultureInfo)
        {
            try
            {
                var type = Default[key].GetType()
                                       .Name;
                switch (type)
                {
                    case "Boolean":
                        Default[key] = Boolean.Parse(value);
                        break;
                    case "Color":
                        var cc = new ColorConverter();
                        var color = cc.ConvertFrom(value);
                        Default[key] = color ?? Colors.Black;
                        break;
                    case "Double":
                        Default[key] = Double.Parse(value, cultureInfo);
                        break;
                    case "Font":
                        var fc = new FontConverter();
                        var font = fc.ConvertFromString(value);
                        Default[key] = font ?? new Font(new FontFamily("Microsoft Sans Serif"), 12);
                        break;
                    case "Int32":
                        Default[key] = Int32.Parse(value, cultureInfo);
                        break;
                    default:
                        Default[key] = value;
                        break;
                }
            }
            catch (Exception ex)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
            }
        }

        #region Property Bindings (Settings)

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("100")]
        public Double Zoom
        {
            get { return ((Double) (this["Zoom"])); }
            set
            {
                this["Zoom"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("10000")]
        public string StoreHistoryInterval
        {
            get { return ((string) (this["StoreHistoryInterval"])); }
            set
            {
                this["StoreHistoryInterval"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool EnableStoreHistoryReset
        {
            get { return ((bool) (this["EnableStoreHistoryReset"])); }
            set
            {
                this["EnableStoreHistoryReset"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ParseAdvanced
        {
            get { return ((bool) (this["ParseAdvanced"])); }
            set
            {
                this["ParseAdvanced"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool IgnoreLimitBreaks
        {
            get { return ((bool) (this["IgnoreLimitBreaks"])); }
            set
            {
                this["IgnoreLimitBreaks"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool TrackXPSFromParseStartEvent
        {
            get { return ((bool) (this["TrackXPSFromParseStartEvent"])); }
            set
            {
                this["TrackXPSFromParseStartEvent"] = value;
                RaisePropertyChanged();
            }
        }

        #region Basic Settings

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowBasicTotalOverallDamage
        {
            get { return ((bool) (this["ShowBasicTotalOverallDamage"])); }
            set
            {
                this["ShowBasicTotalOverallDamage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicRegularDamage
        {
            get { return ((bool) (this["ShowBasicRegularDamage"])); }
            set
            {
                this["ShowBasicRegularDamage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCriticalDamage
        {
            get { return ((bool) (this["ShowBasicCriticalDamage"])); }
            set
            {
                this["ShowBasicCriticalDamage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicTotalDamageActionsUsed
        {
            get { return ((bool) (this["ShowBasicTotalDamageActionsUsed"])); }
            set
            {
                this["ShowBasicTotalDamageActionsUsed"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowBasicDPS
        {
            get { return ((bool) (this["ShowBasicDPS"])); }
            set
            {
                this["ShowBasicDPS"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageRegHit
        {
            get { return ((bool) (this["ShowBasicDamageRegHit"])); }
            set
            {
                this["ShowBasicDamageRegHit"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageRegMiss
        {
            get { return ((bool) (this["ShowBasicDamageRegMiss"])); }
            set
            {
                this["ShowBasicDamageRegMiss"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowBasicDamageRegAccuracy
        {
            get { return ((bool) (this["ShowBasicDamageRegAccuracy"])); }
            set
            {
                this["ShowBasicDamageRegAccuracy"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageRegLow
        {
            get { return ((bool) (this["ShowBasicDamageRegLow"])); }
            set
            {
                this["ShowBasicDamageRegLow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageRegHigh
        {
            get { return ((bool) (this["ShowBasicDamageRegHigh"])); }
            set
            {
                this["ShowBasicDamageRegHigh"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageRegAverage
        {
            get { return ((bool) (this["ShowBasicDamageRegAverage"])); }
            set
            {
                this["ShowBasicDamageRegAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageRegMod
        {
            get { return ((bool) (this["ShowBasicDamageRegMod"])); }
            set
            {
                this["ShowBasicDamageRegMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageRegModAverage
        {
            get { return ((bool) (this["ShowBasicDamageRegModAverage"])); }
            set
            {
                this["ShowBasicDamageRegModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageCritHit
        {
            get { return ((bool) (this["ShowBasicDamageCritHit"])); }
            set
            {
                this["ShowBasicDamageCritHit"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowBasicDamageCritPercent
        {
            get { return ((bool) (this["ShowBasicDamageCritPercent"])); }
            set
            {
                this["ShowBasicDamageCritPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageCritLow
        {
            get { return ((bool) (this["ShowBasicDamageCritLow"])); }
            set
            {
                this["ShowBasicDamageCritLow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageCritHigh
        {
            get { return ((bool) (this["ShowBasicDamageCritHigh"])); }
            set
            {
                this["ShowBasicDamageCritHigh"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageCritAverage
        {
            get { return ((bool) (this["ShowBasicDamageCritAverage"])); }
            set
            {
                this["ShowBasicDamageCritAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageCritMod
        {
            get { return ((bool) (this["ShowBasicDamageCritMod"])); }
            set
            {
                this["ShowBasicDamageCritMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageCritModAverage
        {
            get { return ((bool) (this["ShowBasicDamageCritModAverage"])); }
            set
            {
                this["ShowBasicDamageCritModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageCounter
        {
            get { return ((bool) (this["ShowBasicDamageCounter"])); }
            set
            {
                this["ShowBasicDamageCounter"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageCounterPercent
        {
            get { return ((bool) (this["ShowBasicDamageCounterPercent"])); }
            set
            {
                this["ShowBasicDamageCounterPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageCounterMod
        {
            get { return ((bool) (this["ShowBasicDamageCounterMod"])); }
            set
            {
                this["ShowBasicDamageCounterMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageCounterModAverage
        {
            get { return ((bool) (this["ShowBasicDamageCounterModAverage"])); }
            set
            {
                this["ShowBasicDamageCounterModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageBlock
        {
            get { return ((bool) (this["ShowBasicDamageBlock"])); }
            set
            {
                this["ShowBasicDamageBlock"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageBlockPercent
        {
            get { return ((bool) (this["ShowBasicDamageBlockPercent"])); }
            set
            {
                this["ShowBasicDamageBlockPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageBlockMod
        {
            get { return ((bool) (this["ShowBasicDamageBlockMod"])); }
            set
            {
                this["ShowBasicDamageBlockMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageBlockModAverage
        {
            get { return ((bool) (this["ShowBasicDamageBlockModAverage"])); }
            set
            {
                this["ShowBasicDamageBlockModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageParry
        {
            get { return ((bool) (this["ShowBasicDamageParry"])); }
            set
            {
                this["ShowBasicDamageParry"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageParryPercent
        {
            get { return ((bool) (this["ShowBasicDamageParryPercent"])); }
            set
            {
                this["ShowBasicDamageParryPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageParryMod
        {
            get { return ((bool) (this["ShowBasicDamageParryMod"])); }
            set
            {
                this["ShowBasicDamageParryMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageParryModAverage
        {
            get { return ((bool) (this["ShowBasicDamageParryModAverage"])); }
            set
            {
                this["ShowBasicDamageParryModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageResist
        {
            get { return ((bool) (this["ShowBasicDamageResist"])); }
            set
            {
                this["ShowBasicDamageResist"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageResistPercent
        {
            get { return ((bool) (this["ShowBasicDamageResistPercent"])); }
            set
            {
                this["ShowBasicDamageResistPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageResistMod
        {
            get { return ((bool) (this["ShowBasicDamageResistMod"])); }
            set
            {
                this["ShowBasicDamageResistMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageResistModAverage
        {
            get { return ((bool) (this["ShowBasicDamageResistModAverage"])); }
            set
            {
                this["ShowBasicDamageResistModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageEvade
        {
            get { return ((bool) (this["ShowBasicDamageEvade"])); }
            set
            {
                this["ShowBasicDamageEvade"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageEvadePercent
        {
            get { return ((bool) (this["ShowBasicDamageEvadePercent"])); }
            set
            {
                this["ShowBasicDamageEvadePercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageEvadeMod
        {
            get { return ((bool) (this["ShowBasicDamageEvadeMod"])); }
            set
            {
                this["ShowBasicDamageEvadeMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageEvadeModAverage
        {
            get { return ((bool) (this["ShowBasicDamageEvadeModAverage"])); }
            set
            {
                this["ShowBasicDamageEvadeModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicPercentOfTotalOverallDamage
        {
            get { return ((bool) (this["ShowBasicPercentOfTotalOverallDamage"])); }
            set
            {
                this["ShowBasicPercentOfTotalOverallDamage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicPercentOfRegularDamage
        {
            get { return ((bool) (this["ShowBasicPercentOfRegularDamage"])); }
            set
            {
                this["ShowBasicPercentOfRegularDamage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicPercentOfCriticalDamage
        {
            get { return ((bool) (this["ShowBasicPercentOfCriticalDamage"])); }
            set
            {
                this["ShowBasicPercentOfCriticalDamage"] = value;
                RaisePropertyChanged();
            }
        }

        #region Damage Over Time

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicTotalOverallDamageOverTime
        {
            get { return ((bool) (this["ShowBasicTotalOverallDamageOverTime"])); }
            set
            {
                this["ShowBasicTotalOverallDamageOverTime"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicRegularDamageOverTime
        {
            get { return ((bool) (this["ShowBasicRegularDamageOverTime"])); }
            set
            {
                this["ShowBasicRegularDamageOverTime"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCriticalDamageOverTime
        {
            get { return ((bool) (this["ShowBasicCriticalDamageOverTime"])); }
            set
            {
                this["ShowBasicCriticalDamageOverTime"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicTotalDamageOverTimeActionsUsed
        {
            get { return ((bool) (this["ShowBasicTotalDamageOverTimeActionsUsed"])); }
            set
            {
                this["ShowBasicTotalDamageOverTimeActionsUsed"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDOTPS
        {
            get { return ((bool) (this["ShowBasicDOTPS"])); }
            set
            {
                this["ShowBasicDOTPS"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageOverTimeRegHit
        {
            get { return ((bool) (this["ShowBasicDamageOverTimeRegHit"])); }
            set
            {
                this["ShowBasicDamageOverTimeRegHit"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageOverTimeRegMiss
        {
            get { return ((bool) (this["ShowBasicDamageOverTimeRegMiss"])); }
            set
            {
                this["ShowBasicDamageOverTimeRegMiss"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageOverTimeRegAccuracy
        {
            get { return ((bool) (this["ShowBasicDamageOverTimeRegAccuracy"])); }
            set
            {
                this["ShowBasicDamageOverTimeRegAccuracy"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageOverTimeRegLow
        {
            get { return ((bool) (this["ShowBasicDamageOverTimeRegLow"])); }
            set
            {
                this["ShowBasicDamageOverTimeRegLow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageOverTimeRegHigh
        {
            get { return ((bool) (this["ShowBasicDamageOverTimeRegHigh"])); }
            set
            {
                this["ShowBasicDamageOverTimeRegHigh"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageOverTimeRegAverage
        {
            get { return ((bool) (this["ShowBasicDamageOverTimeRegAverage"])); }
            set
            {
                this["ShowBasicDamageOverTimeRegAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageOverTimeRegMod
        {
            get { return ((bool) (this["ShowBasicDamageOverTimeRegMod"])); }
            set
            {
                this["ShowBasicDamageOverTimeRegMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageOverTimeRegModAverage
        {
            get { return ((bool) (this["ShowBasicDamageOverTimeRegModAverage"])); }
            set
            {
                this["ShowBasicDamageOverTimeRegModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageOverTimeCritHit
        {
            get { return ((bool) (this["ShowBasicDamageOverTimeCritHit"])); }
            set
            {
                this["ShowBasicDamageOverTimeCritHit"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageOverTimeCritPercent
        {
            get { return ((bool) (this["ShowBasicDamageOverTimeCritPercent"])); }
            set
            {
                this["ShowBasicDamageOverTimeCritPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageOverTimeCritLow
        {
            get { return ((bool) (this["ShowBasicDamageOverTimeCritLow"])); }
            set
            {
                this["ShowBasicDamageOverTimeCritLow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageOverTimeCritHigh
        {
            get { return ((bool) (this["ShowBasicDamageOverTimeCritHigh"])); }
            set
            {
                this["ShowBasicDamageOverTimeCritHigh"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageOverTimeCritAverage
        {
            get { return ((bool) (this["ShowBasicDamageOverTimeCritAverage"])); }
            set
            {
                this["ShowBasicDamageOverTimeCritAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageOverTimeCritMod
        {
            get { return ((bool) (this["ShowBasicDamageOverTimeCritMod"])); }
            set
            {
                this["ShowBasicDamageOverTimeCritMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageOverTimeCritModAverage
        {
            get { return ((bool) (this["ShowBasicDamageOverTimeCritModAverage"])); }
            set
            {
                this["ShowBasicDamageOverTimeCritModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicPercentOfTotalOverallDamageOverTime
        {
            get { return ((bool) (this["ShowBasicPercentOfTotalOverallDamageOverTime"])); }
            set
            {
                this["ShowBasicPercentOfTotalOverallDamageOverTime"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicPercentOfRegularDamageOverTime
        {
            get { return ((bool) (this["ShowBasicPercentOfRegularDamageOverTime"])); }
            set
            {
                this["ShowBasicPercentOfRegularDamageOverTime"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicPercentOfCriticalDamageOverTime
        {
            get { return ((bool) (this["ShowBasicPercentOfCriticalDamageOverTime"])); }
            set
            {
                this["ShowBasicPercentOfCriticalDamageOverTime"] = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicTotalOverallHealing
        {
            get { return ((bool) (this["ShowBasicTotalOverallHealing"])); }
            set
            {
                this["ShowBasicTotalOverallHealing"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicRegularHealing
        {
            get { return ((bool) (this["ShowBasicRegularHealing"])); }
            set
            {
                this["ShowBasicRegularHealing"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCriticalHealing
        {
            get { return ((bool) (this["ShowBasicCriticalHealing"])); }
            set
            {
                this["ShowBasicCriticalHealing"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicTotalHealingActionsUsed
        {
            get { return ((bool) (this["ShowBasicTotalHealingActionsUsed"])); }
            set
            {
                this["ShowBasicTotalHealingActionsUsed"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHPS
        {
            get { return ((bool) (this["ShowBasicHPS"])); }
            set
            {
                this["ShowBasicHPS"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingRegHit
        {
            get { return ((bool) (this["ShowBasicHealingRegHit"])); }
            set
            {
                this["ShowBasicHealingRegHit"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingRegLow
        {
            get { return ((bool) (this["ShowBasicHealingRegLow"])); }
            set
            {
                this["ShowBasicHealingRegLow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingRegHigh
        {
            get { return ((bool) (this["ShowBasicHealingRegHigh"])); }
            set
            {
                this["ShowBasicHealingRegHigh"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingRegAverage
        {
            get { return ((bool) (this["ShowBasicHealingRegAverage"])); }
            set
            {
                this["ShowBasicHealingRegAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingRegMod
        {
            get { return ((bool) (this["ShowBasicHealingRegMod"])); }
            set
            {
                this["ShowBasicHealingRegMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingRegModAverage
        {
            get { return ((bool) (this["ShowBasicHealingRegModAverage"])); }
            set
            {
                this["ShowBasicHealingRegModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingCritHit
        {
            get { return ((bool) (this["ShowBasicHealingCritHit"])); }
            set
            {
                this["ShowBasicHealingCritHit"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingCritPercent
        {
            get { return ((bool) (this["ShowBasicHealingCritPercent"])); }
            set
            {
                this["ShowBasicHealingCritPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingCritLow
        {
            get { return ((bool) (this["ShowBasicHealingCritLow"])); }
            set
            {
                this["ShowBasicHealingCritLow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingCritHigh
        {
            get { return ((bool) (this["ShowBasicHealingCritHigh"])); }
            set
            {
                this["ShowBasicHealingCritHigh"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingCritAverage
        {
            get { return ((bool) (this["ShowBasicHealingCritAverage"])); }
            set
            {
                this["ShowBasicHealingCritAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingCritMod
        {
            get { return ((bool) (this["ShowBasicHealingCritMod"])); }
            set
            {
                this["ShowBasicHealingCritMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingCritModAverage
        {
            get { return ((bool) (this["ShowBasicHealingCritModAverage"])); }
            set
            {
                this["ShowBasicHealingCritModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicPercentOfTotalOverallHealing
        {
            get { return ((bool) (this["ShowBasicPercentOfTotalOverallHealing"])); }
            set
            {
                this["ShowBasicPercentOfTotalOverallHealing"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicPercentOfRegularHealing
        {
            get { return ((bool) (this["ShowBasicPercentOfRegularHealing"])); }
            set
            {
                this["ShowBasicPercentOfRegularHealing"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicPercentOfCriticalHealing
        {
            get { return ((bool) (this["ShowBasicPercentOfCriticalHealing"])); }
            set
            {
                this["ShowBasicPercentOfCriticalHealing"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicTotalOverallDamageTaken
        {
            get { return ((bool) (this["ShowBasicTotalOverallDamageTaken"])); }
            set
            {
                this["ShowBasicTotalOverallDamageTaken"] = value;
                RaisePropertyChanged();
            }
        }

        #region Healing Over Time

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicTotalOverallHealingOverTime
        {
            get { return ((bool) (this["ShowBasicTotalOverallHealingOverTime"])); }
            set
            {
                this["ShowBasicTotalOverallHealingOverTime"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicRegularHealingOverTime
        {
            get { return ((bool) (this["ShowBasicRegularHealingOverTime"])); }
            set
            {
                this["ShowBasicRegularHealingOverTime"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCriticalHealingOverTime
        {
            get { return ((bool) (this["ShowBasicCriticalHealingOverTime"])); }
            set
            {
                this["ShowBasicCriticalHealingOverTime"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicTotalHealingOverTimeActionsUsed
        {
            get { return ((bool) (this["ShowBasicTotalHealingOverTimeActionsUsed"])); }
            set
            {
                this["ShowBasicTotalHealingOverTimeActionsUsed"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHOTPS
        {
            get { return ((bool) (this["ShowBasicHOTPS"])); }
            set
            {
                this["ShowBasicHOTPS"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingOverTimeRegHit
        {
            get { return ((bool) (this["ShowBasicHealingOverTimeRegHit"])); }
            set
            {
                this["ShowBasicHealingOverTimeRegHit"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingOverTimeRegLow
        {
            get { return ((bool) (this["ShowBasicHealingOverTimeRegLow"])); }
            set
            {
                this["ShowBasicHealingOverTimeRegLow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingOverTimeRegHigh
        {
            get { return ((bool) (this["ShowBasicHealingOverTimeRegHigh"])); }
            set
            {
                this["ShowBasicHealingOverTimeRegHigh"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingOverTimeRegAverage
        {
            get { return ((bool) (this["ShowBasicHealingOverTimeRegAverage"])); }
            set
            {
                this["ShowBasicHealingOverTimeRegAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingOverTimeRegMod
        {
            get { return ((bool) (this["ShowBasicHealingOverTimeRegMod"])); }
            set
            {
                this["ShowBasicHealingOverTimeRegMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingOverTimeRegModAverage
        {
            get { return ((bool) (this["ShowBasicHealingOverTimeRegModAverage"])); }
            set
            {
                this["ShowBasicHealingOverTimeRegModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingOverTimeCritHit
        {
            get { return ((bool) (this["ShowBasicHealingOverTimeCritHit"])); }
            set
            {
                this["ShowBasicHealingOverTimeCritHit"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingOverTimeCritPercent
        {
            get { return ((bool) (this["ShowBasicHealingOverTimeCritPercent"])); }
            set
            {
                this["ShowBasicHealingOverTimeCritPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingOverTimeCritLow
        {
            get { return ((bool) (this["ShowBasicHealingOverTimeCritLow"])); }
            set
            {
                this["ShowBasicHealingOverTimeCritLow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingOverTimeCritHigh
        {
            get { return ((bool) (this["ShowBasicHealingOverTimeCritHigh"])); }
            set
            {
                this["ShowBasicHealingOverTimeCritHigh"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingOverTimeCritAverage
        {
            get { return ((bool) (this["ShowBasicHealingOverTimeCritAverage"])); }
            set
            {
                this["ShowBasicHealingOverTimeCritAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingOverTimeCritMod
        {
            get { return ((bool) (this["ShowBasicHealingOverTimeCritMod"])); }
            set
            {
                this["ShowBasicHealingOverTimeCritMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingOverTimeCritModAverage
        {
            get { return ((bool) (this["ShowBasicHealingOverTimeCritModAverage"])); }
            set
            {
                this["ShowBasicHealingOverTimeCritModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicPercentOfTotalOverallHealingOverTime
        {
            get { return ((bool) (this["ShowBasicPercentOfTotalOverallHealingOverTime"])); }
            set
            {
                this["ShowBasicPercentOfTotalOverallHealingOverTime"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicPercentOfRegularHealingOverTime
        {
            get { return ((bool) (this["ShowBasicPercentOfRegularHealingOverTime"])); }
            set
            {
                this["ShowBasicPercentOfRegularHealingOverTime"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicPercentOfCriticalHealingOverTime
        {
            get { return ((bool) (this["ShowBasicPercentOfCriticalHealingOverTime"])); }
            set
            {
                this["ShowBasicPercentOfCriticalHealingOverTime"] = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Healing Over Healing

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicTotalOverallHealingOverHealing
        {
            get { return ((bool) (this["ShowBasicTotalOverallHealingOverHealing"])); }
            set
            {
                this["ShowBasicTotalOverallHealingOverHealing"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicRegularHealingOverHealing
        {
            get { return ((bool) (this["ShowBasicRegularHealingOverHealing"])); }
            set
            {
                this["ShowBasicRegularHealingOverHealing"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCriticalHealingOverHealing
        {
            get { return ((bool) (this["ShowBasicCriticalHealingOverHealing"])); }
            set
            {
                this["ShowBasicCriticalHealingOverHealing"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicTotalHealingOverHealingActionsUsed
        {
            get { return ((bool) (this["ShowBasicTotalHealingOverHealingActionsUsed"])); }
            set
            {
                this["ShowBasicTotalHealingOverHealingActionsUsed"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHOHPS
        {
            get { return ((bool) (this["ShowBasicHOHPS"])); }
            set
            {
                this["ShowBasicHOHPS"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingOverHealingRegHit
        {
            get { return ((bool) (this["ShowBasicHealingOverHealingRegHit"])); }
            set
            {
                this["ShowBasicHealingOverHealingRegHit"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingOverHealingRegLow
        {
            get { return ((bool) (this["ShowBasicHealingOverHealingRegLow"])); }
            set
            {
                this["ShowBasicHealingOverHealingRegLow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingOverHealingRegHigh
        {
            get { return ((bool) (this["ShowBasicHealingOverHealingRegHigh"])); }
            set
            {
                this["ShowBasicHealingOverHealingRegHigh"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingOverHealingRegAverage
        {
            get { return ((bool) (this["ShowBasicHealingOverHealingRegAverage"])); }
            set
            {
                this["ShowBasicHealingOverHealingRegAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingOverHealingRegMod
        {
            get { return ((bool) (this["ShowBasicHealingOverHealingRegMod"])); }
            set
            {
                this["ShowBasicHealingOverHealingRegMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingOverHealingRegModAverage
        {
            get { return ((bool) (this["ShowBasicHealingOverHealingRegModAverage"])); }
            set
            {
                this["ShowBasicHealingOverHealingRegModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingOverHealingCritHit
        {
            get { return ((bool) (this["ShowBasicHealingOverHealingCritHit"])); }
            set
            {
                this["ShowBasicHealingOverHealingCritHit"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingOverHealingCritPercent
        {
            get { return ((bool) (this["ShowBasicHealingOverHealingCritPercent"])); }
            set
            {
                this["ShowBasicHealingOverHealingCritPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingOverHealingCritLow
        {
            get { return ((bool) (this["ShowBasicHealingOverHealingCritLow"])); }
            set
            {
                this["ShowBasicHealingOverHealingCritLow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingOverHealingCritHigh
        {
            get { return ((bool) (this["ShowBasicHealingOverHealingCritHigh"])); }
            set
            {
                this["ShowBasicHealingOverHealingCritHigh"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingOverHealingCritAverage
        {
            get { return ((bool) (this["ShowBasicHealingOverHealingCritAverage"])); }
            set
            {
                this["ShowBasicHealingOverHealingCritAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingOverHealingCritMod
        {
            get { return ((bool) (this["ShowBasicHealingOverHealingCritMod"])); }
            set
            {
                this["ShowBasicHealingOverHealingCritMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingOverHealingCritModAverage
        {
            get { return ((bool) (this["ShowBasicHealingOverHealingCritModAverage"])); }
            set
            {
                this["ShowBasicHealingOverHealingCritModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicPercentOfTotalOverallHealingOverHealing
        {
            get { return ((bool) (this["ShowBasicPercentOfTotalOverallHealingOverHealing"])); }
            set
            {
                this["ShowBasicPercentOfTotalOverallHealingOverHealing"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicPercentOfRegularHealingOverHealing
        {
            get { return ((bool) (this["ShowBasicPercentOfRegularHealingOverHealing"])); }
            set
            {
                this["ShowBasicPercentOfRegularHealingOverHealing"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicPercentOfCriticalHealingOverHealing
        {
            get { return ((bool) (this["ShowBasicPercentOfCriticalHealingOverHealing"])); }
            set
            {
                this["ShowBasicPercentOfCriticalHealingOverHealing"] = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Healing Over Healing

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicTotalOverallHealingMitigated
        {
            get { return ((bool) (this["ShowBasicTotalOverallHealingMitigated"])); }
            set
            {
                this["ShowBasicTotalOverallHealingMitigated"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicRegularHealingMitigated
        {
            get { return ((bool) (this["ShowBasicRegularHealingMitigated"])); }
            set
            {
                this["ShowBasicRegularHealingMitigated"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCriticalHealingMitigated
        {
            get { return ((bool) (this["ShowBasicCriticalHealingMitigated"])); }
            set
            {
                this["ShowBasicCriticalHealingMitigated"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicTotalHealingMitigatedActionsUsed
        {
            get { return ((bool) (this["ShowBasicTotalHealingMitigatedActionsUsed"])); }
            set
            {
                this["ShowBasicTotalHealingMitigatedActionsUsed"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHMPS
        {
            get { return ((bool) (this["ShowBasicHMPS"])); }
            set
            {
                this["ShowBasicHMPS"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingMitigatedRegHit
        {
            get { return ((bool) (this["ShowBasicHealingMitigatedRegHit"])); }
            set
            {
                this["ShowBasicHealingMitigatedRegHit"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingMitigatedRegLow
        {
            get { return ((bool) (this["ShowBasicHealingMitigatedRegLow"])); }
            set
            {
                this["ShowBasicHealingMitigatedRegLow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingMitigatedRegHigh
        {
            get { return ((bool) (this["ShowBasicHealingMitigatedRegHigh"])); }
            set
            {
                this["ShowBasicHealingMitigatedRegHigh"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingMitigatedRegAverage
        {
            get { return ((bool) (this["ShowBasicHealingMitigatedRegAverage"])); }
            set
            {
                this["ShowBasicHealingMitigatedRegAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingMitigatedRegMod
        {
            get { return ((bool) (this["ShowBasicHealingMitigatedRegMod"])); }
            set
            {
                this["ShowBasicHealingMitigatedRegMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingMitigatedRegModAverage
        {
            get { return ((bool) (this["ShowBasicHealingMitigatedRegModAverage"])); }
            set
            {
                this["ShowBasicHealingMitigatedRegModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingMitigatedCritHit
        {
            get { return ((bool) (this["ShowBasicHealingMitigatedCritHit"])); }
            set
            {
                this["ShowBasicHealingMitigatedCritHit"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingMitigatedCritPercent
        {
            get { return ((bool) (this["ShowBasicHealingMitigatedCritPercent"])); }
            set
            {
                this["ShowBasicHealingMitigatedCritPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingMitigatedCritLow
        {
            get { return ((bool) (this["ShowBasicHealingMitigatedCritLow"])); }
            set
            {
                this["ShowBasicHealingMitigatedCritLow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingMitigatedCritHigh
        {
            get { return ((bool) (this["ShowBasicHealingMitigatedCritHigh"])); }
            set
            {
                this["ShowBasicHealingMitigatedCritHigh"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingMitigatedCritAverage
        {
            get { return ((bool) (this["ShowBasicHealingMitigatedCritAverage"])); }
            set
            {
                this["ShowBasicHealingMitigatedCritAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingMitigatedCritMod
        {
            get { return ((bool) (this["ShowBasicHealingMitigatedCritMod"])); }
            set
            {
                this["ShowBasicHealingMitigatedCritMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicHealingMitigatedCritModAverage
        {
            get { return ((bool) (this["ShowBasicHealingMitigatedCritModAverage"])); }
            set
            {
                this["ShowBasicHealingMitigatedCritModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicPercentOfTotalOverallHealingMitigated
        {
            get { return ((bool) (this["ShowBasicPercentOfTotalOverallHealingMitigated"])); }
            set
            {
                this["ShowBasicPercentOfTotalOverallHealingMitigated"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicPercentOfRegularHealingMitigated
        {
            get { return ((bool) (this["ShowBasicPercentOfRegularHealingMitigated"])); }
            set
            {
                this["ShowBasicPercentOfRegularHealingMitigated"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicPercentOfCriticalHealingMitigated
        {
            get { return ((bool) (this["ShowBasicPercentOfCriticalHealingMitigated"])); }
            set
            {
                this["ShowBasicPercentOfCriticalHealingMitigated"] = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicRegularDamageTaken
        {
            get { return ((bool) (this["ShowBasicRegularDamageTaken"])); }
            set
            {
                this["ShowBasicRegularDamageTaken"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCriticalDamageTaken
        {
            get { return ((bool) (this["ShowBasicCriticalDamageTaken"])); }
            set
            {
                this["ShowBasicCriticalDamageTaken"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicTotalDamageTakenActionsUsed
        {
            get { return ((bool) (this["ShowBasicTotalDamageTakenActionsUsed"])); }
            set
            {
                this["ShowBasicTotalDamageTakenActionsUsed"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDTPS
        {
            get { return ((bool) (this["ShowBasicDTPS"])); }
            set
            {
                this["ShowBasicDTPS"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenRegHit
        {
            get { return ((bool) (this["ShowBasicDamageTakenRegHit"])); }
            set
            {
                this["ShowBasicDamageTakenRegHit"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenRegMiss
        {
            get { return ((bool) (this["ShowBasicDamageTakenRegMiss"])); }
            set
            {
                this["ShowBasicDamageTakenRegMiss"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenRegAccuracy
        {
            get { return ((bool) (this["ShowBasicDamageTakenRegAccuracy"])); }
            set
            {
                this["ShowBasicDamageTakenRegAccuracy"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenRegLow
        {
            get { return ((bool) (this["ShowBasicDamageTakenRegLow"])); }
            set
            {
                this["ShowBasicDamageTakenRegLow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenRegHigh
        {
            get { return ((bool) (this["ShowBasicDamageTakenRegHigh"])); }
            set
            {
                this["ShowBasicDamageTakenRegHigh"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenRegAverage
        {
            get { return ((bool) (this["ShowBasicDamageTakenRegAverage"])); }
            set
            {
                this["ShowBasicDamageTakenRegAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenRegMod
        {
            get { return ((bool) (this["ShowBasicDamageTakenRegMod"])); }
            set
            {
                this["ShowBasicDamageTakenRegMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenRegModAverage
        {
            get { return ((bool) (this["ShowBasicDamageTakenRegModAverage"])); }
            set
            {
                this["ShowBasicDamageTakenRegModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenCritHit
        {
            get { return ((bool) (this["ShowBasicDamageTakenCritHit"])); }
            set
            {
                this["ShowBasicDamageTakenCritHit"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenCritPercent
        {
            get { return ((bool) (this["ShowBasicDamageTakenCritPercent"])); }
            set
            {
                this["ShowBasicDamageTakenCritPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenCritLow
        {
            get { return ((bool) (this["ShowBasicDamageTakenCritLow"])); }
            set
            {
                this["ShowBasicDamageTakenCritLow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenCritHigh
        {
            get { return ((bool) (this["ShowBasicDamageTakenCritHigh"])); }
            set
            {
                this["ShowBasicDamageTakenCritHigh"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenCritAverage
        {
            get { return ((bool) (this["ShowBasicDamageTakenCritAverage"])); }
            set
            {
                this["ShowBasicDamageTakenCritAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenCritMod
        {
            get { return ((bool) (this["ShowBasicDamageTakenCritMod"])); }
            set
            {
                this["ShowBasicDamageTakenCritMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenCritModAverage
        {
            get { return ((bool) (this["ShowBasicDamageTakenCritModAverage"])); }
            set
            {
                this["ShowBasicDamageTakenCritModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenCounter
        {
            get { return ((bool) (this["ShowBasicDamageTakenCounter"])); }
            set
            {
                this["ShowBasicDamageTakenCounter"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenCounterPercent
        {
            get { return ((bool) (this["ShowBasicDamageTakenCounterPercent"])); }
            set
            {
                this["ShowBasicDamageTakenCounterPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenCounterMod
        {
            get { return ((bool) (this["ShowBasicDamageTakenCounterMod"])); }
            set
            {
                this["ShowBasicDamageTakenCounterMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenCounterModAverage
        {
            get { return ((bool) (this["ShowBasicDamageTakenCounterModAverage"])); }
            set
            {
                this["ShowBasicDamageTakenCounterModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenBlock
        {
            get { return ((bool) (this["ShowBasicDamageTakenBlock"])); }
            set
            {
                this["ShowBasicDamageTakenBlock"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenBlockPercent
        {
            get { return ((bool) (this["ShowBasicDamageTakenBlockPercent"])); }
            set
            {
                this["ShowBasicDamageTakenBlockPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenBlockMod
        {
            get { return ((bool) (this["ShowBasicDamageTakenBlockMod"])); }
            set
            {
                this["ShowBasicDamageTakenBlockMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenBlockModAverage
        {
            get { return ((bool) (this["ShowBasicDamageTakenBlockModAverage"])); }
            set
            {
                this["ShowBasicDamageTakenBlockModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenParry
        {
            get { return ((bool) (this["ShowBasicDamageTakenParry"])); }
            set
            {
                this["ShowBasicDamageTakenParry"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenParryPercent
        {
            get { return ((bool) (this["ShowBasicDamageTakenParryPercent"])); }
            set
            {
                this["ShowBasicDamageTakenParryPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenParryMod
        {
            get { return ((bool) (this["ShowBasicDamageTakenParryMod"])); }
            set
            {
                this["ShowBasicDamageTakenParryMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenParryModAverage
        {
            get { return ((bool) (this["ShowBasicDamageTakenParryModAverage"])); }
            set
            {
                this["ShowBasicDamageTakenParryModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenResist
        {
            get { return ((bool) (this["ShowBasicDamageTakenResist"])); }
            set
            {
                this["ShowBasicDamageTakenResist"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenResistPercent
        {
            get { return ((bool) (this["ShowBasicDamageTakenResistPercent"])); }
            set
            {
                this["ShowBasicDamageTakenResistPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenResistMod
        {
            get { return ((bool) (this["ShowBasicDamageTakenResistMod"])); }
            set
            {
                this["ShowBasicDamageTakenResistMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenResistModAverage
        {
            get { return ((bool) (this["ShowBasicDamageTakenResistModAverage"])); }
            set
            {
                this["ShowBasicDamageTakenResistModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenEvade
        {
            get { return ((bool) (this["ShowBasicDamageTakenEvade"])); }
            set
            {
                this["ShowBasicDamageTakenEvade"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenEvadePercent
        {
            get { return ((bool) (this["ShowBasicDamageTakenEvadePercent"])); }
            set
            {
                this["ShowBasicDamageTakenEvadePercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenEvadeMod
        {
            get { return ((bool) (this["ShowBasicDamageTakenEvadeMod"])); }
            set
            {
                this["ShowBasicDamageTakenEvadeMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenEvadeModAverage
        {
            get { return ((bool) (this["ShowBasicDamageTakenEvadeModAverage"])); }
            set
            {
                this["ShowBasicDamageTakenEvadeModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicPercentOfTotalOverallDamageTaken
        {
            get { return ((bool) (this["ShowBasicPercentOfTotalOverallDamageTaken"])); }
            set
            {
                this["ShowBasicPercentOfTotalOverallDamageTaken"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicPercentOfRegularDamageTaken
        {
            get { return ((bool) (this["ShowBasicPercentOfRegularDamageTaken"])); }
            set
            {
                this["ShowBasicPercentOfRegularDamageTaken"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicPercentOfCriticalDamageTaken
        {
            get { return ((bool) (this["ShowBasicPercentOfCriticalDamageTaken"])); }
            set
            {
                this["ShowBasicPercentOfCriticalDamageTaken"] = value;
                RaisePropertyChanged();
            }
        }

        #region DamageTaken Over Time

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicTotalOverallDamageTakenOverTime
        {
            get { return ((bool) (this["ShowBasicTotalOverallDamageTakenOverTime"])); }
            set
            {
                this["ShowBasicTotalOverallDamageTakenOverTime"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicRegularDamageTakenOverTime
        {
            get { return ((bool) (this["ShowBasicRegularDamageTakenOverTime"])); }
            set
            {
                this["ShowBasicRegularDamageTakenOverTime"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCriticalDamageTakenOverTime
        {
            get { return ((bool) (this["ShowBasicCriticalDamageTakenOverTime"])); }
            set
            {
                this["ShowBasicCriticalDamageTakenOverTime"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicTotalDamageTakenOverTimeActionsUsed
        {
            get { return ((bool) (this["ShowBasicTotalDamageTakenOverTimeActionsUsed"])); }
            set
            {
                this["ShowBasicTotalDamageTakenOverTimeActionsUsed"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDTOTPS
        {
            get { return ((bool) (this["ShowBasicDTOTPS"])); }
            set
            {
                this["ShowBasicDTOTPS"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenOverTimeRegHit
        {
            get { return ((bool) (this["ShowBasicDamageTakenOverTimeRegHit"])); }
            set
            {
                this["ShowBasicDamageTakenOverTimeRegHit"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenOverTimeRegMiss
        {
            get { return ((bool) (this["ShowBasicDamageTakenOverTimeRegMiss"])); }
            set
            {
                this["ShowBasicDamageTakenOverTimeRegMiss"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenOverTimeRegAccuracy
        {
            get { return ((bool) (this["ShowBasicDamageTakenOverTimeRegAccuracy"])); }
            set
            {
                this["ShowBasicDamageTakenOverTimeRegAccuracy"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenOverTimeRegLow
        {
            get { return ((bool) (this["ShowBasicDamageTakenOverTimeRegLow"])); }
            set
            {
                this["ShowBasicDamageTakenOverTimeRegLow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenOverTimeRegHigh
        {
            get { return ((bool) (this["ShowBasicDamageTakenOverTimeRegHigh"])); }
            set
            {
                this["ShowBasicDamageTakenOverTimeRegHigh"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenOverTimeRegAverage
        {
            get { return ((bool) (this["ShowBasicDamageTakenOverTimeRegAverage"])); }
            set
            {
                this["ShowBasicDamageTakenOverTimeRegAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenOverTimeRegMod
        {
            get { return ((bool) (this["ShowBasicDamageTakenOverTimeRegMod"])); }
            set
            {
                this["ShowBasicDamageTakenOverTimeRegMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenOverTimeRegModAverage
        {
            get { return ((bool) (this["ShowBasicDamageTakenOverTimeRegModAverage"])); }
            set
            {
                this["ShowBasicDamageTakenOverTimeRegModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenOverTimeCritHit
        {
            get { return ((bool) (this["ShowBasicDamageTakenOverTimeCritHit"])); }
            set
            {
                this["ShowBasicDamageTakenOverTimeCritHit"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenOverTimeCritPercent
        {
            get { return ((bool) (this["ShowBasicDamageTakenOverTimeCritPercent"])); }
            set
            {
                this["ShowBasicDamageTakenOverTimeCritPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenOverTimeCritLow
        {
            get { return ((bool) (this["ShowBasicDamageTakenOverTimeCritLow"])); }
            set
            {
                this["ShowBasicDamageTakenOverTimeCritLow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenOverTimeCritHigh
        {
            get { return ((bool) (this["ShowBasicDamageTakenOverTimeCritHigh"])); }
            set
            {
                this["ShowBasicDamageTakenOverTimeCritHigh"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenOverTimeCritAverage
        {
            get { return ((bool) (this["ShowBasicDamageTakenOverTimeCritAverage"])); }
            set
            {
                this["ShowBasicDamageTakenOverTimeCritAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenOverTimeCritMod
        {
            get { return ((bool) (this["ShowBasicDamageTakenOverTimeCritMod"])); }
            set
            {
                this["ShowBasicDamageTakenOverTimeCritMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicDamageTakenOverTimeCritModAverage
        {
            get { return ((bool) (this["ShowBasicDamageTakenOverTimeCritModAverage"])); }
            set
            {
                this["ShowBasicDamageTakenOverTimeCritModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicPercentOfTotalOverallDamageTakenOverTime
        {
            get { return ((bool) (this["ShowBasicPercentOfTotalOverallDamageTakenOverTime"])); }
            set
            {
                this["ShowBasicPercentOfTotalOverallDamageTakenOverTime"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicPercentOfRegularDamageTakenOverTime
        {
            get { return ((bool) (this["ShowBasicPercentOfRegularDamageTakenOverTime"])); }
            set
            {
                this["ShowBasicPercentOfRegularDamageTakenOverTime"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicPercentOfCriticalDamageTakenOverTime
        {
            get { return ((bool) (this["ShowBasicPercentOfCriticalDamageTakenOverTime"])); }
            set
            {
                this["ShowBasicPercentOfCriticalDamageTakenOverTime"] = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #endregion

        #region Basic Combined Settings

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedTotalOverallDamage
        {
            get { return ((bool) (this["ShowBasicCombinedTotalOverallDamage"])); }
            set
            {
                this["ShowBasicCombinedTotalOverallDamage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedRegularDamage
        {
            get { return ((bool) (this["ShowBasicCombinedRegularDamage"])); }
            set
            {
                this["ShowBasicCombinedRegularDamage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedCriticalDamage
        {
            get { return ((bool) (this["ShowBasicCombinedCriticalDamage"])); }
            set
            {
                this["ShowBasicCombinedCriticalDamage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedTotalDamageActionsUsed
        {
            get { return ((bool) (this["ShowBasicCombinedTotalDamageActionsUsed"])); }
            set
            {
                this["ShowBasicCombinedTotalDamageActionsUsed"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDPS
        {
            get { return ((bool) (this["ShowBasicCombinedDPS"])); }
            set
            {
                this["ShowBasicCombinedDPS"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageRegHit
        {
            get { return ((bool) (this["ShowBasicCombinedDamageRegHit"])); }
            set
            {
                this["ShowBasicCombinedDamageRegHit"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageRegMiss
        {
            get { return ((bool) (this["ShowBasicCombinedDamageRegMiss"])); }
            set
            {
                this["ShowBasicCombinedDamageRegMiss"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageRegAccuracy
        {
            get { return ((bool) (this["ShowBasicCombinedDamageRegAccuracy"])); }
            set
            {
                this["ShowBasicCombinedDamageRegAccuracy"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageRegLow
        {
            get { return ((bool) (this["ShowBasicCombinedDamageRegLow"])); }
            set
            {
                this["ShowBasicCombinedDamageRegLow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageRegHigh
        {
            get { return ((bool) (this["ShowBasicCombinedDamageRegHigh"])); }
            set
            {
                this["ShowBasicCombinedDamageRegHigh"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageRegAverage
        {
            get { return ((bool) (this["ShowBasicCombinedDamageRegAverage"])); }
            set
            {
                this["ShowBasicCombinedDamageRegAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageRegMod
        {
            get { return ((bool) (this["ShowBasicCombinedDamageRegMod"])); }
            set
            {
                this["ShowBasicCombinedDamageRegMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageRegModAverage
        {
            get { return ((bool) (this["ShowBasicCombinedDamageRegModAverage"])); }
            set
            {
                this["ShowBasicCombinedDamageRegModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageCritHit
        {
            get { return ((bool) (this["ShowBasicCombinedDamageCritHit"])); }
            set
            {
                this["ShowBasicCombinedDamageCritHit"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageCritPercent
        {
            get { return ((bool) (this["ShowBasicCombinedDamageCritPercent"])); }
            set
            {
                this["ShowBasicCombinedDamageCritPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageCritLow
        {
            get { return ((bool) (this["ShowBasicCombinedDamageCritLow"])); }
            set
            {
                this["ShowBasicCombinedDamageCritLow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageCritHigh
        {
            get { return ((bool) (this["ShowBasicCombinedDamageCritHigh"])); }
            set
            {
                this["ShowBasicCombinedDamageCritHigh"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageCritAverage
        {
            get { return ((bool) (this["ShowBasicCombinedDamageCritAverage"])); }
            set
            {
                this["ShowBasicCombinedDamageCritAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageCritMod
        {
            get { return ((bool) (this["ShowBasicCombinedDamageCritMod"])); }
            set
            {
                this["ShowBasicCombinedDamageCritMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageCritModAverage
        {
            get { return ((bool) (this["ShowBasicCombinedDamageCritModAverage"])); }
            set
            {
                this["ShowBasicCombinedDamageCritModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageCounter
        {
            get { return ((bool) (this["ShowBasicCombinedDamageCounter"])); }
            set
            {
                this["ShowBasicCombinedDamageCounter"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageCounterPercent
        {
            get { return ((bool) (this["ShowBasicCombinedDamageCounterPercent"])); }
            set
            {
                this["ShowBasicCombinedDamageCounterPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageCounterMod
        {
            get { return ((bool) (this["ShowBasicCombinedDamageCounterMod"])); }
            set
            {
                this["ShowBasicCombinedDamageCounterMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageCounterModAverage
        {
            get { return ((bool) (this["ShowBasicCombinedDamageCounterModAverage"])); }
            set
            {
                this["ShowBasicCombinedDamageCounterModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageBlock
        {
            get { return ((bool) (this["ShowBasicCombinedDamageBlock"])); }
            set
            {
                this["ShowBasicCombinedDamageBlock"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageBlockPercent
        {
            get { return ((bool) (this["ShowBasicCombinedDamageBlockPercent"])); }
            set
            {
                this["ShowBasicCombinedDamageBlockPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageBlockMod
        {
            get { return ((bool) (this["ShowBasicCombinedDamageBlockMod"])); }
            set
            {
                this["ShowBasicCombinedDamageBlockMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageBlockModAverage
        {
            get { return ((bool) (this["ShowBasicCombinedDamageBlockModAverage"])); }
            set
            {
                this["ShowBasicCombinedDamageBlockModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageParry
        {
            get { return ((bool) (this["ShowBasicCombinedDamageParry"])); }
            set
            {
                this["ShowBasicCombinedDamageParry"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageParryPercent
        {
            get { return ((bool) (this["ShowBasicCombinedDamageParryPercent"])); }
            set
            {
                this["ShowBasicCombinedDamageParryPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageParryMod
        {
            get { return ((bool) (this["ShowBasicCombinedDamageParryMod"])); }
            set
            {
                this["ShowBasicCombinedDamageParryMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageParryModAverage
        {
            get { return ((bool) (this["ShowBasicCombinedDamageParryModAverage"])); }
            set
            {
                this["ShowBasicCombinedDamageParryModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageResist
        {
            get { return ((bool) (this["ShowBasicCombinedDamageResist"])); }
            set
            {
                this["ShowBasicCombinedDamageResist"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageResistPercent
        {
            get { return ((bool) (this["ShowBasicCombinedDamageResistPercent"])); }
            set
            {
                this["ShowBasicCombinedDamageResistPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageResistMod
        {
            get { return ((bool) (this["ShowBasicCombinedDamageResistMod"])); }
            set
            {
                this["ShowBasicCombinedDamageResistMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageResistModAverage
        {
            get { return ((bool) (this["ShowBasicCombinedDamageResistModAverage"])); }
            set
            {
                this["ShowBasicCombinedDamageResistModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageEvade
        {
            get { return ((bool) (this["ShowBasicCombinedDamageEvade"])); }
            set
            {
                this["ShowBasicCombinedDamageEvade"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageEvadePercent
        {
            get { return ((bool) (this["ShowBasicCombinedDamageEvadePercent"])); }
            set
            {
                this["ShowBasicCombinedDamageEvadePercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageEvadeMod
        {
            get { return ((bool) (this["ShowBasicCombinedDamageEvadeMod"])); }
            set
            {
                this["ShowBasicCombinedDamageEvadeMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageEvadeModAverage
        {
            get { return ((bool) (this["ShowBasicCombinedDamageEvadeModAverage"])); }
            set
            {
                this["ShowBasicCombinedDamageEvadeModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedPercentOfTotalOverallDamage
        {
            get { return ((bool) (this["ShowBasicCombinedPercentOfTotalOverallDamage"])); }
            set
            {
                this["ShowBasicCombinedPercentOfTotalOverallDamage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedPercentOfRegularDamage
        {
            get { return ((bool) (this["ShowBasicCombinedPercentOfRegularDamage"])); }
            set
            {
                this["ShowBasicCombinedPercentOfRegularDamage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedPercentOfCriticalDamage
        {
            get { return ((bool) (this["ShowBasicCombinedPercentOfCriticalDamage"])); }
            set
            {
                this["ShowBasicCombinedPercentOfCriticalDamage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedTotalOverallHealing
        {
            get { return ((bool) (this["ShowBasicCombinedTotalOverallHealing"])); }
            set
            {
                this["ShowBasicCombinedTotalOverallHealing"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedRegularHealing
        {
            get { return ((bool) (this["ShowBasicCombinedRegularHealing"])); }
            set
            {
                this["ShowBasicCombinedRegularHealing"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedCriticalHealing
        {
            get { return ((bool) (this["ShowBasicCombinedCriticalHealing"])); }
            set
            {
                this["ShowBasicCombinedCriticalHealing"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedTotalHealingActionsUsed
        {
            get { return ((bool) (this["ShowBasicCombinedTotalHealingActionsUsed"])); }
            set
            {
                this["ShowBasicCombinedTotalHealingActionsUsed"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedHPS
        {
            get { return ((bool) (this["ShowBasicCombinedHPS"])); }
            set
            {
                this["ShowBasicCombinedHPS"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedHealingRegHit
        {
            get { return ((bool) (this["ShowBasicCombinedHealingRegHit"])); }
            set
            {
                this["ShowBasicCombinedHealingRegHit"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedHealingRegLow
        {
            get { return ((bool) (this["ShowBasicCombinedHealingRegLow"])); }
            set
            {
                this["ShowBasicCombinedHealingRegLow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedHealingRegHigh
        {
            get { return ((bool) (this["ShowBasicCombinedHealingRegHigh"])); }
            set
            {
                this["ShowBasicCombinedHealingRegHigh"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedHealingRegAverage
        {
            get { return ((bool) (this["ShowBasicCombinedHealingRegAverage"])); }
            set
            {
                this["ShowBasicCombinedHealingRegAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedHealingRegMod
        {
            get { return ((bool) (this["ShowBasicCombinedHealingRegMod"])); }
            set
            {
                this["ShowBasicCombinedHealingRegMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedHealingRegModAverage
        {
            get { return ((bool) (this["ShowBasicCombinedHealingRegModAverage"])); }
            set
            {
                this["ShowBasicCombinedHealingRegModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedHealingCritHit
        {
            get { return ((bool) (this["ShowBasicCombinedHealingCritHit"])); }
            set
            {
                this["ShowBasicCombinedHealingCritHit"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedHealingCritPercent
        {
            get { return ((bool) (this["ShowBasicCombinedHealingCritPercent"])); }
            set
            {
                this["ShowBasicCombinedHealingCritPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedHealingCritLow
        {
            get { return ((bool) (this["ShowBasicCombinedHealingCritLow"])); }
            set
            {
                this["ShowBasicCombinedHealingCritLow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedHealingCritHigh
        {
            get { return ((bool) (this["ShowBasicCombinedHealingCritHigh"])); }
            set
            {
                this["ShowBasicCombinedHealingCritHigh"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedHealingCritAverage
        {
            get { return ((bool) (this["ShowBasicCombinedHealingCritAverage"])); }
            set
            {
                this["ShowBasicCombinedHealingCritAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedHealingCritMod
        {
            get { return ((bool) (this["ShowBasicCombinedHealingCritMod"])); }
            set
            {
                this["ShowBasicCombinedHealingCritMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedHealingCritModAverage
        {
            get { return ((bool) (this["ShowBasicCombinedHealingCritModAverage"])); }
            set
            {
                this["ShowBasicCombinedHealingCritModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedPercentOfTotalOverallHealing
        {
            get { return ((bool) (this["ShowBasicCombinedPercentOfTotalOverallHealing"])); }
            set
            {
                this["ShowBasicCombinedPercentOfTotalOverallHealing"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedPercentOfRegularHealing
        {
            get { return ((bool) (this["ShowBasicCombinedPercentOfRegularHealing"])); }
            set
            {
                this["ShowBasicCombinedPercentOfRegularHealing"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedPercentOfCriticalHealing
        {
            get { return ((bool) (this["ShowBasicCombinedPercentOfCriticalHealing"])); }
            set
            {
                this["ShowBasicCombinedPercentOfCriticalHealing"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedTotalOverallDamageTaken
        {
            get { return ((bool) (this["ShowBasicCombinedTotalOverallDamageTaken"])); }
            set
            {
                this["ShowBasicCombinedTotalOverallDamageTaken"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedRegularDamageTaken
        {
            get { return ((bool) (this["ShowBasicCombinedRegularDamageTaken"])); }
            set
            {
                this["ShowBasicCombinedRegularDamageTaken"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedCriticalDamageTaken
        {
            get { return ((bool) (this["ShowBasicCombinedCriticalDamageTaken"])); }
            set
            {
                this["ShowBasicCombinedCriticalDamageTaken"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedTotalDamageTakenActionsUsed
        {
            get { return ((bool) (this["ShowBasicCombinedTotalDamageTakenActionsUsed"])); }
            set
            {
                this["ShowBasicCombinedTotalDamageTakenActionsUsed"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDTPS
        {
            get { return ((bool) (this["ShowBasicCombinedDTPS"])); }
            set
            {
                this["ShowBasicCombinedDTPS"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenRegHit
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenRegHit"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenRegHit"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenRegMiss
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenRegMiss"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenRegMiss"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenRegAccuracy
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenRegAccuracy"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenRegAccuracy"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenRegLow
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenRegLow"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenRegLow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenRegHigh
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenRegHigh"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenRegHigh"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenRegAverage
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenRegAverage"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenRegAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenRegMod
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenRegMod"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenRegMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenRegModAverage
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenRegModAverage"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenRegModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenCritHit
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenCritHit"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenCritHit"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenCritPercent
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenCritPercent"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenCritPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenCritLow
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenCritLow"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenCritLow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenCritHigh
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenCritHigh"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenCritHigh"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenCritAverage
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenCritAverage"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenCritAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenCritMod
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenCritMod"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenCritMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenCritModAverage
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenCritModAverage"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenCritModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenCounter
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenCounter"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenCounter"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenCounterPercent
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenCounterPercent"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenCounterPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenCounterMod
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenCounterMod"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenCounterMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenCounterModAverage
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenCounterModAverage"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenCounterModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenBlock
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenBlock"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenBlock"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenBlockPercent
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenBlockPercent"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenBlockPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenBlockMod
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenBlockMod"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenBlockMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenBlockModAverage
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenBlockModAverage"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenBlockModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenParry
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenParry"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenParry"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenParryPercent
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenParryPercent"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenParryPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenParryMod
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenParryMod"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenParryMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenParryModAverage
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenParryModAverage"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenParryModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenResist
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenResist"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenResist"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenResistPercent
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenResistPercent"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenResistPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenResistMod
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenResistMod"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenResistMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenResistModAverage
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenResistModAverage"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenResistModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenEvade
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenEvade"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenEvade"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenEvadePercent
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenEvadePercent"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenEvadePercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenEvadeMod
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenEvadeMod"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenEvadeMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedDamageTakenEvadeModAverage
        {
            get { return ((bool) (this["ShowBasicCombinedDamageTakenEvadeModAverage"])); }
            set
            {
                this["ShowBasicCombinedDamageTakenEvadeModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedPercentOfTotalOverallDamageTaken
        {
            get { return ((bool) (this["ShowBasicCombinedPercentOfTotalOverallDamageTaken"])); }
            set
            {
                this["ShowBasicCombinedPercentOfTotalOverallDamageTaken"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedPercentOfRegularDamageTaken
        {
            get { return ((bool) (this["ShowBasicCombinedPercentOfRegularDamageTaken"])); }
            set
            {
                this["ShowBasicCombinedPercentOfRegularDamageTaken"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowBasicCombinedPercentOfCriticalDamageTaken
        {
            get { return ((bool) (this["ShowBasicCombinedPercentOfCriticalDamageTaken"])); }
            set
            {
                this["ShowBasicCombinedPercentOfCriticalDamageTaken"] = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Column Settings

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnTotalOverallDamage
        {
            get { return ((bool) (this["ShowColumnTotalOverallDamage"])); }
            set
            {
                this["ShowColumnTotalOverallDamage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnRegularDamage
        {
            get { return ((bool) (this["ShowColumnRegularDamage"])); }
            set
            {
                this["ShowColumnRegularDamage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnCriticalDamage
        {
            get { return ((bool) (this["ShowColumnCriticalDamage"])); }
            set
            {
                this["ShowColumnCriticalDamage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnTotalDamageActionsUsed
        {
            get { return ((bool) (this["ShowColumnTotalDamageActionsUsed"])); }
            set
            {
                this["ShowColumnTotalDamageActionsUsed"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDPS
        {
            get { return ((bool) (this["ShowColumnDPS"])); }
            set
            {
                this["ShowColumnDPS"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageRegHit
        {
            get { return ((bool) (this["ShowColumnDamageRegHit"])); }
            set
            {
                this["ShowColumnDamageRegHit"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageRegMiss
        {
            get { return ((bool) (this["ShowColumnDamageRegMiss"])); }
            set
            {
                this["ShowColumnDamageRegMiss"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageRegAccuracy
        {
            get { return ((bool) (this["ShowColumnDamageRegAccuracy"])); }
            set
            {
                this["ShowColumnDamageRegAccuracy"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageRegLow
        {
            get { return ((bool) (this["ShowColumnDamageRegLow"])); }
            set
            {
                this["ShowColumnDamageRegLow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageRegHigh
        {
            get { return ((bool) (this["ShowColumnDamageRegHigh"])); }
            set
            {
                this["ShowColumnDamageRegHigh"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageRegAverage
        {
            get { return ((bool) (this["ShowColumnDamageRegAverage"])); }
            set
            {
                this["ShowColumnDamageRegAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageRegMod
        {
            get { return ((bool) (this["ShowColumnDamageRegMod"])); }
            set
            {
                this["ShowColumnDamageRegMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageRegModAverage
        {
            get { return ((bool) (this["ShowColumnDamageRegModAverage"])); }
            set
            {
                this["ShowColumnDamageRegModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageCritHit
        {
            get { return ((bool) (this["ShowColumnDamageCritHit"])); }
            set
            {
                this["ShowColumnDamageCritHit"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageCritPercent
        {
            get { return ((bool) (this["ShowColumnDamageCritPercent"])); }
            set
            {
                this["ShowColumnDamageCritPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageCritLow
        {
            get { return ((bool) (this["ShowColumnDamageCritLow"])); }
            set
            {
                this["ShowColumnDamageCritLow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageCritHigh
        {
            get { return ((bool) (this["ShowColumnDamageCritHigh"])); }
            set
            {
                this["ShowColumnDamageCritHigh"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageCritAverage
        {
            get { return ((bool) (this["ShowColumnDamageCritAverage"])); }
            set
            {
                this["ShowColumnDamageCritAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageCritMod
        {
            get { return ((bool) (this["ShowColumnDamageCritMod"])); }
            set
            {
                this["ShowColumnDamageCritMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageCritModAverage
        {
            get { return ((bool) (this["ShowColumnDamageCritModAverage"])); }
            set
            {
                this["ShowColumnDamageCritModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageCounter
        {
            get { return ((bool) (this["ShowColumnDamageCounter"])); }
            set
            {
                this["ShowColumnDamageCounter"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageCounterPercent
        {
            get { return ((bool) (this["ShowColumnDamageCounterPercent"])); }
            set
            {
                this["ShowColumnDamageCounterPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageCounterMod
        {
            get { return ((bool) (this["ShowColumnDamageCounterMod"])); }
            set
            {
                this["ShowColumnDamageCounterMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageCounterModAverage
        {
            get { return ((bool) (this["ShowColumnDamageCounterModAverage"])); }
            set
            {
                this["ShowColumnDamageCounterModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageBlock
        {
            get { return ((bool) (this["ShowColumnDamageBlock"])); }
            set
            {
                this["ShowColumnDamageBlock"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageBlockPercent
        {
            get { return ((bool) (this["ShowColumnDamageBlockPercent"])); }
            set
            {
                this["ShowColumnDamageBlockPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageBlockMod
        {
            get { return ((bool) (this["ShowColumnDamageBlockMod"])); }
            set
            {
                this["ShowColumnDamageBlockMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageBlockModAverage
        {
            get { return ((bool) (this["ShowColumnDamageBlockModAverage"])); }
            set
            {
                this["ShowColumnDamageBlockModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageParry
        {
            get { return ((bool) (this["ShowColumnDamageParry"])); }
            set
            {
                this["ShowColumnDamageParry"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageParryPercent
        {
            get { return ((bool) (this["ShowColumnDamageParryPercent"])); }
            set
            {
                this["ShowColumnDamageParryPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageParryMod
        {
            get { return ((bool) (this["ShowColumnDamageParryMod"])); }
            set
            {
                this["ShowColumnDamageParryMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageParryModAverage
        {
            get { return ((bool) (this["ShowColumnDamageParryModAverage"])); }
            set
            {
                this["ShowColumnDamageParryModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageResist
        {
            get { return ((bool) (this["ShowColumnDamageResist"])); }
            set
            {
                this["ShowColumnDamageResist"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageResistPercent
        {
            get { return ((bool) (this["ShowColumnDamageResistPercent"])); }
            set
            {
                this["ShowColumnDamageResistPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageResistMod
        {
            get { return ((bool) (this["ShowColumnDamageResistMod"])); }
            set
            {
                this["ShowColumnDamageResistMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageResistModAverage
        {
            get { return ((bool) (this["ShowColumnDamageResistModAverage"])); }
            set
            {
                this["ShowColumnDamageResistModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageEvade
        {
            get { return ((bool) (this["ShowColumnDamageEvade"])); }
            set
            {
                this["ShowColumnDamageEvade"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageEvadePercent
        {
            get { return ((bool) (this["ShowColumnDamageEvadePercent"])); }
            set
            {
                this["ShowColumnDamageEvadePercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageEvadeMod
        {
            get { return ((bool) (this["ShowColumnDamageEvadeMod"])); }
            set
            {
                this["ShowColumnDamageEvadeMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageEvadeModAverage
        {
            get { return ((bool) (this["ShowColumnDamageEvadeModAverage"])); }
            set
            {
                this["ShowColumnDamageEvadeModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnPercentOfTotalOverallDamage
        {
            get { return ((bool) (this["ShowColumnPercentOfTotalOverallDamage"])); }
            set
            {
                this["ShowColumnPercentOfTotalOverallDamage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnPercentOfRegularDamage
        {
            get { return ((bool) (this["ShowColumnPercentOfRegularDamage"])); }
            set
            {
                this["ShowColumnPercentOfRegularDamage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnPercentOfCriticalDamage
        {
            get { return ((bool) (this["ShowColumnPercentOfCriticalDamage"])); }
            set
            {
                this["ShowColumnPercentOfCriticalDamage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnTotalOverallHealing
        {
            get { return ((bool) (this["ShowColumnTotalOverallHealing"])); }
            set
            {
                this["ShowColumnTotalOverallHealing"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnRegularHealing
        {
            get { return ((bool) (this["ShowColumnRegularHealing"])); }
            set
            {
                this["ShowColumnRegularHealing"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnCriticalHealing
        {
            get { return ((bool) (this["ShowColumnCriticalHealing"])); }
            set
            {
                this["ShowColumnCriticalHealing"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnTotalHealingActionsUsed
        {
            get { return ((bool) (this["ShowColumnTotalHealingActionsUsed"])); }
            set
            {
                this["ShowColumnTotalHealingActionsUsed"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnHPS
        {
            get { return ((bool) (this["ShowColumnHPS"])); }
            set
            {
                this["ShowColumnHPS"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnHealingRegHit
        {
            get { return ((bool) (this["ShowColumnHealingRegHit"])); }
            set
            {
                this["ShowColumnHealingRegHit"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnHealingRegLow
        {
            get { return ((bool) (this["ShowColumnHealingRegLow"])); }
            set
            {
                this["ShowColumnHealingRegLow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnHealingRegHigh
        {
            get { return ((bool) (this["ShowColumnHealingRegHigh"])); }
            set
            {
                this["ShowColumnHealingRegHigh"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnHealingRegAverage
        {
            get { return ((bool) (this["ShowColumnHealingRegAverage"])); }
            set
            {
                this["ShowColumnHealingRegAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnHealingRegMod
        {
            get { return ((bool) (this["ShowColumnHealingRegMod"])); }
            set
            {
                this["ShowColumnHealingRegMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnHealingRegModAverage
        {
            get { return ((bool) (this["ShowColumnHealingRegModAverage"])); }
            set
            {
                this["ShowColumnHealingRegModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnHealingCritHit
        {
            get { return ((bool) (this["ShowColumnHealingCritHit"])); }
            set
            {
                this["ShowColumnHealingCritHit"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnHealingCritPercent
        {
            get { return ((bool) (this["ShowColumnHealingCritPercent"])); }
            set
            {
                this["ShowColumnHealingCritPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnHealingCritLow
        {
            get { return ((bool) (this["ShowColumnHealingCritLow"])); }
            set
            {
                this["ShowColumnHealingCritLow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnHealingCritHigh
        {
            get { return ((bool) (this["ShowColumnHealingCritHigh"])); }
            set
            {
                this["ShowColumnHealingCritHigh"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnHealingCritAverage
        {
            get { return ((bool) (this["ShowColumnHealingCritAverage"])); }
            set
            {
                this["ShowColumnHealingCritAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnHealingCritMod
        {
            get { return ((bool) (this["ShowColumnHealingCritMod"])); }
            set
            {
                this["ShowColumnHealingCritMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnHealingCritModAverage
        {
            get { return ((bool) (this["ShowColumnHealingCritModAverage"])); }
            set
            {
                this["ShowColumnHealingCritModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnPercentOfTotalOverallHealing
        {
            get { return ((bool) (this["ShowColumnPercentOfTotalOverallHealing"])); }
            set
            {
                this["ShowColumnPercentOfTotalOverallHealing"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnPercentOfRegularHealing
        {
            get { return ((bool) (this["ShowColumnPercentOfRegularHealing"])); }
            set
            {
                this["ShowColumnPercentOfRegularHealing"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnPercentOfCriticalHealing
        {
            get { return ((bool) (this["ShowColumnPercentOfCriticalHealing"])); }
            set
            {
                this["ShowColumnPercentOfCriticalHealing"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnTotalOverallDamageTaken
        {
            get { return ((bool) (this["ShowColumnTotalOverallDamageTaken"])); }
            set
            {
                this["ShowColumnTotalOverallDamageTaken"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnRegularDamageTaken
        {
            get { return ((bool) (this["ShowColumnRegularDamageTaken"])); }
            set
            {
                this["ShowColumnRegularDamageTaken"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnCriticalDamageTaken
        {
            get { return ((bool) (this["ShowColumnCriticalDamageTaken"])); }
            set
            {
                this["ShowColumnCriticalDamageTaken"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnTotalDamageTakenActionsUsed
        {
            get { return ((bool) (this["ShowColumnTotalDamageTakenActionsUsed"])); }
            set
            {
                this["ShowColumnTotalDamageTakenActionsUsed"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDTPS
        {
            get { return ((bool) (this["ShowColumnDTPS"])); }
            set
            {
                this["ShowColumnDTPS"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenRegHit
        {
            get { return ((bool) (this["ShowColumnDamageTakenRegHit"])); }
            set
            {
                this["ShowColumnDamageTakenRegHit"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenRegMiss
        {
            get { return ((bool) (this["ShowColumnDamageTakenRegMiss"])); }
            set
            {
                this["ShowColumnDamageTakenRegMiss"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenRegAccuracy
        {
            get { return ((bool) (this["ShowColumnDamageTakenRegAccuracy"])); }
            set
            {
                this["ShowColumnDamageTakenRegAccuracy"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenRegLow
        {
            get { return ((bool) (this["ShowColumnDamageTakenRegLow"])); }
            set
            {
                this["ShowColumnDamageTakenRegLow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenRegHigh
        {
            get { return ((bool) (this["ShowColumnDamageTakenRegHigh"])); }
            set
            {
                this["ShowColumnDamageTakenRegHigh"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenRegAverage
        {
            get { return ((bool) (this["ShowColumnDamageTakenRegAverage"])); }
            set
            {
                this["ShowColumnDamageTakenRegAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenRegMod
        {
            get { return ((bool) (this["ShowColumnDamageTakenRegMod"])); }
            set
            {
                this["ShowColumnDamageTakenRegMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenRegModAverage
        {
            get { return ((bool) (this["ShowColumnDamageTakenRegModAverage"])); }
            set
            {
                this["ShowColumnDamageTakenRegModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenCritHit
        {
            get { return ((bool) (this["ShowColumnDamageTakenCritHit"])); }
            set
            {
                this["ShowColumnDamageTakenCritHit"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenCritPercent
        {
            get { return ((bool) (this["ShowColumnDamageTakenCritPercent"])); }
            set
            {
                this["ShowColumnDamageTakenCritPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenCritLow
        {
            get { return ((bool) (this["ShowColumnDamageTakenCritLow"])); }
            set
            {
                this["ShowColumnDamageTakenCritLow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenCritHigh
        {
            get { return ((bool) (this["ShowColumnDamageTakenCritHigh"])); }
            set
            {
                this["ShowColumnDamageTakenCritHigh"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenCritAverage
        {
            get { return ((bool) (this["ShowColumnDamageTakenCritAverage"])); }
            set
            {
                this["ShowColumnDamageTakenCritAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenCritMod
        {
            get { return ((bool) (this["ShowColumnDamageTakenCritMod"])); }
            set
            {
                this["ShowColumnDamageTakenCritMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenCritModAverage
        {
            get { return ((bool) (this["ShowColumnDamageTakenCritModAverage"])); }
            set
            {
                this["ShowColumnDamageTakenCritModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenCounter
        {
            get { return ((bool) (this["ShowColumnDamageTakenCounter"])); }
            set
            {
                this["ShowColumnDamageTakenCounter"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenCounterPercent
        {
            get { return ((bool) (this["ShowColumnDamageTakenCounterPercent"])); }
            set
            {
                this["ShowColumnDamageTakenCounterPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenCounterMod
        {
            get { return ((bool) (this["ShowColumnDamageTakenCounterMod"])); }
            set
            {
                this["ShowColumnDamageTakenCounterMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenCounterModAverage
        {
            get { return ((bool) (this["ShowColumnDamageTakenCounterModAverage"])); }
            set
            {
                this["ShowColumnDamageTakenCounterModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenBlock
        {
            get { return ((bool) (this["ShowColumnDamageTakenBlock"])); }
            set
            {
                this["ShowColumnDamageTakenBlock"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenBlockPercent
        {
            get { return ((bool) (this["ShowColumnDamageTakenBlockPercent"])); }
            set
            {
                this["ShowColumnDamageTakenBlockPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenBlockMod
        {
            get { return ((bool) (this["ShowColumnDamageTakenBlockMod"])); }
            set
            {
                this["ShowColumnDamageTakenBlockMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenBlockModAverage
        {
            get { return ((bool) (this["ShowColumnDamageTakenBlockModAverage"])); }
            set
            {
                this["ShowColumnDamageTakenBlockModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenParry
        {
            get { return ((bool) (this["ShowColumnDamageTakenParry"])); }
            set
            {
                this["ShowColumnDamageTakenParry"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenParryPercent
        {
            get { return ((bool) (this["ShowColumnDamageTakenParryPercent"])); }
            set
            {
                this["ShowColumnDamageTakenParryPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenParryMod
        {
            get { return ((bool) (this["ShowColumnDamageTakenParryMod"])); }
            set
            {
                this["ShowColumnDamageTakenParryMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenParryModAverage
        {
            get { return ((bool) (this["ShowColumnDamageTakenParryModAverage"])); }
            set
            {
                this["ShowColumnDamageTakenParryModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenResist
        {
            get { return ((bool) (this["ShowColumnDamageTakenResist"])); }
            set
            {
                this["ShowColumnDamageTakenResist"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenResistPercent
        {
            get { return ((bool) (this["ShowColumnDamageTakenResistPercent"])); }
            set
            {
                this["ShowColumnDamageTakenResistPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenResistMod
        {
            get { return ((bool) (this["ShowColumnDamageTakenResistMod"])); }
            set
            {
                this["ShowColumnDamageTakenResistMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenResistModAverage
        {
            get { return ((bool) (this["ShowColumnDamageTakenResistModAverage"])); }
            set
            {
                this["ShowColumnDamageTakenResistModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenEvade
        {
            get { return ((bool) (this["ShowColumnDamageTakenEvade"])); }
            set
            {
                this["ShowColumnDamageTakenEvade"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenEvadePercent
        {
            get { return ((bool) (this["ShowColumnDamageTakenEvadePercent"])); }
            set
            {
                this["ShowColumnDamageTakenEvadePercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenEvadeMod
        {
            get { return ((bool) (this["ShowColumnDamageTakenEvadeMod"])); }
            set
            {
                this["ShowColumnDamageTakenEvadeMod"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnDamageTakenEvadeModAverage
        {
            get { return ((bool) (this["ShowColumnDamageTakenEvadeModAverage"])); }
            set
            {
                this["ShowColumnDamageTakenEvadeModAverage"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnPercentOfTotalOverallDamageTaken
        {
            get { return ((bool) (this["ShowColumnPercentOfTotalOverallDamageTaken"])); }
            set
            {
                this["ShowColumnPercentOfTotalOverallDamageTaken"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnPercentOfRegularDamageTaken
        {
            get { return ((bool) (this["ShowColumnPercentOfRegularDamageTaken"])); }
            set
            {
                this["ShowColumnPercentOfRegularDamageTaken"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowColumnPercentOfCriticalDamageTaken
        {
            get { return ((bool) (this["ShowColumnPercentOfCriticalDamageTaken"])); }
            set
            {
                this["ShowColumnPercentOfCriticalDamageTaken"] = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ParseYou
        {
            get { return ((bool) (this["ParseYou"])); }
            set
            {
                this["ParseYou"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ParseParty
        {
            get { return ((bool) (this["ParseParty"])); }
            set
            {
                this["ParseParty"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ParseAlliance
        {
            get { return ((bool) (this["ParseAlliance"])); }
            set
            {
                this["ParseAlliance"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ParseOther
        {
            get { return ((bool) (this["ParseOther"])); }
            set
            {
                this["ParseOther"] = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public new event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion

        #region Iterative Settings Saving

        private void SaveSettingsNode()
        {
            if (Constants.Parse.XSettings == null)
            {
                return;
            }
            var xElements = Constants.Parse.XSettings.Descendants()
                                     .Elements("Setting");
            var enumerable = xElements as XElement[] ?? xElements.ToArray();
            foreach (var setting in Constants.Parse.Settings)
            {
                var element = enumerable.FirstOrDefault(e => e.Attribute("Key")
                                                              .Value == setting);
                if (element == null)
                {
                    var xKey = setting;
                    var xValue = Default[xKey].ToString();
                    var keyPairList = new List<XValuePair>
                    {
                        new XValuePair
                        {
                            Key = "Value",
                            Value = xValue
                        }
                    };
                    XmlHelper.SaveXmlNode(Constants.Parse.XSettings, "Settings", "Setting", xKey, keyPairList);
                }
                else
                {
                    var xElement = element.Element("Value");
                    if (xElement != null)
                    {
                        xElement.Value = Default[setting].ToString();
                    }
                }
            }
        }

        #endregion
    }
}