// FFXIVAPP.Plugin.Log
// ThemeHelper.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Windows;
using System.Windows.Media;
using FFXIVAPP.Common.Controls;
using FFXIVAPP.Plugin.Log.Properties;

#endregion

namespace FFXIVAPP.Plugin.Log.Helpers
{
    internal static class ThemeHelper
    {
        public static void SetupFont(ref xFlowDocument flowDoc)
        {
            var font = Settings.Default.ChatFont;
            flowDoc._FD.FontFamily = new FontFamily(font.Name);
            flowDoc._FD.FontWeight = font.Bold ? FontWeights.Bold : FontWeights.Regular;
            flowDoc._FD.FontStyle = font.Italic ? FontStyles.Italic : FontStyles.Normal;
            flowDoc._FD.FontSize = font.Size;
        }

        public static void SetupColor(ref xFlowDocument flowDoc)
        {
            flowDoc._FD.Background = new SolidColorBrush(Settings.Default.ChatBackgroundColor);
        }
    }
}
