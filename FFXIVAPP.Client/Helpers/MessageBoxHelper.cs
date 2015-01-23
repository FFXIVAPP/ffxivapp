// FFXIVAPP.Client
// MessageBoxHelper.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using FFXIVAPP.Common.Helpers;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace FFXIVAPP.Client.Helpers
{
    public static class MessageBoxHelper
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
                    MessageBox.Show(String.Format("Unable to process MessageBox[{0}]:NotProcessingResult", message), title, MessageBoxButton.OK);
                }
            }, DispatcherPriority.Send);
        }
    }
}
