// FFXIVAPP.Client
// SettingsHelper.Client.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.Utilities;
using NLog;
using SmartAssembly.Attributes;

#endregion

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
                XmlHelper.DeleteXmlNode(Constants.XColors, "Color");
                foreach (var color in Constants.Colors)
                {
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
                    XmlHelper.SaveXmlNode(Constants.XColors, "Colors", "Color", xKey, keyPairList);
                }
                Constants.XColors.Save(AppViewModel.Instance.ConfigurationsPath + "Colors.xml");
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
                    Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
                }
            }
        }
    }
}
