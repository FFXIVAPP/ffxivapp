// FFXIVAPP.Plugin.Parse
// Constants.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.RegularExpressions;

#endregion

namespace FFXIVAPP.Plugin.Parse
{
    public static class Constants
    {
        #region Declarations

        public const string BaseDirectory = "./Plugins/FFXIVAPP.Plugin.Parse/";

        public const string LibraryPack = "pack://application:,,,/FFXIVAPP.Plugin.Parse;component/";

        public static readonly string[] Supported = new[]
        {
            "ja", "fr", "en"
        };

        public static readonly string[] Abilities = new[]
        {
            "142B", "14AB", "152B", "15AB", "162B", "16AB", "172B", "17AB", "182B", "18AB", "192B", "19AB", "1A2B", "1AAB", "1B2B", "1BAB"
        };

        #endregion

        #region Assembly Property Bindings

        internal static string PluginName
        {
            get
            {
                var att = Assembly.GetCallingAssembly()
                                  .GetCustomAttributes(typeof (AssemblyTitleAttribute), false);
                return att.Length == 0 ? "" : ((AssemblyTitleAttribute) att[0]).Title;
            }
        }

        internal static string PluginDescription
        {
            get
            {
                var att = Assembly.GetCallingAssembly()
                                  .GetCustomAttributes(typeof (AssemblyDescriptionAttribute), false);
                return att.Length == 0 ? "" : ((AssemblyDescriptionAttribute) att[0]).Description;
            }
        }

        public static string PluginCopyright
        {
            get
            {
                var att = Assembly.GetCallingAssembly()
                                  .GetCustomAttributes(typeof (AssemblyCopyrightAttribute), false);
                return att.Length == 0 ? "" : ((AssemblyCopyrightAttribute) att[0]).Copyright;
            }
        }

        internal static Version PluginVersion
        {
            get
            {
                return Assembly.GetCallingAssembly()
                               .GetName()
                               .Version;
            }
        }

        #endregion

        #region Property Bindings

        private static XDocument _xSettings;
        private static XDocument _xRegEx;
        private static List<string> _settings;

        public static XDocument XSettings
        {
            get
            {
                const string file = "./Plugins/FFXIVAPP.Plugin.Parse/Settings.xml";
                if (_xSettings == null)
                {
                    var found = File.Exists(file);
                    _xSettings = found ? XDocument.Load(file) : ResourceHelper.XDocResource(LibraryPack + "/Defaults/Settings.xml");
                }
                return _xSettings;
            }
            set { _xSettings = value; }
        }

        public static XDocument XRegEx
        {
            get
            {
                const string file = "./Plugins/FFXIVAPP.Plugin.Parse/RegularExpressions.xml";
                if (_xRegEx == null)
                {
                    var found = File.Exists(file);
                    _xRegEx = found ? XDocument.Load(file) : ResourceHelper.XDocResource(LibraryPack + "/Defaults/RegularExpressions.xml");
                }
                return _xRegEx;
            }
            set { _xRegEx = value; }
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

        #endregion
    }
}
