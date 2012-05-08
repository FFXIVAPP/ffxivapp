// ParseModXIV
// MainStatusViewModel.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using AppModXIV.Classes;
using AppModXIV.Commands;
using AppModXIV.Security;
using ParseModXIV.Classes;
using ParseModXIV.View;

namespace ParseModXIV.ViewModel
{
    public class MainStatusViewModel : INotifyPropertyChanged
    {
        private DelegateCommand _command;
        private static Hashing _hashing = new Hashing();

        #region " COMMAND FUNCTIONS "

        public ICommand ToggleLoggingCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(ToggleLogging);

                return _command;
            }
        }

        public ICommand ResetStatsCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(ResetStats);

                return _command;
            }
        }

        public ICommand ScreenShotCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(ScreenShot);

                return _command;
            }
        }

        public ICommand CopyStatsCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(CopyStats);

                return _command;
            }
        }

        #endregion

        #region " GUI FUNCTIONS "

        public static void ToggleLogging()
        {
            if (ParseMod.Instance.IsLogging)
            {
                StopLogging();
            }
            else
            {
                StartLogging();
            }
        }

        public static void ResetStats()
        {
            ClearStats();
        }

        private static void ClearStats()
        {
            if (App.MArgs == null)
            {
                if (Settings.Default.Gui_SaveLog)
                {
                    if (ChatWorkerDelegate.XmlWriteLog.LineCount > 1)
                    {
                        ChatWorkerDelegate.XmlWriteLog.WriteToDisk(MainWindow.View.Lpath +
                                                                   DateTime.Now.ToString("dd.MM.yyyy.HH.mm.ss") +
                                                                   "_Log.xml");
                        ChatWorkerDelegate.XmlWriteLog.ClearXml();
                    }
                }
                if (Settings.Default.Gui_ExportXML)
                {
                    StatGroupToXml.ExportPartyStats();
                    StatGroupToXml.ExportMonsterStats();
                }
            }
            MainTabControlView.View.MobAbility_FLOW.Blocks.Clear();
            ParseMod.Instance.StatMonitor.Clear();
        }

        private static void ScreenShot()
        {
            var myTab = MainTabControlView.View.gui_TabControl.SelectedItem as TabItem;
            new ScreenCapture();
            var myHandle = new WindowInteropHelper(MainWindow.View).Handle;
            if (myTab != null)
            {
                ScreenCapture.CaptureWindowToFile(myHandle,
                                                  MainWindow.View.Ipath + DateTime.Now.ToString("dd.MM.yyyy-HH.mm.ss_") +
                                                  myTab.Header.ToString().Replace(" ", "") + ".jpg", ImageFormat.Jpeg);
            }
        }

        private static void CopyStats()
        {
            var myTab = MainTabControlView.View.gui_TabControl.SelectedItem as TabItem;
            if (myTab == null)
            {
                return;
            }
            var statName = myTab.Header.ToString().Replace(" ", "").ToLower().Replace("stats", "");
            Clipboard.SetText(StatGroupToChat.ExportPartyStats(statName));
        }

        #endregion

        private static void StartLogging()
        {
            ParseMod.Desc = MainStatusView.View.gui_UploadDESC.Text;
            var uid = Settings.Default.CICUID + new Random().Next(0, 999999).ToString(CultureInfo.InvariantCulture);
            ParseMod.Uid = Hashing.CalculateMd5Hash(uid);
            MainStatusView.View.gui_UploadDESC.IsEnabled = false;
            MainMenuView.View.gui_Settings.IsEnabled = false;
            MainStatusView.View.gui_ToggleLogging.Content = "Stop Logging";
            ParseMod.Instance.IsLogging = true;
            if (!Settings.Default.Gui_UploadData || ParseMod.Desc == "")
            {
                return;
            }
            //var json = "{\"uid\":\"" + ParseMod.uid + "\",\"cicuid\":\"" + Settings.Default.CICUID + "\",\"parse_desc\":\"" + ParseMod.desc + "\"}";
            //Func<bool> sendJson = () => SubmitData("l", json);
            //sendJson.BeginInvoke(result =>
            //                         {
            //                             if (!sendJson.EndInvoke(result))
            //                             {
            //                             }
            //                         }, null);
        }

        private static void StopLogging()
        {
            MainStatusView.View.gui_UploadDESC.IsEnabled = true;
            MainMenuView.View.gui_Settings.IsEnabled = true;
            MainStatusView.View.gui_UploadDESC.Text = "";
            MainStatusView.View.gui_ToggleLogging.Content = "Start Logging";
            ParseMod.Instance.IsLogging = false;
        }

        private static bool SubmitData(string insert, string message)
        {
            var url = string.Format("http://ffxiv-app.com/battles/insert/?insert={0}&q={1}", insert,
                                    HttpUtility.UrlEncode(message));
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

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion
    }
}