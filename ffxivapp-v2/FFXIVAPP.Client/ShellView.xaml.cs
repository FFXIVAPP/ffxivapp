// FFXIVAPP.Client
// ShellView.xaml.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Models;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.Utilities;
using NLog;

#endregion

namespace FFXIVAPP.Client
{
    /// <summary>
    ///     Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView
    {
        public static ShellView View;

        public ShellView()
        {
            InitializeComponent();
            View = this;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void MetroWindowLoaded(object sender, RoutedEventArgs e)
        {
            LocaleHelper.Update(Settings.Default.Culture);
            ThemeHelper.ChangeTheme(Settings.Default.Theme);
            Initializer.CheckUpdates();
            Initializer.SetGlobals();
            Initializer.SetSignatures();
            Initializer.StartLogging();
            AppViewModel.Instance.NotifyIcon.Text = "FFXIVAPP";
            AppViewModel.Instance.NotifyIcon.ContextMenu.MenuItems[0].Enabled = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindowStateChanged(object sender, EventArgs e)
        {
            switch (View.WindowState)
            {
                case WindowState.Minimized:
                    ShowInTaskbar = false;
                    AppViewModel.Instance.NotifyIcon.Text = "FFXIVAPP - Minimized";
                    AppViewModel.Instance.NotifyIcon.ContextMenu.MenuItems[0].Enabled = true;
                    break;
                case WindowState.Normal:
                    ShowInTaskbar = true;
                    AppViewModel.Instance.NotifyIcon.Text = "FFXIVAPP";
                    AppViewModel.Instance.NotifyIcon.ContextMenu.MenuItems[0].Enabled = false;
                    break;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void MetroWindowClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            DispatcherHelper.Invoke(() => CloseApplication());
        }
        
        /// <summary>
        /// </summary>
        /// <param name="update"></param>
        public static void CloseApplication(bool update = false)
        {
            Application.Current.MainWindow.WindowState = WindowState.Normal;
            SettingsHelper.Save();
            foreach (PluginInstance pluginInstance in App.Plugins.Loaded)
            {
                pluginInstance.Instance.Dispose();
            }
            if (!Settings.Default.SaveLog || !AppViewModel.Instance.ChatHistory.Any())
            {
                CloseDelegate(update);
            }
            var popupContent = new PopupContent();
            popupContent.Title = AppViewModel.Instance.Locale["app_InformationMessage"];
            popupContent.Message = AppViewModel.Instance.Locale["app_SaveHistoryMessage"];
            popupContent.CanSayNo = true;
            PopupHelper.Toggle(popupContent);
            EventHandler closedDelegate = null;
            closedDelegate = delegate
            {
                switch (PopupHelper.Result)
                {
                    case MessageBoxResult.Yes:
                        Initializer.StopLogging();
                        Func<bool> exportHistory = delegate
                        {
                            try
                            {
                                var savedLogName = DateTime.Now.ToString("dd.MM.yyyy.HH.mm.ss") + "_ChatHistory.xml";
                                var savedLog = ResourceHelper.XDocResource(Common.Constants.AppPack + "Defaults/ChatHistory.xml");
                                foreach (var entry in AppViewModel.Instance.ChatHistory)
                                {
                                    var xCode = entry.Code;
                                    var xBytes = entry.Bytes.Aggregate("", (current, bytes) => current + (bytes + " "))
                                                      .Trim();
                                    //var xCombined = entry.Combined;
                                    //var xJP = entry.JP.ToString();
                                    var xLine = entry.Line;
                                    //var xRaw = entry.Raw;
                                    var xTimeStamp = entry.TimeStamp.ToString("[HH:mm:ss]");
                                    var keyPairList = new List<XValuePair>();
                                    keyPairList.Add(new XValuePair
                                    {
                                        Key = "Bytes",
                                        Value = xBytes
                                    });
                                    //keyPairList.Add(new XValuePair {Key = "Combined", Value = xCombined});
                                    //keyPairList.Add(new XValuePair {Key = "JP", Value = xJP});
                                    keyPairList.Add(new XValuePair
                                    {
                                        Key = "Line",
                                        Value = xLine
                                    });
                                    //keyPairList.Add(new XValuePair {Key = "Raw", Value = xRaw});
                                    keyPairList.Add(new XValuePair
                                    {
                                        Key = "TimeStamp",
                                        Value = xTimeStamp
                                    });
                                    XmlHelper.SaveXmlNode(savedLog, "History", "Entry", xCode, keyPairList);
                                }
                                savedLog.Save(AppViewModel.Instance.LogsPath + savedLogName);
                            }
                            catch (Exception ex)
                            {
                                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
                            }
                            return true;
                        };
                        exportHistory.BeginInvoke(delegate
                        {
                            CloseDelegate(update);
                        }, exportHistory);
                        break;
                    case MessageBoxResult.No:
                        CloseDelegate(update);
                        break;
                }
                PopupHelper.MessagePopup.Closed -= closedDelegate;
            };
            PopupHelper.MessagePopup.Closed += closedDelegate;
        }

        /// <summary>
        /// </summary>
        /// <param name="update"></param>
        private static void CloseDelegate(bool update = false)
        {
            AppViewModel.Instance.NotifyIcon.Visible = false;
            if (update)
            {
                var updaters = Process.GetProcessesByName("FFXIVAPP.Updater");
                foreach (var updater in updaters)
                {
                    updater.Kill();
                }
                try
                {
                    File.Move("FFXIVAPP.Updater.exe", "FFXIVAPP.Updater.Backup.exe");
                    Process.Start("FFXIVAPP.Updater.Backup.exe", String.Format("{0} {1}", AppViewModel.Instance.DownloadUri, AppViewModel.Instance.LatestVersion));
                }
                catch (Exception ex)
                {
                }
            }
            Environment.Exit(0);
        }
    }
}