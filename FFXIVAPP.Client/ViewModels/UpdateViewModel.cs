// FFXIVAPP.Client
// UpdateViewModel.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
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
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Common.ViewModelBase;
using NLog;

namespace FFXIVAPP.Client.ViewModels
{
    [Export(typeof (UpdateViewModel))]
    internal sealed class UpdateViewModel : INotifyPropertyChanged
    {
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
        public ICommand AvailableDGDoubleClickCommand { get; private set; }

        #endregion

        #region Loading Functions

        #endregion

        #region Utility Functions

        public void SetupGrouping()
        {
            var cvEvents = CollectionViewSource.GetDefaultView(UpdateView.View.AvailableDG.ItemsSource);
            if (cvEvents != null && cvEvents.CanGroup == true)
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
                                client.DownloadProgressChanged += delegate { DispatcherHelper.Invoke(delegate { UpdateView.View.AvailableLoadingProgressMessage.Text = String.Format("{0}/{1}", pluginFile.Location.Trim('/'), pluginFile.Name); }); };
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
                                            UpdateView.View.AvailableLoadingProgressMessage.Text = "";
                                            UpdateView.View.AvailableLoadingInformation.Visibility = Visibility.Collapsed;
                                            UpdateView.View.AvailableLoadingProgressMessage.Visibility = Visibility.Collapsed;
                                        }, DispatcherPriority.Send);
                                    }
                                };
                            }
                        }
                    }
                    catch (Exception ex)
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
            checkAvailable.BeginInvoke(null, null);
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
                    Instance.SetupGrouping();
                    PluginHost.Instance.UnloadPlugin(plugin.Name);
                    for (var i = ShellView.View.PluginsTC.Items.Count - 1; i > 0; i--)
                    {
                        if (((TabItem) ShellView.View.PluginsTC.Items[i]).Name == Regex.Replace(plugin.Name, @"[^A-Za-z]", ""))
                        {
                            AppViewModel.Instance.PluginTabItems.RemoveAt(i);
                        }
                    }
                }, DispatcherPriority.Send);
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
            string key;
            try
            {
                key = GetValueBySelectedItem(UpdateView.View.PluginSourceDG, "Key");
            }
            catch (Exception ex)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
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
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
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
