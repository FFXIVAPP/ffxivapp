// FFXIVAPP.Client ~ MessageBoxHelper.cs
// 
// Copyright © 2007 - 2017 Ryan Wilson - All Rights Reserved
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using FFXIVAPP.Common.Helpers;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace FFXIVAPP.Client.Helpers
{
    internal static class MessageBoxHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public static void ShowMessage(string title, string message)
        {
            HandleMessage(title, message);
        }

        /// <summary>
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="okAction"></param>
        /// <param name="cancelAction"></param>
        public static void ShowMessageAsync(string title, string message, Action okAction = null, Action cancelAction = null)
        {
            HandleMessage(title, message, okAction, cancelAction);
        }

        /// <summary>
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="okAction"></param>
        /// <param name="cancelAction"></param>
        private static void HandleMessage(string title, string message, Action okAction = null, Action cancelAction = null)
        {
            DispatcherHelper.Invoke(delegate
            {
                var mw = Application.Current.Windows.OfType<MetroWindow>()
                                    .FirstOrDefault();
                if (mw != null)
                {
                    mw.MetroDialogOptions.AffirmativeButtonText = AppViewModel.Instance.Locale["app_OKButtonText"];
                    mw.MetroDialogOptions.NegativeButtonText = AppViewModel.Instance.Locale["app_CancelButtonText"];
                    if (okAction == null && cancelAction == null)
                    {
                        mw.ShowMessageAsync(title, message);
                    }
                    else
                    {
                        if (cancelAction != null)
                        {
                            mw.ShowMessageAsync(title, message, MessageDialogStyle.AffirmativeAndNegative)
                              .ContinueWith(x => DispatcherHelper.Invoke(delegate
                              {
                                  if (x.Result == MessageDialogResult.Affirmative)
                                  {
                                      if (okAction != null)
                                      {
                                          DispatcherHelper.Invoke(okAction, DispatcherPriority.Send);
                                      }
                                  }
                                  if (x.Result == MessageDialogResult.Negative)
                                  {
                                      DispatcherHelper.Invoke(cancelAction, DispatcherPriority.Send);
                                  }
                              }, DispatcherPriority.Send));
                        }
                        else
                        {
                            mw.ShowMessageAsync(title, message)
                              .ContinueWith(x => DispatcherHelper.Invoke(() => DispatcherHelper.Invoke(okAction), DispatcherPriority.Send));
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"Unable to process MessageBox[{message}]:NotProcessingResult", title, MessageBoxButton.OK);
                }
            }, DispatcherPriority.Send);
        }
    }
}
