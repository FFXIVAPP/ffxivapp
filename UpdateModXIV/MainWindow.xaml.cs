// UpdateModXIV
// MainWindow.xaml.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

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

namespace UpdateModXIV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>  
    public partial class MainWindow
    {
        private string _file, _exe, _zfile;
        private readonly WebClient _webClient = new WebClient();

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
                var sUrlToReadFileFrom = "http://ffxiv-app.com/files/public/AppModXIV/" + _zfile;
                var running = "";
                var cm = Process.GetProcessesByName("ChatModXIV");
                var wm = Process.GetProcessesByName("Launcher");
                var lm = Process.GetProcessesByName("LogModXIV");
                var pm = Process.GetProcessesByName("ParseModXIV");
                if (cm.Length > 0)
                {
                    running += "\nChatModXIV";
                }
                if (wm.Length > 0)
                {
                    running += "\nWinModXIV";
                }
                if (lm.Length > 0)
                {
                    running += "\nLogModXIV";
                }
                if (pm.Length > 0)
                {
                    running += "\nParseModXIV";
                }
                if (!String.IsNullOrWhiteSpace(running))
                {
                    var result = MessageBox.Show("The following programs that are running will be closed:" + running, "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        foreach (var p in cm)
                        {
                            p.Kill();
                        }
                        foreach (var p in wm)
                        {
                            p.Kill();
                        }
                        foreach (var p in lm)
                        {
                            p.Kill();
                        }
                        foreach (var p in pm)
                        {
                            p.Kill();
                        }
                        Thread.Sleep(1000);
                        if (File.Exists(_zfile))
                        {
                            File.Delete(_zfile);
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
                            MessageBox.Show("Error getting " + _zfile + ", please try update again.", "UpdateModXIV - Warning!");
                            Environment.Exit(0);
                        }
                    }
                }
            }
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            var bytesIn = double.Parse(e.BytesReceived.ToString(CultureInfo.InvariantCulture));
            var totalBytes = double.Parse(e.TotalBytesToReceive.ToString(CultureInfo.InvariantCulture));
            var percentage = bytesIn/totalBytes*100;

            progressBarSingle.Value = Math.Truncate(percentage);
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            using (var zip = ZipFile.Read(_zfile))
            {
                foreach (var f in zip.Where(f => f.FileName != "UpdateModXIV.exe" && f.FileName != "Ionic.Zip.dll"))
                {
                    try
                    {
                        f.Extract(path, ExtractExistingFileAction.OverwriteSilently);
                    }
                    catch
                    {
                    }
                }
            }
            _webClient.Dispose();
            try
            {
                var m = new Process {StartInfo = {FileName = _exe}};
                m.Start();
            }
            catch
            {
                MessageBox.Show("Error launching " + _exe + ", please launch manually", "UpdateModXIV - Warning!");
            }
            finally
            {
                _webClient.DownloadFileCompleted -= Completed;
                _webClient.DownloadProgressChanged -= ProgressChanged;
                Application.Current.Shutdown();
            }
        }
    }
}