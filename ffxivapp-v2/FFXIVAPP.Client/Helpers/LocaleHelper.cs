// FFXIVAPP.Client
// LocaleHelper.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows;
using FFXIVAPP.Client.Localization;
using FFXIVAPP.Client.Models;

#endregion

namespace FFXIVAPP.Client.Helpers
{
    internal static class LocaleHelper
    {
        public static readonly string[] Supported = new[]
        {
            "en","fr"
        };

        /// <summary>
        /// </summary>
        /// <param name="cultureInfo"> </param>
        public static void Update(CultureInfo cultureInfo)
        {
            var culture = cultureInfo.TwoLetterISOLanguageName;
            ResourceDictionary dictionary;
            if (Supported.Contains(culture))
            {
                switch (culture)
                {
                    case "ja":
                        dictionary = Japanese.Context();
                        break;
                    case "de":
                        dictionary = German.Context();
                        break;
                    case "fr":
                        dictionary = French.Context();
                        break;
                    default:
                        dictionary = English.Context();
                        break;
                }
            }
            else
            {
                dictionary = English.Context();
            }
            var result = dictionary.Cast<DictionaryEntry>()
                                   .ToDictionary(item => (string) item.Key, item => (string) item.Value);
            AppViewModel.Instance.Locale = result;
            foreach (PluginInstance pluginInstance in App.Plugins.Loaded)
            {
                pluginInstance.Instance.Locale = result;
            }
        }
    }
}
