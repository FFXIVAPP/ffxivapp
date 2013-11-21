// FFXIVAPP.Client
// Settings.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.Utilities;
using NLog;
using ColorConverter = System.Windows.Media.ColorConverter;
using FontFamily = System.Drawing.FontFamily;

#endregion

namespace FFXIVAPP.Client.SettingsProviders.Parse
{
    public class Settings : ApplicationSettingsBase, INotifyPropertyChanged
    {
        private static Settings _default;

        public static Settings Default
        {
            get { return _default ?? (_default = ((Settings) (Synchronized(new Settings())))); }
        }

        public override void Save()
        {
            XmlHelper.DeleteXmlNode(Constants.Parse.XSettings, "Setting");
            DefaultSettings();
            foreach (var item in Constants.Parse.Settings)
            {
                try
                {
                    var xKey = item;
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
                catch (Exception ex)
                {
                    Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
                }
            }
            Constants.Parse.XSettings.Save(AppViewModel.Instance.SettingsPath + "Settings.Parse.xml");
        }

        private void DefaultSettings()
        {
            Constants.Parse.Settings.Clear();
            Constants.Parse.Settings.Add("StoreHistoryInterval");
            Constants.Parse.Settings.Add("EnableStoreHistoryReset");
            Constants.Parse.Settings.Add("IgnoreLimitBreaks");
            Constants.Parse.Settings.Add("ShowActionLogTab");
            Constants.Parse.Settings.Add("ShowPartyDamageTab");
            Constants.Parse.Settings.Add("ShowPartyHealingTab");
            Constants.Parse.Settings.Add("ShowPartyDamageTakenTab");
            Constants.Parse.Settings.Add("ShowMonsterDamageTab");
            Constants.Parse.Settings.Add("ShowMonsterHealingTab");
            Constants.Parse.Settings.Add("ShowMonsterDamageTakenTab");
            Constants.Parse.Settings.Add("PlayerDamageByAction");
            Constants.Parse.Settings.Add("PlayerDamageToMonsters");
            Constants.Parse.Settings.Add("PlayerDamageToMonstersByAction");
            Constants.Parse.Settings.Add("PlayerHealingByAction");
            Constants.Parse.Settings.Add("PlayerHealingToPlayers");
            Constants.Parse.Settings.Add("PlayerHealingToPlayersByAction");
            Constants.Parse.Settings.Add("PlayerDamageTakenByAction");
            Constants.Parse.Settings.Add("PlayerDamageTakenByMonsters");
            Constants.Parse.Settings.Add("PlayerDamageTakenByMonstersByAction");
            Constants.Parse.Settings.Add("MonsterDamageByAction");
            Constants.Parse.Settings.Add("MonsterDamageToPlayers");
            Constants.Parse.Settings.Add("MonsterDamageToPlayersByAction");
            Constants.Parse.Settings.Add("MonsterHealingByAction");
            Constants.Parse.Settings.Add("MonsterHealingToMonsters");
            Constants.Parse.Settings.Add("MonsterHealingToMonstersByAction");
            Constants.Parse.Settings.Add("MonsterDamageTakenByAction");
            Constants.Parse.Settings.Add("MonsterDamageTakenByPlayers");
            Constants.Parse.Settings.Add("MonsterDamageTakenByPlayersByAction");
            Constants.Parse.Settings.Add("MonsterDrops");

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
                SetValue(key, value);
            }
        }

        public static void SetValue(string key, string value)
        {
            try
            {
                var type = Default[key].GetType()
                                       .Name;
                switch (type)
                {
                    case "Boolean":
                        Default[key] = Convert.ToBoolean(value);
                        break;
                    case "Color":
                        var cc = new ColorConverter();
                        var color = cc.ConvertFrom(value);
                        Default[key] = color ?? Colors.Black;
                        break;
                    case "Double":
                        Default[key] = Convert.ToDouble(value);
                        break;
                    case "Font":
                        var fc = new FontConverter();
                        var font = fc.ConvertFromString(value);
                        Default[key] = font ?? new Font(new FontFamily("Microsoft Sans Serif"), 12);
                        break;
                    default:
                        Default[key] = value;
                        break;
                }
            }
            catch (SettingsPropertyNotFoundException ex)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
            }
            catch (SettingsPropertyWrongTypeException ex)
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
        [DefaultSettingValue("True")]
        public bool EnableStoreHistoryReset
        {
            get { return ((bool)(this["EnableStoreHistoryReset"])); }
            set
            {
                this["EnableStoreHistoryReset"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool IgnoreLimitBreaks
        {
            get { return ((bool)(this["IgnoreLimitBreaks"])); }
            set
            {
                this["IgnoreLimitBreaks"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowActionLogTab
        {
            get { return ((bool) (this["ShowActionLogTab"])); }
            set
            {
                this["ShowActionLogTab"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowPartyDamageTab
        {
            get { return ((bool) (this["ShowPartyDamageTab"])); }
            set
            {
                this["ShowPartyDamageTab"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowPartyHealingTab
        {
            get { return ((bool) (this["ShowPartyHealingTab"])); }
            set
            {
                this["ShowPartyHealingTab"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowPartyDamageTakenTab
        {
            get { return ((bool) (this["ShowPartyDamageTakenTab"])); }
            set
            {
                this["ShowPartyDamageTakenTab"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowMonsterDamageTab
        {
            get { return ((bool) (this["ShowMonsterDamageTab"])); }
            set
            {
                this["ShowMonsterDamageTab"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowMonsterHealingTab
        {
            get { return ((bool) (this["ShowMonsterHealingTab"])); }
            set
            {
                this["ShowMonsterHealingTab"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowMonsterDamageTakenTab
        {
            get { return ((bool) (this["ShowMonsterDamageTakenTab"])); }
            set
            {
                this["ShowMonsterDamageTakenTab"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool PlayerDamageByAction
        {
            get { return ((bool) (this["PlayerDamageByAction"])); }
            set
            {
                this["PlayerDamageByAction"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool PlayerDamageToMonsters
        {
            get { return ((bool) (this["PlayerDamageToMonsters"])); }
            set
            {
                this["PlayerDamageToMonsters"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool PlayerDamageToMonstersByAction
        {
            get { return ((bool) (this["PlayerDamageToMonstersByAction"])); }
            set
            {
                this["PlayerDamageToMonstersByAction"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool PlayerHealingByAction
        {
            get { return ((bool) (this["PlayerHealingByAction"])); }
            set
            {
                this["PlayerHealingByAction"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool PlayerHealingToPlayers
        {
            get { return ((bool) (this["PlayerHealingToPlayers"])); }
            set
            {
                this["PlayerHealingToPlayers"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool PlayerHealingToPlayersByAction
        {
            get { return ((bool) (this["PlayerHealingToPlayersByAction"])); }
            set
            {
                this["PlayerHealingToPlayersByAction"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool PlayerDamageTakenByAction
        {
            get { return ((bool) (this["PlayerDamageTakenByAction"])); }
            set
            {
                this["PlayerDamageTakenByAction"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool PlayerDamageTakenByMonsters
        {
            get { return ((bool) (this["PlayerDamageTakenByMonsters"])); }
            set
            {
                this["PlayerDamageTakenByMonsters"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool PlayerDamageTakenByMonstersByAction
        {
            get { return ((bool) (this["PlayerDamageTakenByMonstersByAction"])); }
            set
            {
                this["PlayerDamageTakenByMonstersByAction"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool MonsterDamageByAction
        {
            get { return ((bool) (this["MonsterDamageByAction"])); }
            set
            {
                this["MonsterDamageByAction"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool MonsterDamageToPlayers
        {
            get { return ((bool) (this["MonsterDamageToPlayers"])); }
            set
            {
                this["MonsterDamageToPlayers"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool MonsterDamageToPlayersByAction
        {
            get { return ((bool) (this["MonsterDamageToPlayersByAction"])); }
            set
            {
                this["MonsterDamageToPlayersByAction"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool MonsterHealingByAction
        {
            get { return ((bool) (this["MonsterHealingByAction"])); }
            set
            {
                this["MonsterHealingByAction"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool MonsterHealingToMonsters
        {
            get { return ((bool) (this["MonsterHealingToMonsters"])); }
            set
            {
                this["MonsterHealingToMonsters"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool MonsterHealingToMonstersByAction
        {
            get { return ((bool) (this["MonsterHealingToMonstersByAction"])); }
            set
            {
                this["MonsterHealingToMonstersByAction"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool MonsterDamageTakenByAction
        {
            get { return ((bool) (this["MonsterDamageTakenByAction"])); }
            set
            {
                this["MonsterDamageTakenByAction"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool MonsterDamageTakenByPlayers
        {
            get { return ((bool) (this["MonsterDamageTakenByPlayers"])); }
            set
            {
                this["MonsterDamageTakenByPlayers"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool MonsterDamageTakenByPlayersByAction
        {
            get { return ((bool) (this["MonsterDamageTakenByPlayersByAction"])); }
            set
            {
                this["MonsterDamageTakenByPlayersByAction"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool MonsterDrops
        {
            get { return ((bool) (this["MonsterDrops"])); }
            set
            {
                this["MonsterDrops"] = value;
                RaisePropertyChanged();
            }
        }

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

        #region Implementation of INotifyPropertyChanged

        public new event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
