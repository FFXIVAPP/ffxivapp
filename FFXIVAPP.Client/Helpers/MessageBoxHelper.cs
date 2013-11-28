// FFXIVAPP.Client
// MessageBoxHelper.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Linq;
using System.Windows;
using FFXIVAPP.Common.Helpers;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Helpers
{
    [DoNotObfuscate]
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
                    if (okAction == null && cancelAction == null)
                    {
                        mw.ShowMessageAsync(title, message);
                    }
                    else
                    {
                        mw.MetroDialogOptions.AffirmativeButtonText = AppViewModel.Instance.Locale["app_OKButtonText"];
                        mw.MetroDialogOptions.NegativeButtonText = AppViewModel.Instance.Locale["app_CancelButtonText"];
                        if (cancelAction != null)
                        {
                            mw.ShowMessageAsync(title, message, MessageDialogStyle.AffirmativeAndNegative)
                              .ContinueWith(x => DispatcherHelper.Invoke(delegate
                              {
                                  if (x.Result == MessageDialogResult.Affirmative)
                                  {
                                      if (okAction != null)
                                      {
                                          DispatcherHelper.Invoke(okAction);
                                      }
                                  }
                                  if (x.Result == MessageDialogResult.Negative)
                                  {
                                      DispatcherHelper.Invoke(cancelAction);
                                  }
                              }));
                        }
                        else
                        {
                            mw.ShowMessageAsync(title, message)
                              .ContinueWith(x => DispatcherHelper.Invoke(delegate { DispatcherHelper.Invoke(okAction); }));
                        }
                    }
                }
                else
                {
                    MessageBox.Show(String.Format("Unable to process MessageBox[{0}]:NotProcessingResult", message), title, MessageBoxButton.OK);
                }
            });
        }
    }
}
