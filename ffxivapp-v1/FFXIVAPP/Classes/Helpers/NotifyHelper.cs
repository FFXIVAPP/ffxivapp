// FFXIVAPP
// NotifyHelper.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System.Windows.Forms;

#endregion

namespace FFXIVAPP.Classes.Helpers
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
