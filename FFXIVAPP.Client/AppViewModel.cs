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
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Avalonia.Controls;
    using Avalonia.Styling;
    using Avalonia.Threading;
    using FFXIVAPP.Client.Models;
    using FFXIVAPP.Client.ViewModels;
    using FFXIVAPP.Common.Helpers;
    using Sharlayan.Core;
    using Sharlayan.Models;

    public sealed class AppViewModel: ViewModelBase {
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

        // TODO: private NotifyIcon _notifyIcon;

        private string _pluginsSettingsPath;

        private ObservableCollection<TabItem> _pluginTabItems;

        private List<string> _savedLogsDirectoryList;

        private string _screenShotsPath;

        private string _selected;

        private string _settingsPath;

        private List<Signature> _signatures;

        private string _soundsPath;

        private ObservableCollection<UILanguage> _uiLanguages;

        private string _updateNotes;

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

        /* TODO: NotifyIcon
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
        */

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

        /* TODO: NotifyIcon events
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
        */

        private static void SetLocale() {
            /* TODO: SetLocale (from ShellView.xaml.cs)
            var uiLanguage = ShellView.View?.LanguageSelect.SelectedValue.ToString();
            if (string.IsNullOrWhiteSpace(uiLanguage)) {
                return;
            }

            if (uiLanguage == Settings.Default.GameLanguage) {
                return;
            }

            if (SupportedGameLanguages.Contains(uiLanguage)) {
                if (uiLanguage == Settings.Default.GameLanguage) {
                    return;
                }

                Action ok = () => {
                    Settings.Default.GameLanguage = uiLanguage;
                };
                Action cancel = () => { };
                var title = AppViewModel.Instance.Locale["app_WarningMessage"];
                var message = AppViewModel.Instance.Locale["app_UILanguageChangeWarningGeneral"];
                if (uiLanguage == "Chinese" || Settings.Default.GameLanguage == "Chinese") {
                    message = message + AppViewModel.Instance.Locale["app_UILanguageChangeWarningChinese"];
                }

                MessageBoxHelper.ShowMessageAsync(title, message, ok, cancel);
            }
            else {
                var title = AppViewModel.Instance.Locale["app_WarningMessage"];
                var message = AppViewModel.Instance.Locale["app_UILanguageChangeWarningNoGameLanguage"];
                MessageBoxHelper.ShowMessageAsync(title, message);
            }
            */
        }

        private TabItem _tabSelected;
        public TabItem TabSelected {
            get{
                return _tabSelected;
            } 
            set{
                _tabSelected = value;
                AppViewModel.UpdateTitle();
            } 
        }

        private TabItem _pluginTabSelected;
        public TabItem PluginTabSelected {
            get{
                return _pluginTabSelected;
            } 
            set{
                _pluginTabSelected = value;
                AppViewModel.UpdateSelectedPlugin();
            } 
        }

        /// <summary>
        /// </summary>
        private static void UpdateSelectedPlugin() {
            //var selectedItem = (TabItem) ShellView.View.PluginsTC.SelectedItem;
            try {
                var stack = (StackPanel)AppViewModel.Instance.PluginTabSelected.Header;
                var txt = (TextBlock)stack.Children.Single(c => c.Name == "TheLabel");
                AppViewModel.Instance.Selected = txt.Text;
            }
            catch (Exception) {
                AppViewModel.Instance.Selected = "(NONE)";
            }

            UpdateTitle();
        }

        /// <summary>
        /// </summary>
        public static void UpdateTitle() {
            var currentMain = AppViewModel.Instance.TabSelected?.Name ?? "PluginsTI";
            switch (currentMain) {
                case "PluginsTI":
                    AppViewModel.Instance.AppTitle = $"{AppViewModel.Instance.Selected}";
                    break;
                default:
                    AppViewModel.Instance.AppTitle = currentMain.Substring(0, currentMain.Length - 2);
                    break;
            }
        }
    }
}