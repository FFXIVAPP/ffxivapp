// ParseModXIV
// ParseMod.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using AppModXIV.Classes;
using AppModXIV.Memory;
using ParseModXIV.Model;
using ParseModXIV.Monitors;
using ParseModXIV.View;

namespace ParseModXIV.Classes
{
    public class ParseMod
    {
        public static int DeathCount;
        public static string LastKilled = "";
        public static readonly Dictionary<string, string> ServerName = new Dictionary<string, string>();
        public Timeline Timeline { get; private set; }
        public StatMonitor StatMonitor { get; private set; }
        private TimelineMonitor TimelineMonitor { get; set; }
        private static ParseMod _instance;
        private ChatWorker _chatWorker;
        public Boolean IsLogging;
        public static string Desc = "";
        public static string Uid = "";
        public readonly Dictionary<string, string> TotalA = new Dictionary<string, string>();
        public readonly Dictionary<string, string> TotalD = new Dictionary<string, string>();
        public readonly Dictionary<string, string> TotalH = new Dictionary<string, string>();

        /// <summary>
        /// 
        /// </summary>
        public static ParseMod Instance
        {
            get { return _instance ?? (_instance = new ParseMod()); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        public static string TitleCase(string s, bool all)
        {
            var result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(all ? s.ToLower() : s);
            var reg = RegExps.Romans.Match(result);
            if (reg.Success)
            {
                var temp = Convert.ToString(reg.Groups["roman"].Value);
                result = result.Replace(temp, temp.ToUpper());
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        private ParseMod()
        {
            Constants.FfxivOpen = false;
            Constants.Pid = -1;
            Timeline = new Timeline();
            InitMonitors();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitMonitors()
        {
            if (Settings.Default.DebugMode)
            {
            }
            TimelineMonitor = new TimelineMonitor(this);
            StatMonitor = new StatMonitor(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static int GetPid()
        {
            if (Constants.FfxivOpen && Constants.Pid > 0)
            {
                try
                {
                    Process.GetProcessById(Constants.Pid);
                    return Constants.Pid;
                }
                catch (ArgumentException)
                {
                    Constants.FfxivOpen = false;
                }
            }
            Constants.FfxivPid = Process.GetProcessesByName("ffxivgame");
            if (Constants.FfxivPid.Length == 0)
            {
                Constants.FfxivOpen = false;
                return -1;
            }
            Constants.FfxivOpen = true;
            foreach (var proc in Constants.FfxivPid)
            {
                MainMenuView.View.gui_PIDSelect.Items.Add(proc.Id);
            }
            MainMenuView.View.gui_PIDSelect.SelectedIndex = 0;
            Pid(Constants.FfxivPid.First().Id);
            return Constants.FfxivPid.First().Id;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void SetPid()
        {
            if (MainMenuView.View.gui_PIDSelect.Text == "")
            {
                return;
            }
            Pid(Convert.ToInt32(MainMenuView.View.gui_PIDSelect.Text));
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ResetPid()
        {
            Constants.Pid = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        private static void Pid(int pid)
        {
            Constants.Pid = pid;
            Constants.PHandle = Constants.FfxivPid[MainMenuView.View.gui_PIDSelect.SelectedIndex].MainWindowHandle;
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartLogging()
        {
            var id = GetPid();
            if (id < 0)
            {
                MainWindow.View.Title = "ParseModXIV ~ Final Fantasy XIV Not Found!";
                return;
            }
            MainWindow.View.Title = "ParseModXIV ~ OFF";
            MainStatusView.View.gui_ToggleLogging.IsEnabled = true;
            MainStatusView.View.gui_ResetStats.IsEnabled = true;
            var p = Process.GetProcessById(id);
            var o = new Offsets(p);
            _chatWorker = new ChatWorker(p, o);
            _chatWorker.OnNewline += ChatWorkerDelegate.OnNewline;
            _chatWorker.OnRawline += ChatWorkerDelegate.OnRawLine;
            _chatWorker.StartLogging();
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopLogging()
        {
            if (_chatWorker == null)
            {
                return;
            }
            _chatWorker.StopLogging();
            _chatWorker = null;
        }

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
    }
}