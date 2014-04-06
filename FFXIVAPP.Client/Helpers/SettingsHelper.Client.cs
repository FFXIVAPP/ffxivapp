// FFXIVAPP.Client
// SettingsHelper.Client.cs
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
                if (Constants.Client.XColors == null)
                {
                    return;
                }
                var xElements = Constants.Client.XColors.Descendants()
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
                        XmlHelper.SaveXmlNode(Constants.Client.XColors, "Colors", "Color", xKey, keyPairList);
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
                Constants.Client.XColors.Save(Path.Combine(AppViewModel.Instance.ConfigurationsPath, "Colors.xml"));
            }

            #endregion
        }
    }
}
