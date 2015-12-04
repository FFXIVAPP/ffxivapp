// FFXIVAPP.Client ~ AppViewModel.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
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
using FFXIVAPP.Memory.Core;
using FFXIVAPP.Memory.Models;
using ContextMenu = System.Windows.Forms.ContextMenu;

namespace FFXIVAPP.Client
{
    [Export(typeof (AppViewModel))]
    internal sealed class AppViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static AppViewModel _instance;
        private static bool _hasPlugins;
        private string _appTitle;
        private List<ChatLogEntry> _chatHistory;
        private string _configurationsPath;
        private string _currentVersion;
        private string _downloadUri;
        private bool _hasNewPluginUpdate;
        private bool _hasNewVersion;
        private string _latestVersion;
        private Dictionary<string, string> _locale;
        private string _logsPath;
        private NotifyIcon _notifyIcon;
        private ObservableCollection<TabItem> _pluginTabItems;
        private string _pluginsSettingsPath;
        private List<string> _savedLogsDirectoryList;
        private string _screenShotsPath;
        private string _selected;
        private string _settingsPath;
        private List<Signature> _signatures;
        private string _soundsPath;
        private Style _tabControlCollapsedHeader;
        private string _updateNotes;

        #region UILanguages

        private ObservableCollection<UILanguage> _uiLanguages;

        public ObservableCollection<UILanguage> UILanguages
        {
            get { return _uiLanguages ?? (_uiLanguages = new ObservableCollection<UILanguage>()); }
            set
            {
                _uiLanguages = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public static AppViewModel Instance
        {
            get { return _instance ?? (_instance = new AppViewModel()); }
        }

        public Dictionary<string, string> Locale
        {
            get { return _locale ?? (_locale = new Dictionary<string, string>()); }
            set
            {
                _locale = value;
                RaisePropertyChanged();
            }
        }

        public NotifyIcon NotifyIcon
        {
            get
            {
                if (_notifyIcon == null)
                {
                    using (var iconStream = ResourceHelper.StreamResource(Common.Constants.AppPack + "Resources/Media/Icons/FFXIVAPP.ico")
                                                          .Stream)
                    {
                        _notifyIcon = new NotifyIcon
                        {
                            Icon = new Icon(iconStream),
                            Visible = true
                        };
                        iconStream.Dispose();
                        _notifyIcon.Text = "FFXIVAPP";
                        var contextMenu = new ContextMenu();
                        contextMenu.MenuItems.Add("&Restore Application")
                                   .Enabled = false;
                        contextMenu.MenuItems.Add("&Exit");
                        contextMenu.MenuItems[0].Click += NotifyIconOnRestoreClick;
                        contextMenu.MenuItems[1].Click += NotifyIconOnExitClick;
                        _notifyIcon.ContextMenu = contextMenu;
                        _notifyIcon.MouseDoubleClick += NotifyIconOnMouseDoubleClick;
                    }
                }
                return _notifyIcon;
            }
        }

        public string AppTitle
        {
            get { return _appTitle; }
            set
            {
                var tempvalue = Constants.IsOpen ? value : String.Format("{0} : OFFLINE", value);
                _appTitle = String.IsNullOrWhiteSpace(tempvalue) ? "FFXIVAPP" : String.Format("FFXIVAPP ~ {0}", tempvalue);
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<TabItem> PluginTabItems
        {
            get { return _pluginTabItems ?? (_pluginTabItems = new ObservableCollection<TabItem>()); }
            set
            {
                _pluginTabItems = value;
                RaisePropertyChanged();
            }
        }

        public bool HasPlugins
        {
            get { return _hasPlugins; }
            set
            {
                _hasPlugins = value;
                RaisePropertyChanged();
            }
        }

        public string Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                RaisePropertyChanged();
            }
        }

        public Style TabControlCollapsedHeader
        {
            get
            {
                if (_tabControlCollapsedHeader == null)
                {
                    var s = new Style();
                    s.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Collapsed));
                    _tabControlCollapsedHeader = s;
                }
                return _tabControlCollapsedHeader;
            }
            set
            {
                _tabControlCollapsedHeader = value;
                RaisePropertyChanged();
            }
        }

        public string ConfigurationsPath
        {
            get { return _configurationsPath; }
            set
            {
                _configurationsPath = value;
                if (!Directory.Exists(_configurationsPath))
                {
                    Directory.CreateDirectory(_configurationsPath);
                }
                RaisePropertyChanged();
            }
        }

        public string LogsPath
        {
            get { return _logsPath; }
            set
            {
                _logsPath = value;
                if (!Directory.Exists(_logsPath))
                {
                    Directory.CreateDirectory(_logsPath);
                }
                RaisePropertyChanged();
            }
        }

        public List<string> SavedLogsDirectoryList
        {
            get { return _savedLogsDirectoryList ?? (_savedLogsDirectoryList = new List<string>()); }
            set
            {
                foreach (var directoryPath in value)
                {
                    var path = Path.Combine(LogsPath, directoryPath);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }
                _savedLogsDirectoryList = value;
                RaisePropertyChanged();
            }
        }

        public string ScreenShotsPath
        {
            get { return _screenShotsPath; }
            set
            {
                _screenShotsPath = value;
                if (!Directory.Exists(_screenShotsPath))
                {
                    Directory.CreateDirectory(_screenShotsPath);
                }
                RaisePropertyChanged();
            }
        }

        public string SoundsPath
        {
            get { return _soundsPath; }
            set
            {
                _soundsPath = value;
                if (!Directory.Exists(_soundsPath))
                {
                    Directory.CreateDirectory(_soundsPath);
                }
                RaisePropertyChanged();
            }
        }

        public string SettingsPath
        {
            get { return _settingsPath; }
            set
            {
                _settingsPath = value;
                if (!Directory.Exists(_settingsPath))
                {
                    Directory.CreateDirectory(_settingsPath);
                }
                RaisePropertyChanged();
            }
        }

        public string PluginsSettingsPath
        {
            get { return _pluginsSettingsPath; }
            set
            {
                _pluginsSettingsPath = value;
                if (!Directory.Exists(_pluginsSettingsPath))
                {
                    Directory.CreateDirectory(_pluginsSettingsPath);
                }
                RaisePropertyChanged();
            }
        }

        public List<ChatLogEntry> ChatHistory
        {
            get { return _chatHistory ?? (_chatHistory = new List<ChatLogEntry>()); }
            set
            {
                _chatHistory = value;
                RaisePropertyChanged();
            }
        }

        public bool HasNewVersion
        {
            get { return _hasNewVersion; }
            set
            {
                _hasNewVersion = value;
                RaisePropertyChanged();
            }
        }

        public bool HasNewPluginUpdate
        {
            get { return _hasNewPluginUpdate; }
            set
            {
                _hasNewPluginUpdate = value;
                RaisePropertyChanged();
            }
        }

        public string DownloadUri
        {
            get { return _downloadUri; }
            set
            {
                _downloadUri = value;
                RaisePropertyChanged();
            }
        }

        public string LatestVersion
        {
            get { return _latestVersion; }
            set
            {
                _latestVersion = value;
                RaisePropertyChanged();
            }
        }

        public string UpdateNotes
        {
            get { return _updateNotes; }
            set
            {
                _updateNotes = value;
                RaisePropertyChanged();
            }
        }

        public string CurrentVersion
        {
            get { return _currentVersion; }
            set
            {
                _currentVersion = value;
                RaisePropertyChanged();
            }
        }

        public string Copyright
        {
            get
            {
                var att = Assembly.GetExecutingAssembly()
                                  .GetCustomAttributes(typeof (AssemblyCopyrightAttribute), false);
                return att.Length == 0 ? "" : ((AssemblyCopyrightAttribute) att[0]).Copyright;
            }
        }

        public List<Signature> Signatures
        {
            get { return _signatures ?? (_signatures = new List<Signature>()); }
            set
            {
                _signatures = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Loading Functions

        #endregion

        #region Private Functions

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private static void NotifyIconOnRestoreClick(object sender, EventArgs eventArgs)
        {
            ShellView.View.WindowState = WindowState.Normal;
            ShellView.View.Topmost = true;
            ShellView.View.Topmost = Settings.Default.TopMost;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private static void NotifyIconOnExitClick(object sender, EventArgs eventArgs)
        {
            DispatcherHelper.Invoke(() => ShellView.CloseApplication(), DispatcherPriority.Send);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private static void NotifyIconOnMouseDoubleClick(object sender, MouseEventArgs mouseEventArgs)
        {
            ShellView.View.WindowState = WindowState.Normal;
            ShellView.View.Topmost = true;
            ShellView.View.Topmost = Settings.Default.TopMost;
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
