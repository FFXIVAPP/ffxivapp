// FFXIVAPP.Plugin.Log
// Constants.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using FFXIVAPP.Common.Helpers;

namespace FFXIVAPP.Plugin.Log
{
    public static class Constants
    {
        public const string BaseDirectory = "./Plugins/FFXIVAPP.Plugin.Log/";

        #region Assembly Property Bindings

        internal static string PluginName
        {
            get
            {
                var att = Assembly.GetCallingAssembly().GetCustomAttributes(typeof (AssemblyTitleAttribute), false);
                return att.Length == 0 ? "" : ((AssemblyTitleAttribute) att[0]).Title;
            }
        }

        internal static string PluginDescription
        {
            get
            {
                var att = Assembly.GetCallingAssembly().GetCustomAttributes(typeof (AssemblyDescriptionAttribute), false);
                return att.Length == 0 ? "" : ((AssemblyDescriptionAttribute) att[0]).Description;
            }
        }

        internal static string PluginCopyright
        {
            get
            {
                var att = Assembly.GetCallingAssembly().GetCustomAttributes(typeof (AssemblyCopyrightAttribute), false);
                return att.Length == 0 ? "" : ((AssemblyCopyrightAttribute) att[0]).Copyright;
            }
        }

        internal static Version PluginVersion
        {
            get { return Assembly.GetCallingAssembly().GetName().Version; }
        }

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
                    _xSettings = found ? XDocument.Load(file) : ResourceHelper.XDocResource(Common.Constants.LogPack + "/Defaults/Settings.xml");
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
                linkshells.Add("000E", "[1] ");
                linkshells.Add("0005", "[1] ");
                linkshells.Add("000F", "[2] ");
                linkshells.Add("0006", "[2] ");
                linkshells.Add("0010", "[3] ");
                linkshells.Add("0007", "[3] ");
                linkshells.Add("0011", "[4] ");
                linkshells.Add("0008", "[4] ");
                linkshells.Add("0012", "[5] ");
                linkshells.Add("0009", "[5] ");
                linkshells.Add("0013", "[6] ");
                linkshells.Add("000A", "[6] ");
                linkshells.Add("0014", "[7] ");
                linkshells.Add("000B", "[7] ");
                linkshells.Add("0015", "[8] ");
                linkshells.Add("000C", "[8] ");
                return linkshells;
            }
        }

        #endregion
    }
}