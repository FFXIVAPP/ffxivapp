// FFXIVAPP.Client
// Constants.Application.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using FFXIVAPP.Client.SettingsProviders.Application;
using FFXIVAPP.Common.Helpers;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client
{
    public static partial class Constants
    {
        [DoNotObfuscate]
        public static class Application
        {
            public static Settings PluginSettings
            {
                get { return SettingsProviders.Application.Settings.Default; }
            }

            #region Declarations

            #endregion

            #region Property Bindings

            private static XDocument _xSettings;
            private static List<string> _settings;
            private static Dictionary<string, bool> _enabledPlugins;

            public static XDocument XSettings
            {
                get
                {
                    var file = AppViewModel.Instance.SettingsPath + "ApplicationSettings.xml";
                    if (_xSettings == null)
                    {
                        var found = File.Exists(file);
                        _xSettings = found ? XDocument.Load(file) : ResourceHelper.XDocResource(Common.Constants.AppPack + "/Defaults/Settings.xml");
                        foreach (var element in _xSettings.Elements("Plugin"))
                        {
                            var s = element;
                            var t = s;
                        }
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

            public static Dictionary<string, bool> EnabledPlugins
            {
                get { return _enabledPlugins ?? (_enabledPlugins = new Dictionary<string, bool>()); }
                set { _enabledPlugins = value; }
            }

            #endregion
        }
    }
}
