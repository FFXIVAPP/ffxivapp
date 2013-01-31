// FFXIVAPP.Plugin.Log
// TabItemHelper.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.Text.RegularExpressions;
using System.Windows.Controls;
using FFXIVAPP.Common.Controls;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Plugin.Log.Properties;

#endregion

namespace FFXIVAPP.Plugin.Log.Helpers
{
    internal static class TabItemHelper
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
        }
    }
}
