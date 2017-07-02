// FFXIVAPP.Client ~ TabItemHelper.cs
// 
// Copyright © 2007 - 2017 Ryan Wilson - All Rights Reserved
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using FFXIVAPP.Client.Models;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Client.ViewModels;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Utilities;
using NLog;

namespace FFXIVAPP.Client.Helpers
{
    internal static class TabItemHelper
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="image"> </param>
        /// <param name="name"> </param>
        /// <returns> </returns>
        public static DataTemplate ImageHeader(BitmapImage image, string name)
        {
            var dataTemplate = new DataTemplate();
            var stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
            var imageFactory = new FrameworkElementFactory(typeof(Image));
            var labelFactory = new FrameworkElementFactory(typeof(Label));
            imageFactory.SetValue(FrameworkElement.HeightProperty, (double) 24);
            imageFactory.SetValue(FrameworkElement.WidthProperty, (double) 24);
            imageFactory.SetValue(FrameworkElement.ToolTipProperty, name);
            imageFactory.SetValue(Image.SourceProperty, image);
            var binding = BindingHelper.VisibilityBinding(Settings.Default, "EnableHelpLabels");
            labelFactory.SetBinding(UIElement.VisibilityProperty, binding);
            labelFactory.SetValue(ContentControl.ContentProperty, name);
            stackPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            stackPanelFactory.AppendChild(imageFactory);
            stackPanelFactory.AppendChild(labelFactory);
            dataTemplate.VisualTree = stackPanelFactory;
            return dataTemplate;
        }

        public static void LoadPluginTabItem(PluginInstance pluginInstance)
        {
            try
            {
                if (!pluginInstance.Loaded)
                {
                    return;
                }
                var pluginName = pluginInstance.Instance.FriendlyName;
                if (SettingsViewModel.Instance.HomePluginList.Any(p => p.ToUpperInvariant()
                                                                        .StartsWith(pluginName.ToUpperInvariant())))
                {
                    pluginName = $"{pluginName}[{new Random().Next(1000, 9999)}]";
                }
                SettingsViewModel.Instance.HomePluginList.Add(pluginName);
                var tabItem = pluginInstance.Instance.CreateTab();
                tabItem.Name = Regex.Replace(pluginInstance.Instance.Name, @"[^A-Za-z]", "");
                var iconfile = $"{Path.GetDirectoryName(pluginInstance.AssemblyPath)}\\{pluginInstance.Instance.Icon}";
                var icon = new BitmapImage(new Uri(Common.Constants.DefaultIcon));
                icon = File.Exists(iconfile) ? ImageUtilities.LoadImageFromStream(iconfile) : icon;
                tabItem.HeaderTemplate = ImageHeader(icon, pluginInstance.Instance.FriendlyName);
                AppViewModel.Instance.PluginTabItems.Add(tabItem);
            }
            catch (Exception ex)
            {
                Logging.Log(Logger, ex.Message, ex);
            }
        }
    }
}
