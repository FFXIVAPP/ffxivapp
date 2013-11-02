// FFXIVAPP.Client
// TabItemHelper.cs
// 
// © 2013 Ryan Wilson

using System.Text.RegularExpressions;
using System.Windows.Controls;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Controls;
using FFXIVAPP.Common.Helpers;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Plugins.Log.Helpers
{
    [DoNotObfuscate]
    public static class TabItemHelper
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
