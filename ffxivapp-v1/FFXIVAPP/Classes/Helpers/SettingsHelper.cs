// FFXIVAPP
// SettingsHelper.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using FFXIVAPP.Properties;
using FFXIVAPP.ViewModels;
using NLog;

#endregion

namespace FFXIVAPP.Classes.Helpers
{
    public static class SettingsHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// </summary>
        public static void Save()
        {
            //UPDATE SETTINGS
            XmlHelper.DeleteXMLNode(Constants.XSettings, "Tab");
            if (LogVM.TabNames.Count > 0)
            {
                for (var i = 0; i <= LogVM.TabNames.Count - 1; i++)
                {
                    var tabname = LogVM.TabNames[i].ToString();
                    var code = LogVM.ChatScan[i].Items.Cast<object>().Aggregate("", (current, item) => current + (item.ToString().Split(',')[0].Replace("[", "") + ","));
                    code = code.Substring(0, code.Length - 1);
                    var regex = LogVM.RegExs[i].ToString();
                    XmlHelper.SaveXMLNode(Constants.XSettings, "Tab", "Settings", tabname, code, regex);
                }
            }
            //UPDATE COLORS
            XmlHelper.DeleteXMLNode(Constants.XColors, "Color");
            var items = Constants.XColor.Select(item => new XValuePairs {
                Key = item.Key,
                Value = item.Value[0],
                Desc = item.Value[1]
            });
            foreach (var item in items)
            {
                XmlHelper.SaveXMLNode(Constants.XColors, "Color", "Colors", item.Key, item.Value, item.Desc);
            }
            //UPDATE EVENTS
            XmlHelper.DeleteXMLNode(Constants.XEvents, "Event");
            items = Constants.XEvent.Select(item => new XValuePairs {
                Key = item.Key,
                Value = item.Value
            });
            foreach (var item in items)
            {
                XmlHelper.SaveXMLNode(Constants.XEvents, "Event", "Events", item.Key, item.Value);
            }
            //SAVE ALL SETTINGS
            Constants.XSettings.Save("./Settings/Settings.xml");
            Constants.XColors.Save("./Settings/Colors.xml");
            Constants.XEvents.Save("./Settings/Events.xml");
            Settings.Default.Save();
        }

        /// <summary>
        /// </summary>
        public static void Default()
        {
            try
            {
                var p = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Assembly.GetExecutingAssembly().GetName().Name);
                var result = MessageBox.Show(String.Format("Delete : {0}", p), "WARNING!", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Settings.Default.Reset();
                    Settings.Default.Reload();
                    Directory.Delete(p, true);
                    Save();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
