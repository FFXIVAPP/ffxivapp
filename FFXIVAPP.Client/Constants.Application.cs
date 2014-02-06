// FFXIVAPP.Client
// Constants.Application.cs
// 
// © 2013 Ryan Wilson

using System;
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

            #endregion
        }
    }
}
