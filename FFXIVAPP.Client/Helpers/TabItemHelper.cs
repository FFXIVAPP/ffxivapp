// FFXIVAPP.Client
// TabItemHelper.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Client.ViewModels.Plugins.Log;
using FFXIVAPP.Common.Controls;
using FFXIVAPP.Common.Helpers;

#endregion

namespace FFXIVAPP.Client.Helpers
{
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
            labelFactory.SetValue(FrameworkElement.NameProperty, "FriendlyName");
            stackPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            stackPanelFactory.AppendChild(imageFactory);
            stackPanelFactory.AppendChild(labelFactory);
            dataTemplate.VisualTree = stackPanelFactory;
            return dataTemplate;
        }

        public static class Log
        {
            /// <summary>
            /// </summary>
            public static void AddTabByName(string xKey, string xValue, string xRegularExpression)
            {
                xKey = Regex.Replace(xKey, "[^a-zA-Z]", "");
                var tabItem = new TabItem
                {
                    Header = xKey
                };
                var flowDoc = new xFlowDocument();
                foreach (var code in xValue.Split(','))
                {
                    flowDoc.Codes.Items.Add(code);
                }
                flowDoc.RegEx.Text = xRegularExpression;
                var binding = BindingHelper.ZoomBinding(Settings.Default, "Zoom");
                flowDoc._FDR.SetBinding(FlowDocumentReader.ZoomProperty, binding);
                tabItem.Content = flowDoc;
                PluginViewModel.Instance.Tabs.Add(tabItem);
                ThemeHelper.SetupFont(ref flowDoc);
                ThemeHelper.SetupColor(ref flowDoc);
            }
        }
    }
}
