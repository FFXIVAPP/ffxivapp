// FFXIVAPP
// NotifyHelper.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System.Windows.Forms;

namespace FFXIVAPP.Classes
{
    internal static class NotifyHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="bTipTime"> </param>
        /// <param name="title"> </param>
        /// <param name="message"> </param>
        public static void ShowBalloonMessage(int bTipTime, string title, string message)
        {
            MainWindow.MyNotifyIcon.ShowBalloonTip(bTipTime, title, message, ToolTipIcon.Warning);
        }
    }
}