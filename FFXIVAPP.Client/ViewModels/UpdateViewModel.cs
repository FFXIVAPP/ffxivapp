// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateViewModel.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   UpdateViewModel.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.ViewModels {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Cache;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Avalonia.Controls;
    using Avalonia.Threading;
    using FFXIVAPP.Client.Models;
    using FFXIVAPP.Client.Utilities;
    using FFXIVAPP.Common.Helpers;
    using FFXIVAPP.Common.Models;
    using FFXIVAPP.Common.Utilities;
    using FFXIVAPP.Common.ViewModelBase;
    using NLog;

    public class UpdateViewModel : ViewModelBase {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static Lazy<UpdateViewModel> _instance = new Lazy<UpdateViewModel>(() => new UpdateViewModel());

        private ObservableCollection<object> _availablePlugins;

        private int _availablePluginUpdates;

        private ObservableCollection<object> _availableSources;

        public UpdateViewModel() {
            this.RefreshAvailableCommand = new DelegateCommand(RefreshAvailable);
            this.InstallCommand = new DelegateCommand(Install);
            this.UnInstallCommand = new DelegateCommand(UnInstall);
            this.AddOrUpdateSourceCommand = new DelegateCommand(AddOrUpdateSource);
            this.DeleteSourceCommand = new DelegateCommand(DeleteSource);
            this.SourceSelectionCommand = new DelegateCommand(SourceSelection);
            this.AvailableDGDoubleClickCommand = new DelegateCommand(AvailableDGDoubleClick);
        }

        public static UpdateViewModel Instance {
            get {
                return _instance.Value;
            }
        }

        public ICommand AddOrUpdateSourceCommand { get; private set; }

        public ICommand AvailableDGDoubleClickCommand { get; private set; }

        public ObservableCollection<object> AvailablePlugins {
            get {
                return this._availablePlugins ?? (this._availablePlugins = new ObservableCollection<object>());
            }

            set {
                this._availablePlugins = value;
                this.RaisePropertyChanged();
            }
        }

        public bool UpdatingAvailablePlugins { get; set; }
        public bool AvailableLoadingProgressMessageVisible { get; set; }
        public string AvailableLoadingProgressMessageText { get; set; }
        public int AvailablePluginUpdates {
            get {
                return this._availablePluginUpdates;
            }

            set {
                this._availablePluginUpdates = value;
                this.RaisePropertyChanged();
            }
        }

        public ObservableCollection<object> AvailableSources {
            get {
                return this._availableSources ?? (this._availableSources = new ObservableCollection<object>());
            }

            set {
                this._availableSources = value;
                this.RaisePropertyChanged();
            }
        }

        public ICommand DeleteSourceCommand { get; private set; }

        public ICommand InstallCommand { get; private set; }

        public ICommand RefreshAvailableCommand { get; private set; }

        public ICommand SourceSelectionCommand { get; private set; }

        public ICommand UnInstallCommand { get; private set; }

        public object SelectedAvailableDGObject { get; set; }

        public PluginDownloadItem SelectedAvailableDG => SelectedAvailableDGObject == null ? null : (PluginDownloadItem)SelectedAvailableDGObject;

        public ObservableCollection<GridItem> PluginSourceDG { get; set; }
        public GridItem SelectedPluginSourceDG { get; set; }

        /* TODO: WPF Stuff (DataGrid grouping)
        public void SetupGrouping() {
            ICollectionView cvEvents = CollectionViewSource.GetDefaultView(UpdateView.View.AvailableDG.ItemsSource);
            if (cvEvents != null && cvEvents.CanGroup) {
                cvEvents.GroupDescriptions.Clear();
                cvEvents.GroupDescriptions.Add(new PropertyGroupDescription("Status"));
            }
        }
        */

        /// <summary>
        /// </summary>
        private static void AddOrUpdateSource() {
        /* TODO: Direct access to view
            Guid selectedId = Guid.Empty;
            try {
                if (UpdateView.View.PluginSourceDG.SelectedItems.Count == 1) {
                    selectedId = new Guid(GetValueBySelectedItem(UpdateView.View.PluginSourceDG, "Key"));
                }
            }
            catch (Exception ex) {
                Logging.Log(Logger, new LogItem(ex, true));
            }

            if (UpdateView.View.TSource.Text.Trim() == string.Empty) {
                return;
            }

            var pluginSourceItem = new PluginSourceItem {
                Enabled = true,
                SourceURI = UpdateView.View.TSource.Text
            };
            if (selectedId == Guid.Empty) {
                pluginSourceItem.Key = Guid.NewGuid();
                Instance.AvailableSources.Add(pluginSourceItem);
            }
            else {
                pluginSourceItem.Key = selectedId;
                var index = Instance.AvailableSources.TakeWhile(source => source.Key != selectedId).Count();
                Instance.AvailableSources[index] = pluginSourceItem;
            }

            UpdateView.View.PluginSourceDG.UnselectAll();
            UpdateView.View.TSource.Text = string.Empty;
        */
        }
        
        private void AvailableDGDoubleClick() {
            PluginDownloadItem plugin = Instance.AvailablePlugins.Cast<PluginDownloadItem>().FirstOrDefault(p => string.Equals(p.Name, this.SelectedAvailableDG?.Name, Constants.InvariantComparer));
            if (plugin == null) {
                return;
            }

            switch (plugin.Status) {
                case PluginStatus.Installed:
                    UnInstallByKey(plugin.Name);
                    break;
                case PluginStatus.NotInstalled:
                case PluginStatus.UpdateAvailable:
                    InstallByKey(plugin.Name);
                    break;
            }
        }

        /// <summary>
        /// </summary>
        private void DeleteSource() {
            var index = Instance.AvailableSources.Cast<PluginSourceItem>().TakeWhile(source => source.Key.ToString() != this.SelectedPluginSourceDG?.Name).Count();
            Instance.AvailableSources.RemoveAt(index);
        }

        /* TODO: Fix WPF Selector component
        /// <summary>
        /// </summary>
        /// <param name="listView"> </param>
        /// <param name="key"> </param>
        private static string GetValueBySelectedItem(Selector listView, string key) {
            Type type = listView.SelectedItem.GetType();
            PropertyInfo property = type.GetProperty(key);
            return property.GetValue(listView.SelectedItem, null).ToString();
        }
        */

        /// <summary>
        /// </summary>
        private void Install() {
            if (this.SelectedAvailableDG != null) {
                InstallByKey(this.SelectedAvailableDG.Name);
            }
        }

        private static void InstallByKey(string key, Action asyncAction = null) {
            PluginDownloadItem plugin = Instance.AvailablePlugins.Cast<PluginDownloadItem>().FirstOrDefault(p => string.Equals(p.Name, key, Constants.InvariantComparer));
            if (plugin == null) {
                return;
            }

            UpdateViewModel.Instance.AvailableLoadingProgressMessageText = "";
            UpdateViewModel.Instance.AvailableLoadingProgressMessageVisible = true;
            Func<bool> install = delegate {
                var updateCount = 0;
                var updateLimit = plugin.Files.Count;
                var sb = new StringBuilder();
                foreach (PluginFile pluginFile in plugin.Files) {
                    sb.Clear();
                    try {
                        using (var client = new WebClient {
                            CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore)
                        }) {
                            var saveLocation = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Plugins", plugin.Name, pluginFile.Location, pluginFile.Name);
                            Directory.CreateDirectory(Path.GetDirectoryName(saveLocation));

                            if (UpdateUtilities.VerifyFile(saveLocation, pluginFile.Checksum)) {
                                // no need to download file, since it hasn't changed
                                // count this file as "updated" for the purpose of checking if the install finished
                                updateCount++;
                            }
                            else {
                                sb.Append(plugin.SourceURI.Trim('/'));
                                var location = pluginFile.Location.Trim('/');
                                if (!string.IsNullOrWhiteSpace(location)) {
                                    sb.AppendFormat("/{0}", location);
                                }

                                sb.AppendFormat("/{0}", pluginFile.Name.Trim('/'));
                                var uri = new Uri(sb.ToString());
                                client.DownloadFileAsync(uri, saveLocation);
                                client.DownloadProgressChanged += delegate {
                                    DispatcherHelper.Invoke(
                                        delegate {
                                            UpdateViewModel.Instance.AvailableLoadingProgressMessageText = $"{pluginFile.Location.Trim('/')}/{pluginFile.Name}";
                                        });
                                };
                                client.DownloadFileCompleted += delegate {
                                    updateCount++;

                                    if (updateCount >= updateLimit) {
                                        DispatcherHelper.Invoke(
                                            delegate {
                                                if (plugin.Status != PluginStatus.Installed) {
                                                    plugin.Status = PluginStatus.Installed;
                                                    // TODO: Instance.SetupGrouping();
                                                    if (asyncAction != null) {
                                                        DispatcherHelper.Invoke(asyncAction);
                                                    }
                                                }

                                                UpdateViewModel.Instance.AvailableLoadingProgressMessageText = string.Empty;
                                                UpdateViewModel.Instance.AvailableLoadingProgressMessageVisible = false;
                                            },
                                            DispatcherPriority.Send);
                                    }
                                };
                            }
                        }
                    }
                    catch (Exception) {
                        updateCount++;
                    }
                }

                // need to check here aswell, since if all files mathced hash, none will be downloaded, so the DownloadFileCompleted delegate would never be triggered
                if (updateCount >= updateLimit) {
                    if (plugin.Status != PluginStatus.Installed) {
                        plugin.Status = PluginStatus.Installed;
                        // TODO: Instance.SetupGrouping();
                        if (asyncAction != null) {
                            DispatcherHelper.Invoke(asyncAction);
                        }
                    }

                    DispatcherHelper.Invoke(
                        delegate {
                            UpdateViewModel.Instance.AvailableLoadingProgressMessageVisible = false;
                        },
                        DispatcherPriority.Send);
                }

                return true;
            };

            Task.Run(() => install());
        }

        private static void RefreshAvailable() {
            Instance.AvailablePlugins.Clear();
            Initializer.LoadAvailablePlugins();
        }

        /// <summary>
        /// </summary>
        private static void SourceSelection() {
            /* TODO: Direct access to view
            if (UpdateView.View.PluginSourceDG.SelectedItems.Count != 1) {
                return;
            }

            if (UpdateView.View.PluginSourceDG.SelectedIndex < 0) {
                return;
            }

            UpdateView.View.TSource.Text = GetValueBySelectedItem(UpdateView.View.PluginSourceDG, "SourceURI");
            */
        }

        /// <summary>
        /// </summary>
        private static void UnInstall() {
            if (UpdateViewModel.Instance.SelectedAvailableDG != null) {
                UnInstallByKey(UpdateViewModel.Instance.SelectedAvailableDG.Name);
            }
        }

        private static void UnInstallByKey(string key, Action asyncAction = null) {
            PluginDownloadItem plugin = Instance.AvailablePlugins.Cast<PluginDownloadItem>().FirstOrDefault(p => string.Equals(p.Name, key, Constants.InvariantComparer));
            if (plugin == null) {
                return;
            }

            Task.Run(() => {
                try {
                    var saveLocation = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Plugins", plugin.Name);
                    Directory.Delete(saveLocation, true);
                }
                catch (Exception ex) {
                    Logging.Log(Logger, new LogItem(ex, true));
                }

                DispatcherHelper.Invoke(
                    delegate {
                        foreach (PluginDownloadItem pluginDownloadItem in Instance.AvailablePlugins.Cast<PluginDownloadItem>().Where(pluginDownloadItem => string.Equals(pluginDownloadItem.Name, plugin.Name))) {
                            pluginDownloadItem.Status = PluginStatus.NotInstalled;
                        }

                        // TODO: Instance.SetupGrouping();
                        PluginHost.Instance.UnloadPlugin(plugin.Name);
                        for (var i = 0; i < AppViewModel.Instance.PluginTabItems.Count; i++) {
                            if (AppViewModel.Instance.PluginTabItems[i].Name == Regex.Replace(plugin.Name, @"[^A-Za-z]", string.Empty)) {
                                AppViewModel.Instance.PluginTabItems.RemoveAt(i);
                            }
                        }
                    },
                    DispatcherPriority.Send);
            });
        }
    }
}