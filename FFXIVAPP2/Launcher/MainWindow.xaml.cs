// Launcher
// MainWindow.xaml.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Threading;
using System.Xml;
using AppModXIV.Classes;
using Launcher.Classes;
using Launcher.ViewModel;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IDisposable
    {
        #region " VARIABLES "

        private readonly string _lpath = "";
        private readonly AutomaticUpdates _autoUpdates = new AutomaticUpdates();
        public static BitField BitField;
        private Process[] _proc;
        private Process _processById;
        private Globals.Rect _orig;
        private const Boolean CanSave = true;
        private static Targetmap _targetMap;
        public static MainWindow View;
        private NotifyIcon _myNotifyIcon = new NotifyIcon();
        private static string _pName;
        private static int _gBorder;
        private static int _gTop;
        private static int _gHor;
        private static int _gVer;
        private readonly DispatcherTimer _procTimer = new DispatcherTimer();
        private readonly DispatcherTimer _quitTimer = new DispatcherTimer();

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            _myNotifyIcon.BalloonTipClicked += MyNotifyIconBalloonTipClicked;
            var rd = new ResourceDictionary {Source = new Uri("pack://application:,,,/Launcher;component/Launcher.xaml")};
            Resources.MergedDictionaries.Add(rd);

            if (File.Exists("./Resources/Themes/Launcher.xaml"))
            {
                rd = (ResourceDictionary) XamlReader.Load(XmlReader.Create("./Resources/Themes/Launcher.xaml"));
                Resources.MergedDictionaries.Add(rd);
            }

            InitializeComponent();

            _proc = Process.GetProcessesByName("launcher");
            if (_proc.Length > 1)
            {
                Environment.Exit(0);
            }

            BitField = new BitField(BitField.Flag.Clear);

            View = this;

            _lpath = "./Logs/Launcher/";
            if (!Directory.Exists(_lpath))
            {
                Directory.CreateDirectory(_lpath);
            }

            _autoUpdates.CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Func<bool> checkUpdates = () => _autoUpdates.CheckUpdates("Launcher");
            Func<bool> checkLibrary = () => _autoUpdates.CheckDlls("AppModXIV", "");
            Func<bool> checkHook = () => _autoUpdates.CheckDlls("WinModXIV", "");
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
                        else
                        {
                            checkHook.BeginInvoke(hookresult =>
                            {
                                if (checkHook.EndInvoke(hookresult))
                                {
                                    _myNotifyIcon.ShowBalloonTip(bTipTime, "Update Available!", "Click this message to download.", ToolTipIcon.Info);
                                }
                            }, null);
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
            //var proc = Process.GetProcessesByName("Launcher");
            //foreach (var p in proc)
            //{
            //    p.Kill();
            //}
            Process.Start("UpdateModXIV.exe", "WinModXIV");
        }

        #region " FORM OPEN-CLOSE-STATES "

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            WindowState = WindowState.Minimized;
            if (CanSave)
            {
                Settings.Default.Save();

                Globals.EndHook();

                MainViewModel.LoadOptions();
                LoadSettings();

                SetupHook();
            }
            e.Cancel = true;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            _myNotifyIcon.Visible = false;
            Globals.EndHook();
        }

        #endregion

        #region " INITIAL LOAD FUNCTIONS "

        /// <summary>
        /// 
        /// </summary>
        private void Start()
        {
            var streamResourceInfo = Application.GetResourceStream(new Uri("pack://application:,,,/Launcher;component/Launcher.ico"));
            if (streamResourceInfo != null)
            {
                using (var iconStream = streamResourceInfo.Stream)
                {
                    _myNotifyIcon = new NotifyIcon {Icon = new Icon(iconStream), Visible = true};
                    iconStream.Dispose();
                    _myNotifyIcon.Text = "Launcher - Minimized";
                    var myNotify = new ContextMenu();
                    myNotify.MenuItems.Add("&Configure Launcher");
                    myNotify.MenuItems.Add("&Exit");
                    myNotify.MenuItems[0].Click += Restore_Click;
                    myNotify.MenuItems[1].Click += Exit_Click;
                    _myNotifyIcon.ContextMenu = myNotify;
                    _myNotifyIcon.MouseDoubleClick += MyNotifyIcon_MouseDoubleClick;
                    _myNotifyIcon.BalloonTipClicked += MyNotifyIconBalloonTipClicked;
                }
            }
            LoadSettings();
            MainViewModel.LoadOptions();
            SetupHook();
            Launch();
            _procTimer.Tick += procTimer_Tick;
            _procTimer.Interval = TimeSpan.FromMilliseconds(100);
            _procTimer.Start();
            _quitTimer.Tick += quitTimer_Tick;
            _quitTimer.Interval = TimeSpan.FromMilliseconds(3000);
        }

        /// <summary>
        /// 
        /// </summary>
        private static void LoadSettings()
        {
            _targetMap.path = Settings.Default.Game;
            _pName = Settings.Default.Game;
            _pName = _pName.Substring(_pName.LastIndexOf("\\", StringComparison.Ordinal) + 1);
            _pName = _pName.Substring(0, _pName.IndexOf(".", StringComparison.Ordinal));
            _targetMap.dxversion = 0;
            _targetMap.flags = Int32.Parse(Settings.Default.Flags);
            _targetMap.initx = Settings.Default.InitX;
            _targetMap.inity = Settings.Default.InitY;
            _targetMap.minx = Settings.Default.MinX;
            _targetMap.miny = Settings.Default.MinY;
            _targetMap.maxx = Settings.Default.MaxX;
            _targetMap.maxy = Settings.Default.MaxY;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void procTimer_Tick(object sender, EventArgs e)
        {
            Constants.FfxivPid = Process.GetProcessesByName(_pName);
            if (Constants.FfxivPid.Length > 0)
            {
                GetBorders();
                if (Settings.Default.FullScreen)
                {
                    Fullscreen();
                }
            }
            GC.Collect();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void quitTimer_Tick(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
            GC.Collect();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="numberStyle"></param>
        /// <returns></returns>
        public static Boolean IsNumeric(String input, NumberStyles numberStyle)
        {
            Double temp;
            var result = Double.TryParse(input, numberStyle, CultureInfo.CurrentCulture, out temp);
            return result;
        }

        #endregion

        #region " HOOKER STUFF "

        /// <summary>
        /// 
        /// </summary>
        public static void SetupHook()
        {
            Globals.SetTarget(ref _targetMap);
            Globals.StartHook();
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

        #region " LAUNCHER OPTIONS "

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

        #region " LAUNCH/HOOK PATH CONTROL "

        /// <summary>
        /// 
        /// </summary>
        public void Launch()
        {
            _proc = Process.GetProcessesByName(_pName);
            if ((_proc.Length > 0))
            {
                return;
            }
            var m = new Process {StartInfo = {FileName = Settings.Default.Launch}};
            try
            {
                m.Start();
                WindowState = WindowState.Minimized;
            }
            catch
            {
                MessageBox.Show("Launch path is invalid. Please double check settings.", "Warning!");
            }
        }

        #endregion

        #region " WINDOW FUNCTIONS "

        /// <summary>
        /// 
        /// </summary>
        private void GetBorders()
        {
            _proc = Process.GetProcessesByName(_pName);
            if (_proc.Length > 0)
            {
                _processById = Process.GetProcessById(_proc[0].Id);
                var mainWindowHandle = _processById.MainWindowHandle;
                Globals.GetWindowRect((int) mainWindowHandle, ref _orig);
                _gBorder = (_orig.Right - _orig.Left - _targetMap.maxx)/2;
                _gTop = (_orig.Bottom - _orig.Top) - _targetMap.maxy - _gBorder;
                _gHor = _targetMap.maxx;
                _gVer = _targetMap.maxy;
            }
            else
            {
                MessageBox.Show("Game not in memory.", "Warning!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Fullscreen()
        {
            _proc = Process.GetProcessesByName(_pName);
            if (_proc.Length > 0)
            {
                _processById = Process.GetProcessById(_proc[0].Id);
                const int nIndex = -16;
                var mainWindowHandle = _processById.MainWindowHandle;
                const WindowStyles styles = WindowStyles.Visible;
                Globals.SetWindowLongPtr(mainWindowHandle, nIndex, (IntPtr) styles);
                Globals.MoveWindow(mainWindowHandle, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, true);
                if (_quitTimer.IsEnabled == false)
                {
                    _quitTimer.Start();
                }
            }
            else
            {
                MessageBox.Show("Game not in memory.", "Warning!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Restore()
        {
            _proc = Process.GetProcessesByName(_pName);
            if (_proc.Length > 0)
            {
                _processById = Process.GetProcessById(_proc[0].Id);
                const WindowStyles styles = WindowStyles.Caption | WindowStyles.SysMenu | WindowStyles.Visible | WindowStyles.MinimizeBox | WindowStyles.MaximizeBox | WindowStyles.SizeBox | WindowStyles.OverLapped;
                const int nIndex = -16;
                var mainWindowHandle = _processById.MainWindowHandle;
                Globals.SetWindowLongPtr(mainWindowHandle, nIndex, (IntPtr) styles);
                var h = (_gBorder*2) + _gHor;
                var v = _gBorder + _gVer + _gTop;
                Globals.MoveWindow(mainWindowHandle, _targetMap.initx + 10, _targetMap.inity + 10, h, v, true);
            }
            else
            {
                MessageBox.Show("Game not in memory.", "Warning!");
            }
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