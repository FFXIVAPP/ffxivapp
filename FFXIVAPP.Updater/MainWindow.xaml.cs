// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   MainWindow.xaml.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Updater {
    using System;
    using System.Collections;
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

    using NLog;

    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly WebClient _webClient = new WebClient();

        public MainWindow() {
            this.InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// </summary>
        private void CleanupTemporary(string path) {
            try {
                FileInfo[] fileInfos = new DirectoryInfo(path).GetFiles();
                foreach (FileInfo fileInfo in fileInfos.Where(t => t.Extension == ".tmp" || t.Extension == ".PendingOverwrite")) {
                    fileInfo.Delete();
                }
            }
            catch (Exception) {
                // IGNORED
            }
        }

        private void CloseUpdater_OnClick(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown(0);
        }

        /// <summary>
        /// </summary>
        private void DownloadUpdate() {
            try {
                this._webClient.DownloadFileCompleted += this.WebClientOnDownloadFileCompleted;
                this._webClient.DownloadProgressChanged += this.WebClientOnDownloadProgressChanged;
                this._webClient.DownloadFileAsync(new Uri(MainWindowViewModel.Instance.DownloadURI), MainWindowViewModel.Instance.ZipFileName);
            }
            catch (Exception) {
                Environment.Exit(0);
            }
        }

        private void ExtractAndClean() {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            this.CleanupTemporary(path);
            using (ZipFile zip = ZipFile.Read(MainWindowViewModel.Instance.ZipFileName)) {
                foreach (ZipEntry zipEntry in zip) {
                    try {
                        if (File.Exists("FFXIVAPP.Client.exe.nlog") && zipEntry.FileName.Contains("FFXIVAPP.Client.exe.nlog")) {
                            continue;
                        }

                        zipEntry.Extract(path, ExtractExistingFileAction.OverwriteSilently);
                    }
                    catch (Exception ex) {
                        // IGNORED
                    }
                }
            }

            this._webClient.Dispose();
            try {
                var m = new Process {
                    StartInfo = {
                        FileName = "FFXIVAPP.Client.exe",
                    },
                };
                m.Start();
            }
            catch (Exception) {
                // IGNORED
            }
            finally {
                this._webClient.DownloadFileCompleted -= this.WebClientOnDownloadFileCompleted;
                this._webClient.DownloadProgressChanged -= this.WebClientOnDownloadProgressChanged;
                this.CleanupTemporary(path);
                Environment.Exit(0);
            }
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        private void UIElement_OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
            if (Mouse.LeftButton == MouseButtonState.Pressed) {
                this.DragMove();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="asyncCompletedEventArgs"></param>
        private void WebClientOnDownloadFileCompleted(object sender, AsyncCompletedEventArgs asyncCompletedEventArgs) {
            Func<bool> download = delegate {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new ThreadStart(this.ExtractAndClean));
                return true;
            };
            download.BeginInvoke(delegate { }, download);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="downloadProgressChangedEventArgs"></param>
        private void WebClientOnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs downloadProgressChangedEventArgs) {
            var bytesIn = double.Parse(downloadProgressChangedEventArgs.BytesReceived.ToString(CultureInfo.InvariantCulture));
            var totalBytes = double.Parse(downloadProgressChangedEventArgs.TotalBytesToReceive.ToString(CultureInfo.InvariantCulture));
            this.ProgressBarSingle.Value = bytesIn / totalBytes;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowLoaded(object sender, RoutedEventArgs e) {
            IDictionary properties = Application.Current.Properties;
            if (properties["DownloadUri"] == null || properties["Version"] == null) {
                Application.Current.Shutdown();
            }
            else {
                MainWindowViewModel.Instance.DownloadURI = properties["DownloadUri"] as string;
                MainWindowViewModel.Instance.Version = properties["Version"] as string;
                MainWindowViewModel.Instance.ZipFileName = $"FFXIVAPP_{MainWindowViewModel.Instance.Version}.zip";
                Process[] app = Process.GetProcessesByName("FFXIVAPP.Client");
                foreach (Process p in app) {
                    try {
                        p.Kill();
                    }
                    catch (Exception) {
                        // IGNORED
                    }
                }

                Func<bool> update = delegate {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new ThreadStart(this.DownloadUpdate));
                    return true;
                };
                update.BeginInvoke(delegate { }, update);
            }
        }
    }
}