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
using System.Windows.Media.Imaging;
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
            ThemeHelper.ChangeTheme(Settings.Default.Theme);
            Initializer.CheckUpdates();
            Initializer.SetGlobals();
            Initializer.GetHomePlugin();
            Initializer.SetSignatures();
            Initializer.StartMemoryWorkers();
            AppViewModel.Instance.NotifyIcon.Text = "FFXIVAPP";
            AppViewModel.Instance.NotifyIcon.ContextMenu.MenuItems[0].Enabled = false;
            // get official plugin logos
            var eventPluginLogo = new BitmapImage(new Uri(Common.Constants.AppPack + "Resources/Media/Icons/Event.png"));
            var informerPluginLogo = new BitmapImage(new Uri(Common.Constants.AppPack + "Resources/Media/Icons/Informer.png"));
            var logPluginLogo = new BitmapImage(new Uri(Common.Constants.AppPack + "Resources/Media/Icons/Log.png"));
            var parsePluginLogo = new BitmapImage(new Uri(Common.Constants.AppPack + "Resources/Media/Icons/Parse.png"));
            // setup headers for existing plugins
            EventPlugin.HeaderTemplate = TabItemHelper.ImageHeader(eventPluginLogo, "Event");
            InformerPlugin.HeaderTemplate = TabItemHelper.ImageHeader(informerPluginLogo, "Informer");
            LogPlugin.HeaderTemplate = TabItemHelper.ImageHeader(logPluginLogo, "Log");
            ParsePlugin.HeaderTemplate = TabItemHelper.ImageHeader(parsePluginLogo, "Parse");
            // append third party plugins
            foreach (var pluginTabItem in AppViewModel.Instance.PluginTabItems)
            {
                View.PluginsTC.Items.Add(pluginTabItem);
            }
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
