// FFXIVAPP.Client
// Settings.cs
// 
// Copyright © 2007 - 2014 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

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
            Constants.Application.XSettings.Save(Path.Combine(AppViewModel.Instance.SettingsPath, "ApplicationSettings.xml"));
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

        public void SavePluginSourcesNode()
        {
            if (Constants.Application.XSettings == null)
            {
                return;
            }
            Constants.Application.XSettings.Descendants("PluginSource")
                     .Where(node => UpdateViewModel.Instance.AvailableSources.All(source => source.Key.ToString() != node.Attribute("Key")
                                                                                                                         .Value))
                     .Remove();
            var xElements = Constants.Application.XSettings.Descendants()
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
                    },
                };
                var element = enumerable.FirstOrDefault(e => e.Attribute("Key")
                                                              .Value == xKey.ToString());
                if (element == null)
                {
                    XmlHelper.SaveXmlNode(Constants.Application.XSettings, "Settings", "PluginSource", xKey.ToString(), keyPairList);
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
