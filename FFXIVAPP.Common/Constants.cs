// FFXIVAPP.Common
// Constants.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Text.RegularExpressions;
using FFXIVAPP.Common.Helpers;

#endregion

namespace FFXIVAPP.Common
{
    public static class Constants
    {
        public const string AppPack = "pack://application:,,,/FFXIVAPP.Client;component/";
        public const string DefaultIcon = AppPack + "Resources/Media/Images/DefaultIcon.jpg";
        public const string DefaultAvatar = AppPack + "Resources/Media/Images/DefaultAvatar.jpg";

        public static readonly FlowDocHelper FD = new FlowDocHelper();

        public static bool IsValidRegex(string pattern)
        {
            if (String.IsNullOrWhiteSpace(pattern))
            {
                return false;
            }
            try
            {
                var regex = new Regex(pattern);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        #region Auto-Properties

        public static string CharacterName { get; set; }

        public static string ServerName { get; set; }

        public static string GameLanguage { get; set; }

        public static bool EnableNLog { get; set; }

        #endregion
    }
}
