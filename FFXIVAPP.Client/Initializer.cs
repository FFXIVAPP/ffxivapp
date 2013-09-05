// FFXIVAPP.Client
// Initializer.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using FFXIVAPP.Client.Delegates;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Memory;
using FFXIVAPP.Client.Models;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Client.Views;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.Utilities;
using Newtonsoft.Json.Linq;
using NLog;

#endregion

namespace FFXIVAPP.Client
{
    internal static class Initializer
    {
        #region Declarations

        private static ChatWorker _chatWorker;
        private static NPCWorker _npcWorker;

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
                Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("LoadedChatCodes : {0} KeyValuePairs", Constants.ChatCodes.Count));
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
                Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("LoadedAutoTranslate : {0} KeyValuePairs", Constants.AutoTranslate.Count));
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
                Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("LoadedColors : {0} KeyValuePairs", Constants.Colors.Count));
            }
        }

        /// <summary>
        /// </summary>
        public static void LoadPlugins()
        {
            App.Plugins.LoadPlugins(Directory.GetCurrentDirectory() + @"\Plugins");
            foreach (PluginInstance pluginInstance in App.Plugins.Loaded)
            {
                try
                {
                    var tabItem = pluginInstance.Instance.CreateTab();
                    var iconfile = String.Format("{0}\\{1}", Path.GetDirectoryName(pluginInstance.AssemblyPath), pluginInstance.Instance.Icon);
                    var icon = new BitmapImage(new Uri(Common.Constants.DefaultIcon));
                    icon = File.Exists(iconfile) ? new BitmapImage(new Uri(iconfile)) : icon;
                    tabItem.HeaderTemplate = TabItemHelper.ImageHeader(icon, pluginInstance.Instance.Name);
                    var info = new Dictionary<string, string>();
                    info.Add("Icon", pluginInstance.Instance.Icon);
                    info.Add("Description", pluginInstance.Instance.Description);
                    info.Add("Copyright", pluginInstance.Instance.Copyright);
                    info.Add("Version", pluginInstance.Instance.Version);
                    AppViewModel.Instance.PluginTabItems.Add(tabItem);
                }
                catch (AppException ex)
                {
                    Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
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
        public static void SetCharacter()
        {
            var name = String.Format("{0} {1}", Settings.Default.FirstName, Settings.Default.LastName);
            Settings.Default.CharacterName = name.Trim();
        }

        /// <summary>
        /// </summary>
        public static void CheckUpdates()
        {
            Func<bool> updateCheck = delegate
            {
                try
                {
                    File.Delete("FFXIVAPP.Updater.Backup.exe");
                }
                catch (Exception ex)
                {
                }
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
                    AppViewModel.Instance.DownloadUri = jsonResult["DownloadUri"].ToString();
                    try
                    {
                        foreach (var note in updateNotes.Select(updateNote => updateNote.Value<string>()))
                        {
                            AppViewModel.Instance.UpdateNotes.Add(note);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    AppViewModel.Instance.LatestVersion = latest;
                    switch (latest)
                    {
                        case "Unknown":
                            AppViewModel.Instance.HasNewVersion = false;
                            break;
                        default:
                            var lver = latest.Split('.');
                            var cver = current.Split('.');
                            int lmajor = 0, lminor = 0, lbuild = 0, lrevision = 0;
                            int cmajor = 0, cminor = 0, cbuild = 0, crevision = 0;
                            try
                            {
                                lmajor = Int32.Parse(lver[0]);
                                lminor = Int32.Parse(lver[1]);
                                lbuild = Int32.Parse(lver[2]);
                                lrevision = Int32.Parse(lver[3]);
                                cmajor = Int32.Parse(cver[0]);
                                cminor = Int32.Parse(cver[1]);
                                cbuild = Int32.Parse(cver[2]);
                                crevision = Int32.Parse(cver[3]);
                            }
                            catch (Exception ex)
                            {
                                AppViewModel.Instance.HasNewVersion = false;
                                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
                            }
                            if (lmajor <= cmajor)
                            {
                                if (lminor <= cminor)
                                {
                                    if (lbuild == cbuild)
                                    {
                                        AppViewModel.Instance.HasNewVersion = lrevision > crevision;
                                        break;
                                    }
                                    AppViewModel.Instance.HasNewVersion = lbuild > cbuild;
                                    break;
                                }
                                AppViewModel.Instance.HasNewVersion = true;
                            }
                            break;
                    }
                }
                if (AppViewModel.Instance.HasNewVersion)
                {
                    DispatcherHelper.Invoke(delegate
                    {
                        var popupContent = new PopupContent();
                        popupContent.Title = AppViewModel.Instance.Locale["app_DownloadNoticeHeader"];
                        popupContent.Message = AppViewModel.Instance.Locale["app_DownloadNoticeMessage"];
                        popupContent.CanSayNo = true;
                        PopupHelper.Toggle(popupContent);
                        EventHandler closedDelegate = null;
                        closedDelegate = delegate
                        {
                            switch (PopupHelper.Result)
                            {
                                case MessageBoxResult.Yes:
                                    ShellView.CloseApplication(true);
                                    break;
                                case MessageBoxResult.No:
                                    break;
                            }
                            PopupHelper.MessagePopup.Closed -= closedDelegate;
                        };
                        PopupHelper.MessagePopup.Closed += closedDelegate;
                    });
                    //EventHandler notifyEventHandler = null;
                    //notifyEventHandler = delegate
                    //{
                    //    ShellView.CloseApplication(true);
                    //    AppViewModel.Instance.NotifyIcon.BalloonTipClicked -= notifyEventHandler;
                    //};
                    //DispatcherHelper.Invoke(() => NotifyIconHelper.ShowBalloonMessage(title, message, notifyEventHandler));
                }
                const string uri = "http://ffxiv-app.com/Analytics/Google/?eCategory=Application Launch&eAction=Version Check&eLabel=FFXIVAPP";
                DispatcherHelper.Invoke(() => MainView.View.GoogleAnalytics.Navigate(uri));
                return true;
            };
            updateCheck.BeginInvoke(null, null);
        }

        /// <summary>
        /// </summary>
        public static void SetSignatures()
        {
            var signatures = AppViewModel.Instance.Signatures;
            signatures.Clear();
            signatures.Add(new Signature
            {
                Key = "GAMEMAIN",
                Value = "47616D654D61696E000000",
                Offset = 1176
            });
            signatures.Add(new Signature
            {
                Key = "CHARMAP",
                Value = "00000000FFFFFFFF0A000000000000000000000000000000000000000000000000000000000000000000000000000000", /*????????00000000DB0FC93F6F12833A*/
                Offset = 68
            });
            signatures.Add(new Signature
            {
                Key = "MAP",
                Value = "F783843E????????DB0FC93F6F12833A",
                Offset = 784
            });
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        private static int GetPID()
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
                    Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
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
            PID(Constants.ProcessIDs.First()
                         .Id);
            return Constants.ProcessIDs.First()
                            .Id;
        }

        /// <summary>
        /// </summary>
        public static void SetPID()
        {
            StopMemoryWorkers();
            if (SettingsView.View.PIDSelect.Text == "")
            {
                return;
            }
            PID(Convert.ToInt32(SettingsView.View.PIDSelect.Text));
            StartMemoryWorkers();
        }

        /// <summary>
        /// </summary>
        public static void ResetPID()
        {
            Constants.ProcessID = -1;
        }

        /// <summary>
        /// </summary>
        /// <param name="pid"> </param>
        private static void PID(int pid)
        {
            Constants.ProcessID = pid;
            Constants.ProcessHandle = Constants.ProcessIDs[SettingsView.View.PIDSelect.SelectedIndex].MainWindowHandle;
        }

        /// <summary>
        /// </summary>
        public static void StartMemoryWorkers()
        {
            var id = SettingsView.View.PIDSelect.Text == "" ? GetPID() : Constants.ProcessID;
            Constants.IsOpen = true;
            if (id < 0)
            {
                Constants.IsOpen = false;
                return;
            }
            var process = Process.GetProcessById(id);
            var offsets = new SigFinder(process, AppViewModel.Instance.Signatures);
            if (_chatWorker != null || _npcWorker != null)
            {
                StopMemoryWorkers();
            }
            _chatWorker = new ChatWorker(process, offsets);
            _chatWorker.StartScanning();
            _chatWorker.OnNewline += ChatWorkerDelegate.OnNewLine;
            _npcWorker = new NPCWorker(process, offsets);
            _npcWorker.StartScanning();
            _npcWorker.OnNewNPC += NPCWorkerDelegate.OnNewNPC;
        }

        /// <summary>
        /// </summary>
        public static void StopMemoryWorkers()
        {
            if (_chatWorker != null)
            {
                _chatWorker.OnNewline -= ChatWorkerDelegate.OnNewLine;
                _chatWorker.StopScanning();
                _chatWorker.Dispose();
            }
            if (_npcWorker != null)
            {
                _npcWorker.OnNewNPC -= NPCWorkerDelegate.OnNewNPC;
                _npcWorker.StopScanning();
                _npcWorker.Dispose();
            }
        }
    }
}
