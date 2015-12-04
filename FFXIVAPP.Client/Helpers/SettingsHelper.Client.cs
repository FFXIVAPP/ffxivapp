// FFXIVAPP.Client ~ SettingsHelper.Client.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Models;

namespace FFXIVAPP.Client.Helpers
{
    internal static partial class SettingsHelper
    {
        public static class Client
        {
            /// <summary>
            /// </summary>
            public static void Save()
            {
                SaveColorsNode();
                Settings.Default.Save();
            }

            /// <summary>
            /// </summary>
            public static void Default()
            {
                try
                {
                    var att = Assembly.GetCallingAssembly()
                                      .GetCustomAttributes(typeof (AssemblyCompanyAttribute), false);
                    var companyName = ((AssemblyCompanyAttribute) att[0]).Company;
                    var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    var combinedPath = Path.Combine(appDataPath, companyName);
                    var title = AppViewModel.Instance.Locale["app_WarningMessage"];
                    var message = String.Format("{0} : {1}", AppViewModel.Instance.Locale["app_DeleteMessage"], combinedPath);
                    MessageBoxHelper.ShowMessageAsync(title, message, delegate
                    {
                        Settings.Default.Reset();
                        Directory.Delete(combinedPath, true);
                        Settings.Default.Reload();
                    }, delegate { });
                }
                catch (Exception ex)
                {
                }
            }

            #region Iterative Settings Saving

            private static void SaveColorsNode()
            {
                if (Constants.XColors == null)
                {
                    return;
                }
                var xElements = Constants.XColors.Descendants()
                                         .Elements("Color");
                var enumerable = xElements as XElement[] ?? xElements.ToArray();
                foreach (var color in Constants.Colors)
                {
                    var element = enumerable.FirstOrDefault(e => e.Attribute("Key")
                                                                  .Value == color.Key);
                    var xKey = color.Key;
                    var xValue = color.Value[0];
                    var xDescription = color.Value[1];
                    var keyPairList = new List<XValuePair>();
                    keyPairList.Add(new XValuePair
                    {
                        Key = "Value",
                        Value = xValue
                    });
                    keyPairList.Add(new XValuePair
                    {
                        Key = "Description",
                        Value = xDescription
                    });
                    if (element == null)
                    {
                        XmlHelper.SaveXmlNode(Constants.XColors, "Colors", "Color", xKey, keyPairList);
                    }
                    else
                    {
                        var xValueElement = element.Element("Value");
                        if (xValueElement != null)
                        {
                            xValueElement.Value = xValue;
                        }
                        var xDescriptionElement = element.Element("Description");
                        if (xDescriptionElement != null)
                        {
                            xDescriptionElement.Value = xDescription;
                        }
                    }
                }
                Constants.XColors.Save(Path.Combine(AppViewModel.Instance.ConfigurationsPath, "Colors.xml"));
            }

            #endregion
        }
    }
}
