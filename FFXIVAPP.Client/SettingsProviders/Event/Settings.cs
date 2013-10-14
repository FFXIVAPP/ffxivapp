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
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using FFXIVAPP.Client.Plugins.Event;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.Utilities;
using NLog;
using ColorConverter = System.Windows.Media.ColorConverter;
using FontFamily = System.Drawing.FontFamily;

#endregion

namespace FFXIVAPP.Client.SettingsProviders.Event
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
            XmlHelper.DeleteXmlNode(Constants.Event.XSettings, "Setting");
            if (Constants.Event.Settings.Count == 0)
            {
            }
            DefaultSettings();
            foreach (var item in Constants.Event.Settings)
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
                    XmlHelper.SaveXmlNode(Constants.Event.XSettings, "Settings", "Setting", xKey, keyPairList);
                }
                catch (Exception ex)
                {
                    Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
                }
            }
            XmlHelper.DeleteXmlNode(Constants.Event.XSettings, "Event");
            foreach (var item in PluginViewModel.Instance.Events)
            {
                var xRegEx = item.RegEx;
                var xSound = item.Sound;
                var xDelay = item.Delay;
                var keyPairList = new List<XValuePair>
                {
                    new XValuePair
                    {
                        Key = "Sound",
                        Value = xSound
                    },
                    new XValuePair
                    {
                        Key = "Delay",
                        Value = xDelay.ToString(CultureInfo.InvariantCulture)
                    }
                };
                XmlHelper.SaveXmlNode(Constants.Event.XSettings, "Settings", "Event", xRegEx, keyPairList);
            }
            Constants.Event.XSettings.Save(AppViewModel.Instance.SettingsPath + "Settings.Event.xml");
        }

        private void DefaultSettings()
        {
            Constants.Event.Settings.Clear();
        }

        public new void Reset()
        {
            DefaultSettings();
            foreach (var key in Constants.Event.Settings)
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
