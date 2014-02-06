// FFXIVAPP.Client
// ShellView.xaml.cs
// 
// © 2013 Ryan Wilson

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Models;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Helpers;
using NLog;

namespace FFXIVAPP.Client
{
    /// <summary>
    ///     Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

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

            #region Initial BootStrapping

            Initializer.SetupCurrentUICulture();
            Initializer.LoadChatCodes();
            Initializer.LoadAutoTranslate();
            Initializer.LoadActions();
            Initializer.LoadColors();
            Initializer.LoadApplicationSettings();
            Initializer.LoadSoundsIntoCache();
            Initializer.LoadPlugins();
            Initializer.LoadAvailableSources();
            Initializer.LoadAvailablePlugins();

            #endregion

            LocaleHelper.Update(Settings.Default.Culture);
            ThemeHelper.ChangeTheme(Settings.Default.Theme, null);

            #region GUI Finalization

            Initializer.CheckUpdates();
            Initializer.SetGlobals();
            Initializer.SetSignatures();
            Initializer.StartMemoryWorkers();
            Initializer.SetupParsePlugin();
            Initializer.GetHomePlugin();
            Initializer.UpdatePluginConstants();

            #endregion

            AppViewModel.Instance.NotifyIcon.Text = "FFXIVAPP";
            AppViewModel.Instance.NotifyIcon.ContextMenu.MenuItems[0].Enabled = false;
            AppBootstrapper.Instance.ProcessDetachCheckTimer.Enabled = true;
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
            DispatcherHelper.Invoke(() => CloseApplication(), DispatcherPriority.Send);
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
                try
                {
                    var updaters = Process.GetProcessesByName("FFXIVAPP.Updater");
                    foreach (var updater in updaters)
                    {
                        updater.Kill();
                    }
                    if (File.Exists("FFXIVAPP.Updater.exe"))
                    {
                        File.Delete("FFXIVAPP.Updater.Backup.exe");
                    }
                    File.Move("FFXIVAPP.Updater.exe", "FFXIVAPP.Updater.Backup.exe");
                }
                catch (Exception ex)
                {
                }
                Process.Start("FFXIVAPP.Updater.Backup.exe", String.Format("{0} {1}", AppViewModel.Instance.DownloadUri, AppViewModel.Instance.LatestVersion));
            }
            Environment.Exit(0);
        }
    }
}
