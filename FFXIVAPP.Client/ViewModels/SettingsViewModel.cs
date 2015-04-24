// FFXIVAPP.Client
// SettingsViewModel.cs
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
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows;
using System.Windows.Input;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Client.Views;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.ViewModelBase;
using HtmlAgilityPack;
using MahApps.Metro.Controls;
using NLog;

namespace FFXIVAPP.Client.ViewModels
{
    [Export(typeof (SettingsViewModel))]
    internal sealed class SettingsViewModel : INotifyPropertyChanged
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Property Bindings

        private static SettingsViewModel _instance;
        private List<string> _availableAudioDevicesList;
        private List<string> _availableNetworkInterfacesList;
        private List<string> _homePluginList;

        public static SettingsViewModel Instance
        {
            get { return _instance ?? (_instance = new SettingsViewModel()); }
        }

        public List<string> HomePluginList
        {
            get
            {
                return _homePluginList ?? (_homePluginList = new List<string>(Settings.Default.HomePluginList.Cast<string>()
                                                                                      .ToList()));
            }
            set
            {
                if (_homePluginList == null)
                {
                    _homePluginList = new List<string>(Settings.Default.HomePluginList.Cast<string>()
                                                               .ToList());
                }
                _homePluginList = value;
                RaisePropertyChanged();
            }
        }

        public List<string> AvailableAudioDevicesList
        {
            get
            {
                return _availableAudioDevicesList ?? (_availableAudioDevicesList = new List<string>(Settings.Default.DefaultAudioDeviceList.Cast<string>()
                                                                                                            .ToList()));
            }
            set
            {
                if (_availableAudioDevicesList == null)
                {
                    _availableAudioDevicesList = new List<string>(Settings.Default.DefaultAudioDeviceList.Cast<string>()
                                                                          .ToList());
                }
                _availableAudioDevicesList = value;
                RaisePropertyChanged();
            }
        }

        public List<string> AvailableNetworkInterfacesList
        {
            get
            {
                return _availableNetworkInterfacesList ?? (_availableNetworkInterfacesList = new List<string>());
            }
            set
            {
                if (_availableNetworkInterfacesList == null)
                {
                    _availableNetworkInterfacesList = new List<string>();
                }
                _availableNetworkInterfacesList = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Declarations

        private static string _key = "";
        private static string _value = "";

        public ICommand RefreshNetworkWorkerCommand { get; private set; }
        public ICommand SetProcessCommand { get; private set; }
        public ICommand RefreshListCommand { get; private set; }
        public ICommand ChangeThemeCommand { get; private set; }
        public ICommand DefaultSettingsCommand { get; private set; }
        public ICommand ChangeAudioModeCommand { get; private set; }
        public ICommand GetCICUIDCommand { get; private set; }
        public ICommand ColorSelectionCommand { get; private set; }
        public ICommand UpdateColorCommand { get; private set; }

        #endregion

        public SettingsViewModel()
        {
            RefreshNetworkWorkerCommand = new DelegateCommand(RefreshNetworkWorker);
            SetProcessCommand = new DelegateCommand(SetProcess);
            RefreshListCommand = new DelegateCommand(RefreshList);
            ChangeThemeCommand = new DelegateCommand(ChangeTheme);
            DefaultSettingsCommand = new DelegateCommand(DefaultSettings);
            ChangeAudioModeCommand = new DelegateCommand(ChangeAudioMode);
            GetCICUIDCommand = new DelegateCommand(GetCICUID);
            ColorSelectionCommand = new DelegateCommand(ColorSelection);
            UpdateColorCommand = new DelegateCommand(UpdateColor);
        }

        #region Loading Functions

        #endregion

        #region Utility Functions

        #endregion

        #region Command Bindings

        /// <summary>
        /// </summary>
        private static void RefreshNetworkWorker()
        {
            Initializer.RefreshNetworkWorker();
        }

        /// <summary>
        /// </summary>
        private static void SetProcess()
        {
            Initializer.SetProcessID();
        }

        /// <summary>
        /// </summary>
        private static void RefreshList()
        {
            SettingsView.View.PIDSelect.Items.Clear();
            Initializer.StopMemoryWorkers();
            Initializer.ResetProcessID();
            Initializer.StartMemoryWorkers();
        }

        /// <summary>
        /// </summary>
        private static void ChangeTheme()
        {
            var windows = (from object window in Application.Current.Windows select window as MetroWindow).ToList();
            ThemeHelper.ChangeTheme(Settings.Default.Theme, windows);
        }

        /// <summary>
        /// </summary>
        private static void DefaultSettings()
        {
            SettingsHelper.Default();
        }

        /// <summary>
        /// </summary>
        public static void ChangeAudioMode()
        {
            SoundPlayerHelper.CacheSoundFiles();
        }

        /// <summary>
        /// </summary>
        private static void GetCICUID()
        {
            var characterName = Settings.Default.CharacterName;
            var serverName = Settings.Default.ServerName;
            if (characterName.Replace(" ", "")
                             .Length < 3 || String.IsNullOrWhiteSpace(serverName))
            {
                return;
            }
            Func<string> callLodestone = delegate
            {
                var cicuid = "";
                try
                {
                    var url = "http://na.finalfantasyxiv.com/lodestone/character/?q={0}&worldname={1}";
                    var request = (HttpWebRequest) WebRequest.Create(String.Format(url, HttpUtility.UrlEncode(Constants.CharacterName), serverName));
                    request.UserAgent = "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_3; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.70 Safari/533.4";
                    request.Headers.Add("Accept-Language", "en;q=0.8");
                    request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                    var response = (HttpWebResponse) request.GetResponse();
                    var stream = response.GetResponseStream();
                    if (response.StatusCode != HttpStatusCode.OK || stream == null)
                    {
                    }
                    else
                    {
                        var doc = new HtmlDocument();
                        doc.Load(stream);
                        try
                        {
                            var htmlSource = doc.DocumentNode.SelectSingleNode("//html")
                                                .OuterHtml;
                            var CICUID = new Regex(@"(?<cicuid>\d+)/"">" + HttpUtility.HtmlEncode(characterName), RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.IgnoreCase);
                            cicuid = CICUID.Match(htmlSource)
                                           .Groups["cicuid"].Value;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                return cicuid;
            };
            callLodestone.BeginInvoke(LodestoneCallBack, callLodestone);
        }

        /// <summary>
        /// </summary>
        /// <param name="asyncResult"> </param>
        private static void LodestoneCallBack(IAsyncResult asyncResult)
        {
            var function = asyncResult.AsyncState as Func<string>;
            if (function == null)
            {
                return;
            }
            var result = function.EndInvoke(asyncResult);
            Settings.Default.CICUID = result;
        }

        /// <summary>
        /// </summary>
        private static void ColorSelection()
        {
            if (SettingsView.View.Colors.SelectedItems.Count <= 0)
            {
                return;
            }
            var split = SettingsView.View.Colors.SelectedItem.ToString()
                                    .Split(',');
            _key = split[0].Trim()
                           .Replace("[", "");
            _value = Constants.Colors[_key][0];
            SettingsView.View.TCode.Text = _key;
            SettingsView.View.TColor.Text = _value;
        }

        /// <summary>
        /// </summary>
        private static void UpdateColor()
        {
            _key = SettingsView.View.TCode.Text;
            _value = SettingsView.View.TColor.Text;
            if (String.IsNullOrEmpty(_key))
            {
                return;
            }
            if (!Regex.IsMatch(_value, "^[A-F0-9]{6,6}$"))
            {
                return;
            }
            Constants.Colors[_key][0] = _value;
            SettingsView.View.Colors.Items.Refresh();
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
