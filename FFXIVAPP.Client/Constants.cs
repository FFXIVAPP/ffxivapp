// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Constants.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Avalonia.Controls;
    using FFXIVAPP.Client.Helpers;
    using FFXIVAPP.Client.Models;
    using FFXIVAPP.Common.Helpers;
    using Sharlayan.Models;

    internal static class Constants {
        public const string AppPack = "FFXIVAPP.Client.";

        public static readonly string[] Supported = {
            "ja",
            "fr",
            "en",
            "de",
            "zh",
            "ru",
            "ko"
        };

        public static StringComparison CultureComparer = StringComparison.CurrentCultureIgnoreCase;

        public static StringComparison InvariantComparer = StringComparison.InvariantCultureIgnoreCase;

        private static Dictionary<string, string> _autoTranslate;

        private static string _characterName;

        private static Dictionary<string, string> _chatCodes;

        private static ObservableCollection<object> _colors;

        private static CultureInfo _cultureInfo;

        private static bool _enableHelpLabels;

        private static bool _enableNetworkReading;

        private static bool _enableNLog;

        private static string _gameLanguage;

        private static string _serverName;

        private static string _theme;

        private static string _uiScale;

        private static XDocument _xAutoTranslate;

        private static XDocument _xChatCodes;

        private static XDocument _xColors;

        private static XDocument _xSettings;

        public static Dictionary<string, string> AutoTranslate {
            get {
                return _autoTranslate ?? (_autoTranslate = new Dictionary<string, string>());
            }

            set {
                _autoTranslate = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static string CharacterName {
            get {
                return _characterName;
            }

            set {
                _characterName = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static Dictionary<string, string> ChatCodes {
            get {
                return _chatCodes ?? (_chatCodes = new Dictionary<string, string>());
            }

            set {
                _chatCodes = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static string ChatCodesXML {
            get {
                return XChatCodes.ToString();
            }
        }

        public static IEnumerable<KeyValuePair<string, string[]>> ColorsToKeyValue => Colors.Cast<KeyValuePair<string, string[]>>();
        public static Dictionary<string, string[]> ColorsToDictionary => ColorsToKeyValue.ToDictionary(k => k.Key, k => k.Value);

        public static ObservableCollection<object> Colors {
            get {
                return _colors ?? (_colors = new ObservableCollection<object>());
            }

            set {
                _colors = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static CultureInfo CultureInfo {
            get {
                return _cultureInfo ?? (_cultureInfo = new CultureInfo("en"));
            }

            set {
                _cultureInfo = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static bool EnableHelpLabels {
            get {
                return _enableHelpLabels;
            }

            set {
                _enableHelpLabels = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static bool EnableNetworkReading {
            get {
                return _enableNetworkReading;
            }

            set {
                _enableNetworkReading = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static bool EnableNLog {
            get {
                return _enableNLog;
            }

            set {
                _enableNLog = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static string GameLanguage {
            get {
                return _gameLanguage;
            }

            set {
                _gameLanguage = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static bool IsOpen { get; set; }

        public static IntPtr ProcessHandle { get; set; }

        public static ProcessModel ProcessModel { get; set; }

        public static List<ProcessModel> ProcessModels { get; set; }

        public static string ServerName {
            get {
                return _serverName;
            }

            set {
                _serverName = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static string Theme {
            get {
                return _theme;
            }

            set {
                _theme = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static string UIScale {
            get {
                return _uiScale;
            }

            set {
                _uiScale = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static XDocument XAutoTranslate {
            get {
                var file = Path.Combine(Common.Constants.CachePath, "Configurations", "AutoTranslate.xml");
                if (_xAutoTranslate != null) {
                    return _xAutoTranslate;
                }

                try {
                    var found = File.Exists(file);
                    _xAutoTranslate = found
                                          ? XDocument.Load(file)
                                          : ResourceHelper.XDocResource(AppPack + "Defaults.AutoTranslate.xml");
                }
                catch (Exception) {
                    _xAutoTranslate = ResourceHelper.XDocResource(AppPack + "Defaults.AutoTranslate.xml");
                }

                return _xAutoTranslate;
            }

            set {
                _xAutoTranslate = value;
            }
        }

        public static XDocument XChatCodes {
            get {
                var file = Path.Combine(Common.Constants.CachePath, "Configurations", "ChatCodes.xml");
                if (_xChatCodes != null) {
                    return _xChatCodes;
                }

                try {
                    var found = File.Exists(file);
                    _xChatCodes = found
                                      ? XDocument.Load(file)
                                      : ResourceHelper.XDocResource(AppPack + "Resources.ChatCodes.xml");
                }
                catch (Exception) {
                    _xChatCodes = ResourceHelper.XDocResource(AppPack + "Resources.ChatCodes.xml");
                }

                return _xChatCodes;
            }

            set {
                _xChatCodes = value;
            }
        }

        public static XDocument XColors {
            get {
                var file = Path.Combine(Common.Constants.CachePath, "Configurations", "Colors.xml");
                if (_xColors != null) {
                    return _xColors;
                }

                try {
                    var found = File.Exists(file);
                    _xColors = found
                                   ? XDocument.Load(file)
                                   : ResourceHelper.XDocResource(AppPack + "Defaults.Colors.xml");
                }
                catch (Exception) {
                    _xColors = ResourceHelper.XDocResource(AppPack + "Defaults.Colors.xml");
                }

                return _xColors;
            }

            set {
                _xColors = value;
            }
        }

        /* TODO: Time to get rid of xml settings, or?
        public static XDocument XSettings {
            get {
                var settingsFile = Path.Combine(AppViewModel.Instance.SettingsPath, "ApplicationSettings.xml");
                if (_xSettings != null) {
                    return _xSettings;
                }

                try {
                    var found = File.Exists(settingsFile);
                    _xSettings = found
                                     ? XDocument.Load(settingsFile)
                                     : ResourceHelper.XDocResource(AppPack + "Defaults.Settings.xml");
                }
                catch (Exception) {
                    _xSettings = ResourceHelper.XDocResource(AppPack + "Defaults.Settings.xml");
                }

                return _xSettings;
            }

            set {
                _xSettings = value;
            }
        }
        */
    }
}