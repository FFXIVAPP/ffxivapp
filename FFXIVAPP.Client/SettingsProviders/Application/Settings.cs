// FFXIVAPP.Client
// Settings.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
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

namespace FFXIVAPP.Client.SettingsProviders.Application
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
            DefaultSettings();
            SaveSettingsNode();
            SaveEnabledPluginsNode();
            Constants.Application.XSettings.Save(AppViewModel.Instance.SettingsPath + "ApplicationSettings.xml");
        }

        private void DefaultSettings()
        {
            Constants.Application.Settings.Clear();
        }

        public new void Reset()
        {
            DefaultSettings();
            foreach (var key in Constants.Application.Settings)
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
                    case "Int32":
                        Default[key] = Convert.ToInt32(value);
                        break;
                    default:
                        Default[key] = value;
                        break;
                }
            }
            catch (SettingsPropertyNotFoundException ex)
            {
            }
            catch (SettingsPropertyWrongTypeException ex)
            {
            }
        }

        #region Property Bindings (Settings)

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
            if (Constants.Application.XSettings == null)
            {
                return;
            }
            var xElements = Constants.Application.XSettings.Descendants()
                                     .Elements("Setting");
            var enumerable = xElements as XElement[] ?? xElements.ToArray();
            foreach (var setting in Constants.Application.Settings)
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
                    XmlHelper.SaveXmlNode(Constants.Application.XSettings, "Settings", "Setting", xKey, keyPairList);
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

        public void SaveEnabledPluginsNode()
        {
            if (Constants.Application.XSettings == null)
            {
                return;
            }
            var xElements = Constants.Application.XSettings.Descendants()
                                     .Elements("Setting");
            var enumerable = xElements as XElement[] ?? xElements.ToArray();
            // ensure enabled plugin settings
            foreach (var enabledPlugin in Constants.Application.EnabledPlugins)
            {
                var xKey = enabledPlugin.Key;
                var xEnabled = enabledPlugin.Value.ToString();
                var keyPairList = new List<XValuePair>
                {
                    new XValuePair
                    {
                        Key = "Enabled",
                        Value = xEnabled
                    }
                };
                var element = enumerable.FirstOrDefault(e => e.Attribute("Key")
                                                              .Value == xKey);
                if (element == null)
                {
                    XmlHelper.SaveXmlNode(Constants.Application.XSettings, "Settings", "Setting", xKey, keyPairList);
                }
                else
                {
                    var xEnabledElement = element.Element("Enabled");
                    if (xEnabledElement != null)
                    {
                        xEnabledElement.Value = xEnabled;
                    }
                }
            }
        }

        #endregion
    }
}
