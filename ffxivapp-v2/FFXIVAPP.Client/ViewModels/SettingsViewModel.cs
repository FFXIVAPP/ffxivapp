// FFXIVAPP.Client
// SettingsViewModel.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Client.Views;
using FFXIVAPP.Common.RegularExpressions;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Common.ViewModelBase;
using HtmlAgilityPack;
using NLog;

#endregion

namespace FFXIVAPP.Client.ViewModels
{
    [Export(typeof (SettingsViewModel))]
    internal sealed class SettingsViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static SettingsViewModel _instance;

        public static SettingsViewModel Instance
        {
            get { return _instance ?? (_instance = new SettingsViewModel()); }
        }

        #endregion

        #region Declarations

        private static string _key = "";
        private static string _value = "";
        public ICommand ChangeThemeCommand { get; private set; }
        public ICommand DefaultSettingsCommand { get; private set; }
        public ICommand GetCICUIDCommand { get; private set; }
        public ICommand SaveCharacterCommand { get; private set; }
        public ICommand ColorSelectionCommand { get; private set; }
        public ICommand UpdateColorCommand { get; private set; }

        #endregion

        public SettingsViewModel()
        {
            ChangeThemeCommand = new DelegateCommand(ChangeTheme);
            DefaultSettingsCommand = new DelegateCommand(DefaultSettings);
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
        private static void ChangeTheme()
        {
            ThemeHelper.ChangeTheme(Settings.Default.Theme);
        }

        /// <summary>
        /// </summary>
        private static void DefaultSettings()
        {
            SettingsHelper.Default();
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
            var serverNumber = Constants.ServerNumber[serverName];
            Func<string> callLodestone = delegate
            {
                var cicuid = "";
                var uri = "";
                try
                {
                    const string url = "http://lodestone.finalfantasyxiv.com/rc/search/search?tgt=77&q=\"{0}\"&cms=&cw={1}";

                    var request = (HttpWebRequest) WebRequest.Create(String.Format(url, Common.Constants.CharacterName, serverNumber));
                    request.UserAgent = "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_3; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.70 Safari/533.4";
                    request.Headers.Add("Accept-Language", "en;q=0.8");
                    var response = (HttpWebResponse) request.GetResponse();
                    var stream = response.GetResponseStream();
                    if (response.StatusCode != HttpStatusCode.OK || stream == null)
                    {
                    }
                    else
                    {
                        var doc = new HtmlDocument();
                        doc.Load(stream);
                        var iconNode = doc.DocumentNode.SelectSingleNode(("//a[@class='']/@href"));
                        if (iconNode != null)
                        {
                            uri = iconNode.GetAttributeValue("href", "");
                        }
                    }
                    try
                    {
                        cicuid = SharedRegEx.CICUID.Match(uri)
                                            .Groups["cicuid"].Value;
                    }
                    catch (Exception ex)
                    {
                        Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
                    }
                }
                catch (Exception ex)
                {
                    Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
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
            _value = Common.Constants.Colors[_key][0];
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
            Common.Constants.Colors[_key][0] = _value;
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
