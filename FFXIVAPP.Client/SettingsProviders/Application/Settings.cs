// FFXIVAPP.Client ~ Settings.cs
// 
// Copyright © 2007 - 2016 Ryan Wilson - All Rights Reserved
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Xml.Linq;
using FFXIVAPP.Client.ViewModels;
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
            SavePluginSourcesNode();
            Constants.XSettings.Save(Path.Combine(AppViewModel.Instance.SettingsPath, "ApplicationSettings.xml"));
        }

        private void DefaultSettings()
        {
            Constants.Settings.Clear();
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

        public void SavePluginSourcesNode()
        {
            if (Constants.XSettings == null)
            {
                return;
            }
            Constants.XSettings.Descendants("PluginSource")
                     .Where(node => UpdateViewModel.Instance.AvailableSources.All(source => source.Key.ToString() != node.Attribute("Key")
                                                                                                                         .Value))
                     .Remove();
            var xElements = Constants.XSettings.Descendants()
                                     .Elements("PluginSource");
            var enumerable = xElements as XElement[] ?? xElements.ToArray();
            // ensure enabled plugin settings
            foreach (var item in UpdateViewModel.Instance.AvailableSources)
            {
                var xKey = item.Key != Guid.Empty ? item.Key : Guid.NewGuid();
                var xSourceURI = item.SourceURI;
                var xEnabled = item.Enabled;
                var keyPairList = new List<XValuePair>
                {
                    new XValuePair
                    {
                        Key = "SourceURI",
                        Value = xSourceURI
                    },
                    new XValuePair
                    {
                        Key = "Enabled",
                        Value = xEnabled.ToString()
                    }
                };
                var element = enumerable.FirstOrDefault(e => e.Attribute("Key")
                                                              .Value == xKey.ToString());
                if (element == null)
                {
                    XmlHelper.SaveXmlNode(Constants.XSettings, "Settings", "PluginSource", xKey.ToString(), keyPairList);
                }
                else
                {
                    var xKeyElement = element.Attribute("Key");
                    if (xKeyElement != null)
                    {
                        xKeyElement.Value = xKey.ToString();
                    }
                    var xRegExElement = element.Element("SourceURI");
                    if (xRegExElement != null)
                    {
                        xRegExElement.Value = xSourceURI;
                    }
                    var xEnabledElement = element.Element("Enabled");
                    if (xEnabledElement != null)
                    {
                        xEnabledElement.Value = xEnabled.ToString();
                    }
                }
            }
        }

        #endregion
    }
}
