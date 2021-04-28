// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TabItemHelper.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
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
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media.Imaging;

    using FFXIVAPP.Client.Models;
    using FFXIVAPP.Client.Properties;
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
        public static DataTemplate ImageHeader(BitmapImage image, string name) {
            var dataTemplate = new DataTemplate();
            var stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
            var imageFactory = new FrameworkElementFactory(typeof(Image));
            var labelFactory = new FrameworkElementFactory(typeof(Label));
            imageFactory.SetValue(FrameworkElement.HeightProperty, (double) 24);
            imageFactory.SetValue(FrameworkElement.WidthProperty, (double) 24);
            imageFactory.SetValue(FrameworkElement.ToolTipProperty, name);
            imageFactory.SetValue(Image.SourceProperty, image);
            Binding binding = BindingHelper.VisibilityBinding(Settings.Default, "EnableHelpLabels");
            labelFactory.SetBinding(UIElement.VisibilityProperty, binding);
            labelFactory.SetValue(ContentControl.ContentProperty, name);
            stackPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            stackPanelFactory.AppendChild(imageFactory);
            stackPanelFactory.AppendChild(labelFactory);
            dataTemplate.VisualTree = stackPanelFactory;
            return dataTemplate;
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
                var iconfile = $"{Path.GetDirectoryName(pluginInstance.AssemblyPath)}\\{pluginInstance.Instance.Icon}";
                BitmapImage icon = Theme.DefaultPluginLogo;
                icon = File.Exists(iconfile)
                           ? ImageUtilities.LoadImageFromStream(iconfile)
                           : icon;
                tabItem.HeaderTemplate = ImageHeader(icon, pluginInstance.Instance.FriendlyName);
                AppViewModel.Instance.PluginTabItems.Add(tabItem);
            }
            catch (Exception ex) {
                Logging.Log(Logger, new LogItem(ex, true));
            }
        }
    }
}