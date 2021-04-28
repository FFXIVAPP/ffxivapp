// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShellView.xaml.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   ShellView.xaml.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client {
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Threading;

    using FFXIVAPP.Client.Helpers;
    using FFXIVAPP.Client.Models;
    using FFXIVAPP.Client.Properties;
    using FFXIVAPP.Common.Helpers;
    using FFXIVAPP.Common.Models;
    using FFXIVAPP.Common.Utilities;

    using NLog;

    /// <summary>
    ///     Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView {
        public static ShellView View;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ShellView() {
            this.InitializeComponent();
            View = this;
            View.Topmost = true;
        }

        public bool IsRendered { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="update"></param>
        public static void CloseApplication(bool update = false) {
            Application.Current.MainWindow.WindowState = WindowState.Normal;
            SettingsHelper.Save(update);
            foreach (PluginInstance pluginInstance in App.Plugins.Loaded.Cast<PluginInstance>().Where(pluginInstance => pluginInstance.Loaded)) {
                pluginInstance.Instance.Dispose(update);
            }

            Func<bool> export = () => SavedlLogsHelper.SaveCurrentLog(false);
            export.BeginInvoke(
                delegate {
                    CloseDelegate(update);
                }, export);
        }

        /// <summary>
        /// </summary>
        /// <param name="update"></param>
        private static void CloseDelegate(bool update = false) {
            AppViewModel.Instance.NotifyIcon.Visible = false;
            if (update) {
                try {
                    Process[] updaters = Process.GetProcessesByName("FFXIVAPP.Updater");
                    foreach (Process updater in updaters) {
                        updater.Kill();
                    }

                    if (File.Exists("FFXIVAPP.Updater.exe")) {
                        File.Delete("FFXIVAPP.Updater.Backup.exe");
                    }

                    File.Move("FFXIVAPP.Updater.exe", "FFXIVAPP.Updater.Backup.exe");
                }
                catch (Exception ex) {
                    Logging.Log(Logger, new LogItem(ex, true));
                }

                Process.Start("FFXIVAPP.Updater.Backup.exe", $"{AppViewModel.Instance.DownloadUri} {AppViewModel.Instance.LatestVersion}");
            }

            Environment.Exit(0);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void MetroWindowClosing(object sender, CancelEventArgs e) {
            e.Cancel = true;
            DispatcherHelper.Invoke(() => CloseApplication(), DispatcherPriority.Send);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindowContentRendered(object sender, EventArgs e) {
            if (this.IsRendered) {
                return;
            }

            this.IsRendered = true;

            if (string.IsNullOrWhiteSpace(Settings.Default.UILanguage)) {
                Settings.Default.UILanguage = Settings.Default.GameLanguage;
            }
            else {
                LocaleHelper.Update(Settings.Default.Culture);
            }

            DispatcherHelper.Invoke(
                delegate {
                    Initializer.LoadAvailableSources();
                    Initializer.LoadAvailablePlugins();
                    Initializer.CheckUpdates();
                    Initializer.SetGlobals();

                    Initializer.StartMemoryWorkers();
                });

            Initializer.GetHomePlugin();
            Initializer.UpdatePluginConstants();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void MetroWindowLoaded(object sender, RoutedEventArgs e) {
            View.Topmost = Settings.Default.TopMost;

            ThemeHelper.ChangeTheme(Settings.Default.Theme, null);

            AppViewModel.Instance.NotifyIcon.Text = "FFXIVAPP";
            AppViewModel.Instance.NotifyIcon.ContextMenu.MenuItems[0].Enabled = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindowStateChanged(object sender, EventArgs e) {
            switch (View.WindowState) {
                case WindowState.Minimized:
                    this.ShowInTaskbar = false;
                    AppViewModel.Instance.NotifyIcon.Text = "FFXIVAPP - Minimized";
                    AppViewModel.Instance.NotifyIcon.ContextMenu.MenuItems[0].Enabled = true;
                    break;
                case WindowState.Normal:
                    this.ShowInTaskbar = true;
                    AppViewModel.Instance.NotifyIcon.Text = "FFXIVAPP";
                    AppViewModel.Instance.NotifyIcon.ContextMenu.MenuItems[0].Enabled = false;
                    break;
            }
        }
    }
}