// FFXIVAPP
// ThemeHelper.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.
// 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MahApps.Metro;

namespace FFXIVAPP.Classes.Helpers
{
    internal static class ThemeHelper
    {
        private static readonly ResourceDictionary LightResource = new ResourceDictionary {Source = new Uri("pack://application:,,,/FFXIVAPP;component/Styles/Accents/BaseLight.xaml")};
        private static readonly ResourceDictionary DarkResource = new ResourceDictionary {Source = new Uri("pack://application:,,,/FFXIVAPP;component/Styles/Accents/BaseDark.xaml")};

        private static IEnumerable<Accent> _accents;

        public static IEnumerable<Accent> DefaultAccents
        {
            get { return _accents ?? (_accents = new List<Accent> {new Accent("Red", new Uri("pack://application:,,,/FFXIVAPP;component/Styles/Accents/Red.xaml")), new Accent("Green", new Uri("pack://application:,,,/FFXIVAPP;component/Styles/Accents/Green.xaml")), new Accent("Blue", new Uri("pack://application:,,,/FFXIVAPP;component/Styles/Accents/Blue.xaml")), new Accent("Purple", new Uri("pack://application:,,,/FFXIVAPP;component/Styles/Accents/Purple.xaml")), new Accent("Orange", new Uri("pack://application:,,,/FFXIVAPP;component/Styles/Accents/Orange.xaml")),}); }
        }

        /// <summary>
        /// </summary>
        /// <param name="t"> </param>
        public static void ChangeTheme(string t)
        {
            try
            {
                var split = t.Split('|');
                var accent = split[0];
                var theme = split[1];
                switch (theme)
                {
                    case "Dark":
                        ChangeTheme(MainWindow.View, DefaultAccents.First(a => a.Name == accent), Theme.Dark);
                        break;
                    case "Light":
                        ChangeTheme(MainWindow.View, DefaultAccents.First(a => a.Name == accent), Theme.Light);
                        break;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="app"> </param>
        /// <param name="accent"> </param>
        /// <param name="theme"> </param>
        public static void ChangeTheme(Application app, Accent accent, Theme theme)
        {
            ChangeTheme(app.Resources, accent, theme);
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
        /// <param name="r"> </param>
        /// <param name="accent"> </param>
        /// <param name="theme"> </param>
        private static void ChangeTheme(IDictionary r, Accent accent, Theme theme)
        {
            var themeResource = (theme == Theme.Light) ? LightResource : DarkResource;
            ApplyResourceDictionary(themeResource, r);
            ApplyResourceDictionary(accent.Resources, r);
        }

        /// <summary>
        /// </summary>
        /// <param name="newRd"> </param>
        /// <param name="oldRd"> </param>
        private static void ApplyResourceDictionary(IEnumerable newRd, IDictionary oldRd)
        {
            foreach (DictionaryEntry r in newRd)
            {
                if (oldRd.Contains(r.Key))
                {
                    oldRd.Remove(r.Key);
                }

                oldRd.Add(r.Key, r.Value);
            }
        }
    }
}