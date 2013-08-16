// FFXIVAPP.Client
// ThemeHelper.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Linq;
using System.Windows;
using MahApps.Metro;
using ThemeManager = FFXIVAPP.Client.Utilities.ThemeManager;

#endregion

namespace FFXIVAPP.Client.Helpers
{
    internal static class ThemeHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="t"> </param>
        /// <param name="window"></param>
        public static void ChangeTheme(string t, Window window = null)
        {
            try
            {
                Apply(t, ShellView.View);
                if (window != null)
                {
                    Apply(t, window);
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
