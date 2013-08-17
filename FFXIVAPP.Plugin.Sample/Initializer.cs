// FFXIVAPP.Plugin.Sample
// Initializer.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Xml.Linq;
using FFXIVAPP.Plugin.Sample.Properties;

#endregion

namespace FFXIVAPP.Plugin.Sample
{
    internal static class Initializer
    {
        #region Declarations

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
                        return;
                    }
                    Settings.SetValue(xKey, xValue);
                    if (!Constants.Settings.Contains(xKey))
                    {
                        Constants.Settings.Add(xKey);
                    }
                }
            }
        }
    }
}
