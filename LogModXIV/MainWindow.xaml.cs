// LogModXIV
// MainWindow.xaml.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;
using AppModXIV.Classes;
using LogModXIV.Classes;
using LogModXIV.Controls;
using Application = System.Windows.Application;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;
using ContextMenu = System.Windows.Forms.ContextMenu;
using FontFamily = System.Windows.Media.FontFamily;
using ListBox = System.Windows.Controls.ListBox;

namespace LogModXIV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IDisposable
    {
        #region " VARIABLES "

        private readonly string _lpath = "";
        private readonly AutomaticUpdates _autoUpdates = new AutomaticUpdates();
        public static MainWindow View;
        private readonly DispatcherTimer _expTimer = new DispatcherTimer();
        private DateTime _startTime;
        private TimeSpan _timeElapsed;
        public int TotalExp;
        private NotifyIcon _myNotifyIcon;
        private readonly XDocument _xAtCodes = XDocument.Load("./Resources/ATCodes.xml");
        private Color _tsColor;
        private Color _bColor;
        private static int _count;
        public static ListBox[] ChatScan = new ListBox[1];
        public static readonly ArrayList TabNames = new ArrayList();
        private RichTextBoxControl _rtb;
        private static readonly dynamic DFlowDocument = new Dictionary<string, object>();
        public static readonly dynamic DFlowDocumentReader = new Dictionary<string, object>();

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            var rd = new ResourceDictionary {Source = new Uri("pack://application:,,,/LogModXIV;component/LogModXIV.xaml")};
            Resources.MergedDictionaries.Add(rd);
            if (File.Exists("./Resources/Themes/LogModXIV.xaml"))
            {
                rd = (ResourceDictionary) XamlReader.Load(XmlReader.Create("./Resources/Themes/LogModXIV.xaml"));
                Resources.MergedDictionaries.Add(rd);
            }
            InitializeComponent();
            MainToolBarView.gui_Maximize.Visibility = Visibility.Visible;
            MainToolBarView.gui_Restore.Visibility = Visibility.Collapsed;
            View = this;
            _lpath = "./Logs/LogMod/";
            if (!Directory.Exists(_lpath))
            {
                Directory.CreateDirectory(_lpath);
            }
            _autoUpdates.CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Func<bool> checkUpdates = () => _autoUpdates.CheckUpdates("LogModXIV");
            Func<bool> checkLibrary = () => _autoUpdates.CheckDlls("AppModXIV", "");
            checkUpdates.BeginInvoke(appresult =>
            {
                const int bTipTime = 3000;
                if (checkUpdates.EndInvoke(appresult))
                {
                    _myNotifyIcon.ShowBalloonTip(bTipTime, "Update Available!", "Click this message to download.", ToolTipIcon.Info);
                }
                else
                {
                    checkLibrary.BeginInvoke(libresult =>
                    {
                        if (checkLibrary.EndInvoke(libresult))
                        {
                            _myNotifyIcon.ShowBalloonTip(bTipTime, "Update Available!", "Click this message to download.", ToolTipIcon.Info);
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
            //var proc = Process.GetProcessesByName("LogModXIV");
            //foreach (var p in proc)
            //{
            //    p.Kill();
            //}
            Process.Start("UpdateModXIV.exe", "LogModXIV");
        }

        #region " FORM OPEN-CLOSE-STATES "

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _startTime = DateTime.Now;
            LoadXml();
            ApplySettings();
            UpdateColors();
            UpdateFonts();
            Start();
            //var xdoc = new XmlDocument();
            //xdoc.LoadXml(XamlWriter.Save(MainTabControlView.All_FLOW));
            //xdoc.Save("document.xml");
            //var util = new VisualUtilities();
            //Console.WriteLine("Logical Tree:");
            //util.PrintLogicalTree(this);
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
                LogMod.Instance.StopLogging();
                if (Settings.Default.Gui_SaveLog)
                {
                    if (ChatWorkerDelegate.XmlWriteLog.LineCount > 1)
                    {
                        ChatWorkerDelegate.XmlWriteLog.WriteToDisk(_lpath + DateTime.Now.ToString("dd.MM.yyyy.HH.mm.ss") + "_Log.xml");
                    }
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
        private void LoadXml()
        {
            var items = from item in _xAtCodes.Descendants("Code") select new XValuePairs {Key = (string) item.Attribute("Key"), Value = (string) item.Attribute("Value")};
            foreach (var item in items)
            {
                Constants.XAtCodes.Add(item.Key, item.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ApplySettings()
        {
            foreach (var tab in LmSettings.XTab.Where(tab => tab.Key != "" && tab.Value != ""))
            {
                var splitOfChatCodes = tab.Value.Split(',');
                AddTabPageName(tab.Key, splitOfChatCodes);
            }
            MainTabControlView.gui_TabControl.SelectedIndex = 1;
        }

        /// <summary>
        /// 
        /// </summary>
        private void Start()
        {
            MainMenuView.LayoutRoot.Visibility = Visibility.Visible;
            //MainStatusView.LayoutRoot.Visibility = System.Windows.Visibility.Visible;
            try
            {
                MainTabControlView.gui_TabControl.ItemContainerStyle = (Style) MainTabControlView.FindResource("TabItemVisible");
            }
            catch
            {
                var s = new Style();
                s.Setters.Add(new Setter(VisibilityProperty, Visibility.Visible));
                MainTabControlView.gui_TabControl.ItemContainerStyle = s;
            }
            var streamResourceInfo = Application.GetResourceStream(new Uri("pack://application:,,,/LogModXIV;component/LogModXIV.ico"));
            if (streamResourceInfo != null)
            {
                using (var iconStream = streamResourceInfo.Stream)
                {
                    _myNotifyIcon = new NotifyIcon {Icon = new Icon(iconStream), Visible = true};
                    iconStream.Dispose();
                    _myNotifyIcon.Text = "LogModXIV - Minimized";
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
            _expTimer.Tick += expTimer_Tick;
            _expTimer.Interval = TimeSpan.FromMilliseconds(100);
            _expTimer.Start();
            LogMod.Instance.StartLogging();
        }

        #endregion

        #region " TIMERS "

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void expTimer_Tick(object sender, EventArgs e)
        {
            _expTimer.Interval = TimeSpan.FromMilliseconds(1000);
            _timeElapsed = DateTime.Now - _startTime;
            var expPHour = Convert.ToInt32(TotalExp/(_timeElapsed.TotalMinutes/60));
            if (LogMod.Instance.IsLogging)
            {
                View.Title = "LogModXIV ~ Exp: " + TotalExp.ToString(CultureInfo.InvariantCulture) + "   Exp/H: " + expPHour.ToString(CultureInfo.InvariantCulture);
            }

            GC.Collect();
        }

        #endregion

        #region " FONTS AND COLORS "

        /// <summary>
        /// 
        /// </summary>
        private void UpdateFonts()
        {
            if (Settings.Default.Gui_LogFont == null)
            {
                return;
            }
            var font = Settings.Default.Gui_LogFont;
            MainTabControlView.All_FLOW.FontFamily = new FontFamily(font.Name);
            MainTabControlView.All_FLOW.FontWeight = font.Bold ? FontWeights.Bold : FontWeights.Regular;
            MainTabControlView.All_FLOW.FontStyle = font.Italic ? FontStyles.Italic : FontStyles.Normal;
            MainTabControlView.All_FLOW.FontSize = font.Size;
            MainTabControlView.Translated_FLOW.FontFamily = new FontFamily(font.Name);
            MainTabControlView.Translated_FLOW.FontWeight = font.Bold ? FontWeights.Bold : FontWeights.Regular;
            MainTabControlView.Translated_FLOW.FontStyle = font.Italic ? FontStyles.Italic : FontStyles.Normal;
            MainTabControlView.Translated_FLOW.FontSize = font.Size;
            MainTabControlView.Debug_FLOW.FontFamily = new FontFamily(font.Name);
            MainTabControlView.Debug_FLOW.FontWeight = font.Bold ? FontWeights.Bold : FontWeights.Regular;
            MainTabControlView.Debug_FLOW.FontStyle = font.Italic ? FontStyles.Italic : FontStyles.Normal;
            MainTabControlView.Debug_FLOW.FontSize = font.Size;
            for (var a = 0; a <= TabNames.Count - 1; a++)
            {
                FlowDocument tempFlow = DFlowDocument[TabNames[0] + "_FLOW"];
                tempFlow.FontFamily = new FontFamily(font.Name);
                tempFlow.FontWeight = font.Bold ? FontWeights.Bold : FontWeights.Regular;
                tempFlow.FontStyle = font.Italic ? FontStyles.Italic : FontStyles.Normal;
                tempFlow.FontSize = font.Size;
            }
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
            MainTabControlView.All_FLOW.Background = tColor;
            MainTabControlView.Translated_FLOW.Background = tColor;
            MainTabControlView.Debug_FLOW.Background = tColor;
            for (var a = 0; a <= TabNames.Count - 1; a++)
            {
                FlowDocument tempFlow = DFlowDocument[TabNames[0] + "_FLOW"];
                tempFlow.Background = tColor;
            }
        }

        #endregion

        #region " TAB CONTROLS "

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameOfTab"></param>
        /// <param name="splitOfChatCodes"></param>
        private void AddTabPageName(string nameOfTab, string[] splitOfChatCodes)
        {
            var chatString = "";
            for (var i = 0; i <= splitOfChatCodes.Length - 1; i++)
            {
                if (i == splitOfChatCodes.Length - 1)
                {
                    chatString += splitOfChatCodes[i];
                }
                else
                {
                    chatString += splitOfChatCodes[i] + ",";
                }
            }
            if (!LmSettings.XTab.ContainsKey(nameOfTab))
            {
                LmSettings.XTab.Add(nameOfTab, chatString);
            }
            var newTab = new TabItem();
            _rtb = new RichTextBoxControl();
            DFlowDocument.Add(nameOfTab + "_FLOW", _rtb.FLOW);
            DFlowDocumentReader.Add(nameOfTab + "_FDR", _rtb.FDR);
            newTab.Name = nameOfTab + "_TabItem";
            newTab.Content = _rtb;
            newTab.Header = nameOfTab;
            MainTabControlView.gui_TabControl.Items.Add(newTab);
            TabNames.Add(nameOfTab);
            var newListBox = new ListBox();
            Array.Resize(ref ChatScan, (ChatScan).GetUpperBound(0) + 2);
            ChatScan[_count] = newListBox;
            for (var i = 0; i <= splitOfChatCodes.Length - 1; i++)
            {
                ChatScan[_count].Items.Add(splitOfChatCodes[i]);
            }
            FlowDocument tempFlow = DFlowDocument[nameOfTab + "_FLOW"];
            if (Settings.Default.Gui_LogFont != null)
            {
                var f = Settings.Default.Gui_LogFont;
                tempFlow.FontFamily = new FontFamily(f.Name);
                tempFlow.FontWeight = f.Bold ? FontWeights.Bold : FontWeights.Regular;
                tempFlow.FontStyle = f.Italic ? FontStyles.Italic : FontStyles.Normal;
                tempFlow.FontSize = f.Size;
                f.Dispose();
            }
            tempFlow.Background = Brushes.Black;
            tempFlow.Foreground = Brushes.White;
            _count += 1;
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

        #region " LOGMOD OPTIONS "

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

        #region Implementation of IDisposable

        public void Dispose()
        {
            _myNotifyIcon.Dispose();
        }

        #endregion
    }
}