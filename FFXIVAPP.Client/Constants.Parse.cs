// FFXIVAPP.Client
// Constants.Parse.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using FFXIVAPP.Common.Helpers;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client
{
    public static partial class Constants
    {
        [DoNotObfuscate]
        public static class Parse
        {
            #region Declarations

            public static readonly List<string> Abilities = new List<string>
            {
                "142B",
                "14AB",
                "152B",
                "15AB",
                "162B",
                "16AB",
                "172B",
                "17AB",
                "182B",
                "18AB",
                "192B",
                "19AB",
                "1A2B",
                "1AAB",
                "1B2B",
                "1BAB"
            };

            public static readonly List<string> NeedGreed = new List<string>
            {
                "rolls Need",
                "rolls Greed",
                "dés Besoin",
                "dés Cupidité"
            };

            #endregion

            #region Property Bindings

            private static XDocument _xSettings;
            private static XDocument _xRegEx;
            private static List<string> _settings;

            public static XDocument XSettings
            {
                get
                {
                    var file = AppViewModel.Instance.SettingsPath + "Settings.Parse.xml";
                    if (_xSettings == null)
                    {
                        var found = File.Exists(file);
                        _xSettings = found ? XDocument.Load(file) : ResourceHelper.XDocResource(Common.Constants.AppPack + "/Defaults/Settings.Parse.xml");
                    }
                    return _xSettings;
                }
                set { _xSettings = value; }
            }

            public static XDocument XRegEx
            {
                get
                {
                    var file = AppViewModel.Instance.ConfigurationsPath + "RegularExpressions.xml";
                    if (_xRegEx == null)
                    {
                        var found = File.Exists(file);
                        _xRegEx = found ? XDocument.Load(file) : ResourceHelper.XDocResource(Common.Constants.AppPack + "/Defaults/RegularExpressions.xml");
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
        }
    }
}
