// FFXIVAPP.Client
// SettingsViewModel.cs
// 
// © 2013 Ryan Wilson

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
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.ViewModels
{
    [DoNotObfuscate]
    [Export(typeof (SettingsViewModel))]
    internal sealed class SettingsViewModel : INotifyPropertyChanged
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Property Bindings

        private static SettingsViewModel _instance;
        private List<string> _homePluginList;
        private List<string> _availableAudioDevicesList;

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

        #endregion

        #region Declarations

        private static string _key = "";
        private static string _value = "";

        public ICommand SetProcessCommand { get; private set; }
        public ICommand RefreshListCommand { get; private set; }
        public ICommand ChangeThemeCommand { get; private set; }
        public ICommand DefaultSettingsCommand { get; private set; }
        public ICommand ChangeAudioModeCommand { get; private set; }
        public ICommand GetCICUIDCommand { get; private set; }
        public ICommand SaveCharacterCommand { get; private set; }
        public ICommand ColorSelectionCommand { get; private set; }
        public ICommand UpdateColorCommand { get; private set; }

        #endregion

        public SettingsViewModel()
        {
            SetProcessCommand = new DelegateCommand(SetProcess);
            RefreshListCommand = new DelegateCommand(RefreshList);
            ChangeThemeCommand = new DelegateCommand(ChangeTheme);
            DefaultSettingsCommand = new DelegateCommand(DefaultSettings);
            ChangeAudioModeCommand = new DelegateCommand(ChangeAudioMode);
            GetCICUIDCommand = new DelegateCommand(GetCICUID);
            SaveCharacterCommand = new DelegateCommand(SaveCharacter);
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
            SaveCharacter();
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
        private static void SaveCharacter()
        {
            Initializer.SetCharacter();
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
