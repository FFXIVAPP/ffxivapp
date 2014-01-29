// FFXIVAPP.Plugin.Radar
// Constants.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using FFXIVAPP.Common.Helpers;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Plugin.Radar
{
    [DoNotObfuscate]
    public static class Constants
    {
        #region Declarations

        public const string LibraryPack = "pack://application:,,,/FFXIVAPP.Plugin.Radar;component/";

        public static readonly string[] Supported = new[]
        {
            "en"
        };

        public static string BaseDirectory
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly()
                                                     .Location);
            }
        }

        #endregion

        #region Property Bindings

        private static XDocument _xSettings;
        private static List<string> _settings;

        public static XDocument XSettings
        {
            get
            {
                var file = Path.Combine(Common.Constants.PluginsSettingsPath, "FFXIVAPP.Plugin.Radar.xml");
                var legacyFile = "./Plugins/FFXIVAPP.Plugin.Radar/Settings.xml";
                if (_xSettings != null)
                {
                    return _xSettings;
                }
                try
                {
                    var found = File.Exists(file);
                    if (found)
                    {
                        _xSettings = XDocument.Load(file);
                    }
                    else
                    {
                        found = File.Exists(legacyFile);
                        _xSettings = found ? XDocument.Load(legacyFile) : ResourceHelper.XDocResource(LibraryPack + "/Defaults/Settings.xml");
                    }
                }
                catch (Exception ex)
                {
                    _xSettings = ResourceHelper.XDocResource(LibraryPack + "/Defaults/Settings.xml");
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

        #endregion

        #region Property Bindings

        private static Dictionary<string, string> _autoTranslate;
        private static Dictionary<string, string> _chatCodes;
        private static Dictionary<string, string[]> _colors;
        private static CultureInfo _cultureInfo;

        public static Dictionary<string, string> AutoTranslate
        {
            get { return _autoTranslate ?? (_autoTranslate = new Dictionary<string, string>()); }
            set { _autoTranslate = value; }
        }

        public static Dictionary<string, string> ChatCodes
        {
            get { return _chatCodes ?? (_chatCodes = new Dictionary<string, string>()); }
            set { _chatCodes = value; }
        }

        public static string ChatCodesXml { get; set; }

        public static Dictionary<string, string[]> Colors
        {
            get { return _colors ?? (_colors = new Dictionary<string, string[]>()); }
            set { _colors = value; }
        }

        public static CultureInfo CultureInfo
        {
            get { return _cultureInfo ?? (_cultureInfo = new CultureInfo("en")); }
            set { _cultureInfo = value; }
        }

        #endregion

        #region Auto-Properties

        public static string CharacterName { get; set; }

        public static string ServerName { get; set; }

        public static string GameLanguage { get; set; }

        public static bool EnableHelpLabels { get; set; }

        public static string Theme { get; set; }

        #endregion
    }
}
