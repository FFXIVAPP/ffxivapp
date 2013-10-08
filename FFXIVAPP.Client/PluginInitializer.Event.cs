// FFXIVAPP.Client
// PluginInitializer.Event.cs
// 
// © 2013 Ryan Wilson

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using FFXIVAPP.Client.Models.Event;
using FFXIVAPP.Client.ViewModels.Plugins.Event;

namespace FFXIVAPP.Client
{
    internal static partial class PluginInitializer
    {
        public static class Event
        {
            public static void LoadSettings()
            {
                if (Constants.Event.XSettings != null)
                {
                    foreach (var xElement in Constants.Event.XSettings.Descendants()
                                                      .Elements("Setting"))
                    {
                        var xKey = (string) xElement.Attribute("Key");
                        var xValue = (string) xElement.Element("Value");
                        if (String.IsNullOrWhiteSpace(xKey) || String.IsNullOrWhiteSpace(xValue))
                        {
                            continue;
                        }
                        //SettingsHelper.Event.SetValue(xKey, xValue);
                        if (!Constants.Event.Settings.Contains(xKey))
                        {
                            Constants.Event.Settings.Add(xKey);
                        }
                    }
                }
            }

            public static void LoadSounds()
            {
                //do your gui stuff here
                var files = Directory.GetFiles(AppViewModel.Instance.SoundsPath)
                                     .Where(file => Regex.IsMatch(file, @"^.+\.(wav)$"))
                                     .Select(file => new FileInfo(file));
                foreach (var file in files)
                {
                    PluginViewModel.Instance.SoundFiles.Add(file.Name);
                }
            }

            public static void LoadSoundEvents()
            {
                if (Constants.Event.XSettings != null)
                {
                    foreach (var xElement in Constants.Event.XSettings.Descendants()
                                                      .Elements("Event"))
                    {
                        var xRegEx = (string) xElement.Attribute("Key");
                        var xValue = (string) xElement.Element("Value");
                        var xSound = (string) xElement.Element("Sound");
                        var xDelay = (string) xElement.Element("Delay");
                        if (String.IsNullOrWhiteSpace(xRegEx))
                        {
                            continue;
                        }
                        xSound = String.IsNullOrWhiteSpace(xSound) ? "aruba.wav" : String.IsNullOrWhiteSpace(xValue) ? xSound : xValue;
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
}
