// FFXIVAPP.Plugin.Event
// Initializer.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Linq;
using System.Xml.Linq;
using FFXIVAPP.Plugin.Event.Models;
using FFXIVAPP.Plugin.Event.Properties;

#endregion

namespace FFXIVAPP.Plugin.Event
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
        public static void LoadEvents()
        {
            if (Constants.XSettings != null)
            {
                foreach (var xElement in Constants.XSettings.Descendants()
                                                  .Elements("Event"))
                {
                    var xRegEx = (string) xElement.Attribute("Key");
                    var xSound = (string) xElement.Element("Sound");
                    var xDelay = (string) xElement.Element("Delay");
                    if (String.IsNullOrWhiteSpace(xRegEx))
                    {
                        continue;
                    }
                    xSound = String.IsNullOrWhiteSpace(xSound) ? "aruba.wav" : xSound;
                    var soundEvent = new SoundEvent
                    {
                        Sound = xSound,
                        Delay = 0,
                        RegEx = xRegEx
                    };
                    int result;
                    if (Int32.TryParse(xDelay, out result))
                    {
                        soundEvent.Delay = result;
                    }
                    var found = PluginViewModel.Instance.Events.Any(se => se.RegEx == soundEvent.RegEx);
                    if (!found)
                    {
                        PluginViewModel.Instance.Events.Add(soundEvent);
                    }
                }
            }
        }
    }
}
