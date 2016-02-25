// FFXIVAPP.Client ~ Constants.cs
// 
// Copyright © 2007 - 2016 Ryan Wilson - All Rights Reserved
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
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Common.Core.Constant;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Memory.Models;

namespace FFXIVAPP.Client
{
    public static class Constants
    {
        #region Declarations

        public static readonly string[] Supported =
        {
            "ja", "fr", "en", "de", "zh", "ru", "ko"
        };

        public static StringComparison InvariantComparer = StringComparison.InvariantCultureIgnoreCase;
        public static StringComparison CultureComparer = StringComparison.CurrentCultureIgnoreCase;

        #endregion

        #region Property Bindings

        private static XDocument _xSettings;
        private static List<string> _settings;
        private static XDocument _xActions;
        private static XDocument _xAutoTranslate;
        private static XDocument _xChatCodes;
        private static XDocument _xColors;
        private static Dictionary<string, ActionInfo> _actions;
        private static Dictionary<string, string> _autoTranslate;
        private static Dictionary<string, string> _chatCodes;
        private static Dictionary<string, string[]> _colors;
        private static CultureInfo _cultureInfo;
        private static string _chatCodesXml;
        private static string _characterName;
        private static string _serverName;
        private static string _gameLanguage;
        private static bool _enableNLog;
        private static bool _enableNetworkReading;
        private static bool _enableHelpLabels;
        private static string _theme;
        private static string _uiScale;

        public static XDocument XSettings
        {
            get
            {
                var settingsFile = Path.Combine(AppViewModel.Instance.SettingsPath, "ApplicationSettings.xml");
                if (_xSettings != null)
                {
                    return _xSettings;
                }
                try
                {
                    var found = File.Exists(settingsFile);
                    _xSettings = found ? XDocument.Load(settingsFile) : ResourceHelper.XDocResource(Common.Constants.AppPack + "/Defaults/Settings.xml");
                }
                catch (Exception ex)
                {
                    _xSettings = ResourceHelper.XDocResource(Common.Constants.AppPack + "/Defaults/Settings.xml");
                }
                return _xSettings;
            }
            set { _xSettings = value; }
        }

        public static List<string> Settings
        {
            get { return _settings ?? (_settings = new List<string>()); }
            set { _settings = value; }
        }

        public static XDocument XActions
        {
            get
            {
                var file = Path.Combine(Common.Constants.CachePath, "Configurations", "Actions.xml");
                if (_xActions != null)
                {
                    return _xActions;
                }
                try
                {
                    var found = File.Exists(file);
                    _xActions = found ? XDocument.Load(file) : ResourceHelper.XDocResource(Common.Constants.AppPack + "Resources/Actions.xml");
                }
                catch (Exception ex)
                {
                    _xActions = ResourceHelper.XDocResource(Common.Constants.AppPack + "Resources/Actions.xml");
                }
                return _xActions;
            }
            set { _xActions = value; }
        }

        public static XDocument XAutoTranslate
        {
            get
            {
                var file = Path.Combine(Common.Constants.CachePath, "Configurations", "AutoTranslate.xml");
                if (_xAutoTranslate != null)
                {
                    return _xAutoTranslate;
                }
                try
                {
                    var found = File.Exists(file);
                    _xAutoTranslate = found ? XDocument.Load(file) : ResourceHelper.XDocResource(Common.Constants.AppPack + "Defaults/AutoTranslate.xml");
                }
                catch (Exception ex)
                {
                    _xAutoTranslate = ResourceHelper.XDocResource(Common.Constants.AppPack + "Defaults/AutoTranslate.xml");
                }
                return _xAutoTranslate;
            }
            set { _xAutoTranslate = value; }
        }

        public static XDocument XChatCodes
        {
            get
            {
                var file = Path.Combine(Common.Constants.CachePath, "Configurations", "ChatCodes.xml");
                if (_xChatCodes != null)
                {
                    return _xChatCodes;
                }
                try
                {
                    var found = File.Exists(file);
                    _xChatCodes = found ? XDocument.Load(file) : ResourceHelper.XDocResource(Common.Constants.AppPack + "Resources/ChatCodes.xml");
                }
                catch (Exception ex)
                {
                    _xChatCodes = ResourceHelper.XDocResource(Common.Constants.AppPack + "Resources/ChatCodes.xml");
                }
                return _xChatCodes;
            }
            set { _xChatCodes = value; }
        }

        public static XDocument XColors
        {
            get
            {
                var file = Path.Combine(Common.Constants.CachePath, "Configurations", "Colors.xml");
                if (_xColors != null)
                {
                    return _xColors;
                }
                try
                {
                    var found = File.Exists(file);
                    _xColors = found ? XDocument.Load(file) : ResourceHelper.XDocResource(Common.Constants.AppPack + "Defaults/Colors.xml");
                }
                catch (Exception ex)
                {
                    _xColors = ResourceHelper.XDocResource(Common.Constants.AppPack + "Defaults/Colors.xml");
                }
                return _xColors;
            }
            set { _xColors = value; }
        }

        public static Dictionary<string, ActionInfo> Actions
        {
            get { return _actions ?? (_actions = new Dictionary<string, ActionInfo>()); }
            set
            {
                _actions = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static Dictionary<string, string> AutoTranslate
        {
            get { return _autoTranslate ?? (_autoTranslate = new Dictionary<string, string>()); }
            set
            {
                _autoTranslate = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static Dictionary<string, string> ChatCodes
        {
            get { return _chatCodes ?? (_chatCodes = new Dictionary<string, string>()); }
            set
            {
                _chatCodes = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static string ChatCodesXml
        {
            get { return XChatCodes.ToString(); }
        }

        public static Dictionary<string, string[]> Colors
        {
            get { return _colors ?? (_colors = new Dictionary<string, string[]>()); }
            set
            {
                _colors = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static CultureInfo CultureInfo
        {
            get { return _cultureInfo ?? (_cultureInfo = new CultureInfo("en")); }
            set
            {
                _cultureInfo = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static string CharacterName
        {
            get { return _characterName; }
            set
            {
                _characterName = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static string ServerName
        {
            get { return _serverName; }
            set
            {
                _serverName = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static string GameLanguage
        {
            get { return _gameLanguage; }
            set
            {
                _gameLanguage = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static bool EnableNLog
        {
            get { return _enableNLog; }
            set
            {
                _enableNLog = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static bool EnableNetworkReading
        {
            get { return _enableNetworkReading; }
            set
            {
                _enableNetworkReading = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static bool EnableHelpLabels
        {
            get { return _enableHelpLabels; }
            set
            {
                _enableHelpLabels = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static string Theme
        {
            get { return _theme; }
            set
            {
                _theme = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static string UIScale
        {
            get { return _uiScale; }
            set
            {
                _uiScale = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        #endregion

        #region Auto-Properties

        public static IntPtr ProcessHandle { get; set; }

        public static ProcessModel ProcessModel { get; set; }

        public static bool IsOpen { get; set; }

        public static List<ProcessModel> ProcessModels { get; set; }

        #endregion
    }
}
