// FFXIVAPP.Common
// Constants.cs
// 
// © 2013 Ryan Wilson

using System;
using System.IO;
using FFXIVAPP.Common.Helpers;

namespace FFXIVAPP.Common
{
    public static class Constants
    {
        public const string AppPack = "pack://application:,,,/FFXIVAPP.Client;component/";
        public const string DefaultIcon = AppPack + "Resources/Media/Images/DefaultIcon.jpg";
        public const string DefaultAvatar = AppPack + "Resources/Media/Images/DefaultAvatar.jpg";

        public static readonly FlowDocHelper FD = new FlowDocHelper();

        public static string CachePath
        {
            get
            {
                try
                {
                    var location = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    return Path.Combine(location, "FFXIVAPP");
                }
                catch
                {
                    return "./";
                }
            }
        }

        #region Directories

        public static string ConfigurationsPath
        {
            get
            {
                return Path.Combine(CachePath, "./Configurations/");
            }
        }

        public static string LogsPath
        {
            get
            {
                return Path.Combine(CachePath, "./Logs/");
            }
        }

        public static string ScreenShotsPath
        {
            get
            {
                return Path.Combine(CachePath, "./ScreenShots/");
            }
        }

        public static string SoundsPath
        {
            get
            {
                return Path.Combine(CachePath, "./Sounds/");
            }
        }

        public static string SettingsPath
        {
            get
            {
                return Path.Combine(CachePath, "./Settings/");
            }
        }

        public static string PluginsSettingsPath
        {
            get
            {
                return Path.Combine(CachePath, "./Settings/Plugins/");
            }
        }

        #endregion

        #region Auto-Properties

        public static bool EnableNLog { get; set; }

        #endregion
    }
}
