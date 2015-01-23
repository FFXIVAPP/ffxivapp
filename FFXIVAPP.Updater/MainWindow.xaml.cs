// FFXIVAPP.Updater
// MainWindow.xaml.cs
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
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Ionic.Zip;

namespace FFXIVAPP.Updater
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        #region Auto Properties

        #endregion

        #region Declarations

        private readonly WebClient _webClient = new WebClient();

        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            var properties = Application.Current.Properties;
            if (properties["DownloadUri"] == null || properties["Version"] == null)
            {
                Application.Current.Shutdown();
            }
            else
            {
                MainWindowViewModel.Instance.DownloadURI = properties["DownloadUri"] as string;
                MainWindowViewModel.Instance.Version = properties["Version"] as string;
                MainWindowViewModel.Instance.ZipFileName = String.Format("FFXIVAPP_{0}.zip", MainWindowViewModel.Instance.Version);
                var app = Process.GetProcessesByName("FFXIVAPP.Client");
                foreach (var p in app)
                {
                    try
                    {
                        p.Kill();
                    }
                    catch (Exception ex)
                    {
                    }
                }
                Func<bool> initializeUpdate = delegate
                {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new ThreadStart((DownloadUpdate)));
                    return true;
                };
                initializeUpdate.BeginInvoke(null, null);
            }
        }

        /// <summary>
        /// </summary>
        private void DownloadUpdate()
        {
            GoogleAnalytics.Navigate("http://ffxiv-app.com/Analytics/Google/?eCategory=Application Update&eAction=Download&eLabel=FFXIVAPP");
            GoogleAnalytics.LoadCompleted += delegate
            {
                try
                {
                    _webClient.DownloadFileCompleted += WebClientOnDownloadFileCompleted;
                    _webClient.DownloadProgressChanged += WebClientOnDownloadProgressChanged;
                    _webClient.DownloadFileAsync(new Uri(MainWindowViewModel.Instance.DownloadURI), MainWindowViewModel.Instance.ZipFileName);
                }
                catch (Exception ex)
                {
                    Environment.Exit(0);
                }
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="asyncCompletedEventArgs"></param>
        private void WebClientOnDownloadFileCompleted(object sender, AsyncCompletedEventArgs asyncCompletedEventArgs)
        {
            Func<bool> func = delegate
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new ThreadStart(ExtractAndClean));
                return true;
            };
            func.BeginInvoke(null, null);
        }

        private void ExtractAndClean()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly()
                                                     .Location);
            CleanupTemporary(path);
            using (var zip = ZipFile.Read(MainWindowViewModel.Instance.ZipFileName))
            {
                foreach (var zipEntry in zip)
                {
                    try
                    {
                        if (File.Exists("FFXIVAPP.Client.exe.nlog") && zipEntry.FileName.Contains("FFXIVAPP.Client.exe.nlog"))
                        {
                            continue;
                        }
                        zipEntry.Extract(path, ExtractExistingFileAction.OverwriteSilently);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            _webClient.Dispose();
            try
            {
                var m = new Process
                {
                    StartInfo =
                    {
                        FileName = "FFXIVAPP.Client.exe"
                    }
                };
                m.Start();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _webClient.DownloadFileCompleted -= WebClientOnDownloadFileCompleted;
                _webClient.DownloadProgressChanged -= WebClientOnDownloadProgressChanged;
                CleanupTemporary(path);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="downloadProgressChangedEventArgs"></param>
        private void WebClientOnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs downloadProgressChangedEventArgs)
        {
            var bytesIn = double.Parse(downloadProgressChangedEventArgs.BytesReceived.ToString(CultureInfo.InvariantCulture));
            var totalBytes = double.Parse(downloadProgressChangedEventArgs.TotalBytesToReceive.ToString(CultureInfo.InvariantCulture));
            ProgressBarSingle.Value = bytesIn / totalBytes;
        }

        /// <summary>
        /// </summary>
        private void CleanupTemporary(string path)
        {
            try
            {
                var fileInfos = new DirectoryInfo(path).GetFiles();
                foreach (var fileInfo in fileInfos.Where(t => t.Extension == ".tmp" || t.Extension == ".PendingOverwrite"))
                {
                    fileInfo.Delete();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void UIElement_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void CloseUpdater_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
