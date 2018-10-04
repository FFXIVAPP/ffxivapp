// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotifyIconHelper.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   NotifyIconHelper.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Helpers {
    using System;

    internal static class NotifyIconHelper {
        /// <summary>
        /// </summary>
        /// <param name="title"> </param>
        /// <param name="message"> </param>
        /// <param name="eventHandler"></param>
        public static void ShowBalloonMessage(string title = "Information!", string message = "Unassigned Message", EventHandler eventHandler = null) {
            /* TODO: NotifyIcon ShowBalloonMessage
            if (eventHandler != null) {
                AppViewModel.Instance.NotifyIcon.BalloonTipClicked += eventHandler;
            }

            AppViewModel.Instance.NotifyIcon.ShowBalloonTip(3000, title, message, ToolTipIcon.Info);
            */
        }
    }
}