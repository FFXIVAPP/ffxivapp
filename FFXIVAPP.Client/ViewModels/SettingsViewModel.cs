// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsViewModel.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   SettingsViewModel.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.ViewModels {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.IO;
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
    using FFXIVAPP.Common.Models;
    using FFXIVAPP.Common.Utilities;
    using FFXIVAPP.Common.ViewModelBase;

    using HtmlAgilityPack;

    using MahApps.Metro.Controls;

    using NLog;

    [Export(typeof(SettingsViewModel)),]
    internal sealed class SettingsViewModel : INotifyPropertyChanged {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static Lazy<SettingsViewModel> _instance = new Lazy<SettingsViewModel>(() => new SettingsViewModel());

        private static string _key = string.Empty;

        private static string _value = string.Empty;

        private List<string> _availableAudioDevicesList;

        private List<string> _availableNetworkInterfacesList;

        private List<string> _homePluginList;

        public SettingsViewModel() {
            this.RefreshMemoryWorkersCommand = new DelegateCommand(RefreshMemoryWorkers);
            this.SetProcessCommand = new DelegateCommand(SetProcess);
            this.RefreshListCommand = new DelegateCommand(RefreshList);
            this.ChangeThemeCommand = new DelegateCommand(ChangeTheme);
            this.DefaultSettingsCommand = new DelegateCommand(DefaultSettings);
            this.ChangeAudioModeCommand = new DelegateCommand(ChangeAudioMode);
            this.GetCICUIDCommand = new DelegateCommand(GetCICUID);
            this.ColorSelectionCommand = new DelegateCommand(ColorSelection);
            this.UpdateColorCommand = new DelegateCommand(UpdateColor);
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public static SettingsViewModel Instance {
            get {
                return _instance.Value;
            }
        }

        public List<string> AvailableAudioDevicesList {
            get {
                return this._availableAudioDevicesList ?? (this._availableAudioDevicesList = new List<string>(Settings.Default.DefaultAudioDeviceList.Cast<string>().ToList()));
            }

            set {
                if (this._availableAudioDevicesList == null) {
                    this._availableAudioDevicesList = new List<string>(Settings.Default.DefaultAudioDeviceList.Cast<string>().ToList());
                }

                this._availableAudioDevicesList = value;
                this.RaisePropertyChanged();
            }
        }

        public List<string> AvailableNetworkInterfacesList {
            get {
                return this._availableNetworkInterfacesList ?? (this._availableNetworkInterfacesList = new List<string>());
            }

            set {
                if (this._availableNetworkInterfacesList == null) {
                    this._availableNetworkInterfacesList = new List<string>();
                }

                this._availableNetworkInterfacesList = value;
                this.RaisePropertyChanged();
            }
        }

        public ICommand ChangeAudioModeCommand { get; private set; }

        public ICommand ChangeThemeCommand { get; private set; }

        public ICommand ColorSelectionCommand { get; private set; }

        public ICommand DefaultSettingsCommand { get; private set; }

        public ICommand GetCICUIDCommand { get; private set; }

        public List<string> HomePluginList {
            get {
                return this._homePluginList ?? (this._homePluginList = new List<string>(Settings.Default.HomePluginList.Cast<string>().ToList()));
            }

            set {
                if (this._homePluginList == null) {
                    this._homePluginList = new List<string>(Settings.Default.HomePluginList.Cast<string>().ToList());
                }

                this._homePluginList = value;
                this.RaisePropertyChanged();
            }
        }

        public ICommand RefreshListCommand { get; private set; }

        public ICommand RefreshMemoryWorkersCommand { get; private set; }

        public ICommand SetProcessCommand { get; private set; }

        public ICommand UpdateColorCommand { get; private set; }

        /// <summary>
        /// </summary>
        public static void ChangeAudioMode() {
            SoundPlayerHelper.CacheSoundFiles();
        }

        /// <summary>
        /// </summary>
        private static void ChangeTheme() {
            List<MetroWindow> windows = (from object window in Application.Current.Windows
                                         select window as MetroWindow).ToList();
            ThemeHelper.ChangeTheme(Settings.Default.Theme, windows);
        }

        /// <summary>
        /// </summary>
        private static void ColorSelection() {
            if (SettingsView.View.Colors.SelectedItems.Count <= 0) {
                return;
            }

            string[] split = SettingsView.View.Colors.SelectedItem.ToString().Split(',');
            _key = split[0].Trim().Replace("[", string.Empty);
            _value = Constants.Colors[_key][0];
            SettingsView.View.TCode.Text = _key;
            SettingsView.View.TColor.Text = _value;
        }

        /// <summary>
        /// </summary>
        private static void DefaultSettings() {
            SettingsHelper.Default();
        }

        /// <summary>
        /// </summary>
        private static void GetCICUID() {
            var characterName = Settings.Default.CharacterName;
            var serverName = Settings.Default.ServerName;
            if (characterName.Replace(" ", string.Empty).Length < 3 || string.IsNullOrWhiteSpace(serverName)) {
                return;
            }

            Func<string> lodestoneRender = delegate {
                var cicuid = string.Empty;
                try {
                    var url = "http://na.finalfantasyxiv.com/lodestone/character/?q={0}&worldname={1}";
                    var request = (HttpWebRequest) WebRequest.Create(string.Format(url, HttpUtility.UrlEncode(Constants.CharacterName), serverName));
                    request.UserAgent = "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_3; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.70 Safari/533.4";
                    request.Headers.Add("Accept-Language", "en;q=0.8");
                    request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                    var response = (HttpWebResponse) request.GetResponse();
                    Stream stream = response.GetResponseStream();
                    if (response.StatusCode != HttpStatusCode.OK || stream == null) { }
                    else {
                        var doc = new HtmlDocument();
                        doc.Load(stream);
                        try {
                            var htmlSource = doc.DocumentNode.SelectSingleNode("//html").OuterHtml;
                            var CICUID = new Regex(@"(?<cicuid>\d+)/string.Empty>" + HttpUtility.HtmlEncode(characterName), RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.IgnoreCase);
                            cicuid = CICUID.Match(htmlSource).Groups["cicuid"].Value;
                        }
                        catch (Exception ex) {
                            Logging.Log(Logger, new LogItem(ex, true));
                        }
                    }
                }
                catch (Exception ex) {
                    Logging.Log(Logger, new LogItem(ex, true));
                }

                return cicuid;
            };
            lodestoneRender.BeginInvoke(LodestoneCallBack, lodestoneRender);
        }

        /// <summary>
        /// </summary>
        /// <param name="asyncResult"> </param>
        private static void LodestoneCallBack(IAsyncResult asyncResult) {
            Func<string> function = asyncResult.AsyncState as Func<string>;
            if (function == null) {
                return;
            }

            var result = function.EndInvoke(asyncResult);
            Settings.Default.CICUID = result;
        }

        /// <summary>
        /// </summary>
        private static void RefreshList() {
            SettingsView.View.PIDSelect.Items.Clear();
            Initializer.StopMemoryWorkers();
            Initializer.ResetProcessID();
            Initializer.StartMemoryWorkers();
        }

        private static void RefreshMemoryWorkers() {
            Initializer.RefreshMemoryWorkers();
        }

        /// <summary>
        /// </summary>
        private static void SetProcess() {
            Initializer.SetProcessID();
        }

        /// <summary>
        /// </summary>
        private static void UpdateColor() {
            _key = SettingsView.View.TCode.Text;
            _value = SettingsView.View.TColor.Text;
            if (string.IsNullOrEmpty(_key)) {
                return;
            }

            if (!Regex.IsMatch(_value, "^[A-F0-9]{6,6}$")) {
                return;
            }

            Constants.Colors[_key][0] = _value;
            SettingsView.View.Colors.Items.Refresh();
        }

        private void RaisePropertyChanged([CallerMemberName,] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
    }
}