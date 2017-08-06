// FFXIVAPP.Client ~ UpdateViewModel.cs
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using FFXIVAPP.Client.Models;
using FFXIVAPP.Client.Utilities;
using FFXIVAPP.Client.Views;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Common.ViewModelBase;
using NLog;

namespace FFXIVAPP.Client.ViewModels
{
    [Export(typeof(UpdateViewModel))]
    internal sealed class UpdateViewModel : INotifyPropertyChanged
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        public UpdateViewModel()
        {
            RefreshAvailableCommand = new DelegateCommand(RefreshAvailable);
            InstallCommand = new DelegateCommand(Install);
            UnInstallCommand = new DelegateCommand(UnInstall);
            AddOrUpdateSourceCommand = new DelegateCommand(AddOrUpdateSource);
            DeleteSourceCommand = new DelegateCommand(DeleteSource);
            SourceSelectionCommand = new DelegateCommand(SourceSelection);
            AvailableDGDoubleClickCommand = new DelegateCommand(AvailableDGDoubleClick);
        }

        #region Property Bindings

        private static Lazy<UpdateViewModel> _instance = new Lazy<UpdateViewModel>(() => new UpdateViewModel());
        private ObservableCollection<PluginDownloadItem> _availablePlugins;
        private int _availablePluginUpdates;
        private ObservableCollection<PluginSourceItem> _availableSources;

        public static UpdateViewModel Instance
        {
            get { return _instance.Value; }
        }

        public ObservableCollection<PluginDownloadItem> AvailablePlugins
        {
            get { return _availablePlugins ?? (_availablePlugins = new ObservableCollection<PluginDownloadItem>()); }
            set
            {
                _availablePlugins = value;
                RaisePropertyChanged();
            }
        }

        public int AvailablePluginUpdates
        {
            get { return _availablePluginUpdates; }
            set
            {
                _availablePluginUpdates = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<PluginSourceItem> AvailableSources
        {
            get { return _availableSources ?? (_availableSources = new ObservableCollection<PluginSourceItem>()); }
            set
            {
                _availableSources = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Declarations

        public ICommand RefreshAvailableCommand { get; private set; }
        public ICommand InstallCommand { get; private set; }
        public ICommand UnInstallCommand { get; private set; }
        public ICommand AddOrUpdateSourceCommand { get; private set; }
        public ICommand DeleteSourceCommand { get; private set; }
        public ICommand SourceSelectionCommand { get; private set; }
        public ICommand AvailableDGDoubleClickCommand { get; private set; }

        #endregion

        #region Loading Functions

        #endregion

        #region Utility Functions

        public void SetupGrouping()
        {
            var cvEvents = CollectionViewSource.GetDefaultView(UpdateView.View.AvailableDG.ItemsSource);
            if (cvEvents != null && cvEvents.CanGroup)
            {
                cvEvents.GroupDescriptions.Clear();
                cvEvents.GroupDescriptions.Add(new PropertyGroupDescription("Status"));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="listView"> </param>
        /// <param name="key"> </param>
        private static string GetValueBySelectedItem(Selector listView, string key)
        {
            var type = listView.SelectedItem.GetType();
            var property = type.GetProperty(key);
            return property.GetValue(listView.SelectedItem, null)
                           .ToString();
        }

        #endregion

        #region Command Bindings

        private static void RefreshAvailable()
        {
            Instance.AvailablePlugins.Clear();
            Initializer.LoadAvailablePlugins();
        }

        /// <summary>
        /// </summary>
        private static void Install()
        {
            foreach (var selectedItem in UpdateView.View.AvailableDG.SelectedItems)
            {
                InstallByKey(((PluginDownloadItem) selectedItem).Name);
            }
        }

        private static void InstallByKey(string key, Action asyncAction = null)
        {
            var plugin = Instance.AvailablePlugins.FirstOrDefault(p => String.Equals(p.Name, key, Constants.InvariantComparer));
            if (plugin == null)
            {
                return;
            }
            UpdateView.View.AvailableLoadingInformation.Visibility = Visibility.Visible;
            UpdateView.View.AvailableLoadingProgressMessage.Visibility = Visibility.Visible;
            Func<bool> install = delegate
            {
                var updateCount = 0;
                var updateLimit = plugin.Files.Count;
                var sb = new StringBuilder();
                foreach (var pluginFile in plugin.Files)
                {
                    sb.Clear();
                    try
                    {
                        using (var client = new WebClient
                        {
                            CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore)
                        })
                        {
                            var saveLocation = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly()
                                                                                          .Location), "Plugins", plugin.Name, pluginFile.Location, pluginFile.Name);
                            Directory.CreateDirectory(Path.GetDirectoryName(saveLocation));


                            if (UpdateUtilities.VerifyFile(saveLocation, pluginFile.Checksum))
                            {
                                // no need to download file, since it hasn't changed
                                // count this file as "updated" for the purpose of checking if the install finished
                                updateCount++;
                            }
                            else
                            {
                                sb.Append(plugin.SourceURI.Trim('/'));
                                var location = pluginFile.Location.Trim('/');
                                if (!String.IsNullOrWhiteSpace(location))
                                {
                                    sb.AppendFormat("/{0}", location);
                                }
                                sb.AppendFormat("/{0}", pluginFile.Name.Trim('/'));
                                var uri = new Uri(sb.ToString());
                                client.DownloadFileAsync(uri, saveLocation);
                                client.DownloadProgressChanged += delegate { DispatcherHelper.Invoke(delegate { UpdateView.View.AvailableLoadingProgressMessage.Text = $"{pluginFile.Location.Trim('/')}/{pluginFile.Name}"; }); };
                                client.DownloadFileCompleted += delegate
                                {
                                    updateCount++;

                                    if (updateCount >= updateLimit)
                                    {
                                        DispatcherHelper.Invoke(delegate
                                        {
                                            if (plugin.Status != PluginStatus.Installed)
                                            {
                                                plugin.Status = PluginStatus.Installed;
                                                Instance.SetupGrouping();
                                                if (asyncAction != null)
                                                {
                                                    DispatcherHelper.Invoke(asyncAction);
                                                }
                                            }
                                            UpdateView.View.AvailableLoadingProgressMessage.Text = string.Empty;
                                            UpdateView.View.AvailableLoadingInformation.Visibility = Visibility.Collapsed;
                                            UpdateView.View.AvailableLoadingProgressMessage.Visibility = Visibility.Collapsed;
                                        }, DispatcherPriority.Send);
                                    }
                                };
                            }
                        }
                    }
                    catch (Exception)
                    {
                        updateCount++;
                    }
                }

                // need to check here aswell, since if all files mathced hash, none will be downloaded, so the DownloadFileCompleted delegate would never be triggered
                if (updateCount >= updateLimit)
                {
                    if (plugin.Status != PluginStatus.Installed)
                    {
                        plugin.Status = PluginStatus.Installed;
                        Instance.SetupGrouping();
                        if (asyncAction != null)
                        {
                            DispatcherHelper.Invoke(asyncAction);
                        }
                    }
                    DispatcherHelper.Invoke(delegate
                    {
                        UpdateView.View.AvailableLoadingInformation.Visibility = Visibility.Collapsed;
                        UpdateView.View.AvailableLoadingProgressMessage.Visibility = Visibility.Collapsed;
                    }, DispatcherPriority.Send);
                }

                return true;
            };
            install.BeginInvoke(delegate { }, install);
        }

        /// <summary>
        /// </summary>
        private static void UnInstall()
        {
            foreach (var selectedItem in UpdateView.View.AvailableDG.SelectedItems)
            {
                UnInstallByKey(((PluginDownloadItem) selectedItem).Name);
            }
        }

        private static void UnInstallByKey(string key, Action asyncAction = null)
        {
            var plugin = Instance.AvailablePlugins.FirstOrDefault(p => String.Equals(p.Name, key, Constants.InvariantComparer));
            if (plugin == null)
            {
                return;
            }
            Func<bool> uninstall = delegate
            {
                try
                {
                    var saveLocation = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly()
                                                                                  .Location), "Plugins", plugin.Name);
                    Directory.Delete(saveLocation, true);
                }
                catch (Exception ex)
                {
                    Logging.Log(Logger, new LogItem(ex, true));
                }
                return true;
            };
            uninstall.BeginInvoke(delegate
            {
                DispatcherHelper.Invoke(delegate
                {
                    foreach (var pluginDownloadItem in Instance.AvailablePlugins.Where(pluginDownloadItem => String.Equals(pluginDownloadItem.Name, plugin.Name)))
                    {
                        pluginDownloadItem.Status = PluginStatus.NotInstalled;
                    }
                    Instance.SetupGrouping();
                    PluginHost.Instance.UnloadPlugin(plugin.Name);
                    for (var i = ShellView.View.PluginsTC.Items.Count - 1; i > 0; i--)
                    {
                        if (((TabItem) ShellView.View.PluginsTC.Items[i]).Name == Regex.Replace(plugin.Name, @"[^A-Za-z]", string.Empty))
                        {
                            AppViewModel.Instance.PluginTabItems.RemoveAt(i);
                        }
                    }
                }, DispatcherPriority.Send);
            }, uninstall);
        }

        /// <summary>
        /// </summary>
        private static void AddOrUpdateSource()
        {
            var selectedId = Guid.Empty;
            try
            {
                if (UpdateView.View.PluginSourceDG.SelectedItems.Count == 1)
                {
                    selectedId = new Guid(GetValueBySelectedItem(UpdateView.View.PluginSourceDG, "Key"));
                }
            }
            catch (Exception ex)
            {
                Logging.Log(Logger, new LogItem(ex, true));
            }
            if (UpdateView.View.TSource.Text.Trim() == string.Empty)
            {
                return;
            }
            var pluginSourceItem = new PluginSourceItem
            {
                Enabled = true,
                SourceURI = UpdateView.View.TSource.Text
            };
            if (selectedId == Guid.Empty)
            {
                pluginSourceItem.Key = Guid.NewGuid();
                Instance.AvailableSources.Add(pluginSourceItem);
            }
            else
            {
                pluginSourceItem.Key = selectedId;
                var index = Instance.AvailableSources.TakeWhile(source => source.Key != selectedId)
                                    .Count();
                Instance.AvailableSources[index] = pluginSourceItem;
            }
            UpdateView.View.PluginSourceDG.UnselectAll();
            UpdateView.View.TSource.Text = string.Empty;
        }

        /// <summary>
        /// </summary>
        private static void DeleteSource()
        {
            string key;
            try
            {
                key = GetValueBySelectedItem(UpdateView.View.PluginSourceDG, "Key");
            }
            catch (Exception ex)
            {
                Logging.Log(Logger, new LogItem(ex, true));
                return;
            }
            var index = Instance.AvailableSources.TakeWhile(source => source.Key.ToString() != key)
                                .Count();
            Instance.AvailableSources.RemoveAt(index);
        }

        /// <summary>
        /// </summary>
        private static void SourceSelection()
        {
            if (UpdateView.View.PluginSourceDG.SelectedItems.Count != 1)
            {
                return;
            }
            if (UpdateView.View.PluginSourceDG.SelectedIndex < 0)
            {
                return;
            }
            UpdateView.View.TSource.Text = GetValueBySelectedItem(UpdateView.View.PluginSourceDG, "SourceURI");
        }

        private static void AvailableDGDoubleClick()
        {
            string key;
            try
            {
                key = GetValueBySelectedItem(UpdateView.View.AvailableDG, "Name");
            }
            catch (Exception ex)
            {
                Logging.Log(Logger, new LogItem(ex, true));
                return;
            }
            var plugin = Instance.AvailablePlugins.FirstOrDefault(p => String.Equals(p.Name, key, Constants.InvariantComparer));
            if (plugin == null)
            {
                return;
            }
            switch (plugin.Status)
            {
                case PluginStatus.Installed:
                    UnInstallByKey(plugin.Name);
                    break;
                case PluginStatus.NotInstalled:
                case PluginStatus.UpdateAvailable:
                    InstallByKey(plugin.Name);
                    break;
            }
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
