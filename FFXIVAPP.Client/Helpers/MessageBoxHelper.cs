// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageBoxHelper.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   MessageBoxHelper.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Helpers {
    using System;
    using System.Linq;
    using System.Windows;
    using Avalonia.Threading;
    using FFXIVAPP.Common.Helpers;

    internal static class MessageBoxHelper {
        /// <summary>
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public static void ShowMessage(string title, string message) {
            HandleMessage(title, message);
        }

        /// <summary>
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="okAction"></param>
        /// <param name="cancelAction"></param>
        public static void ShowMessageAsync(string title, string message, Action okAction = null, Action cancelAction = null) {
            HandleMessage(title, message, okAction, cancelAction);
        }

        /// <summary>
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="okAction"></param>
        /// <param name="cancelAction"></param>
        private static void HandleMessage(string title, string message, Action okAction = null, Action cancelAction = null) {
            DispatcherHelper.Invoke(async () =>{
                await MessageBox.ShowAsync(title, message, cancelAction != null ? MessageDialogStyle.AffirmativeAndNegative : MessageDialogStyle.Affirmative, okAction, cancelAction);
            }, DispatcherPriority.Send);
        }
    }
}