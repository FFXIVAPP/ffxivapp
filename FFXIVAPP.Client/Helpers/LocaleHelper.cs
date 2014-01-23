// FFXIVAPP.Client
// LocaleHelper.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FFXIVAPP.Client.Models;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Helpers
{
    [DoNotObfuscate]
    internal static class LocaleHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="cultureInfo"> </param>
        public static void Update(CultureInfo cultureInfo)
        {
            var results = new Dictionary<string, string>();
            var client = Localization.LocaleHelper.ResolveOne(cultureInfo, "client")
                                     .Cast<DictionaryEntry>()
                                     .ToDictionary(item => (string) item.Key, item => (string) item.Value);
            var parse = Localization.LocaleHelper.ResolveOne(cultureInfo, "parse")
                                    .Cast<DictionaryEntry>()
                                    .ToDictionary(item => (string) item.Key, item => (string) item.Value);
            foreach (var resource in client)
            {
                try
                {
                    results.Add(resource.Key, resource.Value);
                }
                catch (Exception ex)
                {
                }
            }
            foreach (var resource in parse)
            {
                try
                {
                    results.Add(resource.Key, resource.Value);
                }
                catch (Exception ex)
                {
                }
            }
            AppViewModel.Instance.Locale = results;
            foreach (PluginInstance pluginInstance in App.Plugins.Loaded)
            {
                pluginInstance.Instance.Locale = results;
            }
        }
    }
}
