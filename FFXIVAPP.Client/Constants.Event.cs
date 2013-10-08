// FFXIVAPP.Client
// Constants.Event.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using FFXIVAPP.Common.Helpers;

namespace FFXIVAPP.Client
{
    public static partial class Constants
    {
        public static class Event
        {
            private static XDocument _xSettings;
            private static List<string> _settings;

            public static XDocument XSettings
            {
                get
                {
                    var file = AppViewModel.Instance.SettingsPath + "Settings.Event.xml";
                    if (_xSettings == null)
                    {
                        var found = File.Exists(file);
                        _xSettings = found ? XDocument.Load(file) : ResourceHelper.XDocResource(Common.Constants.AppPack + "/Defaults/Settings.Event.xml");
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
        }
    }
}
