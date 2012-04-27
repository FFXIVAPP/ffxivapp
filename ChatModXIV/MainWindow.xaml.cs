// Project: ChatModXIV
// File: MainWindow.xaml.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Xml;
using System.Xml.Linq;
using AppModXIV.Classes;
using ChatModXIV.Classes;
using SocketIOClient;
using Application = System.Windows.Application;
using Clipboard = System.Windows.Clipboard;
using ErrorEventArgs = SocketIOClient.ErrorEventArgs;
using Keys = AppModXIV.Classes.Keys;
using Message = ChatModXIV.Classes.Message;
using MessageBox = System.Windows.MessageBox;

namespace ChatModXIV
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
        private NotifyIcon _myNotifyIcon;
        private readonly XDocument _xAtCodes = XDocument.Load("./Resources/ATCodes.xml");
        public Boolean Connected;
        public Client Socket;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            var rd = new ResourceDictionary { Source = new Uri("pack://application:,,,/ChatModXIV;component/ChatModXIV.xaml") };
            Resources.MergedDictionaries.Add(rd);
            if (File.Exists("./Resources/Themes/ChatModXIV.xaml"))
            {
                rd = (ResourceDictionary) XamlReader.Load(XmlReader.Create("./Resources/Themes/ChatModXIV.xaml"));
                Resources.MergedDictionaries.Add(rd);
            }
            InitializeComponent();
            View = this;
            _lpath = "./Logs/ChatMod/";
            if (!Directory.Exists(_lpath))
            {
                Directory.CreateDirectory(_lpath);
            }
            _autoUpdates.CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Func<bool> checkUpdates = () => _autoUpdates.CheckUpdates("ChatModXIV");
            Func<bool> checkLibrary = () => _autoUpdates.CheckDlls("AppModXIV", "");
            checkUpdates.BeginInvoke(appresult =>
                                         {
                                             if (checkUpdates.EndInvoke(appresult))
                                             {
                                                 var msgbox = MessageBox.Show("Go to main site and download the latest archive?", "Required Update!", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                                                 if (msgbox == MessageBoxResult.Yes)
                                                 {
                                                     Process.Start("http://ffxiv-app.com/products/");
                                                 }
                                                 var proc = Process.GetProcessesByName("ChatModXIV");
                                                 foreach (var p in proc)
                                                 {
                                                     p.Kill();
                                                 }
                                             }
                                             else
                                             {
                                                 checkLibrary.BeginInvoke(libresult => { }, null);
                                             }
                                         }, null);
        }

        #region " FORM OPEN-CLOSE-STATES "

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadXml();
            Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        public void ProcessXml(string filePath)
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
                    var mServer = Settings.Default.Server;
                    var mTimeStamp = DateTime.Now.ToString("[HH:mm:ss]");
                    var mCode = key;
                    var mMessage = value;
                    SendMessage("message", new Message { Type = "Global", Server = mServer, Time = mTimeStamp, Code = mCode, Data = mMessage });
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
                PreClose();
                ChatMod.Instance.StopLogging();
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
                    _myNotifyIcon.Visible = true;
                    break;
                case WindowState.Normal:
                    _myNotifyIcon.Visible = false;
                    ShowInTaskbar = true;
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
            var items = from item in _xAtCodes.Descendants("Code") select new XValuePairs { Key = (string) item.Attribute("Key"), Value = (string) item.Attribute("Value") };
            foreach (var item in items)
            {
                Constants.XAtCodes.Add(item.Key, item.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Start()
        {
            var streamResourceInfo = Application.GetResourceStream(new Uri("pack://application:,,,/ChatModXIV;component/ChatModXIV.ico"));
            if (streamResourceInfo != null)
            {
                using (var iconStream = streamResourceInfo.Stream)
                {
                    _myNotifyIcon = new NotifyIcon { Icon = new Icon(iconStream) };
                    iconStream.Dispose();
                    _myNotifyIcon.Text = "ChatModXIV - Minimized";
                    var myNotify = new ContextMenu();
                    myNotify.MenuItems.Add("&Restore Application");
                    myNotify.MenuItems.Add("&Exit");
                    myNotify.MenuItems[0].Click += Restore_Click;
                    myNotify.MenuItems[1].Click += Exit_Click;
                    _myNotifyIcon.ContextMenu = myNotify;
                    _myNotifyIcon.MouseDoubleClick += MyNotifyIcon_MouseDoubleClick;
                }
            }
            ChatMod.Instance.StartLogging();
            UpdateControls(false);
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

        #region " CHATMOD OPTIONS "

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyNotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WindowState = WindowState.Normal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connected"></param>
        public void UpdateControls(bool connected)
        {
            Connected = connected;
            MainMenuView.gui_ConnectToogle.Header = Connected ? "Disconnected" : "Connect";
            var connectStatus = !Connected ? "ChatModXIV - Not Connected" : "ChatModXIV - Connected";
            View.Title = connectStatus;
        }

        #endregion

        #region " SOCKETS "

        /// <summary>
        /// 
        /// </summary>
        public void Connect()
        {
            if (Settings.Default.SiteName == "")
            {
                MessageBox.Show("Site Username Required!");
                return;
            }
            try
            {
                Socket = new Client("http://ffxiv-app.com:4000");
                Socket.Opened += SocketOpened;
                Socket.Message += SocketMessage;
                Socket.SocketConnectionClosed += SocketConnectionClosed;
                Socket.Error += SocketError;
                Socket.On("welcome", data =>
                                         {
                                             var message = data.Json.GetFirstArgAs<Message>();
                                             Dispatcher.BeginInvoke(new Action(() => OnNewLine(message)), null);
                                         });
                Socket.On("message", data =>
                                         {
                                             var message = data.Json.GetFirstArgAs<Message>();
                                             Dispatcher.BeginInvoke(new Action(() => OnNewLine(message)), null);
                                         });
                Socket.Connect();
            }
            catch (Exception se)
            {
                var str = "\nConnection failed, is the server running?\n" + se.Message;
                MessageBox.Show(str);
                UpdateControls(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Event"></param>
        /// <param name="data"></param>
        public void SendMessage(string Event, object data)
        {
            Socket.Emit(Event, data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SocketError(object sender, ErrorEventArgs e)
        {
            UpdateControls(false);
            PreClose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SocketConnectionClosed(object sender, EventArgs e)
        {
            UpdateControls(false);
            PreClose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void SocketMessage(object sender, MessageEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void SocketOpened(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void PreClose()
        {
            if (Socket == null)
            {
                return;
            }
            Socket.Opened -= SocketOpened;
            Socket.Message -= SocketMessage;
            Socket.SocketConnectionClosed -= SocketConnectionClosed;
            Socket.Error -= SocketError;
            Socket.Dispose(); // close & dispose of socket client
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        private void OnNewLine(Message line)
        {
            switch (line.Source)
            {
                case "Server":
                    switch (line.Data)
                    {
                        case "Hello":
                            SendMessage("login", new Login { Source = "PC", Name = Settings.Default.SiteName, Api = Settings.Default.APIKey });
                            break;
                        case "Login Success":
                            UpdateControls(true);
                            break;
                        case "Login Error":
                            MessageBox.Show("Did you enter the right Site Name and Keys?");
                            PreClose();
                            break;
                    }
                    break;
                case "Web":
                case "Phone":
                    Clipboard.SetText(line.Command + " " + line.Data);
                    KeyHelper.KeyPress(Keys.Escape);
                    KeyHelper.KeyPress(Keys.Space);
                    KeyHelper.Paste();
                    KeyHelper.KeyPress(Keys.Return);
                    break;
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