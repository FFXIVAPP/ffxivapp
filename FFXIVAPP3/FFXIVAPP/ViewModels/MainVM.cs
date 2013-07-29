// FFXIVAPP
// MainVM.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Windows.Input;
using FFXIVAPP.Classes;
using FFXIVAPP.Classes.Commands;
using FFXIVAPP.Views;

namespace FFXIVAPP.ViewModels
{
    public class MainVM
    {
        public ICommand ResetStatsCommand { get; private set; }
        public ICommand SetProcessCommand { get; private set; }
        public ICommand RefreshListCommand { get; private set; }
        public ICommand ResetExpCommand { get; private set; }

        public MainVM()
        {
            ResetStatsCommand = new DelegateCommand(ResetStats);
            SetProcessCommand = new DelegateCommand(SetProcess);
            RefreshListCommand = new DelegateCommand(RefreshList);
            ResetExpCommand = new DelegateCommand(ResetExp);
        }

        #region GUI Functions

        /// <summary>
        /// </summary>
        private static void ResetStats()
        {
            ClearStats();
        }

        /// <summary>
        /// </summary>
        internal static void ClearStats()
        {
            if (App.MArgs == null)
            {
                if (Settings.Default.Parse_SaveLog)
                {
                    if (ChatWorkerDelegate.ParseXmlWriteLog.LineCount > 1)
                    {
                        ChatWorkerDelegate.ParseXmlWriteLog.WriteToDisk(MainWindow.View.Lpath + DateTime.Now.ToString("dd.MM.yyyy.HH.mm.ss") + "_Log.xml");
                        ChatWorkerDelegate.ParseXmlWriteLog.ClearXml();
                    }
                }
                if (ChatWorkerDelegate.ParseXmlWriteUnmatchedLog.LineCount > 1)
                {
                    ChatWorkerDelegate.ParseXmlWriteUnmatchedLog.WriteToDisk(MainWindow.View.Lpath + DateTime.Now.ToString("dd.MM.yyyy.HH.mm.ss") + "_Parse_Unmatched_Log.xml");
                    ChatWorkerDelegate.ParseXmlWriteUnmatchedLog.ClearXml();
                }
            }
            MainWindow.View.ParseView.MA._FD.Blocks.Clear();
            FFXIV.Instance.StatMonitor.Clear();
        }

        /// <summary>
        /// </summary>
        private static void SetProcess()
        {
            FFXIV.SetPID();
        }

        /// <summary>
        /// </summary>
        private static void RefreshList()
        {
            MainV.View.PIDSelect.Items.Clear();
            FFXIV.StopLogging();
            FFXIV.ResetPID();
            FFXIV.StartLogging();
        }

        /// <summary>
        /// </summary>
        private static void ResetExp()
        {
            MainWindow.TotalExp = 0;
            MainWindow.StartTime = DateTime.Now;
        }

        #endregion
    }
}