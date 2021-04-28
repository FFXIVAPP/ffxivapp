// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsHelper.Client.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   SettingsHelper.Client.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Helpers {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;

    using FFXIVAPP.Client.Properties;
    using FFXIVAPP.Common.Helpers;
    using FFXIVAPP.Common.Models;
    using FFXIVAPP.Common.Utilities;

    internal static partial class SettingsHelper {
        public static class Client {
            /// <summary>
            /// </summary>
            public static void Default() {
                try {
                    object[] att = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                    var companyName = ((AssemblyCompanyAttribute) att[0]).Company;
                    var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    var combinedPath = Path.Combine(appDataPath, companyName);
                    var title = AppViewModel.Instance.Locale["app_WarningMessage"];
                    var message = $"{AppViewModel.Instance.Locale["app_DeleteMessage"]} : {combinedPath}";
                    MessageBoxHelper.ShowMessageAsync(
                        title, message, delegate {
                            Settings.Default.Reset();
                            Directory.Delete(combinedPath, true);
                            Settings.Default.Reload();
                        }, delegate { });
                }
                catch (Exception ex) {
                    Logging.Log(Logger, new LogItem(ex, true));
                }
            }

            /// <summary>
            /// </summary>
            public static void Save() {
                SaveColorsNode();
                Settings.Default.Save();
            }

            private static void SaveColorsNode() {
                if (Constants.XColors == null) {
                    return;
                }

                IEnumerable<XElement> xElements = Constants.XColors.Descendants().Elements("Color");
                XElement[] enumerable = xElements as XElement[] ?? xElements.ToArray();
                foreach (KeyValuePair<string, string[]> color in Constants.Colors) {
                    XElement element = enumerable.FirstOrDefault(e => e.Attribute("Key").Value == color.Key);
                    var xKey = color.Key;
                    var xValue = color.Value[0];
                    var xDescription = color.Value[1];
                    List<XValuePair> keyPairList = new List<XValuePair>();
                    keyPairList.Add(
                        new XValuePair {
                            Key = "Value",
                            Value = xValue,
                        });
                    keyPairList.Add(
                        new XValuePair {
                            Key = "Description",
                            Value = xDescription,
                        });
                    if (element == null) {
                        XmlHelper.SaveXmlNode(Constants.XColors, "Colors", "Color", xKey, keyPairList);
                    }
                    else {
                        XElement xValueElement = element.Element("Value");
                        if (xValueElement != null) {
                            xValueElement.Value = xValue;
                        }

                        XElement xDescriptionElement = element.Element("Description");
                        if (xDescriptionElement != null) {
                            xDescriptionElement.Value = xDescription;
                        }
                    }
                }

                Constants.XColors.Save(Path.Combine(AppViewModel.Instance.ConfigurationsPath, "Colors.xml"));
            }
        }
    }
}