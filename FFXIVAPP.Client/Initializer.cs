// FFXIVAPP.Client ~ Initializer.cs
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml.Linq;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Memory;
using FFXIVAPP.Client.Models;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Client.Utilities;
using FFXIVAPP.Client.ViewModels;
using FFXIVAPP.Client.Views;
using FFXIVAPP.Common.Core.Constant;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.RegularExpressions;
using FFXIVAPP.Common.Utilities;
using Machina;
using Machina.Events;
using Machina.Models;
using Newtonsoft.Json.Linq;
using NLog;
using Sharlayan;
using Sharlayan.Events;
using Sharlayan.Models;
using Application = System.Windows.Forms.Application;
using ExceptionEvent = Sharlayan.Events.ExceptionEvent;
using NetworkPacket = FFXIVAPP.Common.Core.Network.NetworkPacket;

namespace FFXIVAPP.Client
{
    internal static class Initializer
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        public static bool NetworkWorking
        {
            get { return NetworkHandler.Instance.IsRunning; }
        }

        /// <summary>
        /// </summary>
        public static void SetupCurrentUICulture()
        {
            var cultureInfo = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var currentCulture = new CultureInfo(cultureInfo);
            Constants.CultureInfo = Settings.Default.CultureSet ? Settings.Default.Culture : currentCulture;
            Settings.Default.CultureSet = true;
        }

        /// <summary>
        /// </summary>
        public static void LoadChatCodes()
        {
            if (Constants.XChatCodes != null)
            {
                foreach (var xElement in Constants.XChatCodes.Descendants()
                                                  .Elements("Code"))
                {
                    var xKey = (string) xElement.Attribute("Key");
                    var xDescription = (string) xElement.Element("Description");
                    if (String.IsNullOrWhiteSpace(xKey) || String.IsNullOrWhiteSpace(xDescription))
                    {
                        continue;
                    }
                    Constants.ChatCodes.Add(xKey, xDescription);
                }
            }
        }

        /// <summary>
        /// </summary>
        public static void LoadAutoTranslate()
        {
            if (Constants.XAutoTranslate != null)
            {
                foreach (var xElement in Constants.XAutoTranslate.Descendants()
                                                  .Elements("Code"))
                {
                    var xKey = (string) xElement.Attribute("Key");
                    var xValue = (string) xElement.Element(Settings.Default.GameLanguage);
                    if (String.IsNullOrWhiteSpace(xKey) || String.IsNullOrWhiteSpace(xValue))
                    {
                        continue;
                    }
                    Constants.AutoTranslate.Add(xKey, xValue);
                }
            }
        }

        /// <summary>
        /// </summary>
        public static void LoadColors()
        {
            if (Constants.XColors != null)
            {
                foreach (var xElement in Constants.XColors.Descendants()
                                                  .Elements("Color"))
                {
                    var xKey = (string) xElement.Attribute("Key");
                    var xValue = (string) xElement.Element("Value");
                    var xDescription = (string) xElement.Element("Description");
                    if (String.IsNullOrWhiteSpace(xKey) || String.IsNullOrWhiteSpace(xValue))
                    {
                        continue;
                    }
                    if (Constants.ChatCodes.ContainsKey(xKey))
                    {
                        if (xDescription.ToLower()
                                        .Contains("unknown") || String.IsNullOrWhiteSpace(xDescription))
                        {
                            xDescription = Constants.ChatCodes[xKey];
                        }
                    }
                    Constants.Colors.Add(xKey, new[]
                    {
                        xValue, xDescription
                    });
                }
                foreach (var chatCode in Constants.ChatCodes.Where(chatCode => !Constants.Colors.ContainsKey(chatCode.Key)))
                {
                    Constants.Colors.Add(chatCode.Key, new[]
                    {
                        "FFFFFF", chatCode.Value
                    });
                }
            }
        }

        /// <summary>
        /// </summary>
        public static void LoadApplicationSettings()
        {
            if (Constants.XSettings != null)
            {
            }
        }

        /// <summary>
        /// </summary>
        public static void LoadAvailableAudioDevices()
        {
            foreach (var device in App.AvailableAudioDevices)
            {
                SettingsViewModel.Instance.AvailableAudioDevicesList.Add(device.Description);
            }
        }

        public static void LoadAvailableNetworkDevices()
        {
            foreach (var networkInterface in App.AvailableNetworkInterfaces)
            {
                SettingsViewModel.Instance.AvailableNetworkInterfacesList.Add(networkInterface.Name);
            }
        }

        /// <summary>
        /// </summary>
        public static void LoadSoundsIntoCache()
        {
            SoundPlayerHelper.CacheSoundFiles();
        }

        /// <summary>
        /// </summary>
        public static void LoadPlugins()
        {
            var pluginsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");
            App.Plugins.LoadPlugins(pluginsDirectory);
            foreach (PluginInstance pluginInstance in App.Plugins.Loaded)
            {
                TabItemHelper.LoadPluginTabItem(pluginInstance);
            }
            AppViewModel.Instance.HasPlugins = App.Plugins.Loaded.Count > 0;
        }

        public static void LoadAvailableSources()
        {
            if (Constants.XSettings != null)
            {
                foreach (var xElement in Constants.XSettings.Descendants()
                                                  .Elements("PluginSource"))
                {
                    var xKey = Guid.Empty;
                    var xSourceURI = (string) xElement.Element("SourceURI");
                    var xEnabled = true;
                    try
                    {
                        xEnabled = (bool) xElement.Element("Enabled");
                    }
                    catch (Exception ex)
                    {
                        Logging.Log(Logger, new LogItem(ex, true));
                    }
                    try
                    {
                        xKey = (Guid) xElement.Attribute("Key");
                    }
                    catch (Exception ex)
                    {
                        Logging.Log(Logger, new LogItem(ex, true));
                    }
                    if (String.IsNullOrWhiteSpace(xSourceURI))
                    {
                        continue;
                    }
                    xKey = xKey != Guid.Empty ? xKey : Guid.NewGuid();
                    var pluginSourceItem = new PluginSourceItem
                    {
                        Key = xKey,
                        SourceURI = xSourceURI,
                        Enabled = xEnabled
                    };
                    var found = UpdateViewModel.Instance.AvailableSources.Any(source => source.Key == pluginSourceItem.Key);
                    if (!found)
                    {
                        UpdateViewModel.Instance.AvailableSources.Add(pluginSourceItem);
                    }
                }
            }
        }

        public static void LoadAvailablePlugins()
        {
            UpdateView.View.AvailableLoadingInformation.Visibility = Visibility.Visible;
            UpdateViewModel.Instance.AvailablePlugins.Clear();

            UpdateView.View.PluginUpdateSpinner.Spin = true;
            ShellView.View.PluginUpdateSpinner.Spin = true;

            Func<bool> update = delegate
            {
                var pluginSourceList = new List<PluginSourceItem>();
                try
                {
                    var httpWebRequest = (HttpWebRequest) WebRequest.Create("https://github.com/Icehunter/ffxivapp/raw/master/PACKAGES.json");
                    httpWebRequest.UserAgent = "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_3; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.70 Safari/533.4";
                    httpWebRequest.Headers.Add("Accept-Language", "en;q=0.8");
                    httpWebRequest.ContentType = "application/text; charset=utf-8";
                    httpWebRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                    using (var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse())
                    {
                        using (var response = httpResponse.GetResponseStream())
                        {
                            var responseText = string.Empty;
                            if (response != null)
                            {
                                using (var streamReader = new StreamReader(response))
                                {
                                    responseText = streamReader.ReadToEnd();
                                }
                            }
                            if (httpResponse.StatusCode == HttpStatusCode.OK || !String.IsNullOrWhiteSpace(responseText))
                            {
                                var jsonResult = JArray.Parse(responseText);
                                foreach (var jToken in jsonResult)
                                {
                                    bool enabled;
                                    bool.TryParse(jToken["Enabled"]
                                        .ToString(), out enabled);
                                    var sourceURI = jToken["SourceURI"]
                                        .ToString();

                                    if (enabled)
                                    {
                                        pluginSourceList.Add(new PluginSourceItem
                                        {
                                            Enabled = true,
                                            Key = Guid.NewGuid(),
                                            SourceURI = sourceURI
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logging.Log(Logger, new LogItem(ex, true));
                }
                foreach (var pluginSourceItem in UpdateViewModel.Instance.AvailableSources)
                {
                    if (pluginSourceList.Any(p => String.Equals(p.SourceURI, pluginSourceItem.SourceURI, Constants.InvariantComparer)))
                    {
                        continue;
                    }
                    pluginSourceList.Add(pluginSourceItem);
                }
                foreach (var item in pluginSourceList)
                {
                    if (item.Enabled)
                    {
                        try
                        {
                            var httpWebRequest = (HttpWebRequest) WebRequest.Create(item.SourceURI);
                            httpWebRequest.UserAgent = "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_3; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.70 Safari/533.4";
                            httpWebRequest.Headers.Add("Accept-Language", "en;q=0.8");
                            httpWebRequest.ContentType = "application/text; charset=utf-8";
                            httpWebRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                            using (var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse())
                            {
                                using (var response = httpResponse.GetResponseStream())
                                {
                                    var responseText = string.Empty;
                                    if (response != null)
                                    {
                                        using (var streamReader = new StreamReader(response))
                                        {
                                            responseText = streamReader.ReadToEnd();
                                        }
                                    }
                                    if (httpResponse.StatusCode == HttpStatusCode.OK || !String.IsNullOrWhiteSpace(responseText))
                                    {
                                        var jsonResult = JObject.Parse(responseText);
                                        var pluginInfo = jsonResult["PluginInfo"];
                                        var pluginFiles = pluginInfo["Files"]
                                            .ToArray();
                                        var pluginDownload = new PluginDownloadItem
                                        {
                                            Files = new List<PluginFile>(pluginFiles.Select(pluginFile => new PluginFile
                                            {
                                                Location = pluginFile["Location"]
                                                    .ToString(),
                                                Name = pluginFile["Name"]
                                                    .ToString(),
                                                Checksum = pluginFile["Checksum"] == null ? string.Empty : pluginFile["Checksum"]
                                                    .ToString()
                                            })),
                                            Name = pluginInfo["Name"]
                                                .ToString(),
                                            FriendlyName = pluginInfo["FriendlyName"] == null ? pluginInfo["Name"]
                                                .ToString() : pluginInfo["FriendlyName"]
                                                .ToString(),
                                            Description = pluginInfo["Description"] == null ? string.Empty : pluginInfo["Description"]
                                                .ToString(),
                                            SourceURI = pluginInfo["SourceURI"]
                                                .ToString(),
                                            LatestVersion = pluginInfo["Version"]
                                                .ToString()
                                        };
                                        var found = App.Plugins.Loaded.Find(pluginDownload.Name);
                                        if (found != null)
                                        {
                                            var latest = pluginDownload.LatestVersion;
                                            var current = found.Instance.Version;
                                            pluginDownload.CurrentVersion = current;
                                            pluginDownload.Status = PluginStatus.Installed;
                                            var latestBuild = new BuildNumber();
                                            var currentBuild = new BuildNumber();
                                            if (BuildUtilities.NeedsUpdate(latest, current, ref latestBuild, ref currentBuild))
                                            {
                                                pluginDownload.Status = PluginStatus.UpdateAvailable;
                                                AppViewModel.Instance.HasNewPluginUpdate = true;

                                                DispatcherHelper.Invoke(() => UpdateViewModel.Instance.AvailablePluginUpdates++);
                                            }
                                            else
                                            {
                                                if (!found.Loaded)
                                                {
                                                    pluginDownload.Status = PluginStatus.OutOfDate;
                                                }
                                            }
                                        }
                                        DispatcherHelper.Invoke(() => UpdateViewModel.Instance.AvailablePlugins.Add(pluginDownload));
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logging.Log(Logger, new LogItem(ex, true));
                        }
                    }
                }

                DispatcherHelper.Invoke(delegate
                {
                    if (UpdateView.View.AvailableDG.Items.Count == UpdateViewModel.Instance.AvailablePlugins.Count)
                    {
                        UpdateView.View.AvailableLoadingInformation.Visibility = Visibility.Collapsed;
                    }
                    UpdateView.View.AvailableDG.Items.Refresh();

                    UpdateViewModel.Instance.SetupGrouping();

                    UpdateView.View.PluginUpdateSpinner.Spin = false;
                    ShellView.View.PluginUpdateSpinner.Spin = false;
                });

                return true;
            };
            update.BeginInvoke(delegate { }, update);
        }

        /// <summary>
        /// </summary>
        public static void SetGlobals()
        {
            Constants.CharacterName = Settings.Default.CharacterName;
            Constants.ServerName = Settings.Default.ServerName;
            Constants.GameLanguage = Settings.Default.GameLanguage;
        }

        /// <summary>
        /// </summary>
        public static void GetHomePlugin()
        {
            var homePlugin = Settings.Default.HomePlugin;
            switch (homePlugin)
            {
                case "None": break;
                default:
                    try
                    {
                        var index = SettingsViewModel.Instance.HomePluginList.IndexOf(homePlugin);
                        if (index > 0)
                        {
                            SetHomePlugin(--index);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.Log(Logger, new LogItem(ex, true));
                    }
                    break;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="pluginIndex"></param>
        private static void SetHomePlugin(int pluginIndex)
        {
            ShellView.View.ShellViewTC.SelectedIndex = 1;
            ShellView.View.PluginsTC.SelectedIndex = pluginIndex;
        }

        /// <summary>
        /// </summary>
        public static void CheckUpdates()
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
                else
                {
                    if (File.Exists("FFXIVAPP.Updater.Backup.exe"))
                    {
                        File.Move("FFXIVAPP.Updater.Backup.exe", "FFXIVAPP.Updater.exe");
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Log(Logger, new LogItem(ex, true));
            }

            Func<bool> update = delegate
            {
                var current = Assembly.GetExecutingAssembly()
                                      .GetName()
                                      .Version.ToString();
                AppViewModel.Instance.CurrentVersion = current;
                var httpWebRequest = (HttpWebRequest) WebRequest.Create("https://api.github.com/repos/Icehunter/ffxivapp/releases");
                httpWebRequest.UserAgent = "Icehunter-FFXIVAPP";
                httpWebRequest.Headers.Add("Accept-Language", "en;q=0.8");
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                using (var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse())
                {
                    using (var response = httpResponse.GetResponseStream())
                    {
                        var responseText = string.Empty;
                        if (response != null)
                        {
                            using (var streamReader = new StreamReader(response))
                            {
                                responseText = streamReader.ReadToEnd();
                            }
                        }
                        var latestBuild = new BuildNumber();
                        var currentBuild = new BuildNumber();
                        if (httpResponse.StatusCode != HttpStatusCode.OK || String.IsNullOrWhiteSpace(responseText))
                        {
                            AppViewModel.Instance.HasNewVersion = false;
                            AppViewModel.Instance.LatestVersion = "Unknown";
                        }
                        else
                        {
                            var releases = JArray.Parse(responseText);
                            var release = releases.FirstOrDefault(r => r?["target_commitish"]
                                                                           .ToString() == "master");
                            var latest = release?["name"]
                                             .ToString() ?? "Unknown";
                            latest = latest.Split(' ')[0];
                            AppViewModel.Instance.LatestVersion = latest;
                            switch (latest)
                            {
                                case "Unknown":
                                    AppViewModel.Instance.HasNewVersion = false;
                                    break;
                                default:
                                    AppViewModel.Instance.DownloadUri = String.Format("https://github.com/Icehunter/ffxivapp/releases/download/{0}/{0}.zip", latest);
                                    AppViewModel.Instance.HasNewVersion = BuildUtilities.NeedsUpdate(latest, current, ref latestBuild, ref currentBuild);
                                    break;
                            }

                            if (AppViewModel.Instance.HasNewVersion)
                            {
                                var title = $"{AppViewModel.Instance.Locale["app_DownloadNoticeHeader"]} {AppViewModel.Instance.Locale["app_DownloadNoticeMessage"]}";
                                var message = new StringBuilder();
                                try
                                {
                                    var latestBuildDateTime = new DateTime(2000, 1, 1).Add(new TimeSpan(TimeSpan.TicksPerDay * latestBuild.Build + TimeSpan.TicksPerSecond * 2 * latestBuild.Revision));
                                    var currentBuildDateTime = new DateTime(2000, 1, 1).Add(new TimeSpan(TimeSpan.TicksPerDay * currentBuild.Build + TimeSpan.TicksPerSecond * 2 * currentBuild.Revision));
                                    var timeSpan = latestBuildDateTime - currentBuildDateTime;
                                    if (timeSpan.TotalSeconds > 0)
                                    {
                                        message.AppendLine(String.Format("Missing {0} days, {1} hours and {2} seconds of updates.{3}", timeSpan.Days, timeSpan.Hours, timeSpan.Seconds));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Logging.Log(Logger, new LogItem(ex, true));
                                }
                                finally
                                {
                                    message.AppendLine(AppViewModel.Instance.Locale["app_AlwaysReadUpdatesMessage"]);
                                }
                                MessageBoxHelper.ShowMessageAsync(title, message.ToString(), () => ShellView.CloseApplication(true), delegate { });
                            }
                            var uri = "https://ffxiv-app.com/Analytics/Google/?eCategory=Application Launch&eAction=Version Check&eLabel=FFXIVAPP";
                            DispatcherHelper.Invoke(() => MainView.View.GoogleAnalytics.Navigate(uri));
                        }
                    }
                }
                return true;
            };
            update.BeginInvoke(delegate { }, update);
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        private static int GetProcessID()
        {
            if (Constants.IsOpen && Constants.ProcessModel.ProcessID > 0)
            {
                try
                {
                    Process.GetProcessById(Constants.ProcessModel.ProcessID);
                    return Constants.ProcessModel.ProcessID;
                }
                catch (ArgumentException)
                {
                    Constants.IsOpen = false;
                }
            }
            Constants.ProcessModels = new List<ProcessModel>();
            foreach (var process in Process.GetProcessesByName("ffxiv"))
            {
                Constants.ProcessModels.Add(new ProcessModel
                {
                    Process = process
                });
            }
            foreach (var process in Process.GetProcessesByName("ffxiv_dx11"))
            {
                Constants.ProcessModels.Add(new ProcessModel
                {
                    Process = process,
                    IsWin64 = true
                });
            }
            if (Constants.ProcessModels.Any())
            {
                Constants.IsOpen = true;
                foreach (var processModel in Constants.ProcessModels)
                {
                    SettingsView.View.PIDSelect.Items.Add($"[{processModel.Process.Id}] - {(processModel.IsWin64 ? "64-Bit" : "32-Bit")}");
                }
                SettingsView.View.PIDSelect.SelectedIndex = 0;
                UpdateProcessID(Constants.ProcessModels.First());
                return Constants.ProcessModels.First()
                                .Process.Id;
            }
            Constants.IsOpen = false;
            return -1;
        }

        /// <summary>
        /// </summary>
        public static void SetProcessID()
        {
            StopMemoryWorkers();
            if (SettingsView.View.PIDSelect.Text == string.Empty)
            {
                return;
            }
            var ID = Regex.Match(SettingsView.View.PIDSelect.Text, @"\[(?<id>\d+)\]", SharedRegEx.DefaultOptions)
                          .Groups["id"];
            UpdateProcessID(Constants.ProcessModels.FirstOrDefault(pm => pm.ProcessID == Convert.ToInt32(ID)));
            StartMemoryWorkers();
        }

        /// <summary>
        /// </summary>
        public static void ResetProcessID()
        {
            Constants.ProcessModel = new ProcessModel();
        }

        /// <summary>
        /// </summary>
        /// <param name="processModel"> </param>
        private static void UpdateProcessID(ProcessModel processModel)
        {
            Constants.ProcessModel = processModel;
        }

        public static void UpdatePluginConstants()
        {
            ConstantsHelper.UpdatePluginConstants();
        }

        /// <summary>
        /// </summary>
        public static void StartMemoryWorkers()
        {
            StopMemoryWorkers();
            var id = SettingsView.View.PIDSelect.Text == string.Empty ? GetProcessID() : Constants.ProcessModel.ProcessID;
            Constants.IsOpen = true;
            if (id < 0)
            {
                Constants.IsOpen = false;
                return;
            }

            MemoryHandler.Instance.ExceptionEvent += MemoryHandler_ExceptionEvent;
            MemoryHandler.Instance.SignaturesFoundEvent += MemoryHandler_SignaturesFoundEvent;

            MemoryHandler.Instance.SetProcess(Constants.ProcessModel, Settings.Default.GameLanguage, "latest", Settings.Default.UseLocalMemoryJSONDataCache);

            _chatLogWorker = new ChatLogWorker();
            _chatLogWorker.StartScanning();
            _actorWorker = new ActorWorker();
            _actorWorker.StartScanning();
            _playerInfoWorker = new PlayerInfoWorker();
            _playerInfoWorker.StartScanning();
            _targetWorker = new TargetWorker();
            _targetWorker.StartScanning();
            _partyInfoWorker = new PartyInfoWorker();
            _partyInfoWorker.StartScanning();
            _inventoryWorker = new InventoryWorker();
            _inventoryWorker.StartScanning();
            _hotBarRecastWorker = new HotBarRecastWorker();
            _hotBarRecastWorker.StartScanning();
        }

        /// <summary>
        /// </summary>
        public static void StopMemoryWorkers()
        {
            MemoryHandler.Instance.ExceptionEvent -= MemoryHandler_ExceptionEvent;
            MemoryHandler.Instance.SignaturesFoundEvent -= MemoryHandler_SignaturesFoundEvent;

            if (_chatLogWorker != null)
            {
                _chatLogWorker.StopScanning();
                _chatLogWorker.Dispose();
            }
            if (_actorWorker != null)
            {
                _actorWorker.StopScanning();
                _actorWorker.Dispose();
            }
            if (_playerInfoWorker != null)
            {
                _playerInfoWorker.StopScanning();
                _playerInfoWorker.Dispose();
            }
            if (_targetWorker != null)
            {
                _targetWorker.StopScanning();
                _targetWorker.Dispose();
            }
            if (_partyInfoWorker != null)
            {
                _partyInfoWorker.StopScanning();
                _partyInfoWorker.Dispose();
            }
            if (_inventoryWorker != null)
            {
                _inventoryWorker.StopScanning();
                _inventoryWorker.Dispose();
            }
            if (_hotBarRecastWorker != null)
            {
                _hotBarRecastWorker.StopScanning();
                _hotBarRecastWorker.Dispose();
            }
        }

        private static void MemoryHandler_ExceptionEvent(object sender, ExceptionEvent e)
        {
            Logging.Log(e.Logger, new LogItem(e.Exception, e.LevelIsError));
        }

        private static void MemoryHandler_SignaturesFoundEvent(object sender, SignaturesFoundEvent e)
        {
            foreach (var kvp in e.Signatures)
            {
                Logging.Log(e.Logger, new LogItem($"Signature [{kvp.Key}] Found At Address: [{((IntPtr) kvp.Value).ToString("X")}]"));
            }
        }

        public static void RefreshMemoryWorkers()
        {
            StopMemoryWorkers();
            StartMemoryWorkers();
        }

        public static void StartNetworkWorker()
        {
            StopNetworkWorker();

            NetworkHandler.Instance.ExceptionEvent += NetworkHandler_ExceptionEvent;
            NetworkHandler.Instance.NewNetworkPacketEvent += NetworkHandler_NewPacketEvent;

            var config = new NetworkConfig();
            config.ApplicationName = AssemblyHelper.Name;
            config.CurrentProcessID = Constants.ProcessModel.ProcessID;
            config.ExecutablePath = Application.ExecutablePath;
            config.UserSelectedInterface = Settings.Default.DefaultNetworkInterface;
            config.UseWinPCap = Settings.Default.NetworkUseWinPCap;

            NetworkHandler.Instance.SetProcess(config);

            NetworkHandler.Instance.StartDecrypting();
        }

        public static void StopNetworkWorker()
        {
            NetworkHandler.Instance.ExceptionEvent -= NetworkHandler_ExceptionEvent;
            NetworkHandler.Instance.NewNetworkPacketEvent -= NetworkHandler_NewPacketEvent;

            NetworkHandler.Instance.StopDecrypting();
        }

        private static void NetworkHandler_ExceptionEvent(object sender, Machina.Events.ExceptionEvent e)
        {
            Logging.Log(e.Logger, new LogItem(e.Exception, e.LevelIsError));
        }

        private static void NetworkHandler_NewPacketEvent(object sender, NewNetworkPacketEvent e)
        {
            var packet = e.NetworkPacket;

            AppContextHelper.Instance.RaiseNewPacket(new NetworkPacket
            {
                Buffer = packet.Buffer,
                CurrentPosition = packet.CurrentPosition,
                Key = packet.Key,
                MessageSize = packet.MessageSize,
                PacketDate = packet.PacketDate
            });
        }

        public static void RefreshNetworkWorker()
        {
            StopNetworkWorker();
            StartNetworkWorker();
        }

        #region Declarations

        private static ActorWorker _actorWorker;
        private static ChatLogWorker _chatLogWorker;
        private static PlayerInfoWorker _playerInfoWorker;
        private static TargetWorker _targetWorker;
        private static PartyInfoWorker _partyInfoWorker;
        private static InventoryWorker _inventoryWorker;
        private static HotBarRecastWorker _hotBarRecastWorker;

        #endregion
    }
}
