// FFXIVAPP.Plugin.Event
// English.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System.Windows;

namespace FFXIVAPP.Plugin.Event.Localization
{
    public abstract class English
    {
        private static readonly ResourceDictionary Dictionary = new ResourceDictionary();

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public static ResourceDictionary Context()
        {
            Dictionary.Clear();
            Dictionary.Add("event_", "PLACEHOLDER");
            Dictionary.Add("event_addupdateevent", "Add Or Update Event");
            Dictionary.Add("event_regex", "RegEx");
            Dictionary.Add("event_regexlabel", "RegEx:");
            Dictionary.Add("event_sample", "The scout vulture readies Wing Cutter.");
            Dictionary.Add("event_sound", "Sound");
            Dictionary.Add("event_soundlabel", "Sound:");
            return Dictionary;
        }
    }
}