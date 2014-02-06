// FFXIVAPP.Client
// TabItemHelper.cs
// 
// © 2013 Ryan Wilson

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
using FFXIVAPP.Client.Views;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Utilities;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Helpers
{
    [DoNotObfuscate]
    internal static class TabItemHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="image"> </param>
        /// <param name="name"> </param>
        /// <returns> </returns>
        public static DataTemplate ImageHeader(BitmapImage image, string name)
        {
            var dataTemplate = new DataTemplate();
            var stackPanelFactory = new FrameworkElementFactory(typeof (StackPanel));
            var imageFactory = new FrameworkElementFactory(typeof (Image));
            var labelFactory = new FrameworkElementFactory(typeof (Label));
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
                var pluginName = pluginInstance.Instance.FriendlyName;
                if (SettingsViewModel.Instance.HomePluginList.Any(p => p.ToUpperInvariant()
                                                                        .StartsWith(pluginName.ToUpperInvariant())))
                {
                    pluginName = String.Format("{0}[{1}]", pluginName, new Random().Next(1000, 9999));
                }
                SettingsViewModel.Instance.HomePluginList.Add(pluginName);
                var tabItem = pluginInstance.Instance.CreateTab();
                tabItem.Name = Regex.Replace(pluginInstance.Instance.Name, @"[^A-Za-z]", "");
                var iconfile = String.Format("{0}\\{1}", Path.GetDirectoryName(pluginInstance.AssemblyPath), pluginInstance.Instance.Icon);
                var icon = new BitmapImage(new Uri(Common.Constants.DefaultIcon));
                icon = File.Exists(iconfile) ? ImageUtilities.LoadImageFromStream(iconfile) : icon;
                tabItem.HeaderTemplate = ImageHeader(icon, pluginInstance.Instance.FriendlyName);
                AppViewModel.Instance.PluginTabItems.Add(tabItem);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
