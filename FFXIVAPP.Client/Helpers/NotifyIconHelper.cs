// FFXIVAPP.Client
// NotifyIconHelper.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Windows.Forms;

#endregion

namespace FFXIVAPP.Client.Helpers
{
    internal static class NotifyIconHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="title"> </param>
        /// <param name="message"> </param>
        /// <param name="eventHandler"></param>
        public static void ShowBalloonMessage(string title = "Information!", string message = "Unassigned Message", EventHandler eventHandler = null)
        {
            if (eventHandler != null)
            {
                AppViewModel.Instance.NotifyIcon.BalloonTipClicked += eventHandler;
            }
            AppViewModel.Instance.NotifyIcon.ShowBalloonTip(3000, title, message, ToolTipIcon.Info);
        }
    }
}
