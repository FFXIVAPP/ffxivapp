// FFXIVAPP.Client ~ ShellView.xaml.cs
// 
// Copyright © 2007 - 2017 Ryan Wilson - All Rights Reserved
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

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

        public static ShellView View;

        public ShellView()
        {
            InitializeComponent();
            View = this;
            View.Topmost = true;
        }

        #region Property Bindings

        public bool IsRendered { get; set; }

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void MetroWindowLoaded(object sender, RoutedEventArgs e)
        {
            View.Topmost = Settings.Default.TopMost;

            ThemeHelper.ChangeTheme(Settings.Default.Theme, null);

            AppViewModel.Instance.NotifyIcon.Text = "FFXIVAPP";
            AppViewModel.Instance.NotifyIcon.ContextMenu.MenuItems[0]
                        .Enabled = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindowContentRendered(object sender, EventArgs e)
        {
            if (IsRendered)
            {
                return;
            }
            IsRendered = true;

            #region GUI Finalization

            if (String.IsNullOrWhiteSpace(Settings.Default.UILanguage))
            {
                Settings.Default.UILanguage = Settings.Default.GameLanguage;
            }
            else
            {
                LocaleHelper.Update(Settings.Default.Culture);
            }

            DispatcherHelper.Invoke(delegate
            {
                Initializer.LoadAvailableSources();
                Initializer.LoadAvailablePlugins();
                Initializer.CheckUpdates();
                Initializer.SetGlobals();

                Initializer.StartMemoryWorkers();
                if (Settings.Default.EnableNetworkReading && !Initializer.NetworkWorking)
                {
                    Initializer.StartNetworkWorker();
                }
            });

            Initializer.GetHomePlugin();
            Initializer.UpdatePluginConstants();

            #endregion
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
                    AppViewModel.Instance.NotifyIcon.ContextMenu.MenuItems[0]
                                .Enabled = true;
                    break;
                case WindowState.Normal:
                    ShowInTaskbar = true;
                    AppViewModel.Instance.NotifyIcon.Text = "FFXIVAPP";
                    AppViewModel.Instance.NotifyIcon.ContextMenu.MenuItems[0]
                                .Enabled = false;
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
            foreach (var pluginInstance in App.Plugins.Loaded.Cast<PluginInstance>()
                                              .Where(pluginInstance => pluginInstance.Loaded))
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
                    Logging.Log(Logger, new LogItem(ex, true));
                }
                Process.Start("FFXIVAPP.Updater.Backup.exe", $"{AppViewModel.Instance.DownloadUri} {AppViewModel.Instance.LatestVersion}");
            }
            Environment.Exit(0);
        }

        #region Declarations

        #endregion
    }
}
