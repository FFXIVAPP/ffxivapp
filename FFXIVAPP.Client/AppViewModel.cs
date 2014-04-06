// FFXIVAPP.Client
// AppViewModel.cs
// 
// Copyright © 2007 - 2014 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Threading;
using FFXIVAPP.Client.Memory;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Helpers;
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
        private List<string> _updateNotes;

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
            get
            {
                if (!Directory.Exists(_configurationsPath))
                {
                    Directory.CreateDirectory(_configurationsPath);
                }
                return _configurationsPath;
            }
            set
            {
                _configurationsPath = value;
                RaisePropertyChanged();
            }
        }

        public string LogsPath
        {
            get
            {
                if (!Directory.Exists(_logsPath))
                {
                    Directory.CreateDirectory(_logsPath);
                }
                return _logsPath;
            }
            set
            {
                _logsPath = value;
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
            get
            {
                if (!Directory.Exists(_screenShotsPath))
                {
                    Directory.CreateDirectory(_screenShotsPath);
                }
                return _screenShotsPath;
            }
            set
            {
                _screenShotsPath = value;
                RaisePropertyChanged();
            }
        }

        public string SoundsPath
        {
            get
            {
                if (!Directory.Exists(_soundsPath))
                {
                    Directory.CreateDirectory(_soundsPath);
                }
                return _soundsPath;
            }
            set
            {
                _soundsPath = value;
                RaisePropertyChanged();
            }
        }

        public string SettingsPath
        {
            get
            {
                if (!Directory.Exists(_settingsPath))
                {
                    Directory.CreateDirectory(_settingsPath);
                }
                return _settingsPath;
            }
            set
            {
                _settingsPath = value;
                RaisePropertyChanged();
            }
        }

        public string PluginsSettingsPath
        {
            get
            {
                if (!Directory.Exists(_pluginsSettingsPath))
                {
                    Directory.CreateDirectory(_pluginsSettingsPath);
                }
                return _pluginsSettingsPath;
            }
            set
            {
                _pluginsSettingsPath = value;
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

        public List<string> UpdateNotes
        {
            get { return _updateNotes ?? (_updateNotes = new List<string>()); }
            set
            {
                if (_updateNotes == null)
                {
                    _updateNotes = new List<string>();
                }
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
            get { return AssemblyHelper.Copyright; }
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
