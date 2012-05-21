using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using NLog;

namespace UpdateModXIV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>  
    public partial class MainWindow
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        string exe;
        string[] filelist;
        Boolean Success = false;
        WebClient webClient = new WebClient();

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainContainer_Loaded);
        }

        void MainContainer_Loaded(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Properties["file"] != null)
            {
                exe = Application.Current.Properties["file"].ToString();
                string sUrlToReadFileFrom = "http://ffxiv-app.com/files/public/AppModXIV/" + file;
                Process[] aProc = Process.GetProcessesByName(exe);
                foreach (Process p in aProc)
                {
                    p.Kill();
                }
                System.Threading.Thread.Sleep(1000);
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
                System.Threading.Thread.Sleep(1000);
                try
                {
                    webClient.DownloadFileCompleted += Completed;
                    webClient.DownloadProgressChanged += ProgressChanged;
                    webClient.DownloadFileAsync(new Uri(sUrlToReadFileFrom), file);
                }
                catch
                {
                    Logger.Error("ErrorEvent : {0}", "Error getting " + file + ", please try update again.");
                    MessageBox.Show("Error getting " + file + ", please try update again.", "UpdateModXIV - Warning!");
                    Environment.Exit(0);
                }
            }
            else
            {
                //Application.Current.Shutdown();
            }
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;

            progressBarSingle.Value = Math.Truncate(percentage);
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            webClient.DownloadFileCompleted -= Completed;
            webClient.DownloadProgressChanged -= ProgressChanged;
            using (ZipFile zip = ZipFile.Read(ExistingZipFile))
            {
                foreach (ZipEntry e in zip)
                {
                    e.Extract(TargetDirectory, true);  // overwrite == true  
                }
            } 
            //Success = true;
            //webClient.Dispose();
            //try
            //{
            //    if (Success)
            //    {
            //        MessageBox.Show("Download Complete - Launch " + file, "UpdateModXIV - Complete!");
            //        System.Diagnostics.Process m = new System.Diagnostics.Process();
            //        m.StartInfo.FileName = exe;
            //        m.Start();
            //    }
            //}
            //catch
            //{
            //    Logger.Error("ErrorEvent : {0}", "Error launching " + exe + ", please launch manually");
            //    MessageBox.Show("Error launching " + exe + ", please launch manually", "UpdateModXIV - Warning!");
            //}
            //finally
            //{
            //    Application.Current.Shutdown();
            //}
        }
    }
}
