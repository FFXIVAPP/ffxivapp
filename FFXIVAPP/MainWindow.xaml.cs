// FFXIVAPP
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
using System.Text.RegularExpressions;
using System.Web;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;
using FFXIVAPP.Classes;
using FFXIVAPP.Classes.Helpers;
using FFXIVAPP.Classes.RegExs;
using FFXIVAPP.ViewModels;
using FFXIVAPP.Views;
using MahApps.Metro;
using Application = System.Windows.Application;
using Color = System.Windows.Media.Color;
using FontFamily = System.Windows.Media.FontFamily;

namespace FFXIVAPP
{
    /// <summary>
    ///   Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IDisposable
    {
        public readonly string Lpath = "./Logs/";
        public readonly string Ipath = "./ScreenShots/";
        public int Counter;
        public string Mysql = "";
        private readonly AutomaticUpdates _autoUpdates = new AutomaticUpdates();
        private static readonly DispatcherTimer ExpTimer = new DispatcherTimer();
        public static NotifyIcon MyNotifyIcon;
        private Color _tsColor;
        private Color _bColor;
        private readonly XDocument _xAtCodes = XDocument.Load("./Resources/ATCodes.xml");
        private readonly XDocument _xColors = XDocument.Load("./Resources/Colors.xml");
        private readonly XDocument _xSettings = XDocument.Load("./Resources/Settings.xml");
        private readonly XDocument _xRegEx = XDocument.Load("./Resources/RegularExpressions.xml");
        public static readonly List<string[]> BattleLog = new List<string[]>();
        public static readonly List<string[]> HealingLog = new List<string[]>();
        public static string Lang = "en";
        public static MainWindow View;
        public static DateTime StartTime;
        private static TimeSpan _timeElapsed;
        public static int TotalExp;

        public MainWindow()
        {
            InitializeComponent();
            // Insert code required on object creation below this point.
            View = this;
            //MessageBox.Show(AppDomain.CurrentDomain.FriendlyName);
            StartTime = DateTime.Now;
        }

        #region Open-Close-States

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ApplyLocale();
            CreateNotify();
            LoadXml();
            ApplySettings();
            ApplyTheme();
            ApplyColor();
            ApplyFonts();
            CheckUpdates();
            Start();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void MetroWindow_Closing(object sender, CancelEventArgs e)
        {
            Settings.Default.Save();
            if (Constants.FFXIVOpen)
            {
                FFXIV.StopLogging();
            }
            if (App.MArgs == null)
            {
                if (Settings.Default.Parse_ExportXML)
                {
                    //StatGroupToXml.ExportParty();
                    //StatGroupToXml.ExportMonsterStats();
                    //StatGroupToXml.ExportBattleLog();
                    //StatGroupToXml.ExportHealingLog();
                }

                #region Save Parse Log

                if (Settings.Default.Parse_SaveLog)
                {
                    if (ChatWorkerDelegate.ParseXmlWriteLog.LineCount > 1)
                    {
                        ChatWorkerDelegate.ParseXmlWriteLog.WriteToDisk(Lpath + DateTime.Now.ToString("dd.MM.yyyy.HH.mm.ss") + "_Parse_Log.xml");
                    }
                    if (ChatWorkerDelegate.ParseXmlWriteUnmatchedLog.LineCount > 1)
                    {
                        ChatWorkerDelegate.ParseXmlWriteUnmatchedLog.WriteToDisk(Lpath + DateTime.Now.ToString("dd.MM.yyyy.HH.mm.ss") + "_Parse_Unmatched_Log.xml");
                    }
                }

                #endregion

                #region Save Log

                if (Settings.Default.Log_SaveLog)
                {
                    if (ChatWorkerDelegate.LogXmlWriteLog.LineCount > 1)
                    {
                        ChatWorkerDelegate.LogXmlWriteLog.WriteToDisk(Lpath + DateTime.Now.ToString("dd.MM.yyyy.HH.mm.ss") + "_Log.xml");
                    }
                }

                #endregion
            }
            else if (Settings.Default.DebugMode)
            {
                if (ChatWorkerDelegate.ParseXmlWriteUnmatchedLog.LineCount > 1)
                {
                    ChatWorkerDelegate.ParseXmlWriteUnmatchedLog.WriteToDisk(Lpath + DateTime.Now.ToString("dd.MM.yyyy.HH.mm.ss") + "_Parse_Unmatched_Log.xml");
                }
            }
            XmlHelper.DeleteXMLNode(_xSettings, "Tab");
            if (LogVM.TabNames.Count > 0)
            {
                for (var i = 0; i <= LogVM.TabNames.Count - 1; i++)
                {
                    var tabname = LogVM.TabNames[i].ToString();
                    var code = LogVM.ChatScan[i].Items.Cast<object>().Aggregate("", (current, item) => current + (item.ToString().Split(',')[0].Replace("[", "") + ","));
                    code = code.Substring(0, code.Length - 1);
                    var regex = LogVM.RegExs[i].ToString();
                    XmlHelper.SaveXMLNode(_xSettings, "Tab", "Settings", tabname, code, regex);
                }
            }
            XmlHelper.DeleteXMLNode(_xColors, "Color");
            var items = Constants.XColor.Select(item => new XValuePairs {Key = item.Key, Value = item.Value[0], Desc = item.Value[1]});
            foreach (var item in items)
            {
                XmlHelper.SaveXMLNode(_xColors, "Color", "Colors", item.Key, item.Value, item.Desc);
            }
            MyNotifyIcon.Visible = false;
            Application.Current.MainWindow.WindowState = WindowState.Normal;
            _xSettings.Save("./Resources/Settings.xml");
            _xColors.Save("./Resources/Colors.xml");
            GC.Collect();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void MetroWindow_StateChanged(object sender, EventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Minimized:
                    ShowInTaskbar = false;
                    MyNotifyIcon.Text = "FFXIVAPP - Minimized";
                    MyNotifyIcon.ContextMenu.MenuItems[0].Enabled = true;
                    break;
                case WindowState.Normal:
                    ShowInTaskbar = true;
                    MyNotifyIcon.Text = "FFXIVAPP";
                    MyNotifyIcon.ContextMenu.MenuItems[0].Enabled = false;
                    break;
            }
        }

        #endregion

        #region Loading Functions

        /// <summary>
        /// </summary>
        private void ApplyLocale()
        {
            ResourceDictionary dict;
            Lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            switch (Lang)
            {
                case "ja":
                    dict = new ResourceDictionary {Source = new Uri("pack://application:,,,/FFXIVAPP;component/Localization/Japanese.xaml")};
                    Settings.Default.TranslateJPOnly = false;
                    Settings.Default.TranslateTo = "Japanese";
                    break;
                case "de":
                    dict = new ResourceDictionary {Source = new Uri("pack://application:,,,/FFXIVAPP;component/Localization/German.xaml")};
                    Settings.Default.TranslateJPOnly = false;
                    Settings.Default.TranslateTo = "German";
                    break;
                case "fr":
                    dict = new ResourceDictionary {Source = new Uri("pack://application:,,,/FFXIVAPP;component/Localization/French.xaml")};
                    Settings.Default.TranslateJPOnly = false;
                    Settings.Default.TranslateTo = "French";
                    break;
                default:
                    dict = new ResourceDictionary {Source = new Uri("pack://application:,,,/FFXIVAPP;component/Localization/English.xaml")};
                    break;
            }
            Resources.MergedDictionaries.Add(dict);
        }

        /// <summary>
        /// </summary>
        private void CreateNotify()
        {
            var streamResourceInfo = Application.GetResourceStream(new Uri("pack://application:,,,/FFXIVAPP;component/Resources/FFXIVAPP.ico"));
            if (streamResourceInfo != null)
            {
                using (var iconStream = streamResourceInfo.Stream)
                {
                    MyNotifyIcon = new NotifyIcon {Icon = new Icon(iconStream), Visible = true};
                    iconStream.Dispose();
                    MyNotifyIcon.Text = "FFXIVAPP";
                    var myNotify = new ContextMenu();
                    myNotify.MenuItems.Add("&Restore Application").Enabled = false;
                    myNotify.MenuItems.Add(ResourceHelper.StringR("loc_ManualUpdate"));
                    myNotify.MenuItems.Add("&Exit");
                    myNotify.MenuItems[0].Click += Restore_Click;
                    myNotify.MenuItems[1].Click += Update_Click;
                    myNotify.MenuItems[2].Click += Exit_Click;
                    MyNotifyIcon.ContextMenu = myNotify;
                    MyNotifyIcon.MouseDoubleClick += MyNotifyIcon_MouseDoubleClick;
                }
            }
        }

        /// <summary>
        /// </summary>
        private void LoadXml()
        {
            //LOAD AUTOTRANSLATE CODES
            var items = _xAtCodes.Descendants("Code").Select(item => new XValuePairs {Key = (string) item.Attribute("Key"), Value = (string) item.Attribute("Value")});
            foreach (var item in items)
            {
                Constants.XAtCodes.Add(item.Key, item.Value);
            }
            //LOAD CUSTOM PLAYER REGEX
            items = _xRegEx.Descendants("Player").Select(item => new XValuePairs {ID = (string) item.Attribute("ID"), Value = item.FirstNode.ToString()});
            foreach (var item in items)
            {
                var v = (item.Value.Length == 12) ? "" : item.Value.Substring(9, item.Value.Length - 12);
                if (String.IsNullOrWhiteSpace(v))
                {
                    continue;
                }
                if (Constants.IsValidRegex(v))
                {
                    Constants.XPlayerRegEx.Add(item.ID, new Regex(v, Shared.DefaultOptions));
                }
            }
            //LOAD CUSTOM MONSTER REGEX
            items = _xRegEx.Descendants("Monster").Select(item => new XValuePairs {ID = (string) item.Attribute("ID"), Value = item.FirstNode.ToString()});
            foreach (var item in items)
            {
                var v = (item.Value.Length == 12) ? "" : item.Value.Substring(9, item.Value.Length - 12);
                if (String.IsNullOrWhiteSpace(v))
                {
                    continue;
                }
                if (Constants.IsValidRegex(v))
                {
                    Constants.XMonsterRegEx.Add(item.ID, new Regex(v, Shared.DefaultOptions));
                }
            }
            //LOAD CHATCODES
            var resourceUri = new Uri("pack://application:,,,/FFXIVAPP;component/Resources/ChatCodes.xml");
            var resource = Application.GetResourceStream(resourceUri);
            if (resource != null)
            {
                var xdoc = XElement.Load(resource.Stream);
                items = xdoc.Descendants("ChatCode").Select(item => new XValuePairs {ID = (string) item.Attribute("ID"), Desc = (string) item.Attribute("Desc")});
                foreach (var item in items)
                {
                    Constants.XChatCodes.Add(item.ID, item.Desc);
                }
            }
            foreach (var t in Constants.Settings)
            {
                LoadSettings(t);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="setting"> </param>
        private void LoadSettings(string setting)
        {
            var items = _xSettings.Descendants(setting).Select(item => new XValuePairs {Key = (string) item.Attribute("Key"), Value = (string) item.Attribute("Value"), RegEx = (string) item.Attribute("RegEx")});
            switch (setting)
            {
                case "Color":
                    items = _xColors.Descendants(setting).Select(item => new XValuePairs {Key = (string) item.Attribute("Key"), Value = (string) item.Attribute("Value"), Desc = (string) item.Attribute("Desc")});
                    break;
            }
            foreach (var item in items.Where(item => !String.IsNullOrWhiteSpace(item.Key) && !String.IsNullOrWhiteSpace(item.Value)))
            {
                switch (setting)
                {
                    case "Tab":
                        if (String.IsNullOrWhiteSpace(item.RegEx))
                        {
                            item.RegEx = "*";
                        }
                        var splitOfChatCodes = item.Value.Split(',').Where(t => t.Length == 4);
                        LogVM.AddTabPageName(item.Key, splitOfChatCodes, item.RegEx);
                        break;
                    case "Color":
                        if (String.IsNullOrWhiteSpace(item.Desc))
                        {
                            item.Desc = (Constants.XChatCodes.ContainsKey(item.Key)) ? Constants.XChatCodes[item.Key] : "Unknown";
                        }
                        Constants.XColor.Add(item.Key, new[] {item.Value, item.Desc});
                        break;
                }
            }
        }

        /// <summary>
        /// </summary>
        private static void ApplySettings()
        {
            //BUILD SERVER LOOKUP
            Constants.XServerName.Add("Durandal", "2");
            Constants.XServerName.Add("Hyperion", "3");
            Constants.XServerName.Add("Masamune", "4");
            Constants.XServerName.Add("Gungnir", "5");
            Constants.XServerName.Add("Aegis", "7");
            Constants.XServerName.Add("Sargatanas", "10");
            Constants.XServerName.Add("Balmung", "11");
            Constants.XServerName.Add("Ridill", "12");
            Constants.XServerName.Add("Excalibur", "16");
            Constants.XServerName.Add("Ragnarok", "20");
            if (!String.IsNullOrWhiteSpace(Settings.Default.Server))
            {
                Settings.Default.ServerName = Constants.XServerName[Settings.Default.Server];
            }
            LogV.View.TabControl.SelectedIndex = 2;
        }

        /// <summary>
        /// </summary>
        private void CheckUpdates()
        {
            //Func<bool> d = delegate
            //{
            //    return true;
            //};
            //d.BeginInvoke(null, null);
            _autoUpdates.CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Func<bool> checkUpdates = () => _autoUpdates.CheckUpdates("FFXIVAPP");
            checkUpdates.BeginInvoke(appresult =>
            {
                const int bTipTime = 3000;
                if (checkUpdates.EndInvoke(appresult))
                {
                    string title, message;
                    switch (Lang)
                    {
                        case "ja":
                            title = "利用可能な更新！";
                            message = "ダウンロードするにはこのメッセージをクリックします。";
                            break;
                        case "de":
                            title = "Update Verfügbar!";
                            message = "Klicken sie auf diese nachricht zu downloaden.";
                            break;
                        case "fr":
                            title = "Mise À Jour Possible!";
                            message = "Cliquez sur ce message pour télécharger.";
                            break;
                        default:
                            title = "Update Available!";
                            message = "Click this message to download.";
                            break;
                    }
                    MyNotifyIcon.BalloonTipClicked += MyNotifyIconBalloonTipClicked;
                    MyNotifyIcon.ShowBalloonTip(bTipTime, title, message, ToolTipIcon.Info);
                }
            }, null);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private static void MyNotifyIconBalloonTipClicked(object sender, EventArgs e)
        {
            Process.Start("Updater.exe", "FFXIVAPP");
            MyNotifyIcon.BalloonTipClicked -= MyNotifyIconBalloonTipClicked;
        }

        /// <summary>
        /// </summary>
        private void Start()
        {
            FFXIV.StartLogging();
            if (Settings.Default.DebugMode)
            {
                return;
            }
            ExpTimer.Tick += ExpTimer_Tick;
            ExpTimer.Interval = TimeSpan.FromMilliseconds(100);
            ExpTimer.Start();
            if (App.MArgs != null)
            {
                if (File.Exists(App.MArgs[0]))
                {
                    ProcessXml(App.MArgs[0]);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="filePath"> </param>
        private static void ProcessXml(string filePath)
        {
            using (var reader = new XmlTextReader(filePath))
            {
                string value, key, time;
                value = key = time = "";
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            while (reader.MoveToNextAttribute())
                            {
                                switch (reader.Name)
                                {
                                    case "Key":
                                        key = XmlCleaner.SanitizeXmlString(reader.Value);
                                        break;
                                    case "Value":
                                        value = XmlCleaner.SanitizeXmlString(reader.Value);
                                        break;
                                    case "Time":
                                        time = XmlCleaner.SanitizeXmlString(reader.Value);
                                        break;
                                }
                            }
                            break;
                    }
                    if (String.IsNullOrWhiteSpace(key) || String.IsNullOrWhiteSpace(value))
                    {
                        continue;
                    }
                    ChatWorkerDelegate.OnDebugline(time, key, value);
                    value = key = time = "";
                }
            }
        }

        #endregion

        #region Timers

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void ExpTimer_Tick(object sender, EventArgs e)
        {
            ExpTimer.Interval = TimeSpan.FromMilliseconds(1000);
            _timeElapsed = DateTime.Now - StartTime;
            var expPHour = Convert.ToInt32(TotalExp/(_timeElapsed.TotalMinutes/60));
            var t = ResourceHelper.StringR("loc_Total");
            MainV.View.Exp.Content = t + ": " + TotalExp.ToString(CultureInfo.InvariantCulture) + "   " + t + "/H: " + expPHour.ToString(CultureInfo.InvariantCulture);
            GC.Collect();
        }

        #endregion

        #region Themes, Fonts & Colors

        /// <summary>
        /// </summary>
        private void ApplyFonts()
        {
            if (Settings.Default.LogFont == null)
            {
                return;
            }
            var font = Settings.Default.LogFont;
            ChatView.Log._FD.FontFamily = new FontFamily(font.Name);
            ChatView.Log._FD.FontWeight = font.Bold ? FontWeights.Bold : FontWeights.Regular;
            ChatView.Log._FD.FontStyle = font.Italic ? FontStyles.Italic : FontStyles.Normal;
            ChatView.Log._FD.FontSize = font.Size;
            ParseView.MA._FD.FontFamily = new FontFamily(font.Name);
            ParseView.MA._FD.FontWeight = font.Bold ? FontWeights.Bold : FontWeights.Regular;
            ParseView.MA._FD.FontStyle = font.Italic ? FontStyles.Italic : FontStyles.Normal;
            ParseView.MA._FD.FontSize = font.Size;
            LogView.All._FD.FontFamily = new FontFamily(font.Name);
            LogView.All._FD.FontWeight = font.Bold ? FontWeights.Bold : FontWeights.Regular;
            LogView.All._FD.FontStyle = font.Italic ? FontStyles.Italic : FontStyles.Normal;
            LogView.All._FD.FontSize = font.Size;
            LogView.Translated._FD.FontFamily = new FontFamily(font.Name);
            LogView.Translated._FD.FontWeight = font.Bold ? FontWeights.Bold : FontWeights.Regular;
            LogView.Translated._FD.FontStyle = font.Italic ? FontStyles.Italic : FontStyles.Normal;
            LogView.Translated._FD.FontSize = font.Size;
            LogView.Debug._FD.FontFamily = new FontFamily(font.Name);
            LogView.Debug._FD.FontWeight = font.Bold ? FontWeights.Bold : FontWeights.Regular;
            LogView.Debug._FD.FontStyle = font.Italic ? FontStyles.Italic : FontStyles.Normal;
            LogView.Debug._FD.FontSize = font.Size;
            for (var a = 0; a <= LogVM.TabNames.Count - 1; a++)
            {
                var tempFlow = LogVM.DFlowDoc[LogVM.TabNames[a] + "_FD"];
                tempFlow.FontFamily = new FontFamily(font.Name);
                tempFlow.FontWeight = font.Bold ? FontWeights.Bold : FontWeights.Regular;
                tempFlow.FontStyle = font.Italic ? FontStyles.Italic : FontStyles.Normal;
                tempFlow.FontSize = font.Size;
            }
            font.Dispose();
        }

        /// <summary>
        /// </summary>
        private void ApplyColor()
        {
            _tsColor = Settings.Default.Color_TimeStamp;
            _bColor = Settings.Default.Color_ChatlogBackground;
            var tColor = new SolidColorBrush {Color = _bColor};
            ChatView.Log._FD.Background = tColor;
            ParseView.MA._FD.Background = tColor;
            LogView.All._FD.Background = tColor;
            LogView.Translated._FD.Background = tColor;
            LogView.Debug._FD.Background = tColor;
            for (var a = 0; a <= LogVM.TabNames.Count - 1; a++)
            {
                var tempFlow = LogVM.DFlowDoc[LogVM.TabNames[a] + "_FD"];
                tempFlow.Background = tColor;
            }
        }

        /// <summary>
        /// </summary>
        private static void ApplyTheme()
        {
            try
            {
                var split = Settings.Default.Theme.Split('|');
                var accent = split[0];
                var theme = split[1];
                switch (theme)
                {
                    case "Dark":
                        ThemeHelper.ChangeTheme(View, ThemeHelper.DefaultAccents.First(a => a.Name == accent), Theme.Dark);
                        break;
                    case "Light":
                        ThemeHelper.ChangeTheme(View, ThemeHelper.DefaultAccents.First(a => a.Name == accent), Theme.Light);
                        break;
                }
            }
            catch
            {
            }
        }

        #endregion

        #region Notify Functions

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void Restore_Click(object sender, EventArgs e)
        {
            WindowState = WindowState.Normal;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private static void Update_Click(object sender, EventArgs e)
        {
            Process.Start("Updater.exe", "FFXIVAPP");
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private static void Exit_Click(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region Options

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void MyNotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WindowState = WindowState.Normal;
        }

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="insert"> </param>
        /// <param name="message"> </param>
        /// <returns> </returns>
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
        }

        #endregion
    }
}