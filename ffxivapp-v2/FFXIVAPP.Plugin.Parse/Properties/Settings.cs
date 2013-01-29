// FFXIVAPP.Plugin.Parse
// Settings.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

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
            if (Constants.Settings.Count == 0)
            {
                Constants.Settings.Add("ExportXML");
                Constants.Settings.Add("UploadParse");
                Constants.Settings.Add("ShowAbilityChatTab");
                Constants.Settings.Add("ShowPartyTab");
                Constants.Settings.Add("ShowHealingTab");
                Constants.Settings.Add("ShowDamageTab");
                Constants.Settings.Add("ShowMonsterTab");
                Constants.Settings.Add("ShowDebugTab");
                Constants.Settings.Add("PlayerAbility");
                Constants.Settings.Add("PlayerOnMonster");
                Constants.Settings.Add("PlayerAbilityOnMonsterByPlayer");
                Constants.Settings.Add("PlayerHealing");
                Constants.Settings.Add("PlayerOnOther");
                Constants.Settings.Add("PlayerHealingByPlayerOnOther");
                Constants.Settings.Add("PlayerDamageTaken");
                Constants.Settings.Add("PlayerDamageTakenByAbility");
                Constants.Settings.Add("MonsterDamageTakenByAbility");
                Constants.Settings.Add("MonsterDrops");
            }
            foreach (var item in Constants.Settings)
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
            Constants.XSettings.Save(Constants.BaseDirectory + "Settings.xml");
        }

        public new void Reset()
        {
            foreach (var key in Constants.Settings)
            {
                var value = Default.Properties[key].DefaultValue.ToString();
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
        public bool ShowAbilityChatTab
        {
            get { return ((bool) (this["ShowAbilityChatTab"])); }
            set
            {
                this["ShowAbilityChatTab"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowPartyTab
        {
            get { return ((bool) (this["ShowPartyTab"])); }
            set
            {
                this["ShowPartyTab"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowHealingTab
        {
            get { return ((bool) (this["ShowHealingTab"])); }
            set
            {
                this["ShowHealingTab"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowDamageTab
        {
            get { return ((bool) (this["ShowDamageTab"])); }
            set
            {
                this["ShowDamageTab"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowMonsterTab
        {
            get { return ((bool) (this["ShowMonsterTab"])); }
            set
            {
                this["ShowMonsterTab"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool ShowDebugTab
        {
            get { return ((bool) (this["ShowDebugTab"])); }
            set
            {
                this["ShowDebugTab"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool PlayerAbility
        {
            get { return ((bool) (this["PlayerAbility"])); }
            set
            {
                this["PlayerAbility"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool PlayerOnMonster
        {
            get { return ((bool) (this["PlayerOnMonster"])); }
            set
            {
                this["PlayerOnMonster"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool PlayerAbilityOnMonsterByPlayer
        {
            get { return ((bool) (this["PlayerAbilityOnMonsterByPlayer"])); }
            set
            {
                this["PlayerAbilityOnMonsterByPlayer"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool PlayerHealing
        {
            get { return ((bool) (this["PlayerHealing"])); }
            set
            {
                this["PlayerHealing"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool PlayerOnOther
        {
            get { return ((bool) (this["PlayerOnOther"])); }
            set
            {
                this["PlayerOnOther"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool PlayerHealingByPlayerOnOther
        {
            get { return ((bool) (this["PlayerHealingByPlayerOnOther"])); }
            set
            {
                this["PlayerHealingByPlayerOnOther"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool PlayerDamageTaken
        {
            get { return ((bool) (this["PlayerDamageTaken"])); }
            set
            {
                this["PlayerDamageTaken"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool PlayerDamageTakenByAbility
        {
            get { return ((bool) (this["PlayerDamageTakenByAbility"])); }
            set
            {
                this["PlayerDamageTakenByAbility"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool MonsterDamageTakenByAbility
        {
            get { return ((bool) (this["MonsterDamageTakenByAbility"])); }
            set
            {
                this["MonsterDamageTakenByAbility"] = value;
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
