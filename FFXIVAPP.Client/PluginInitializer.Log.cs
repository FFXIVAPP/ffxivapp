// FFXIVAPP.Client
// PluginInitializer.Log.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Windows.Controls;
using System.Xml.Linq;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Plugins.Log;
using FFXIVAPP.Client.Plugins.Log.Views;
using FFXIVAPP.Client.SettingsProviders.Log;
using FFXIVAPP.Common.Controls;
using TabItemHelper = FFXIVAPP.Client.Plugins.Log.Helpers.TabItemHelper;

namespace FFXIVAPP.Client {
    internal static partial class PluginInitializer {
        public static class Log {
            /// <summary>
            /// </summary>
            public static void LoadSettings() {
                if (Constants.Log.XSettings != null) {
                    foreach (var xElement in Constants.Log.XSettings.Descendants()
                                                      .Elements("Setting")) {
                        var xKey = (string) xElement.Attribute("Key");
                        var xValue = (string) xElement.Element("Value");
                        if (String.IsNullOrWhiteSpace(xKey) || String.IsNullOrWhiteSpace(xValue)) {
                            continue;
                        }
                        Settings.SetValue(xKey, xValue);
                        if (!Constants.Log.Settings.Contains(xKey)) {
                            Constants.Log.Settings.Add(xKey);
                        }
                    }
                }
            }

            /// <summary>
            /// </summary>
            public static void LoadTabs() {
                if (Constants.Log.XSettings != null) {
                    foreach (var xElement in Constants.Log.XSettings.Descendants()
                                                      .Elements("Tab")) {
                        var xKey = (string) xElement.Attribute("Key");
                        var xValue = (string) xElement.Element("Value");
                        var xRegularExpression = (string) xElement.Element("RegularExpression");
                        if (String.IsNullOrWhiteSpace(xKey) || String.IsNullOrWhiteSpace(xValue)) {
                            continue;
                        }
                        xRegularExpression = String.IsNullOrWhiteSpace(xRegularExpression) ? "*" : xRegularExpression;
                        TabItemHelper.AddTabByName(xKey, xValue, xRegularExpression);
                    }
                }
            }

            /// <summary>
            /// </summary>
            public static void ApplyTheming() {
                ThemeHelper.SetupFont(ref MainView.View.AllFD);
                ThemeHelper.SetupFont(ref MainView.View.TranslatedFD);
                ThemeHelper.SetupFont(ref MainView.View.DebugFD);
                ThemeHelper.SetupColor(ref MainView.View.AllFD);
                ThemeHelper.SetupColor(ref MainView.View.TranslatedFD);
                ThemeHelper.SetupColor(ref MainView.View.DebugFD);
                foreach (TabItem s in PluginViewModel.Instance.Tabs) {
                    var flowDocument = (xFlowDocument) s.Content;
                    ThemeHelper.SetupFont(ref flowDocument);
                    ThemeHelper.SetupColor(ref flowDocument);
                }
            }
        }
    }
}
