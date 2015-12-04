// FFXIVAPP.Updater ~ MainWindow.xaml.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
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

        #region Auto Properties

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
