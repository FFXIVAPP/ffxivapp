// FFXIVAPP.Common
// ThemeHelper.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MahApps.Metro;
using MahApps.Metro.Controls;
using NLog;

namespace FFXIVAPP.Common.Helpers
{
    public static class ThemeHelper
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="theme"> </param>
        /// <param name="window"></param>
        public static void ChangeTheme(string theme, List<MetroWindow> window)
        {
            try
            {
                if (window == null || !window.Any())
                {
                    Apply(theme, Application.Current.MainWindow);
                    return;
                }
                foreach (var metroWindow in window.Where(metroWindow => metroWindow != null))
                {
                    Apply(theme, metroWindow);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="t"></param>
        /// <param name="window"></param>
        private static void Apply(string theme, Window window)
        {
            var split = theme.Split('|');
            var accent = split[0];
            var shade = split[1];
            switch (shade)
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
