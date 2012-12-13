// Updater
// MainWindow.xaml.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

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
using Ionic.Zip;

#endregion

namespace Updater
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly WebClient _webClient = new WebClient();
        private string _exe;
        private string _file;
        private string _zfile;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainContainer_Loaded(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Properties["file"] == null)
            {
                Application.Current.Shutdown();
            }
            else
            {
                _file = Application.Current.Properties["file"].ToString();
                _exe = _file + ".exe";
                _zfile = _file + ".zip";
                if (File.Exists(_zfile))
                {
                    File.Delete(_zfile);
                }
                var sUrlToReadFileFrom = "http://ffxiv-app.com/files/public/AppModXIV/" + _zfile;
                var app = Process.GetProcessesByName(_file.ToLower());
                foreach (var p in app)
                {
                    try
                    {
                        p.Kill();
                    }
                    catch {}
                }
                Thread.Sleep(1000);
                try
                {
                    _webClient.DownloadFileCompleted += Completed;
                    _webClient.DownloadProgressChanged += ProgressChanged;
                    _webClient.DownloadFileAsync(new Uri(sUrlToReadFileFrom), _zfile);
                }
                catch
                {
                    MessageBox.Show("Error getting " + _zfile + ", please try update again.", "Updater - Warning!");
                    Environment.Exit(0);
                }
            }
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            var bytesIn = double.Parse(e.BytesReceived.ToString(CultureInfo.InvariantCulture));
            var totalBytes = double.Parse(e.TotalBytesToReceive.ToString(CultureInfo.InvariantCulture));
            var percentage = bytesIn / totalBytes * 100;

            progressBarSingle.Value = Math.Truncate(percentage);
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var d = new DirectoryInfo(path).GetFiles();
            foreach (var f in d.Where(t => t.Extension == ".tmp" || t.Extension == ".PendingOverwrite"))
            {
                f.Delete();
            }
            using (var zip = ZipFile.Read(_zfile))
            {
                foreach (var f in zip)
                {
                    try
                    {
                        f.Extract(path, ExtractExistingFileAction.OverwriteSilently);
                    }
                    catch {}
                }
            }
            _webClient.Dispose();
            try
            {
                var m = new Process {
                    StartInfo = {
                        FileName = _exe
                    }
                };
                m.Start();
            }
            catch
            {
                MessageBox.Show("Error launching " + _exe + ", please launch manually", "Updater - Warning!");
            }
            finally
            {
                _webClient.DownloadFileCompleted -= Completed;
                _webClient.DownloadProgressChanged -= ProgressChanged;
                d = new DirectoryInfo(path).GetFiles();
                foreach (var f in d.Where(t => t.Extension == ".tmp" || t.Extension == ".PendingOverwrite"))
                {
                    f.Delete();
                }
                Application.Current.Shutdown();
            }
        }
    }
}
