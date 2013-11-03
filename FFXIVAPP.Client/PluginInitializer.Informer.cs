// FFXIVAPP.Client
// PluginInitializer.Informer.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Xml.Linq;
using FFXIVAPP.Client.SettingsProviders.Parse;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client
{
    internal static partial class PluginInitializer
    {
        [DoNotObfuscate]
        public static class Informer
        {
            /// <summary>
            /// </summary>
            public static void LoadSettings()
            {
                if (Constants.Informer.XSettings != null)
                {
                    foreach (var xElement in Constants.Informer.XSettings.Descendants()
                                                      .Elements("Setting"))
                    {
                        var xKey = (string) xElement.Attribute("Key");
                        var xValue = (string) xElement.Element("Value");
                        if (String.IsNullOrWhiteSpace(xKey) || String.IsNullOrWhiteSpace(xValue))
                        {
                            continue;
                        }
                        Settings.SetValue(xKey, xValue);
                        if (!Constants.Informer.Settings.Contains(xKey))
                        {
                            Constants.Informer.Settings.Add(xKey);
                        }
                    }
                }
            }
        }
    }
}
