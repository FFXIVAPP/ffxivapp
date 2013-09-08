// FFXIVAPP.Plugin.Log
// Constants.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using FFXIVAPP.Common.Helpers;

#endregion

namespace FFXIVAPP.Plugin.Log
{
    public static class Constants
    {
        #region Declarations

        public const string BaseDirectory = "./Plugins/FFXIVAPP.Plugin.Log/";

        public const string LibraryPack = "pack://application:,,,/FFXIVAPP.Plugin.Log;component/";

        public static readonly string[] Supported = new[]
        {
            "ja", "fr", "en", "de"
        };

        public static readonly string[] ChatPublic =
        {
            "000A", "000C", "000D", "000E"
        };

        public static readonly string[] ChatSay =
        {
            "000A"
        };

        public static readonly string[] ChatTell =
        {
            "000C", "000D"
        };

        public static readonly string[] ChatParty =
        {
            "000E"
        };

        public static readonly string[] ChatShout =
        {
            "000B"
        };

        public static readonly string[] ChatYell =
        {
            "001E"
        };

        public static readonly string[] ChatLS =
        {
            "0010", "0011", "0012", "0013", "0014", "0015", "0016", "0017"
        };

        public static readonly string[] ChatFC =
        {
            "0018"
        };

        #endregion

        #region Property Bindings

        private static XDocument _xSettings;
        private static List<string> _settings;

        public static XDocument XSettings
        {
            get
            {
                const string file = "./Plugins/FFXIVAPP.Plugin.Log/Settings.xml";
                if (_xSettings == null)
                {
                    var found = File.Exists(file);
                    _xSettings = found ? XDocument.Load(file) : ResourceHelper.XDocResource(LibraryPack + "/Defaults/Settings.xml");
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

        public static Dictionary<string, string> Linkshells
        {
            get
            {
                var linkshells = new Dictionary<string, string>();
                linkshells.Add("0010", "[1] ");
                linkshells.Add("0011", "[2] ");
                linkshells.Add("0012", "[3] ");
                linkshells.Add("0013", "[4] ");
                linkshells.Add("0014", "[5] ");
                linkshells.Add("0015", "[6] ");
                linkshells.Add("0016", "[7] ");
                linkshells.Add("0017", "[8] ");
                linkshells.Add("0018", "[FC] ");
                return linkshells;
            }
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

        #endregion
    }
}
