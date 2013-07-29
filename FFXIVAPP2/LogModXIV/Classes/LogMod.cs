// LogModXIV
// LogMod.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using AppModXIV.Classes;
using AppModXIV.Memory;
using LogModXIV.View;

namespace LogModXIV.Classes
{
    public class LogMod
    {
        private static LogMod _instance;
        private ChatWorker _chatWorker;
        public Boolean IsLogging;

        /// <summary>
        /// 
        /// </summary>
        public static LogMod Instance
        {
            get { return _instance ?? (_instance = new LogMod()); }
        }

        /// <summary>
        /// 
        /// </summary>
        private LogMod()
        {
            Constants.FfxivOpen = false;
            Constants.Pid = -1;
        }

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
                MainWindow.View.Title = "LogModXIV ~ Final Fantasy XIV Not Found!";
                return;
            }
            IsLogging = true;
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
    }
}