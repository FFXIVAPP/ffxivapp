// FFXIVAPP.Client
// Constants.Informer.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using FFXIVAPP.Client.SettingsProviders.Informer;
using FFXIVAPP.Common.Helpers;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client
{
    public static partial class Constants
    {
        [DoNotObfuscate]
        public static class Informer
        {
            public static Settings PluginSettings
            {
                get { return SettingsProviders.Informer.Settings.Default; }
            }

            #region Declarations

            #endregion

            #region Property Bindings

            private static XDocument _xSettings;
            private static XDocument _xRegEx;
            private static List<string> _settings;

            public static XDocument XSettings
            {
                get
                {
                    var file = AppViewModel.Instance.SettingsPath + "Settings.Informer.xml";
                    if (_xSettings == null)
                    {
                        var found = File.Exists(file);
                        _xSettings = found ? XDocument.Load(file) : ResourceHelper.XDocResource(Common.Constants.AppPack + "/Defaults/Settings.Informer.xml");
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
