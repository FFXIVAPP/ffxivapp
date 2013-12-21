// FFXIVAPP.Client
// Constants.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Common.Helpers;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client
{
    [DoNotObfuscate]
    public static partial class Constants
    {
        #region Declarations

        public static readonly string[] Supported =
        {
            "ja", "fr", "en", "de"
        };

        public static StringComparison InvariantComparer = StringComparison.InvariantCultureIgnoreCase;

        #endregion

        #region Property Bindings

        private static XDocument _xAutoTranslate;
        private static XDocument _xChatCodes;
        private static XDocument _xColors;

        public static XDocument XAutoTranslate
        {
            get
            {
                if (_xAutoTranslate == null)
                {
                    var file = Path.Combine(Common.Constants.CachePath, "./Configurations/AutoTranslate.xml");
                    var found = File.Exists(file);
                    _xAutoTranslate = found ? XDocument.Load(file) : ResourceHelper.XDocResource(Common.Constants.AppPack + "Defaults/AutoTranslate.xml");
                }
                return _xAutoTranslate;
            }
            set { _xAutoTranslate = value; }
        }

        public static XDocument XChatCodes
        {
            get
            {
                if (_xChatCodes == null)
                {
                    var file = Path.Combine(Common.Constants.CachePath, "./Configurations/ChatCodes.xml");
                    var found = File.Exists(file);
                    _xChatCodes = found ? XDocument.Load(file) : ResourceHelper.XDocResource(Common.Constants.AppPack + "Resources/ChatCodes.xml");
                }
                return _xChatCodes;
            }
            set { _xChatCodes = value; }
        }

        public static XDocument XColors
        {
            get
            {
                if (_xColors == null)
                {
                    var file = Path.Combine(Common.Constants.CachePath, "./Configurations/Colors.xml");
                    var found = File.Exists(file);
                    _xColors = found ? XDocument.Load(file) : ResourceHelper.XDocResource(Common.Constants.AppPack + "Defaults/Colors.xml");
                }
                return _xColors;
            }
            set { _xColors = value; }
        }

        #endregion

        #region Property Bindings

        private static Dictionary<string, string> _autoTranslate;
        private static Dictionary<string, string> _chatCodes;
        private static Dictionary<string, string[]> _colors;
        private static CultureInfo _cultureInfo;
        private static string _chatCodesXml;
        private static string _characterName;
        private static string _serverName;
        private static string _gameLanguage;
        private static bool _enableNLog;
        private static bool _enableHelpLabels;

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

        public static bool EnableHelpLabels
        {
            get { return _enableHelpLabels; }
            set
            {
                _enableHelpLabels = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        #endregion

        #region Auto-Properties

        public static IntPtr ProcessHandle { get; set; }

        public static int ProcessID { get; set; }

        public static bool IsOpen { get; set; }

        public static Process[] ProcessIDs { get; set; }

        #endregion
    }
}
