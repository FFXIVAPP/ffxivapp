// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TabItemHelper.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   TabItemHelper.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Helpers {
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows;

    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Layout;
    using Avalonia.Media.Imaging;

    using FFXIVAPP.Client.Models;
    using FFXIVAPP.Client.ViewModels;
    using FFXIVAPP.Common.Helpers;
    using FFXIVAPP.Common.Models;
    using FFXIVAPP.Common.Utilities;
    using FFXIVAPP.ResourceFiles;

    using NLog;

    internal static class TabItemHelper {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// </summary>
        /// <param name="image"> </param>
        /// <param name="name"> </param>
        /// <returns> </returns>
        public static StackPanel ImageHeader(Bitmap img, string name) {
            var stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
            var image = new Image { Width = 24, Height = 24, [ToolTip.TipProperty] = name, Source = img};
            var label = new TextBlock { Name = "TheLabel", Text = name, VerticalAlignment = VerticalAlignment.Center, Margin = Thickness.Parse("5,0,0,0") };

            stackPanel.Children.Add(image);
            stackPanel.Children.Add(label);
            return stackPanel;
        }

        public static void LoadPluginTabItem(PluginInstance pluginInstance) {
            try {
                if (!pluginInstance.Loaded) {
                    return;
                }

                var pluginName = pluginInstance.Instance.FriendlyName;
                if (SettingsViewModel.Instance.HomePluginList.Any(p => p.ToUpperInvariant().StartsWith(pluginName.ToUpperInvariant()))) {
                    pluginName = $"{pluginName}[{new Random().Next(1000, 9999)}]";
                }

                SettingsViewModel.Instance.HomePluginList.Add(pluginName);
                TabItem tabItem = pluginInstance.Instance.CreateTab();
                tabItem.Name = Regex.Replace(pluginInstance.Instance.Name, @"[^A-Za-z]", string.Empty);
                var iconfile = Path.Combine(Path.GetDirectoryName(pluginInstance.AssemblyPath), pluginInstance.Instance.Icon);
                var icon = File.Exists(iconfile)
                           ? new Bitmap(iconfile)
                           : Theme.DefaultPluginLogo;
                tabItem.Header = ImageHeader(icon, pluginInstance.Instance.FriendlyName);
                AppViewModel.Instance.PluginTabItems.Add(tabItem);
            }
            catch (Exception ex) {
                Logging.Log(Logger, new LogItem(ex, true));
            }
        }
    }
}