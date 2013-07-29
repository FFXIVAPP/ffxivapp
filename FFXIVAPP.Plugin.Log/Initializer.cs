// FFXIVAPP.Plugin.Log
// Initializer.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Windows.Controls;
using System.Xml.Linq;
using FFXIVAPP.Common.Controls;
using FFXIVAPP.Plugin.Log.Helpers;
using FFXIVAPP.Plugin.Log.Properties;
using FFXIVAPP.Plugin.Log.Views;

#endregion

namespace FFXIVAPP.Plugin.Log
{
    internal static class Initializer
    {
        #region Declarations

        private static readonly string[] TranslateKeys = new[]
        {
            "Say", "Tell", "Party", "LS", "Shout"
        };

        #endregion

        public static void LoadConstants()
        {
            Plugin.PHost.GetConstants(Plugin.PName);
        }

        /// <summary>
        /// </summary>
        public static void LoadSettings()
        {
            if (Constants.XSettings != null)
            {
                foreach (var xElement in Constants.XSettings.Descendants()
                                                  .Elements("Setting"))
                {
                    var xKey = (string) xElement.Attribute("Key");
                    var xValue = (string) xElement.Element("Value");
                    if (String.IsNullOrWhiteSpace(xKey) || String.IsNullOrWhiteSpace(xValue))
                    {
                        continue;
                    }
                    Settings.SetValue(xKey, xValue);
                    if (!Constants.Settings.Contains(xKey))
                    {
                        Constants.Settings.Add(xKey);
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        public static void LoadTabs()
        {
            if (Constants.XSettings != null)
            {
                foreach (var xElement in Constants.XSettings.Descendants()
                                                  .Elements("Tab"))
                {
                    var xKey = (string) xElement.Attribute("Key");
                    var xValue = (string) xElement.Element("Value");
                    var xRegularExpression = (string) xElement.Element("RegularExpression");
                    if (String.IsNullOrWhiteSpace(xKey) || String.IsNullOrWhiteSpace(xValue))
                    {
                        continue;
                    }
                    xRegularExpression = String.IsNullOrWhiteSpace(xRegularExpression) ? "*" : xRegularExpression;
                    TabItemHelper.AddTabByName(xKey, xValue, xRegularExpression);
                }
            }
        }

        /// <summary>
        /// </summary>
        public static void ApplyTheming()
        {
            ThemeHelper.SetupFont(ref MainView.View.AllFD);
            ThemeHelper.SetupFont(ref MainView.View.TranslatedFD);
            ThemeHelper.SetupFont(ref MainView.View.DebugFD);
            ThemeHelper.SetupColor(ref MainView.View.AllFD);
            ThemeHelper.SetupColor(ref MainView.View.TranslatedFD);
            ThemeHelper.SetupColor(ref MainView.View.DebugFD);
            foreach (TabItem s in PluginViewModel.Instance.Tabs)
            {
                var flowDocument = (xFlowDocument) s.Content;
                ThemeHelper.SetupFont(ref flowDocument);
                ThemeHelper.SetupColor(ref flowDocument);
            }
        }
    }
}
