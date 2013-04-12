// FFXIVAPP.Plugin.Parse
// Settings.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

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
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;
using FontFamily = System.Drawing.FontFamily;

#endregion

namespace FFXIVAPP.Plugin.Parse.Properties
{
    internal class Settings : ApplicationSettingsBase, INotifyPropertyChanged
    {
        private static Settings _default;

        public static Settings Default
        {
            get { return _default ?? (_default = ((Settings) (Synchronized(new Settings())))); }
        }

        public override void Save()
        {
            XmlHelper.DeleteXmlNode(Constants.XSettings, "Setting");
            DefaultSettings();
            foreach (var item in Constants.Settings)
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
                    XmlHelper.SaveXmlNode(Constants.XSettings, "Settings", "Setting", xKey, keyPairList);
                }
                catch (Exception ex)
                {
                    Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
                }
            }
            Constants.XSettings.Save(Constants.BaseDirectory + "Settings.xml");
        }

        private void DefaultSettings()
        {
            Constants.Settings.Clear();
            Constants.Settings.Add("ExportXML");
            Constants.Settings.Add("UploadParse");
            Constants.Settings.Add("ShowActionLogTab");
            Constants.Settings.Add("ShowPartyDamageTab");
            Constants.Settings.Add("ShowPartyHealingTab");
            Constants.Settings.Add("ShowPartyDamageTakenTab");
            Constants.Settings.Add("ShowMonsterDamageTakenTab");
            Constants.Settings.Add("PlayerDamageByAction");
            Constants.Settings.Add("PlayerDamageToMonsters");
            Constants.Settings.Add("PlayerDamageToMonstersByAction");
            Constants.Settings.Add("PlayerHealingByAction");
            Constants.Settings.Add("PlayerHealingToPlayers");
            Constants.Settings.Add("PlayerHealingToPlayersByAction");
            Constants.Settings.Add("PlayerDamageTakenByMonsters");
            Constants.Settings.Add("PlayerDamageTakenByMonstersByAction");
            Constants.Settings.Add("MonsterDamageTakenByAction");
            Constants.Settings.Add("MonsterDrops");
        }

        public new void Reset()
        {
            DefaultSettings();
            foreach (var key in Constants.Settings)
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
        [DefaultSettingValue("#FF000000")]
        public Color ChatBackgroundColor
        {
            get { return ((Color) (this["ChatBackgroundColor"])); }
            set
            {
                this["ChatBackgroundColor"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("#FF800080")]
        public Color TimeStampColor
        {
            get { return ((Color) (this["TimeStampColor"])); }
            set
            {
                this["TimeStampColor"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("Microsoft Sans Serif, 12pt")]
        public Font ChatFont
        {
            get { return ((Font) (this["ChatFont"])); }
            set
            {
                this["ChatFont"] = value;
                RaisePropertyChanged();
            }
        }

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
        [DefaultSettingValue("False")]
        public bool ExportXML
        {
            get { return ((bool) (this["ExportXML"])); }
            set
            {
                this["ExportXML"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool UploadParse
        {
            get { return ((bool) (this["UploadParse"])); }
            set
            {
                this["UploadParse"] = value;
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
        public bool MonsterDrops
        {
            get { return ((bool) (this["MonsterDrops"])); }
            set
            {
                this["MonsterDrops"] = value;
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
