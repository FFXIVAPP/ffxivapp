// FFXIVAPP.Client
// ThemeHelper.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MahApps.Metro;
using MahApps.Metro.Controls;
using SmartAssembly.Attributes;
using ThemeManager = FFXIVAPP.Client.Utilities.ThemeManager;

namespace FFXIVAPP.Client.Helpers
{
    [DoNotObfuscate]
    internal static class ThemeHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="t"> </param>
        /// <param name="window"></param>
        public static void ChangeTheme(string t, List<MetroWindow> window)
        {
            try
            {
                if (window == null || !window.Any())
                {
                    Apply(t, ShellView.View);
                    return;
                }
                foreach (var metroWindow in window)
                {
                    Apply(t, metroWindow);
                }
            }
            catch (Exception ex)
            {
                //Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="t"></param>
        /// <param name="window"></param>
        private static void Apply(string t, Window window)
        {
            var split = t.Split('|');
            var accent = split[0];
            var theme = split[1];
            switch (theme)
            {
                case "Dark":
                    ThemeManager.ChangeTheme(window, ThemeManager.DefaultAccents.First(a => a.Name == accent), Theme.Dark);
                    break;
                case "Light":
                    ThemeManager.ChangeTheme(window, ThemeManager.DefaultAccents.First(a => a.Name == accent), Theme.Light);
                    break;
            }
        }
    }
}
