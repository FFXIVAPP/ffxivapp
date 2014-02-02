// FFXIVAPP.Client
// UpdateViewModel.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
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
using System.Windows.Input;
using System.Windows.Threading;
using FFXIVAPP.Client.Models;
using FFXIVAPP.Client.Views;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Common.ViewModelBase;
using NLog;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.ViewModels
{
    [DoNotObfuscate]
    [Export(typeof (UpdateViewModel))]
    internal sealed class UpdateViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static UpdateViewModel _instance;
        private ObservableCollection<PluginDownloadItem> _availablePlugins;
        private ObservableCollection<PluginSourceItem> _availableSources;

        public static UpdateViewModel Instance
        {
            get { return _instance ?? (_instance = new UpdateViewModel()); }
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

        #endregion

        public UpdateViewModel()
        {
            RefreshAvailableCommand = new DelegateCommand(RefreshAvailable);
            InstallCommand = new DelegateCommand(Install);
            UnInstallCommand = new DelegateCommand(UnInstall);
            AddOrUpdateSourceCommand = new DelegateCommand(AddOrUpdateSource);
            DeleteSourceCommand = new DelegateCommand(DeleteSource);
            SourceSelectionCommand = new DelegateCommand(SourceSelection);
        }

        #region Loading Functions

        #endregion

        #region Utility Functions

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
                var index = UpdateView.View.AvailableDG.Items.IndexOf(selectedItem)
                                      .ToString(CultureInfo.InvariantCulture);
                InstallByIndex(Convert.ToInt32(index));
            }
        }

        private static void InstallByIndex(int index)
        {
            var plugin = Instance.AvailablePlugins[index];
            UpdateView.View.AvailableLoadingInformation.Visibility = Visibility.Visible;
            UpdateView.View.AvailableLoadingProgressMessage.Visibility = Visibility.Visible;
            Func<bool> checkAvailable = delegate
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
                            sb.Append(plugin.SourceURI.Trim('/'));
                            sb.AppendFormat("/{0}", pluginFile.Location.Trim('/'));
                            sb.AppendFormat("/{0}", pluginFile.Name.Trim('/'));
                            client.DownloadFileAsync(new Uri(sb.ToString()), saveLocation);
                            client.DownloadProgressChanged += delegate { DispatcherHelper.Invoke(delegate { UpdateView.View.AvailableLoadingProgressMessage.Text = String.Format("{0}/{1}", pluginFile.Location.Trim('/'), pluginFile.Name); }); };
                            client.DownloadFileCompleted += delegate
                            {
                                updateCount++;
                                DispatcherHelper.Invoke(delegate
                                {
                                    UpdateView.View.AvailableLoadingProgressMessage.Text = "";
                                    if (updateCount >= updateLimit)
                                    {
                                        plugin.Status = PluginStatus.Installed;
                                        UpdateView.View.AvailableLoadingInformation.Visibility = Visibility.Collapsed;
                                        UpdateView.View.AvailableLoadingProgressMessage.Visibility = Visibility.Collapsed;
                                    }
                                }, DispatcherPriority.Send);
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        updateCount++;
                    }
                }
                return true;
            };
            checkAvailable.BeginInvoke(null, null);
        }

        /// <summary>
        /// </summary>
        private static void UnInstall()
        {
            foreach (var selectedItem in UpdateView.View.AvailableDG.SelectedItems)
            {
                var index = UpdateView.View.AvailableDG.Items.IndexOf(selectedItem)
                                      .ToString(CultureInfo.InvariantCulture);
                UnInstallByIndex(Convert.ToInt32(index));
            }
        }

        private static void UnInstallByIndex(int index)
        {
            var plugin = Instance.AvailablePlugins[index];
            Func<bool> checkAvailable = delegate
            {
                try
                {
                    var saveLocation = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly()
                                                                                  .Location), "Plugins", plugin.Name);
                    Directory.Delete(saveLocation, true);
                }
                catch (Exception ex)
                {
                }
                return true;
            };
            checkAvailable.BeginInvoke(delegate
            {
                DispatcherHelper.Invoke(delegate
                {
                    foreach (var pluginDownloadItem in Instance.AvailablePlugins.Where(pluginDownloadItem => String.Equals(pluginDownloadItem.Name, plugin.Name)))
                    {
                        pluginDownloadItem.Status = PluginStatus.NotInstalled;
                    }
                    var found = App.Plugins.Loaded.Find(plugin.Name);
                    if (found == null)
                    {
                        return;
                    }
                    found.Instance.Dispose();
                    App.Plugins.Loaded.Remove(found);
                    for (var i = ShellView.View.PluginsTC.Items.Count - 1; i > 0; i--)
                    {
                        if (((TabItem) ShellView.View.PluginsTC.Items[i]).Name == Regex.Replace(found.Instance.Name, @"[^A-Za-z]", ""))
                        {
                            ShellView.View.PluginsTC.Items.RemoveAt(i);
                        }
                    }
                });
            }, checkAvailable);
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
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
            }
            if (UpdateView.View.TSource.Text.Trim() == "")
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
            UpdateView.View.TSource.Text = "";
        }

        /// <summary>
        /// </summary>
        private static void DeleteSource()
        {
            string selectedKey;
            try
            {
                selectedKey = GetValueBySelectedItem(UpdateView.View.PluginSourceDG, "Key");
            }
            catch (Exception ex)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
                return;
            }
            var index = Instance.AvailableSources.TakeWhile(source => source.Key.ToString() != selectedKey)
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
