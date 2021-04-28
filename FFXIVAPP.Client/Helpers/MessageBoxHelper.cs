// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageBoxHelper.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
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
    using System.Windows.Threading;

    using FFXIVAPP.Common.Helpers;

    using MahApps.Metro.Controls;
    using MahApps.Metro.Controls.Dialogs;

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
            DispatcherHelper.Invoke(
                delegate {
                    MetroWindow mw = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
                    if (mw != null) {
                        mw.MetroDialogOptions.AffirmativeButtonText = AppViewModel.Instance.Locale["app_OKButtonText"];
                        mw.MetroDialogOptions.NegativeButtonText = AppViewModel.Instance.Locale["app_CancelButtonText"];
                        if (okAction == null && cancelAction == null) {
                            mw.ShowMessageAsync(title, message);
                        }
                        else {
                            if (cancelAction != null) {
                                mw.ShowMessageAsync(title, message, MessageDialogStyle.AffirmativeAndNegative).ContinueWith(
                                    x => DispatcherHelper.Invoke(
                                        delegate {
                                            if (x.Result == MessageDialogResult.Affirmative) {
                                                if (okAction != null) {
                                                    DispatcherHelper.Invoke(okAction, DispatcherPriority.Send);
                                                }
                                            }

                                            if (x.Result == MessageDialogResult.Negative) {
                                                DispatcherHelper.Invoke(cancelAction, DispatcherPriority.Send);
                                            }
                                        }, DispatcherPriority.Send));
                            }
                            else {
                                mw.ShowMessageAsync(title, message).ContinueWith(x => DispatcherHelper.Invoke(() => DispatcherHelper.Invoke(okAction), DispatcherPriority.Send));
                            }
                        }
                    }
                    else {
                        MessageBox.Show($"Unable to process MessageBox[{message}]:NotProcessingResult", title, MessageBoxButton.OK);
                    }
                }, DispatcherPriority.Send);
        }
    }
}