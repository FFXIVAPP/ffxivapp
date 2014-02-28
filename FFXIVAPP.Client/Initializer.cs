// FFXIVAPP.Client
// Initializer.cs
// 
// © 2013 Ryan Wilson

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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Memory;
using FFXIVAPP.Client.Models;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Client.Utilities;
using FFXIVAPP.Client.ViewModels;
using FFXIVAPP.Client.Views;
using FFXIVAPP.Common.Helpers;
using Newtonsoft.Json.Linq;
using NLog;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client
{
    [DoNotObfuscate]
    internal static class Initializer
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Declarations

        private static ActionWorker _actionWorker;
        private static ActorWorker _actorWorker;
        private static ChatLogWorker _chatLogWorker;
        private static MonsterWorker _monsterWorker;
        private static PlayerInfoWorker _playerInfoWorker;
        private static TargetWorker _targetWorker;
        private static PartyInfoWorker _partyInfoWorker;

        #endregion

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
            if (Constants.Client.XChatCodes != null)
            {
                foreach (var xElement in Constants.Client.XChatCodes.Descendants()
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
        public static void LoadActions()
        {
            if (Constants.Client.XActions != null)
            {
                foreach (var xElement in Constants.Client.XActions.Descendants()
                                                  .Elements("Action"))
                {
                    var xKey = (string) xElement.Attribute("Key");
                    var xJA = (string) xElement.Element("JA");
                    var xEN = (string) xElement.Element("EN");
                    var xFR = (string) xElement.Element("FR");
                    var xDE = (string) xElement.Element("DE");
                    var xJA_HelpLabel = (string) xElement.Element("JA_HelpLabel");
                    var xEN_HelpLabel = (string) xElement.Element("EN_HelpLabel");
                    var xFR_HelpLabel = (string) xElement.Element("FR_HelpLabel");
                    var xDE_HelpLabel = (string) xElement.Element("DE_HelpLabel");
                    if (String.IsNullOrWhiteSpace(xKey) || Constants.Actions.ContainsKey(xKey))
                    {
                        continue;
                    }
                    Constants.Actions.Add(xKey, new ActionInfo
                    {
                        JA = xJA,
                        EN = xEN,
                        FR = xFR,
                        DE = xDE,
                        JA_HelpLabel = xJA_HelpLabel,
                        EN_HelpLabel = xEN_HelpLabel,
                        FR_HelpLabel = xFR_HelpLabel,
                        DE_HelpLabel = xDE_HelpLabel,
                    });
                }
            }
        }

        /// <summary>
        /// </summary>
        public static void LoadAutoTranslate()
        {
            if (Constants.Client.XAutoTranslate != null)
            {
                foreach (var xElement in Constants.Client.XAutoTranslate.Descendants()
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
            if (Constants.Client.XColors != null)
            {
                foreach (var xElement in Constants.Client.XColors.Descendants()
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
            if (Constants.Application.XSettings != null)
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
            if (Constants.Application.XSettings != null)
            {
                foreach (var xElement in Constants.Application.XSettings.Descendants()
                                                  .Elements("PluginSource"))
                {
                    var xKey = Guid.Empty;
                    var xSourceURI = (string) xElement.Element("SourceURI");
                    var xEnabled = true;
                    try
                    {
                        xEnabled = (bool) xElement.Element("Enabled");
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        xKey = (Guid) xElement.Attribute("Key");
                    }
                    catch (Exception)
                    {
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
                        Enabled = xEnabled,
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

            Func<bool> updateCheck = delegate
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
                            var responseText = "";
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
                                pluginSourceList.AddRange(from item in jsonResult
                                                          let name = item["Name"].ToString()
                                                          let enabled = Boolean.Parse(item["Enabled"].ToString())
                                                          let sourceURI = item["SourceURI"].ToString()
                                                          where enabled
                                                          select new PluginSourceItem
                                                          {
                                                              Enabled = enabled,
                                                              Key = Guid.NewGuid(),
                                                              SourceURI = sourceURI
                                                          });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
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
                                    var responseText = "";
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
                                        var pluginFiles = pluginInfo["Files"].ToArray();
                                        var pluginDownload = new PluginDownloadItem
                                        {
                                            Files = new List<PluginFile>(pluginFiles.Select(pluginFile => new PluginFile
                                            {
                                                Location = pluginFile["Location"].ToString(),
                                                Name = pluginFile["Name"].ToString()
                                            })),
                                            Name = pluginInfo["Name"].ToString(),
                                            SourceURI = pluginInfo["SourceURI"].ToString(),
                                            LatestVersion = pluginInfo["Version"].ToString()
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
                                            }
                                        }
                                        DispatcherHelper.Invoke(() => UpdateViewModel.Instance.AvailablePlugins.Add(pluginDownload));
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        DispatcherHelper.Invoke(delegate
                        {
                            if (UpdateView.View.AvailableDG.Items.Count == UpdateViewModel.Instance.AvailablePlugins.Count)
                            {
                                UpdateView.View.AvailableLoadingInformation.Visibility = Visibility.Collapsed;
                            }
                            UpdateView.View.AvailableDG.Items.Refresh();
                            UpdateViewModel.Instance.SetupGrouping();
                        });
                    }
                }
                return true;
            };
            updateCheck.BeginInvoke(null, null);
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
                case "None":
                    break;
                case "Parse":
                    SetHomePlugin(0);
                    break;
                default:
                    try
                    {
                        var index = SettingsViewModel.Instance.HomePluginList.IndexOf(homePlugin);
                        SetHomePlugin(--index);
                    }
                    catch (Exception ex)
                    {
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
        public static void SetCharacter()
        {
            var name = String.Format("{0} {1}", Settings.Default.FirstName, Settings.Default.LastName);
            Settings.Default.CharacterName = StringHelper.TrimAndCleanSpaces(name);
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
            }
            Func<bool> updateCheck = delegate
            {
                var current = Assembly.GetExecutingAssembly()
                                      .GetName()
                                      .Version.ToString();
                AppViewModel.Instance.CurrentVersion = current;
                var httpWebRequest = (HttpWebRequest) WebRequest.Create(String.Format("http://ffxiv-app.com/Json/CurrentVersion/"));
                httpWebRequest.UserAgent = "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_3; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.70 Safari/533.4";
                httpWebRequest.Headers.Add("Accept-Language", "en;q=0.8");
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                using (var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse())
                {
                    using (var response = httpResponse.GetResponseStream())
                    {
                        var responseText = "";
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
                            var jsonResult = JObject.Parse(responseText);
                            var latest = jsonResult["Version"].ToString();
                            var updateNotes = jsonResult["Notes"].ToList();
                            var enabledFeatures = jsonResult["Features"];
                            try
                            {
                                foreach (var feature in enabledFeatures)
                                {
                                    var key = feature["Hash"].ToString();
                                    var enabled = (bool) feature["Enabled"];
                                    switch (key)
                                    {
                                        case "E9FA3917-ACEB-47AE-88CC-58AB014058F5":
                                            XIVDBViewModel.Instance.MonsterUploadEnabled = enabled;
                                            break;
                                        case "6D2DB102-B1AE-4249-9E73-4ABC7B1947BC":
                                            XIVDBViewModel.Instance.NPCUploadEnabled = enabled;
                                            break;
                                        case "D95ADD76-7DA7-4692-AD00-DB12F2853908":
                                            XIVDBViewModel.Instance.KillUploadEnabled = enabled;
                                            break;
                                        case "6A50A13B-BA83-45D7-862F-F110049E7E78":
                                            XIVDBViewModel.Instance.LootUploadEnabled = enabled;
                                            break;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                            try
                            {
                                foreach (var note in updateNotes.Select(updateNote => updateNote.Value<string>()))
                                {
                                    AppViewModel.Instance.UpdateNotes.Add(note);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBoxHelper.ShowMessage("Error", ex.Message);
                            }
                            AppViewModel.Instance.DownloadUri = jsonResult["DownloadUri"].ToString();
                            latest = (latest == "Unknown") ? "Unknown" : String.Format("3{0}", latest.Substring(1));
                            AppViewModel.Instance.LatestVersion = latest;
                            switch (latest)
                            {
                                case "Unknown":
                                    AppViewModel.Instance.HasNewVersion = false;
                                    break;
                                default:
                                    AppViewModel.Instance.HasNewVersion = BuildUtilities.NeedsUpdate(latest, current, ref latestBuild, ref currentBuild);
                                    break;
                            }

                            if (AppViewModel.Instance.HasNewVersion)
                            {
                                var title = String.Format("{0} {1}", AppViewModel.Instance.Locale["app_DownloadNoticeHeader"], AppViewModel.Instance.Locale["app_DownloadNoticeMessage"]);
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
                                }
                                finally
                                {
                                    message.AppendLine(AppViewModel.Instance.Locale["app_AlwaysReadUpdatesMessage"]);
                                }
                                MessageBoxHelper.ShowMessageAsync(title, message.ToString(), () => ShellView.CloseApplication(true), delegate { });
                            }
                            var uri = "http://ffxiv-app.com/Analytics/Google/?eCategory=Application Launch&eAction=Version Check&eLabel=FFXIVAPP";
                            DispatcherHelper.Invoke(() => MainView.View.GoogleAnalytics.Navigate(uri));
                        }
                    }
                }
                return true;
            };
            updateCheck.BeginInvoke(null, null);
        }


        /// <summary>
        /// </summary>
        public static void SetSignatures()
        {
            AppViewModel.Instance.Signatures.Clear();
            AppViewModel.Instance.Signatures.Add(new Signature
            {
                Key = "GAMEMAIN",
                Value = "47616D654D61696E000000",
                Offset = 1180
            });
            AppViewModel.Instance.Signatures.Add(new Signature
            {
                Key = "CHARMAP",
                Value = "??FFFFFF000000??000000??DB0FC93FDB0F49416F1283????FFFFFF",
                Offset = 792
            });
            AppViewModel.Instance.Signatures.Add(new Signature
            {
                Key = "NPCMAP",
                Value = "3E000000????????4000000001000000000000000001000000",
                Offset = 2548
            });
            AppViewModel.Instance.Signatures.Add(new Signature
            {
                Key = "ACTORMAP",
                Value = "3E000000????????4000000001000000000000000001000000",
                Offset = 1164
            });
            AppViewModel.Instance.Signatures.Add(new Signature
            {
                Key = "PARTYMAP",
                Value = "0000DB0FC93FDB0F49416F1283??FFFFFFFFDB0FC93FDB0F49416F1283??00000000??0000",
                Offset = 50
            });
            AppViewModel.Instance.Signatures.Add(new Signature
            {
                Key = "PARTYCOUNT",
                Value = "5F50617274794C69737400",
                Offset = 520
            });
            AppViewModel.Instance.Signatures.Add(new Signature
            {
                Key = "MAP",
                Value = "F783843E????????????????DB0FC93FDB0F49416F1283??",
                Offset = 820
            });
            AppViewModel.Instance.Signatures.Add(new Signature
            {
                Key = "TARGET",
                Value = "40??00000000000000000000000000000000000000000000000000000000????0000????000000000000DB0FC93FDB0F49416F1283??FFFFFFFF",
                Offset = 214
            });
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        private static int GetProcessID()
        {
            if (Constants.IsOpen && Constants.ProcessID > 0)
            {
                try
                {
                    Process.GetProcessById(Constants.ProcessID);
                    return Constants.ProcessID;
                }
                catch (ArgumentException ex)
                {
                    Constants.IsOpen = false;
                }
            }
            Constants.ProcessIDs = Process.GetProcessesByName("ffxiv");
            if (Constants.ProcessIDs.Length == 0)
            {
                Constants.IsOpen = false;
                return -1;
            }
            Constants.IsOpen = true;
            foreach (var process in Constants.ProcessIDs)
            {
                SettingsView.View.PIDSelect.Items.Add(process.Id);
            }
            SettingsView.View.PIDSelect.SelectedIndex = 0;
            UpdateProcessID(Constants.ProcessIDs.First()
                                     .Id);
            return Constants.ProcessIDs.First()
                            .Id;
        }

        /// <summary>
        /// </summary>
        public static void SetProcessID()
        {
            StopMemoryWorkers();
            if (SettingsView.View.PIDSelect.Text == "")
            {
                return;
            }
            UpdateProcessID(Convert.ToInt32(SettingsView.View.PIDSelect.Text));
            StartMemoryWorkers();
        }

        /// <summary>
        /// </summary>
        public static void ResetProcessID()
        {
            Constants.ProcessID = -1;
        }

        /// <summary>
        /// </summary>
        /// <param name="pid"> </param>
        private static void UpdateProcessID(int pid)
        {
            Constants.ProcessID = pid;
        }

        /// <summary>
        /// </summary>
        public static void StartMemoryWorkers()
        {
            StopMemoryWorkers();
            var id = SettingsView.View.PIDSelect.Text == "" ? GetProcessID() : Constants.ProcessID;
            Constants.IsOpen = true;
            if (id < 0)
            {
                Constants.IsOpen = false;
                return;
            }
            var process = Process.GetProcessById(id);
            MemoryHandler.Instance.SetProcess(process);
            MemoryHandler.Instance.SigScanner.LoadOffsets(AppViewModel.Instance.Signatures);
            _chatLogWorker = new ChatLogWorker();
            _chatLogWorker.StartScanning();
            //_actionWorker = new ActionWorker();
            //_actionWorker.StartScanning();
            _actorWorker = new ActorWorker();
            _actorWorker.StartScanning();
            _monsterWorker = new MonsterWorker();
            _monsterWorker.StartScanning();
            _playerInfoWorker = new PlayerInfoWorker();
            _playerInfoWorker.StartScanning();
            _targetWorker = new TargetWorker();
            _targetWorker.StartScanning();
            _partyInfoWorker = new PartyInfoWorker();
            _partyInfoWorker.StartScanning();
        }

        /// <summary>
        /// </summary>
        public static void SetupParsePlugin()
        {
            var parseLogo = new BitmapImage(new Uri(Common.Constants.AppPack + "Resources/Media/Icons/Parse.png"));
            AppViewModel.Instance.PluginTabItems.Insert(0, new TabItem
            {
                Content = new Views.Parse.ShellView(),
                Name = "FFXIVAPPPluginParse",
                Header = "FFXIVAPP.Plugin.Parse",
                HeaderTemplate = TabItemHelper.ImageHeader(parseLogo, "Parse")
            });
        }

        public static void UpdatePluginConstants()
        {
            ConstantsHelper.UpdatePluginConstants();
        }

        /// <summary>
        /// </summary>
        public static void StopMemoryWorkers()
        {
            if (_chatLogWorker != null)
            {
                _chatLogWorker.StopScanning();
                _chatLogWorker.Dispose();
            }
            if (_actionWorker != null)
            {
                _actionWorker.StopScanning();
                _actionWorker.Dispose();
            }
            if (_actorWorker != null)
            {
                _actorWorker.StopScanning();
                _actorWorker.Dispose();
            }
            if (_monsterWorker != null)
            {
                _monsterWorker.StopScanning();
                _monsterWorker.Dispose();
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
        }
    }
}
