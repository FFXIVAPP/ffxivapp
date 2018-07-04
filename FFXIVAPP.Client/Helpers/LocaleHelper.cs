// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocaleHelper.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   LocaleHelper.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Helpers {
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows;

    using FFXIVAPP.Client.Localization;
    using FFXIVAPP.Client.Models;

    internal static class LocaleHelper {
        /// <summary>
        /// </summary>
        /// <param name="cultureInfo"> </param>
        public static void Update(CultureInfo cultureInfo) {
            var culture = cultureInfo.TwoLetterISOLanguageName;
            ResourceDictionary dictionary;
            if (Constants.Supported.Contains(culture)) {
                switch (culture) {
                    case "fr":
                        dictionary = French.Context();
                        break;
                    case "ja":
                        dictionary = Japanese.Context();
                        break;
                    case "de":
                        dictionary = German.Context();
                        break;
                    case "zh":
                        dictionary = Chinese.Context();
                        break;
                    case "ru":
                        dictionary = Russian.Context();
                        break;
                    case "ko":
                        dictionary = Korean.Context();
                        break;
                    default:
                        dictionary = English.Context();
                        break;
                }
            }
            else {
                dictionary = English.Context();
            }

            Dictionary<string, string> locale = dictionary.Cast<DictionaryEntry>().ToDictionary(item => (string) item.Key, item => (string) item.Value);
            AppViewModel.Instance.Locale = locale;
            foreach (PluginInstance pluginInstance in App.Plugins.Loaded.Cast<PluginInstance>().Where(pluginInstance => pluginInstance.Loaded)) {
                pluginInstance.Instance.Locale = locale;
            }
        }
    }
}