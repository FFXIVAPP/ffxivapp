// FFXIVAPP.Client
// ShellView.xaml.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using FFXIVAPP.Client.Delegates;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Models;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Helpers;

#endregion

namespace FFXIVAPP.Client
{
    /// <summary>
    ///     Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView
    {
        #region Property Bindings

        #endregion

        #region Declarations

        #endregion

        public static ShellView View;

        public ShellView()
        {
            InitializeComponent();
            View = this;
            View.Topmost = true;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void MetroWindowLoaded(object sender, RoutedEventArgs e)
        {
            View.Topmost = Settings.Default.TopMost;
            LocaleHelper.Update(Settings.Default.Culture);
            ThemeHelper.ChangeTheme(Settings.Default.Theme, null);
            Initializer.CheckUpdates();
            Initializer.SetGlobals();
            Initializer.GetHomePlugin();
            Initializer.SetSignatures();
            Initializer.StartMemoryWorkers();
            Initializer.SetupPlugins();
            Initializer.UpdatePluginConstants();
            MemoryDelegates.Instance.Initialize();
            AppViewModel.Instance.NotifyIcon.Text = "FFXIVAPP";
            AppViewModel.Instance.NotifyIcon.ContextMenu.MenuItems[0].Enabled = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindowStateChanged(object sender, EventArgs e)
        {
            switch (View.WindowState)
            {
                case WindowState.Minimized:
                    ShowInTaskbar = false;
                    AppViewModel.Instance.NotifyIcon.Text = "FFXIVAPP - Minimized";
                    AppViewModel.Instance.NotifyIcon.ContextMenu.MenuItems[0].Enabled = true;
                    break;
                case WindowState.Normal:
                    ShowInTaskbar = true;
                    AppViewModel.Instance.NotifyIcon.Text = "FFXIVAPP";
                    AppViewModel.Instance.NotifyIcon.ContextMenu.MenuItems[0].Enabled = false;
                    break;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void MetroWindowClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            DispatcherHelper.Invoke(() => CloseApplication());
        }

        /// <summary>
        /// </summary>
        /// <param name="update"></param>
        public static void CloseApplication(bool update = false)
        {
            Application.Current.MainWindow.WindowState = WindowState.Normal;
            SettingsHelper.Save(update);
            foreach (PluginInstance pluginInstance in App.Plugins.Loaded)
            {
                pluginInstance.Instance.Dispose(update);
            }
            Func<bool> exportHistory = () => SavedlLogsHelper.SaveCurrentLog(false);
            exportHistory.BeginInvoke(delegate { CloseDelegate(update); }, exportHistory);
        }

        /// <summary>
        /// </summary>
        /// <param name="update"></param>
        private static void CloseDelegate(bool update = false)
        {
            AppViewModel.Instance.NotifyIcon.Visible = false;
            if (update)
            {
                var updaters = Process.GetProcessesByName("FFXIVAPP.Updater");
                foreach (var updater in updaters)
                {
                    updater.Kill();
                }
                try
                {
                    File.Move("FFXIVAPP.Updater.exe", "FFXIVAPP.Updater.Backup.exe");
                    Process.Start("FFXIVAPP.Updater.Backup.exe", String.Format("{0} {1}", AppViewModel.Instance.DownloadUri, AppViewModel.Instance.LatestVersion));
                }
                catch (Exception ex)
                {
                }
            }
            Environment.Exit(0);
        }
    }
}
