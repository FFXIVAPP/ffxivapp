// FFXIVAPP
// FFXIV.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using FFXIVAPP.Classes.Memory;
using FFXIVAPP.Classes.RegExs;
using FFXIVAPP.Models;
using FFXIVAPP.Monitors;
using FFXIVAPP.Properties;
using FFXIVAPP.Views;
using NLog;

namespace FFXIVAPP.Classes
{
    public class FFXIV
    {
        public static string LastKilled = "";
        public Timeline Timeline { get; private set; }
        public StatMonitor StatMonitor { get; private set; }
        private TimelineMonitor TimelineMonitor { get; set; }
        private static FFXIV _instance;
        private static ChatWorker _chatWorker;
        public static string Desc = "";
        public static string UID = "";
        public readonly Dictionary<string, string> TotalA = new Dictionary<string, string>();
        public readonly Dictionary<string, string> TotalD = new Dictionary<string, string>();
        public readonly Dictionary<string, string> TotalH = new Dictionary<string, string>();
        public readonly Dictionary<string, string> TotalDPS = new Dictionary<string, string>();
        public Sio SIO;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// </summary>
        public static FFXIV Instance
        {
            get { return _instance ?? (_instance = new FFXIV()); }
        }

        /// <summary>
        /// </summary>
        /// <param name="s"> </param>
        /// <param name="all"> </param>
        /// <returns> </returns>
        public static string TitleCase(string s, bool all = true)
        {
            var result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(all ? s.ToLower() : s);
            var reg = Shared.Romans.Match(s);
            if (reg.Success)
            {
                var replace = Convert.ToString(reg.Groups["roman"].Value);
                var orig = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(replace.ToLower());
                result = result.Replace(orig, replace.ToUpper());
            }
            return result;
        }

        /// <summary>
        /// </summary>
        private FFXIV()
        {
            Constants.FFXIVOpen = false;
            Constants.PID = -1;
            Timeline = new Timeline();
            InitMonitors();
            SIO = new Sio();
        }

        /// <summary>
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
        /// </summary>
        /// <returns> </returns>
        private static int GetPID()
        {
            if (Constants.FFXIVOpen && Constants.PID > 0)
            {
                try
                {
                    Process.GetProcessById(Constants.PID);
                    return Constants.PID;
                }
                catch (ArgumentException)
                {
                    Constants.FFXIVOpen = false;
                }
            }
            Constants.FFXIVPID = Process.GetProcessesByName("ffxivgame");
            if (Constants.FFXIVPID.Length == 0)
            {
                Constants.FFXIVOpen = false;
                return -1;
            }
            Constants.FFXIVOpen = true;
            foreach (var proc in Constants.FFXIVPID)
            {
                MainV.View.PIDSelect.Items.Add(proc.Id);
            }
            MainV.View.PIDSelect.SelectedIndex = 0;
            PID(Constants.FFXIVPID.First().Id);
            return Constants.FFXIVPID.First().Id;
        }

        /// <summary>
        /// </summary>
        public static void SetPID()
        {
            StopLogging();
            if (MainV.View.PIDSelect.Text == "")
            {
                return;
            }
            PID(Convert.ToInt32(MainV.View.PIDSelect.Text));
            StartLogging();
        }

        /// <summary>
        /// </summary>
        public static void ResetPID()
        {
            Constants.PID = -1;
        }

        /// <summary>
        /// </summary>
        /// <param name="PID"> </param>
        private static void PID(int PID)
        {
            Constants.PID = PID;
            Constants.PHandle = Constants.FFXIVPID[MainV.View.PIDSelect.SelectedIndex].MainWindowHandle;
        }

        /// <summary>
        /// </summary>
        public static void StartLogging()
        {
            var id = MainV.View.PIDSelect.Text == "" ? GetPID() : Constants.PID;
            if (id < 0)
            {
                MainWindow.View.Title = "FFXIVAPP ~ OFFLINE";
                return;
            }
            //StatusView.View.ToggleLogging.IsEnabled = true;
            //StatusView.View.ResetStats.IsEnabled = true;
            var p = Process.GetProcessById(id);
            var o = new Offsets(p);
            _chatWorker = new ChatWorker(p, o);
            _chatWorker.OnNewline += ChatWorkerDelegate.OnNewline;
            _chatWorker.OnRawline += ChatWorkerDelegate.OnRawLine;
            _chatWorker.StartLogging();
        }

        /// <summary>
        /// </summary>
        public static void StopLogging()
        {
            if (_chatWorker == null)
            {
                return;
            }
            _chatWorker.StopLogging();
            _chatWorker = null;
        }

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
    }
}