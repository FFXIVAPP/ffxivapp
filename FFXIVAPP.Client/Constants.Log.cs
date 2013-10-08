// FFXIVAPP.Client
// Constants.Log.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using FFXIVAPP.Client.SettingsProviders.Log;
using FFXIVAPP.Common.Helpers;

namespace FFXIVAPP.Client
{
    public static partial class Constants
    {
        public static class Log
        {
            public static Settings PluginSettings
            {
                get { return SettingsProviders.Log.Settings.Default; }
            }

            #region Declarations

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
                    var file = AppViewModel.Instance.SettingsPath + "Settings.Log.xml";
                    if (_xSettings == null)
                    {
                        var found = File.Exists(file);
                        _xSettings = found ? XDocument.Load(file) : ResourceHelper.XDocResource(Common.Constants.AppPack + "/Defaults/Settings.Log.xml");
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
        }
    }
}
