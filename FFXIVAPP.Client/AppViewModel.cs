// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppViewModel.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   AppViewModel.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Forms;
    using System.Windows.Threading;

    using FFXIVAPP.Client.Models;
    using FFXIVAPP.Client.Properties;
    using FFXIVAPP.Common.Helpers;

    using Sharlayan.Core;
    using Sharlayan.Models;

    using ContextMenu = System.Windows.Forms.ContextMenu;

    [Export(typeof(AppViewModel))]
    internal sealed class AppViewModel : INotifyPropertyChanged {
        private static bool _hasPlugins;

        private static Lazy<AppViewModel> _instance = new Lazy<AppViewModel>(() => new AppViewModel());

        private string _appTitle;

        private List<ChatLogItem> _chatHistory;

        private string _configurationsPath;

        private string _currentVersion;

        private string _downloadUri;

        private bool _hasNewPluginUpdate;

        private bool _hasNewVersion;

        private string _latestVersion;

        private Dictionary<string, string> _locale;

        private string _logsPath;

        private NotifyIcon _notifyIcon;

        private string _pluginsSettingsPath;

        private ObservableCollection<TabItem> _pluginTabItems;

        private List<string> _savedLogsDirectoryList;

        private string _screenShotsPath;

        private string _selected;

        private string _settingsPath;

        private List<Signature> _signatures;

        private string _soundsPath;

        private Style _tabControlCollapsedHeader;

        private ObservableCollection<UILanguage> _uiLanguages;

        private string _updateNotes;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public static AppViewModel Instance {
            get {
                return _instance.Value;
            }
        }

        public string AppTitle {
            get {
                return this._appTitle;
            }

            set {
                var tempvalue = Constants.IsOpen
                                    ? value
                                    : $"{value} : OFFLINE";
                this._appTitle = string.IsNullOrWhiteSpace(tempvalue)
                                     ? "FFXIVAPP"
                                     : $"FFXIVAPP ~ {tempvalue}";
                this.RaisePropertyChanged();
            }
        }

        public List<ChatLogItem> ChatHistory {
            get {
                return this._chatHistory ?? (this._chatHistory = new List<ChatLogItem>());
            }

            set {
                this._chatHistory = value;
                this.RaisePropertyChanged();
            }
        }

        public string ConfigurationsPath {
            get {
                return this._configurationsPath;
            }

            set {
                this._configurationsPath = value;
                if (!Directory.Exists(this._configurationsPath)) {
                    Directory.CreateDirectory(this._configurationsPath);
                }

                this.RaisePropertyChanged();
            }
        }

        public string Copyright {
            get {
                object[] att = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                return att.Length == 0
                           ? string.Empty
                           : ((AssemblyCopyrightAttribute) att[0]).Copyright;
            }
        }

        public string CurrentVersion {
            get {
                return this._currentVersion;
            }

            set {
                this._currentVersion = value;
                this.RaisePropertyChanged();
            }
        }

        public string DownloadUri {
            get {
                return this._downloadUri;
            }

            set {
                this._downloadUri = value;
                this.RaisePropertyChanged();
            }
        }

        public bool HasNewPluginUpdate {
            get {
                return this._hasNewPluginUpdate;
            }

            set {
                this._hasNewPluginUpdate = value;
                this.RaisePropertyChanged();
            }
        }

        public bool HasNewVersion {
            get {
                return this._hasNewVersion;
            }

            set {
                this._hasNewVersion = value;
                this.RaisePropertyChanged();
            }
        }

        public bool HasPlugins {
            get {
                return _hasPlugins;
            }

            set {
                _hasPlugins = value;
                this.RaisePropertyChanged();
            }
        }

        public string LatestVersion {
            get {
                return this._latestVersion;
            }

            set {
                this._latestVersion = value;
                this.RaisePropertyChanged();
            }
        }

        public Dictionary<string, string> Locale {
            get {
                return this._locale ?? (this._locale = new Dictionary<string, string>());
            }

            set {
                this._locale = value;
                this.RaisePropertyChanged();
            }
        }

        public string LogsPath {
            get {
                return this._logsPath;
            }

            set {
                this._logsPath = value;
                if (!Directory.Exists(this._logsPath)) {
                    Directory.CreateDirectory(this._logsPath);
                }

                this.RaisePropertyChanged();
            }
        }

        public NotifyIcon NotifyIcon {
            get {
                if (this._notifyIcon == null) {
                    using (Stream iconStream = ResourceHelper.StreamResource(Constants.AppPack + "FFXIVAPP.ico").Stream) {
                        this._notifyIcon = new NotifyIcon {
                            Icon = new Icon(iconStream),
                            Visible = true
                        };
                        iconStream.Dispose();
                        this._notifyIcon.Text = "FFXIVAPP";
                        var contextMenu = new ContextMenu();
                        contextMenu.MenuItems.Add("&Restore Application").Enabled = false;
                        contextMenu.MenuItems.Add("&Exit");
                        contextMenu.MenuItems[0].Click += NotifyIconOnRestoreClick;
                        contextMenu.MenuItems[1].Click += NotifyIconOnExitClick;
                        this._notifyIcon.ContextMenu = contextMenu;
                        this._notifyIcon.MouseDoubleClick += NotifyIconOnMouseDoubleClick;
                    }
                }

                return this._notifyIcon;
            }
        }

        public string PluginsSettingsPath {
            get {
                return this._pluginsSettingsPath;
            }

            set {
                this._pluginsSettingsPath = value;
                if (!Directory.Exists(this._pluginsSettingsPath)) {
                    Directory.CreateDirectory(this._pluginsSettingsPath);
                }

                this.RaisePropertyChanged();
            }
        }

        public ObservableCollection<TabItem> PluginTabItems {
            get {
                return this._pluginTabItems ?? (this._pluginTabItems = new ObservableCollection<TabItem>());
            }

            set {
                this._pluginTabItems = value;
                this.RaisePropertyChanged();
            }
        }

        public List<string> SavedLogsDirectoryList {
            get {
                return this._savedLogsDirectoryList ?? (this._savedLogsDirectoryList = new List<string>());
            }

            set {
                foreach (var directoryPath in value) {
                    var path = Path.Combine(this.LogsPath, directoryPath);
                    if (!Directory.Exists(path)) {
                        Directory.CreateDirectory(path);
                    }
                }

                this._savedLogsDirectoryList = value;
                this.RaisePropertyChanged();
            }
        }

        public string ScreenShotsPath {
            get {
                return this._screenShotsPath;
            }

            set {
                this._screenShotsPath = value;
                if (!Directory.Exists(this._screenShotsPath)) {
                    Directory.CreateDirectory(this._screenShotsPath);
                }

                this.RaisePropertyChanged();
            }
        }

        public string Selected {
            get {
                return this._selected;
            }

            set {
                this._selected = value;
                this.RaisePropertyChanged();
            }
        }

        public string SettingsPath {
            get {
                return this._settingsPath;
            }

            set {
                this._settingsPath = value;
                if (!Directory.Exists(this._settingsPath)) {
                    Directory.CreateDirectory(this._settingsPath);
                }

                this.RaisePropertyChanged();
            }
        }

        public List<Signature> Signatures {
            get {
                return this._signatures ?? (this._signatures = new List<Signature>());
            }

            set {
                this._signatures = value;
                this.RaisePropertyChanged();
            }
        }

        public string SoundsPath {
            get {
                return this._soundsPath;
            }

            set {
                this._soundsPath = value;
                if (!Directory.Exists(this._soundsPath)) {
                    Directory.CreateDirectory(this._soundsPath);
                }

                this.RaisePropertyChanged();
            }
        }

        public Style TabControlCollapsedHeader {
            get {
                if (this._tabControlCollapsedHeader == null) {
                    var s = new Style();
                    s.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Collapsed));
                    this._tabControlCollapsedHeader = s;
                }

                return this._tabControlCollapsedHeader;
            }

            set {
                this._tabControlCollapsedHeader = value;
                this.RaisePropertyChanged();
            }
        }

        public ObservableCollection<UILanguage> UILanguages {
            get {
                return this._uiLanguages ?? (this._uiLanguages = new ObservableCollection<UILanguage>());
            }

            set {
                this._uiLanguages = value;
                this.RaisePropertyChanged();
            }
        }

        public string UpdateNotes {
            get {
                return this._updateNotes;
            }

            set {
                this._updateNotes = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private static void NotifyIconOnExitClick(object sender, EventArgs eventArgs) {
            DispatcherHelper.Invoke(() => ShellView.CloseApplication(), DispatcherPriority.Send);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private static void NotifyIconOnMouseDoubleClick(object sender, MouseEventArgs mouseEventArgs) {
            ShellView.View.WindowState = WindowState.Normal;
            ShellView.View.Topmost = true;
            ShellView.View.Topmost = Settings.Default.TopMost;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private static void NotifyIconOnRestoreClick(object sender, EventArgs eventArgs) {
            ShellView.View.WindowState = WindowState.Normal;
            ShellView.View.Topmost = true;
            ShellView.View.Topmost = Settings.Default.TopMost;
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
    }
}