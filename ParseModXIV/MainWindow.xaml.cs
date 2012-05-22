// ParseModXIV
// MainWindow.xaml.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;
using AppModXIV.Classes;
using ParseModXIV.Classes;
using Application = System.Windows.Application;
using Color = System.Windows.Media.Color;
using FontFamily = System.Windows.Media.FontFamily;

namespace ParseModXIV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IDisposable
    {
        #region " VARIABLES "

        public readonly string Lpath = "";
        public readonly string Ipath = "";
        public int Counter;
        public string Mysql = "";
        private readonly AutomaticUpdates _autoUpdates = new AutomaticUpdates();
        public static MainWindow View;
        private DispatcherTimer _expTimer = new DispatcherTimer();
        private NotifyIcon _myNotifyIcon;
        private readonly XDocument _xAtCodes = XDocument.Load("./Resources/ATCodes.xml");
        private readonly XDocument _xSettings = XDocument.Load("./Resources/Settings_Parse.xml");
        private Color _tsColor;
        private Color _bColor;
        public static readonly List<string[]> BattleLog = new List<string[]>();
        public static readonly List<string[]> HealingLog = new List<string[]>();

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            var rd = new ResourceDictionary {Source = new Uri("pack://application:,,,/ParseModXIV;component/ParseModXIV.xaml")};
            Resources.MergedDictionaries.Add(rd);
            if (File.Exists("./Resources/Themes/ParseModXIV.xaml"))
            {
                rd = (ResourceDictionary) XamlReader.Load(XmlReader.Create("./Resources/Themes/ParseModXIV.xaml"));
                Resources.MergedDictionaries.Add(rd);
            }
            var ci = CultureInfo.CurrentUICulture;
            switch (ci.TwoLetterISOLanguageName)
            {
                case "ja":
                    Settings.Default.Language = "Japanese";
                    break;
                case "de":
                    Settings.Default.Language = "German";
                    break;
                case "fr":
                    Settings.Default.Language = "French";
                    break;
                default:
                    Settings.Default.Language = "English";
                    break;
            }
            var dict = new ResourceDictionary {Source = new Uri(String.Format("pack://application:,,,/ParseModXIV;component/Localization/{0}.xaml", Settings.Default.Language))};
            Resources.MergedDictionaries.Add(dict);
            InitializeComponent();
            Main_ToolBar_View.gui_Maximize.Visibility = Visibility.Visible;
            Main_ToolBar_View.gui_Restore.Visibility = Visibility.Collapsed;
            View = this;
            Lpath = "./Logs/ParseMod/";
            Ipath = "./ScreenShots/ParseMod/";
            if (!Directory.Exists(Lpath))
            {
                Directory.CreateDirectory(Lpath);
            }
            if (!Directory.Exists(Ipath))
            {
                Directory.CreateDirectory(Ipath);
            }
            _autoUpdates.CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Func<bool> checkUpdates = () => _autoUpdates.CheckUpdates("ParseModXIV");
            Func<bool> checkLibrary = () => _autoUpdates.CheckDlls("AppModXIV", "");
            checkUpdates.BeginInvoke(appresult =>
            {
                const int bTipTime = 3000;
                if (checkUpdates.EndInvoke(appresult))
                {
                    switch (Settings.Default.Language)
                    {
                        case "English":
                            _myNotifyIcon.ShowBalloonTip(bTipTime, "Update Available!", "Click this message to download.", ToolTipIcon.Info);
                            break;
                        case "French":
                            _myNotifyIcon.ShowBalloonTip(bTipTime, "Update Available!", "Cliquez sur ce message pour télécharger.", ToolTipIcon.Info);
                            break;
                        case "Japanese":
                            _myNotifyIcon.ShowBalloonTip(bTipTime, "利用可能な更新！", "ダウンロードするにはこのメッセージをクリックします。", ToolTipIcon.Info);
                            break;
                        case "German":
                            _myNotifyIcon.ShowBalloonTip(bTipTime, "Update Verfügbar!", "Klicken sie auf diese nachricht zu downloaden.", ToolTipIcon.Info);
                            break;
                    }
                }
                else
                {
                    checkLibrary.BeginInvoke(libresult =>
                    {
                        if (checkLibrary.EndInvoke(libresult))
                        {
                            switch (Settings.Default.Language)
                            {
                                case "English":
                                    _myNotifyIcon.ShowBalloonTip(bTipTime, "Update Available!", "Click this message to download.", ToolTipIcon.Info);
                                    break;
                                case "French":
                                    _myNotifyIcon.ShowBalloonTip(bTipTime, "Update Available!", "Cliquez sur ce message pour télécharger.", ToolTipIcon.Info);
                                    break;
                                case "Japanese":
                                    _myNotifyIcon.ShowBalloonTip(bTipTime, "利用可能な更新！", "ダウンロードするにはこのメッセージをクリックします。", ToolTipIcon.Info);
                                    break;
                                case "German":
                                    _myNotifyIcon.ShowBalloonTip(bTipTime, "Update Verfügbar!", "Klicken sie auf diese nachricht zu downloaden.", ToolTipIcon.Info);
                                    break;
                            }
                        }
                    }, null);
                }
            }, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MyNotifyIconBalloonTipClicked(object sender, EventArgs e)
        {
            //Process.Start("http://ffxiv-app.com/products/");
            //var proc = Process.GetProcessesByName("ParseModXIV");
            //foreach (var p in proc)
            //{
            //    p.Kill();
            //}
            Process.Start("UpdateModXIV.exe", "ParseModXIV");
        }

        #region " FORM OPEN-CLOSE-STATES "

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateColors();
            UpdateFonts();
            Start();
            LoadXml();
            ApplySettings();
            if (Settings.Default.DebugMode)
            {
            }
            if (App.MArgs == null)
            {
                return;
            }
            if (File.Exists(App.MArgs[0]))
            {
                ProcessXml(App.MArgs[0]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        private static void ProcessXml(string filePath)
        {
            using (var reader = new XmlTextReader(filePath))
            {
                string key, time;
                var value = key = time = "";
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            while (reader.MoveToNextAttribute())
                            {
                                if (reader.Name == "Value")
                                {
                                    value = reader.Value;
                                }
                                if (reader.Name == "Key")
                                {
                                    key = reader.Value;
                                }
                                if (reader.Name == "Time")
                                {
                                    time = reader.Value;
                                }
                            }
                            break;
                    }
                    if (value == "" || key == "" || time == "")
                    {
                        continue;
                    }
                    ChatWorkerDelegate.OnDebugline(time, key, value);
                    value = key = time = "";
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (Constants.FfxivOpen)
            {
                ParseMod.Instance.StopLogging();
            }
            if (App.MArgs == null)
            {
                if (Settings.Default.Gui_ExportXML)
                {
                    //StatGroupToXml.ExportParty();
                    //StatGroupToXml.ExportMonsterStats();
                    //StatGroupToXml.ExportBattleLog();
                    //StatGroupToXml.ExportHealingLog();
                }
                if (Settings.Default.Gui_SaveLog)
                {
                    if (ChatWorkerDelegate.XmlWriteLog.LineCount > 1)
                    {
                        ChatWorkerDelegate.XmlWriteLog.WriteToDisk(Lpath + DateTime.Now.ToString("dd.MM.yyyy.HH.mm.ss") + "_Log.xml");
                    }
                }
                if (ChatWorkerDelegate.XmlWriteUnmatchedLog.LineCount > 1)
                {
                    ChatWorkerDelegate.XmlWriteUnmatchedLog.WriteToDisk(Lpath + DateTime.Now.ToString("dd.MM.yyyy.HH.mm.ss") + "_Unmatched_Log.xml");
                }
            }
            else if (Settings.Default.DebugMode)
            {
                if (ChatWorkerDelegate.XmlWriteUnmatchedLog.LineCount > 1)
                {
                    ChatWorkerDelegate.XmlWriteUnmatchedLog.WriteToDisk(Lpath + DateTime.Now.ToString("dd.MM.yyyy.HH.mm.ss") + "_Unmatched_Log.xml");
                }
            }
            _myNotifyIcon.Visible = false;
            Application.Current.MainWindow.WindowState = WindowState.Normal;
            Settings.Default.Save();
            GC.Collect();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_StateChanged(object sender, EventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Minimized:
                    ShowInTaskbar = false;
                    //_myNotifyIcon.Visible = true;
                    _myNotifyIcon.ContextMenu.MenuItems[0].Enabled = true;
                    break;
                case WindowState.Normal:
                    //_myNotifyIcon.Visible = false;
                    ShowInTaskbar = true;
                    _myNotifyIcon.ContextMenu.MenuItems[0].Enabled = false;
                    break;
            }
        }

        #endregion

        #region " INITIAL LOAD FUNCTIONS "

        /// <summary>
        /// 
        /// </summary>
        private void Start()
        {
            MainMenuView.LayoutRoot.Visibility = Visibility.Visible;
            MainStatusView.LayoutRoot.Visibility = Visibility.Visible;
            try
            {
                MainTabControlViewModel.gui_TabControl.ItemContainerStyle = (Style) MainTabControlViewModel.FindResource("TabItemVisible");
            }
            catch
            {
                var s = new Style();
                s.Setters.Add(new Setter(VisibilityProperty, Visibility.Visible));
                MainTabControlViewModel.gui_TabControl.ItemContainerStyle = s;
            }
            var streamResourceInfo = Application.GetResourceStream(new Uri("pack://application:,,,/ParseModXIV;component/ParseModXIV.ico"));
            if (streamResourceInfo != null)
            {
                using (var iconStream = streamResourceInfo.Stream)
                {
                    _myNotifyIcon = new NotifyIcon {Icon = new Icon(iconStream), Visible = true};
                    iconStream.Dispose();
                    _myNotifyIcon.Text = "ParseModXIV - Minimized";
                    var myNotify = new ContextMenu();
                    myNotify.MenuItems.Add("&Restore Application").Enabled = false;
                    myNotify.MenuItems.Add("&Exit");
                    myNotify.MenuItems[0].Click += Restore_Click;
                    myNotify.MenuItems[1].Click += Exit_Click;
                    _myNotifyIcon.ContextMenu = myNotify;
                    _myNotifyIcon.MouseDoubleClick += MyNotifyIcon_MouseDoubleClick;
                    _myNotifyIcon.BalloonTipClicked += MyNotifyIconBalloonTipClicked;
                }
            }
            ParseMod.Instance.StartLogging();
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadXml()
        {
            var items = from item in _xAtCodes.Descendants("Code") select new XValuePairs {Key = (string) item.Attribute("Key"), Value = (string) item.Attribute("Value")};
            foreach (var item in items)
            {
                Constants.XAtCodes.Add(item.Key, item.Value);
            }
            items = from item in _xSettings.Descendants("Server") select new XValuePairs {Key = (string) item.Attribute("Key"), Value = (string) item.Attribute("Value")};
            foreach (var item in items)
            {
                ParseMod.ServerName.Add(item.Value, item.Key);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static void ApplySettings()
        {
            Constants.LogErrors = Settings.Default.LogErrors ? 1 : 0;
            if (Settings.Default.Server != "")
            {
                Settings.Default.ServerName = ParseMod.ServerName[Settings.Default.Server];
            }
        }

        #endregion

        #region " FONTS AND COLORS "

        private void UpdateFonts()
        {
            if (Settings.Default.Gui_LogFont == null)
            {
                return;
            }
            var font = Settings.Default.Gui_LogFont;
            MainTabControlViewModel.MobAbility_FLOW.FontFamily = new FontFamily(font.Name);
            MainTabControlViewModel.MobAbility_FLOW.FontWeight = font.Bold ? FontWeights.Bold : FontWeights.Regular;
            MainTabControlViewModel.MobAbility_FLOW.FontStyle = font.Italic ? FontStyles.Italic : FontStyles.Normal;
            MainTabControlViewModel.MobAbility_FLOW.FontSize = font.Size;
            font.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateColors()
        {
            _tsColor = Settings.Default.Color_TimeStamp;
            _bColor = Settings.Default.Color_ChatlogBackground;
            var tColor = new SolidColorBrush {Color = _bColor};
            MainTabControlViewModel.MobAbility_FLOW.Background = tColor;
        }

        #endregion

        #region " NOTIFY FUNCTIONS "

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Restore_Click(object sender, EventArgs e)
        {
            WindowState = WindowState.Normal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Exit_Click(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region " PARSEMOD OPTIONS "

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyNotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WindowState = WindowState.Normal;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="insert"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool SubmitData(string insert, string message)
        {
            var url = string.Format("http://ffxiv-app.com/battles/insert/?insert={0}&q={1}", insert, HttpUtility.UrlEncode(message));
            var httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";

            var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var responseText = streamReader.ReadToEnd();
                return true;
            }
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _myNotifyIcon.Dispose();
        }

        #endregion
    }
}