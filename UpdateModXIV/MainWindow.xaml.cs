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
using NLog;

namespace UpdateModXIV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>  
    public partial class MainWindow
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private string file;
        private readonly WebClient _webClient = new WebClient();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainContainer_Loaded(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Properties["file"] != null)
            {
                //Application.Current.Shutdown();
            }
            else
            {
                //file = Application.Current.Properties["file"].ToString();
                file = "ParseModXIV";
                var tfile = file + ".zip";
                var sUrlToReadFileFrom = "http://ffxiv-app.com/files/public/AppModXIV/" + tfile;
                var aProc = Process.GetProcessesByName(file);
                foreach (var p in aProc)
                {
                    p.Kill();
                }
                Thread.Sleep(1000);
                if (File.Exists(tfile))
                {
                    File.Delete(tfile);
                }
                Thread.Sleep(1000);
                try
                {
                    _webClient.DownloadFileCompleted += Completed;
                    _webClient.DownloadProgressChanged += ProgressChanged;
                    _webClient.DownloadFileAsync(new Uri(sUrlToReadFileFrom), tfile);
                }
                catch
                {
                    Logger.Error("ErrorEvent : {0}", "Error getting " + tfile + ", please try update again.");
                    MessageBox.Show("Error getting " + tfile + ", please try update again.", "UpdateModXIV - Warning!");
                    Environment.Exit(0);
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
            using (var zip = ZipFile.Read(file + ".zip"))
            {
                foreach (var efile in zip.Where(efile => efile.FileName != "NLog.dll"))
                {
                    efile.Extract(path, ExtractExistingFileAction.OverwriteSilently); // overwrite == true  
                }
            }
            _webClient.Dispose();
            var tfile = file + ".exe";
            try
            {
                MessageBox.Show("Download Complete - Launch " + tfile, "UpdateModXIV - Complete!");
               Process m = new Process();
               m.StartInfo.FileName = tfile;
                m.Start();
            }
            catch
            {
                Logger.Error("ErrorEvent : {0}", "Error launching " + tfile + ", please launch manually");
                MessageBox.Show("Error launching " + tfile + ", please launch manually", "UpdateModXIV - Warning!");
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