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
using System.Reflection;
using System.Text;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Memory;
using FFXIVAPP.Client.Models;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Client.ViewModels;
using FFXIVAPP.Client.Views;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.Utilities;
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

        private static ChatLogWorker _chatLogWorker;
        private static MonsterWorker _monsterWorker;
        private static NPCWorker _npcWorker;
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
            if (Constants.Application.XSettings != null)
            {
                foreach (var xElement in Constants.Application.XSettings.Descendants()
                                                  .Elements("Setting"))
                {
                    var xKey = (string) xElement.Attribute("Key");
                    bool xEnabled;
                    bool.TryParse((string) xElement.Element("Enabled"), out xEnabled);
                    if (String.IsNullOrWhiteSpace(xKey))
                    {
                        continue;
                    }
                    try
                    {
                        Constants.Application.EnabledPlugins.Add(xKey, xEnabled);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        public static void LoadPlugins()
        {
            App.Plugins.LoadPlugins(Directory.GetCurrentDirectory() + @"\Plugins");
            var removed = new List<PluginInstance>();
            foreach (PluginInstance pluginInstance in App.Plugins.Loaded)
            {
                var pluginName = pluginInstance.Instance.FriendlyName;
                if (SettingsViewModel.Instance.HomePluginList.Any(p => p.ToUpperInvariant()
                                                                        .StartsWith(pluginName.ToUpperInvariant())))
                {
                    pluginName = String.Format("{0}[{1}]", pluginName, new Random().Next(1000, 9999));
                }
                SettingsViewModel.Instance.HomePluginList.Add(pluginName);
                try
                {
                    // get basic plugin info for settings panel
                    var pluginInfo = new PluginInformation
                    {
                        Copyright = pluginInstance.Instance.Copyright,
                        Description = pluginInstance.Instance.Description,
                        Icon = pluginInstance.Instance.Icon,
                        Name = pluginInstance.Instance.Name,
                        Version = pluginInstance.Instance.Version
                    };
                    // if enabled setup tabItem
                    if (Constants.Application.EnabledPlugins.ContainsKey(pluginInstance.Instance.Name))
                    {
                        if (Constants.Application.EnabledPlugins[pluginInstance.Instance.Name])
                        {
                            pluginInfo.IsEnabled = true;
                        }
                    }
                    else
                    {
                        Constants.Application.EnabledPlugins.Add(pluginInstance.Instance.Name, true);
                        pluginInfo.IsEnabled = true;
                    }
                    AppViewModel.Instance.PluginInfo.Add(pluginInfo);
                    if (!pluginInfo.IsEnabled)
                    {
                        removed.Add(pluginInstance);
                        continue;
                    }
                    var tabItem = pluginInstance.Instance.CreateTab();
                    var iconfile = String.Format("{0}\\{1}", Path.GetDirectoryName(pluginInstance.AssemblyPath), pluginInstance.Instance.Icon);
                    var icon = new BitmapImage(new Uri(Common.Constants.DefaultIcon));
                    icon = File.Exists(iconfile) ? new BitmapImage(new Uri(iconfile)) : icon;
                    tabItem.HeaderTemplate = TabItemHelper.ImageHeader(icon, pluginInstance.Instance.FriendlyName);
                    AppViewModel.Instance.PluginTabItems.Add(tabItem);
                }
                catch (AppException ex)
                {
                }
            }
            foreach (var pluginInstance in removed)
            {
                try
                {
                    App.Plugins.Loaded.Remove(pluginInstance);
                }
                catch (Exception ex)
                {
                }
            }
            AppViewModel.Instance.HasPlugins = App.Plugins.Loaded.Count > 0;
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
            ;
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
                var request = (HttpWebRequest) WebRequest.Create(String.Format("http://ffxiv-app.com/Json/CurrentVersion/"));
                request.UserAgent = "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_3; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.70 Safari/533.4";
                request.Headers.Add("Accept-Language", "en;q=0.8");
                request.ContentType = "application/json; charset=utf-8";
                var response = (HttpWebResponse) request.GetResponse();
                var responseText = "";
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    try
                    {
                        responseText = streamReader.ReadToEnd();
                    }
                    catch (Exception ex)
                    {
                    }
                }
                int latestMajor = 0, latestMinor = 0, latestBuild = 0, latestRevision = 0;
                int currentMajor = 0, currentMinor = 0, currentBuild = 0, currentRevision = 0;
                if (response.StatusCode != HttpStatusCode.OK || String.IsNullOrWhiteSpace(responseText))
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
                            var latestVersionSplit = latest.Split('.');
                            var currentVersionSplit = current.Split('.');
                            try
                            {
                                latestMajor = Int32.Parse(latestVersionSplit[0]);
                                latestMinor = Int32.Parse(latestVersionSplit[1]);
                                latestBuild = Int32.Parse(latestVersionSplit[2]);
                                latestRevision = Int32.Parse(latestVersionSplit[3]);
                                currentMajor = Int32.Parse(currentVersionSplit[0]);
                                currentMinor = Int32.Parse(currentVersionSplit[1]);
                                currentBuild = Int32.Parse(currentVersionSplit[2]);
                                currentRevision = Int32.Parse(currentVersionSplit[3]);
                            }
                            catch (Exception ex)
                            {
                                AppViewModel.Instance.HasNewVersion = false;
                            }
                            if (latestMajor <= currentMajor)
                            {
                                if (latestMinor <= currentMinor)
                                {
                                    if (latestBuild == currentBuild)
                                    {
                                        AppViewModel.Instance.HasNewVersion = latestRevision > currentRevision;
                                        break;
                                    }
                                    AppViewModel.Instance.HasNewVersion = latestBuild > currentBuild;
                                    break;
                                }
                                AppViewModel.Instance.HasNewVersion = true;
                                break;
                            }
                            AppViewModel.Instance.HasNewVersion = true;
                            break;
                    }
                }
                if (AppViewModel.Instance.HasNewVersion)
                {
                    var title = String.Format("{0} {1}", AppViewModel.Instance.Locale["app_DownloadNoticeHeader"], AppViewModel.Instance.Locale["app_DownloadNoticeMessage"]);
                    var message = new StringBuilder();
                    try
                    {
                        var latestBuildDateTime = new DateTime(2000, 1, 1).Add(new TimeSpan(TimeSpan.TicksPerDay * latestBuild + TimeSpan.TicksPerSecond * 2 * latestRevision));
                        var currentBuildDateTime = new DateTime(2000, 1, 1).Add(new TimeSpan(TimeSpan.TicksPerDay * currentBuild + TimeSpan.TicksPerSecond * 2 * currentRevision));
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
                Offset = 1176
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
                Offset = 2524
            });
            AppViewModel.Instance.Signatures.Add(new Signature
            {
                Key = "PARTYMAP",
                Value = "??FFFFFF0000000000000000DB0FC93FDB0F49416F1283??00",
                Offset = 44
            });
            AppViewModel.Instance.Signatures.Add(new Signature
            {
                Key = "PARTYCOUNT",
                Value = "5F50617274794C69737400",
                Offset = 512
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
                Value = "DB0F49416F1283??????????????????DB0FC940920A063F",
                Offset = 136
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
            _monsterWorker = new MonsterWorker();
            _monsterWorker.StartScanning();
            _npcWorker = new NPCWorker();
            _npcWorker.StartScanning();
            _playerInfoWorker = new PlayerInfoWorker();
            _playerInfoWorker.StartScanning();
            _targetWorker = new TargetWorker();
            _targetWorker.StartScanning();
            _partyInfoWorker = new PartyInfoWorker();
            _partyInfoWorker.StartScanning();
        }

        /// <summary>
        /// </summary>
        public static void SetupPlugins()
        {
            // get official plugin logos
            var parseLogo = new BitmapImage(new Uri(Common.Constants.AppPack + "Resources/Media/Icons/Parse.png"));
            // setup headers for existing plugins
            ShellView.View.ParsePlugin.HeaderTemplate = TabItemHelper.ImageHeader(parseLogo, "Parse");
            // append third party plugins
            foreach (var pluginTabItem in AppViewModel.Instance.PluginTabItems)
            {
                ShellView.View.PluginsTC.Items.Add(pluginTabItem);
            }
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
            if (_monsterWorker != null)
            {
                _monsterWorker.StopScanning();
                _monsterWorker.Dispose();
            }
            if (_npcWorker != null)
            {
                _npcWorker.StopScanning();
                _npcWorker.Dispose();
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
