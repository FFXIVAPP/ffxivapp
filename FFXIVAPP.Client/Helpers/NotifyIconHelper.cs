// FFXIVAPP.Client
// NotifyIconHelper.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Windows.Forms;

#endregion

namespace FFXIVAPP.Client.Helpers {
    internal static class NotifyIconHelper {
        /// <summary>
        /// </summary>
        /// <param name="title"> </param>
        /// <param name="message"> </param>
        /// <param name="eventHandler"></param>
        public static void ShowBalloonMessage(string title = "Information!", string message = "Unassigned Message", EventHandler eventHandler = null) {
            if (eventHandler != null) {
                AppViewModel.Instance.NotifyIcon.BalloonTipClicked += eventHandler;
            }
            AppViewModel.Instance.NotifyIcon.ShowBalloonTip(3000, title, message, ToolTipIcon.Info);
        }
    }
}
