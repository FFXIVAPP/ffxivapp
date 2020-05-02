// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Settings.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2020 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Settings.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.SettingsProviders.Application {
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

    using FFXIVAPP.Client.Models;
    using FFXIVAPP.Client.ViewModels;
    using FFXIVAPP.Common.Helpers;
    using FFXIVAPP.Common.Models;
    using FFXIVAPP.Common.Utilities;

    using NLog;

    using ColorConverter = System.Windows.Media.ColorConverter;
    using FontFamily = System.Drawing.FontFamily;

    internal class Settings : ApplicationSettingsBase, INotifyPropertyChanged {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static Settings _default;

        public new event PropertyChangedEventHandler PropertyChanged = delegate { };

        public static Settings Default {
            get {
                return _default ?? (_default = (Settings) Synchronized(new Settings()));
            }
        }

        public static void SetValue(string key, string value, CultureInfo cultureInfo) {
            try {
                var type = Default[key].GetType().Name;
                switch (type) {
                    case "Boolean":
                        Default[key] = bool.Parse(value);
                        break;
                    case "Color":
                        var cc = new ColorConverter();
                        object color = cc.ConvertFrom(value);
                        Default[key] = color ?? Colors.Black;
                        break;
                    case "Double":
                        Default[key] = double.Parse(value, cultureInfo);
                        break;
                    case "Font":
                        var fc = new FontConverter();
                        object font = fc.ConvertFromString(value);
                        Default[key] = font ?? new Font(new FontFamily("Microsoft Sans Serif"), 12);
                        break;
                    case "Int32":
                        Default[key] = int.Parse(value, cultureInfo);
                        break;
                    default:
                        Default[key] = value;
                        break;
                }
            }
            catch (Exception ex) {
                Logging.Log(Logger, new LogItem(ex, true));
            }
        }

        public new void Reset() {
            this.DefaultSettings();
            foreach (var key in Constants.Settings) {
                SettingsProperty settingsProperty = Default.Properties[key];
                if (settingsProperty == null) {
                    continue;
                }

                var value = settingsProperty.DefaultValue.ToString();
                SetValue(key, value, CultureInfo.InvariantCulture);
            }
        }

        public override void Save() {
            this.DefaultSettings();
            this.SaveSettingsNode();
            this.SavePluginSourcesNode();
            Constants.XSettings.Save(Path.Combine(AppViewModel.Instance.SettingsPath, "ApplicationSettings.xml"));
        }

        public void SavePluginSourcesNode() {
            if (Constants.XSettings == null) {
                return;
            }

            Constants.XSettings.Descendants("PluginSource").Where(node => UpdateViewModel.Instance.AvailableSources.All(source => source.Key.ToString() != node.Attribute("Key").Value)).Remove();
            IEnumerable<XElement> xElements = Constants.XSettings.Descendants().Elements("PluginSource");
            XElement[] enumerable = xElements as XElement[] ?? xElements.ToArray();

            // ensure enabled plugin settings
            foreach (PluginSourceItem item in UpdateViewModel.Instance.AvailableSources) {
                Guid xKey = item.Key != Guid.Empty
                                ? item.Key
                                : Guid.NewGuid();
                var xSourceURI = item.SourceURI;
                var xEnabled = item.Enabled;
                List<XValuePair> keyPairList = new List<XValuePair> {
                    new XValuePair {
                        Key = "SourceURI",
                        Value = xSourceURI,
                    },
                    new XValuePair {
                        Key = "Enabled",
                        Value = xEnabled.ToString(),
                    },
                };
                XElement element = enumerable.FirstOrDefault(e => e.Attribute("Key").Value == xKey.ToString());
                if (element == null) {
                    XmlHelper.SaveXmlNode(Constants.XSettings, "Settings", "PluginSource", xKey.ToString(), keyPairList);
                }
                else {
                    XAttribute xKeyElement = element.Attribute("Key");
                    if (xKeyElement != null) {
                        xKeyElement.Value = xKey.ToString();
                    }

                    XElement xRegExElement = element.Element("SourceURI");
                    if (xRegExElement != null) {
                        xRegExElement.Value = xSourceURI;
                    }

                    XElement xEnabledElement = element.Element("Enabled");
                    if (xEnabledElement != null) {
                        xEnabledElement.Value = xEnabled.ToString();
                    }
                }
            }
        }

        private void DefaultSettings() {
            Constants.Settings.Clear();
        }

        private void RaisePropertyChanged([CallerMemberName,] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        private void SaveSettingsNode() {
            if (Constants.XSettings == null) {
                return;
            }

            IEnumerable<XElement> xElements = Constants.XSettings.Descendants().Elements("Setting");
            XElement[] enumerable = xElements as XElement[] ?? xElements.ToArray();
            foreach (var setting in Constants.Settings) {
                XElement element = enumerable.FirstOrDefault(e => e.Attribute("Key").Value == setting);
                if (element == null) {
                    var xKey = setting;
                    var xValue = Default[xKey].ToString();
                    List<XValuePair> keyPairList = new List<XValuePair> {
                        new XValuePair {
                            Key = "Value",
                            Value = xValue,
                        },
                    };
                    XmlHelper.SaveXmlNode(Constants.XSettings, "Settings", "Setting", xKey, keyPairList);
                }
                else {
                    XElement xElement = element.Element("Value");
                    if (xElement != null) {
                        xElement.Value = Default[setting].ToString();
                    }
                }
            }
        }
    }
}