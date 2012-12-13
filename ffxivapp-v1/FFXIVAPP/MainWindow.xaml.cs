// FFXIVAPP
// MainWindow.xaml.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

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
using FFXIVAPP.Properties;
using FFXIVAPP.ViewModels;
using FFXIVAPP.Views;
using Application = System.Windows.Application;
using Color = System.Windows.Media.Color;
using FontFamily = System.Windows.Media.FontFamily;
using WebBrowser = System.Windows.Controls.WebBrowser;

#endregion

namespace FFXIVAPP
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IDisposable
    {
        private static WebBrowser _ga = new WebBrowser();
        private static readonly DispatcherTimer ExpTimer = new DispatcherTimer();
        public static NotifyIcon MyNotifyIcon;
        public static readonly List<string[]> BattleLog = new List<string[]>();
        public static readonly List<string[]> HealingLog = new List<string[]>();
        public static string Lang = "en";
        public static MainWindow View;
        public static DateTime StartTime;
        private static TimeSpan _timeElapsed;
        public static int TotalExp;
        public readonly string Ipath = "./ScreenShots/";
        public readonly string Lpath = "./Logs/";
        private readonly AutomaticUpdates _autoUpdates = new AutomaticUpdates();
        private readonly XDocument _xRegEx = XDocument.Load("./Settings/RegularExpressions.xml");
        public int Counter;
        public string Mysql = "";
        private Color _bColor;
        private Color _tsColor;

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
            View.WindowState = WindowState.Normal;
            SettingsHelper.Save();
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
            MyNotifyIcon.Visible = false;
            GC.Collect();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void MetroWindow_StateChanged(object sender, EventArgs e)
        {
            switch (View.WindowState)
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
                    dict = new ResourceDictionary {
                        Source = new Uri("pack://application:,,,/FFXIVAPP;component/Localization/Japanese.xaml")
                    };
                    Settings.Default.TranslateTo = "Japanese";
                    break;
                case "de":
                    dict = new ResourceDictionary {
                        Source = new Uri("pack://application:,,,/FFXIVAPP;component/Localization/German.xaml")
                    };
                    Settings.Default.TranslateTo = "German";
                    break;
                case "fr":
                    dict = new ResourceDictionary {
                        Source = new Uri("pack://application:,,,/FFXIVAPP;component/Localization/French.xaml")
                    };
                    Settings.Default.TranslateTo = "French";
                    break;
                default:
                    dict = new ResourceDictionary {
                        Source = new Uri("pack://application:,,,/FFXIVAPP;component/Localization/English.xaml")
                    };
                    break;
            }
            Resources.MergedDictionaries.Add(dict);
        }

        /// <summary>
        /// </summary>
        private void CreateNotify()
        {
            var res = new Uri("pack://application:,,,/FFXIVAPP;component/Media/Icons/FFXIVAPP.ico");
            var streamResourceInfo = Application.GetResourceStream(res);
            if (streamResourceInfo != null)
            {
                using (var iconStream = streamResourceInfo.Stream)
                {
                    MyNotifyIcon = new NotifyIcon {
                        Icon = new Icon(iconStream),
                        Visible = true
                    };
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
            var _xATFile = "./Resources/AutoTranslate.xml";
            if (File.Exists(_xATFile))
            {
                var _xATCodes = XDocument.Load(_xATFile);
                var items = _xATCodes.Descendants("Code").Select(item => new XValuePairs {
                    Key = (string) item.Attribute("Key"),
                    Value = (string) item.Attribute(Settings.Default.Language)
                });
                foreach (var item in items)
                {
                    Constants.XATCodes.Add(item.Key, item.Value);
                }
            }
            //LOAD CHATCODES
            var resourceUri = new Uri("pack://application:,,,/FFXIVAPP;component/Resources/ChatCodes.xml");
            var resource = Application.GetResourceStream(resourceUri);
            if (resource != null)
            {
                var xdoc = XElement.Load(resource.Stream);
                var items = xdoc.Descendants("ChatCode").Select(item => new XValuePairs {
                    ID = (string) item.Attribute("ID"),
                    Desc = (string) item.Attribute("Desc")
                });
                foreach (var item in items)
                {
                    Constants.XChatCodes.Add(item.ID, item.Desc);
                }
            }
            foreach (var t in Constants.Settings)
            {
                LoadSettings(t);
            }
            //LOAD CUSTOM REGEX
            ProcessRegEx("Player");
            ProcessRegEx("Monster");
        }

        /// <summary>
        /// </summary>
        /// <param name="type"> </param>
        private void ProcessRegEx(string type)
        {
            var items = _xRegEx.Descendants(type).Select(item => new XValuePairs {
                ID = (string) item.Attribute("ID"),
                Value = item.FirstNode.ToString()
            });
            foreach (var item in items)
            {
                var v = (item.Value.Length == 12) ? "" : item.Value.Substring(9, item.Value.Length - 12);
                if (String.IsNullOrWhiteSpace(v))
                {
                    continue;
                }
                if (Constants.IsValidRegex(v))
                {
                    var regex = new Regex(v, Shared.DefaultOptions);
                    switch (type)
                    {
                            #region Player

                        case "Player":
                            Player.ActionEn = item.Key == "ActionEn" ? regex : Player.ActionEn;
                            Player.ActionFr = item.Key == "ActionFr" ? regex : Player.ActionFr;
                            Player.ActionJa = item.Key == "ActionJa" ? regex : Player.ActionJa;
                            Player.ActionDe = item.Key == "ActionDe" ? regex : Player.ActionDe;

                            Player.UsedEn = item.Key == "UsedEn" ? regex : Player.UsedEn;
                            Player.UsedFr = item.Key == "UsedFr" ? regex : Player.UsedFr;
                            Player.UsedJa = item.Key == "UsedJa" ? regex : Player.UsedJa;
                            Player.UsedDe = item.Key == "UsedDe" ? regex : Player.UsedDe;

                            Player.AdditionalEn = item.Key == "AdditionalEn" ? regex : Player.AdditionalEn;
                            Player.AdditionalFr = item.Key == "AdditionalFr" ? regex : Player.AdditionalFr;
                            Player.AdditionalJa = item.Key == "AdditionalJa" ? regex : Player.AdditionalJa;
                            Player.AdditionalDe = item.Key == "AdditionalDe" ? regex : Player.AdditionalDe;

                            Player.CounterEn = item.Key == "CounterEn" ? regex : Player.CounterEn;
                            Player.CounterFr = item.Key == "CounterFr" ? regex : Player.CounterFr;
                            Player.CounterJa = item.Key == "CounterJa" ? regex : Player.CounterJa;
                            Player.CounterDe = item.Key == "CounterDe" ? regex : Player.CounterDe;

                            Player.BlockEn = item.Key == "BlockEn" ? regex : Player.BlockEn;
                            Player.BlockFr = item.Key == "BlockFr" ? regex : Player.BlockFr;
                            Player.BlockJa = item.Key == "BlockJa" ? regex : Player.BlockJa;
                            Player.BlockDe = item.Key == "BlockDe" ? regex : Player.BlockDe;

                            Player.ParryEn = item.Key == "ParryEn" ? regex : Player.ParryEn;
                            Player.ParryFr = item.Key == "ParryFr" ? regex : Player.ParryFr;
                            Player.ParryJa = item.Key == "ParryJa" ? regex : Player.ParryJa;
                            Player.ParryDe = item.Key == "ParryDe" ? regex : Player.ParryDe;

                            Player.ResistEn = item.Key == "ResistEn" ? regex : Player.ResistEn;
                            Player.ResistFr = item.Key == "ResistFr" ? regex : Player.ResistFr;
                            Player.ResistJa = item.Key == "ResistJa" ? regex : Player.ResistJa;
                            Player.ResistDe = item.Key == "ResistDe" ? regex : Player.ResistDe;

                            Player.EvadeEn = item.Key == "EvadeEn" ? regex : Player.EvadeEn;
                            Player.EvadeFr = item.Key == "EvadeFr" ? regex : Player.EvadeFr;
                            Player.EvadeJa = item.Key == "EvadeJa" ? regex : Player.EvadeJa;
                            Player.EvadeDe = item.Key == "EvadeDe" ? regex : Player.EvadeDe;

                            Player.DefeatsEn = item.Key == "DefeatsEn" ? regex : Player.DefeatsEn;
                            Player.DefeatsFr = item.Key == "DefeatsFr" ? regex : Player.DefeatsFr;
                            Player.DefeatsJa = item.Key == "DefeatsJa" ? regex : Player.DefeatsJa;
                            Player.DefeatsDe = item.Key == "DefeatsDe" ? regex : Player.DefeatsDe;

                            Player.ObtainsEn = item.Key == "ObtainsEn" ? regex : Player.ObtainsEn;
                            Player.ObtainsFr = item.Key == "ObtainsFr" ? regex : Player.ObtainsFr;
                            Player.ObtainsJa = item.Key == "ObtainsJa" ? regex : Player.ObtainsJa;
                            Player.ObtainsDe = item.Key == "ObtainsDe" ? regex : Player.ObtainsDe;

                            Player.JoinEn = item.Key == "JoinEn" ? regex : Player.JoinEn;
                            Player.JoinFr = item.Key == "JoinFr" ? regex : Player.JoinFr;
                            Player.JoinJa = item.Key == "JoinJa" ? regex : Player.JoinJa;
                            Player.JoinDe = item.Key == "JoinDe" ? regex : Player.JoinDe;

                            Player.DisbandEn = item.Key == "DisbandEn" ? regex : Player.DisbandEn;
                            Player.DisbandFr = item.Key == "DisbandFr" ? regex : Player.DisbandFr;
                            Player.DisbandJa = item.Key == "DisbandJa" ? regex : Player.DisbandJa;
                            Player.DisbandDe = item.Key == "DisbandDe" ? regex : Player.DisbandDe;

                            Player.LeftEn = item.Key == "LeftEn" ? regex : Player.LeftEn;
                            Player.LeftFr = item.Key == "LeftFr" ? regex : Player.LeftFr;
                            Player.LeftJa = item.Key == "LeftJa" ? regex : Player.LeftJa;
                            Player.LeftDe = item.Key == "LeftDe" ? regex : Player.LeftDe;

                            Player.MultiFlagEn = item.Key == "MultiFlagEn" ? regex : Player.MultiFlagEn;
                            Player.MultiFlagFr = item.Key == "MultiFlagFr" ? regex : Player.MultiFlagFr;
                            Player.MultiFlagJa = item.Key == "MultiFlagJa" ? regex : Player.MultiFlagJa;
                            Player.MultiFlagDe = item.Key == "MultiFlagDe" ? regex : Player.MultiFlagDe;

                            Player.MultiEn = item.Key == "MultiEn" ? regex : Player.MultiEn;
                            Player.MultiFr = item.Key == "MultiFr" ? regex : Player.MultiFr;
                            Player.MultiJa = item.Key == "MultiJa" ? regex : Player.MultiJa;
                            Player.MultiDe = item.Key == "MultiDe" ? regex : Player.MultiDe;
                            break;

                            #endregion

                            #region Monster

                        case "Monster":
                            Monster.ActionEn = item.Key == "ActionEn" ? regex : Monster.ActionEn;
                            Monster.ActionFr = item.Key == "ActionFr" ? regex : Monster.ActionFr;
                            Monster.ActionJa = item.Key == "ActionJa" ? regex : Monster.ActionJa;
                            Monster.ActionDe = item.Key == "ActionDe" ? regex : Monster.ActionDe;

                            Monster.UsedEn = item.Key == "UsedEn" ? regex : Monster.UsedEn;
                            Monster.UsedFr = item.Key == "UsedFr" ? regex : Monster.UsedFr;
                            Monster.UsedJa = item.Key == "UsedJa" ? regex : Monster.UsedJa;
                            Monster.UsedDe = item.Key == "UsedDe" ? regex : Monster.UsedDe;

                            Monster.AdditionalEn = item.Key == "AdditionalEn" ? regex : Monster.AdditionalEn;
                            Monster.AdditionalFr = item.Key == "AdditionalFr" ? regex : Monster.AdditionalFr;
                            Monster.AdditionalJa = item.Key == "AdditionalJa" ? regex : Monster.AdditionalJa;
                            Monster.AdditionalDe = item.Key == "AdditionalDe" ? regex : Monster.AdditionalDe;

                            Monster.CounterEn = item.Key == "CounterEn" ? regex : Monster.CounterEn;
                            Monster.CounterFr = item.Key == "CounterFr" ? regex : Monster.CounterFr;
                            Monster.CounterJa = item.Key == "CounterJa" ? regex : Monster.CounterJa;
                            Monster.CounterDe = item.Key == "CounterDe" ? regex : Monster.CounterDe;

                            Monster.BlockEn = item.Key == "BlockEn" ? regex : Monster.BlockEn;
                            Monster.BlockFr = item.Key == "BlockFr" ? regex : Monster.BlockFr;
                            Monster.BlockJa = item.Key == "BlockJa" ? regex : Monster.BlockJa;
                            Monster.BlockDe = item.Key == "BlockDe" ? regex : Monster.BlockDe;

                            Monster.ParryEn = item.Key == "ParryEn" ? regex : Monster.ParryEn;
                            Monster.ParryFr = item.Key == "ParryFr" ? regex : Monster.ParryFr;
                            Monster.ParryJa = item.Key == "ParryJa" ? regex : Monster.ParryJa;
                            Monster.ParryDe = item.Key == "ParryDe" ? regex : Monster.ParryDe;

                            Monster.ResistEn = item.Key == "ResistEn" ? regex : Monster.ResistEn;
                            Monster.ResistFr = item.Key == "ResistFr" ? regex : Monster.ResistFr;
                            Monster.ResistJa = item.Key == "ResistJa" ? regex : Monster.ResistJa;
                            Monster.ResistDe = item.Key == "ResistDe" ? regex : Monster.ResistDe;

                            Monster.EvadeEn = item.Key == "EvadeEn" ? regex : Monster.EvadeEn;
                            Monster.EvadeFr = item.Key == "EvadeFr" ? regex : Monster.EvadeFr;
                            Monster.EvadeJa = item.Key == "EvadeJa" ? regex : Monster.EvadeJa;
                            Monster.EvadeDe = item.Key == "EvadeDe" ? regex : Monster.EvadeDe;
                            break;

                            #endregion
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="setting"> </param>
        private void LoadSettings(string setting)
        {
            var items = Constants.XSettings.Descendants(setting).Select(item => new XValuePairs {
                Key = (string) item.Attribute("Key"),
                Value = (string) item.Attribute("Value"),
                RegEx = (string) item.Attribute("RegEx")
            });
            switch (setting)
            {
                case "Color":
                    items = Constants.XColors.Descendants(setting).Select(item => new XValuePairs {
                        Key = (string) item.Attribute("Key"),
                        Value = (string) item.Attribute("Value"),
                        Desc = (string) item.Attribute("Desc")
                    });
                    break;
                case "Event":
                    items = Constants.XEvents.Descendants(setting).Select(item => new XValuePairs {
                        Key = (string) item.Attribute("Key"),
                        Value = (string) item.Attribute("Value")
                    });
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
                    case "Event":
                        Constants.XEvent.Add(item.Key, item.Value);
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
            Func<bool> checkUpdates = () => _autoUpdates.CheckUpdates();
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
            var expPHour = Convert.ToInt32(TotalExp / (_timeElapsed.TotalMinutes / 60));
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
            var tColor = new SolidColorBrush {
                Color = _bColor
            };
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
            var s = new Style();
            s.Setters.Add(new Setter(VisibilityProperty, Visibility.Collapsed));
            View.MainWindowTC.ItemContainerStyle = s;
            ThemeHelper.ChangeTheme(Settings.Default.Theme);
        }

        #endregion

        #region Notify Functions

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void Restore_Click(object sender, EventArgs e)
        {
            View.WindowState = WindowState.Normal;
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
            View.WindowState = WindowState.Normal;
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

        public void Dispose() {}

        #endregion
    }
}
