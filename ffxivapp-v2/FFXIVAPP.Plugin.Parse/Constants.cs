// FFXIVAPP.Plugin.Parse
// Constants.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using FFXIVAPP.Common.Helpers;

#endregion

namespace FFXIVAPP.Plugin.Parse
{
    public static class Constants
    {
        public const string BaseDirectory = "./Plugins/FFXIVAPP.Plugin.Parse/";

        public static readonly string[] Abilities = new[]
        {
            "142B", "14AB", "152B", "15AB", "162B", "16AB", "172B", "17AB", "182B", "18AB", "192B", "19AB", "1A2B", "1AAB", "1B2B", "1BAB"
        };

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
        private static List<string> _settings;

        public static XDocument XSettings
        {
            get
            {
                const string file = "./Plugins/FFXIVAPP.Plugin.Parse/Settings.xml";
                if (_xSettings == null)
                {
                    var found = File.Exists(file);
                    _xSettings = found ? XDocument.Load(file) : ResourceHelper.XDocResource(Common.Constants.ParsePack + "/Defaults/Settings.xml");
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
    }
}
