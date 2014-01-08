// FFXIVAPP.Client
// SettingsHelper.Client.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Models;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Helpers
{
    internal static partial class SettingsHelper
    {
        [DoNotObfuscate]
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
