// FFXIVAPP.Updater
// MainWindow.xaml.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Ionic.Zip;

#endregion

namespace FFXIVAPP.Updater
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly WebClient _webClient = new WebClient();

        public MainWindow()
        {
            InitializeComponent();
        }

        private string DownloadUri { get; set; }
        private string Version { get; set; }
        private string ZipFileName { get; set; }

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
                DownloadUri = properties["DownloadUri"] as string;
                Version = properties["Version"] as string;
                ZipFileName = String.Format("FFXIVAPP_{0}.zip", Version);
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
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, new ThreadStart((DownloadUpdate)));
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
                Thread.Sleep(1000);
                try
                {
                    _webClient.DownloadFileCompleted += WebClientOnDownloadFileCompleted;
                    _webClient.DownloadProgressChanged += WebClientOnDownloadProgressChanged;
                    _webClient.DownloadFileAsync(new Uri(DownloadUri), ZipFileName);
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
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly()
                                                     .Location);
            CleanupTemporary(path);
            using (var zip = ZipFile.Read(ZipFileName))
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
            ProgressBarSingle.Value = Math.Truncate(bytesIn / totalBytes * 100);
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
    }
}
