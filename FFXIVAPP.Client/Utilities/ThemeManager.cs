// FFXIVAPP.Client
// ThemeManager.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MahApps.Metro;

#endregion

namespace FFXIVAPP.Client.Utilities
{
    internal static class ThemeManager
    {
        private static readonly ResourceDictionary LightResource = new ResourceDictionary
        {
            Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml")
        };

        private static readonly ResourceDictionary DarkResource = new ResourceDictionary
        {
            Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseDark.xaml")
        };

        public static IEnumerable<Accent> DefaultAccents
        {
            get
            {
                var accents = new[]
                {
                    "Red", "Green", "Blue", "Purple", "Orange"
                };
                const string path = "pack://application:,,,/MahApps.Metro;component/Styles/Accents/{0}.xaml";
                return accents.Select(accent => new Accent(accent, new Uri(String.Format(path, accent))))
                    .ToList();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="application"> </param>
        /// <param name="accent"> </param>
        /// <param name="theme"> </param>
        public static void ChangeTheme(Application application, Accent accent, Theme theme)
        {
            ChangeTheme(application.Resources, accent, theme);
        }

        /// <summary>
        /// </summary>
        /// <param name="window"> </param>
        /// <param name="accent"> </param>
        /// <param name="theme"> </param>
        public static void ChangeTheme(Window window, Accent accent, Theme theme)
        {
            ChangeTheme(window.Resources, accent, theme);
        }

        /// <summary>
        /// </summary>
        /// <param name="dictionary"> </param>
        /// <param name="accent"> </param>
        /// <param name="theme"> </param>
        public static void ChangeTheme(ResourceDictionary dictionary, Accent accent, Theme theme)
        {
            var themeResource = (theme == Theme.Light) ? LightResource : DarkResource;
            ApplyResourceDictionary(themeResource, dictionary);
            ApplyResourceDictionary(accent.Resources, dictionary);
        }

        /// <summary>
        /// </summary>
        /// <param name="newDictionary"> </param>
        /// <param name="oldDictionary"> </param>
        private static void ApplyResourceDictionary(IEnumerable newDictionary, IDictionary oldDictionary)
        {
            foreach (DictionaryEntry entry in newDictionary)
            {
                if (oldDictionary.Contains(entry.Key))
                {
                    oldDictionary.Remove(entry.Key);
                }
                oldDictionary.Add(entry.Key, entry.Value);
            }
        }
    }
}
