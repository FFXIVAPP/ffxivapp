// Project: ChatModXIV
// File: ChatMod.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using AppModXIV.Classes;
using AppModXIV.Memory;
using ChatModXIV.View;

namespace ChatModXIV.Classes
{
    public class ChatMod
    {
        private static ChatMod _instance;
        private ChatWorker _chatWorker;
        private Boolean _isLogging;

        /// <summary>
        /// 
        /// </summary>
        public static ChatMod Instance
        {
            get { return _instance ?? (_instance = new ChatMod()); }
        }

        /// <summary>
        /// 
        /// </summary>
        private ChatMod()
        {
            Constants.FfxivOpen = false;
            Constants.Pid = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int GetPid()
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
                MainWindow.View.Title = "ChatModXIV ~ Final Fantasy XIV Not Found!";
                return;
            }
            _isLogging = true;
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