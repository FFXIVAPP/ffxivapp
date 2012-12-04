// FFXIVAPP.Plugin.Sample
// Constants.cs

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using FFXIVAPP.Common.Helpers;

namespace FFXIVAPP.Plugin.Sample
{
    public static class Constants
    {
        public const string BaseDirectory = "./Plugins/FFXIVAPP.Plugin.Sample/";

        #region Property Bindings

        private static XDocument _xSettings;
        private static List<string> _settings;

        public static XDocument XSettings
        {
            get
            {
                const string file = "./Plugins/FFXIVAPP.Plugin.Sample/Settings.xml";
                if (_xSettings == null)
                {
                    var found = File.Exists(file);
                    _xSettings = found ? XDocument.Load(file) : ResourceHelper.XDocResource(Common.Constants.ChatPack + "/Defaults/Settings.xml");
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
    }
}