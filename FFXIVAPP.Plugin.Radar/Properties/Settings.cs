// FFXIVAPP.Plugin.Radar
// Settings.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

namespace FFXIVAPP.Plugin.Radar.Properties
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
            // this call to default settings only ensures we keep the settings we want and delete the ones we don't (old)
            DefaultSettings();
            SaveSettingsNode();
            // I would make a function for each node itself; other examples such as log/event would showcase this
            Constants.XSettings.Save(Path.Combine(Common.Constants.PluginsSettingsPath, "FFXIVAPP.Plugin.Radar.xml"));
        }

        private void DefaultSettings()
        {
            Constants.Settings.Clear();
            Constants.Settings.Add("RadarWidgetWidth");
            Constants.Settings.Add("RadarWidgetHeight");
            Constants.Settings.Add("RadarWidgetUIScale");
            Constants.Settings.Add("ShowRadarWidgetOnLoad");
            Constants.Settings.Add("RadarWidgetTop");
            Constants.Settings.Add("RadarWidgetLeft");
            Constants.Settings.Add("WidgetClickThroughEnabled");
            Constants.Settings.Add("ShowTitleOnWidgets");
            Constants.Settings.Add("WidgetOpacity");
            Constants.Settings.Add("RadarCompassMode");

            #region PC Options

            Constants.Settings.Add("PCShow");
            Constants.Settings.Add("PCShowName");
            Constants.Settings.Add("PCShowHPPercent");
            Constants.Settings.Add("PCShowJob");

            #endregion

            #region NPC Options

            Constants.Settings.Add("NPCShow");
            Constants.Settings.Add("NPCShowName");
            Constants.Settings.Add("NPCShowHPPercent");

            #endregion

            #region Monster Options

            Constants.Settings.Add("MonsterShow");
            Constants.Settings.Add("MonsterShowName");
            Constants.Settings.Add("MonsterShowHPPercent");

            #endregion

            #region Gathering Options

            Constants.Settings.Add("GatheringShow");
            Constants.Settings.Add("GatheringShowName");
            Constants.Settings.Add("GatheringShowHPPercent");

            #endregion

            #region Other Options

            Constants.Settings.Add("OtherShow");
            Constants.Settings.Add("OtherShowName");
            Constants.Settings.Add("OtherShowHPPercent");

            #endregion
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
        [DefaultSettingValue("0.7")]
        public string WidgetOpacity
        {
            get { return ((string) (this["WidgetOpacity"])); }
            set
            {
                this["WidgetOpacity"] = value;
                RaisePropertyChanged();
            }
        }

        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string>0.5</string>
  <string>0.6</string>
  <string>0.7</string>
  <string>0.8</string>
  <string>0.9</string>
  <string>1.0</string>
</ArrayOfString>")]
        public StringCollection WidgetOpacityList
        {
            get { return ((StringCollection) (this["WidgetOpacityList"])); }
            set
            {
                this["WidgetOpacityList"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool WidgetClickThroughEnabled
        {
            get { return ((bool) (this["WidgetClickThroughEnabled"])); }
            set
            {
                this["WidgetClickThroughEnabled"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowTitleOnWidgets
        {
            get { return ((bool) (this["ShowTitleOnWidgets"])); }
            set
            {
                this["ShowTitleOnWidgets"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool RadarCompassMode
        {
            get { return ((bool) (this["RadarCompassMode"])); }
            set
            {
                this["RadarCompassMode"] = value;
                RaisePropertyChanged();
            }
        }

        #region Radar Widget Settings

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("600")]
        public int RadarWidgetWidth
        {
            get { return ((int) (this["RadarWidgetWidth"])); }
            set
            {
                this["RadarWidgetWidth"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("600")]
        public int RadarWidgetHeight
        {
            get { return ((int) (this["RadarWidgetHeight"])); }
            set
            {
                this["RadarWidgetHeight"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool ShowRadarWidgetOnLoad
        {
            get { return ((bool) (this["ShowRadarWidgetOnLoad"])); }
            set
            {
                this["ShowRadarWidgetOnLoad"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("100")]
        public int RadarWidgetTop
        {
            get { return ((int) (this["RadarWidgetTop"])); }
            set
            {
                this["RadarWidgetTop"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("100")]
        public int RadarWidgetLeft
        {
            get { return ((int) (this["RadarWidgetLeft"])); }
            set
            {
                this["RadarWidgetLeft"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("1.0")]
        public string RadarWidgetUIScale
        {
            get { return ((string) (this["RadarWidgetUIScale"])); }
            set
            {
                this["RadarWidgetUIScale"] = value;
                RaisePropertyChanged();
            }
        }

        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string>0.8</string>
  <string>0.9</string>
  <string>1.0</string>
  <string>1.1</string>
  <string>1.2</string>
  <string>1.3</string>
  <string>1.4</string>
  <string>1.5</string>
</ArrayOfString>")]
        public StringCollection RadarWidgetUIScaleList
        {
            get { return ((StringCollection) (this["RadarWidgetUIScaleList"])); }
        }

        #endregion

        #region PC Options

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool PCShow
        {
            get { return ((bool) (this["PCShow"])); }
            set
            {
                this["PCShow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool PCShowName
        {
            get { return ((bool) (this["PCShowName"])); }
            set
            {
                this["PCShowName"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool PCShowHPPercent
        {
            get { return ((bool) (this["PCShowHPPercent"])); }
            set
            {
                this["PCShowHPPercent"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool PCShowJob
        {
            get { return ((bool) (this["PCShowJob"])); }
            set
            {
                this["PCShowJob"] = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region NPC Options

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool NPCShow
        {
            get { return ((bool) (this["NPCShow"])); }
            set
            {
                this["NPCShow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool NPCShowName
        {
            get { return ((bool) (this["NPCShowName"])); }
            set
            {
                this["NPCShowName"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool NPCShowHPPercent
        {
            get { return ((bool) (this["NPCShowHPPercent"])); }
            set
            {
                this["NPCShowHPPercent"] = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Monster Options

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool MonsterShow
        {
            get { return ((bool) (this["MonsterShow"])); }
            set
            {
                this["MonsterShow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool MonsterShowName
        {
            get { return ((bool) (this["MonsterShowName"])); }
            set
            {
                this["MonsterShowName"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool MonsterShowHPPercent
        {
            get { return ((bool) (this["MonsterShowHPPercent"])); }
            set
            {
                this["MonsterShowHPPercent"] = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Gathering Options

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool GatheringShow
        {
            get { return ((bool) (this["GatheringShow"])); }
            set
            {
                this["GatheringShow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool GatheringShowName
        {
            get { return ((bool) (this["GatheringShowName"])); }
            set
            {
                this["GatheringShowName"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool GatheringShowHPPercent
        {
            get { return ((bool) (this["GatheringShowHPPercent"])); }
            set
            {
                this["GatheringShowHPPercent"] = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Other Options

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool OtherShow
        {
            get { return ((bool) (this["OtherShow"])); }
            set
            {
                this["OtherShow"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool OtherShowName
        {
            get { return ((bool) (this["OtherShowName"])); }
            set
            {
                this["OtherShowName"] = value;
                RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool OtherShowHPPercent
        {
            get { return ((bool) (this["OtherShowHPPercent"])); }
            set
            {
                this["OtherShowHPPercent"] = value;
                RaisePropertyChanged();
            }
        }

        #endregion

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
            if (Constants.XSettings == null)
            {
                return;
            }
            var xElements = Constants.XSettings.Descendants()
                                     .Elements("Setting");
            var enumerable = xElements as XElement[] ?? xElements.ToArray();
            foreach (var setting in Constants.Settings)
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
                    XmlHelper.SaveXmlNode(Constants.XSettings, "Settings", "Setting", xKey, keyPairList);
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
