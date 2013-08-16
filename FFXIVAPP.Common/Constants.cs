// FFXIVAPP.Common
// Constants.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Reflection;
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

        public static string ServerName { get; set; }

        public static bool EnableNLog { get; set; }

        #endregion

        #region Assembly Property Bindings

        public static string Name
        {
            get
            {
                var att = Assembly.GetCallingAssembly()
                    .GetCustomAttributes(typeof (AssemblyTitleAttribute), false);
                return att.Length == 0 ? "" : ((AssemblyTitleAttribute) att[0]).Title;
            }
        }

        public static string Description
        {
            get
            {
                var att = Assembly.GetCallingAssembly()
                    .GetCustomAttributes(typeof (AssemblyDescriptionAttribute), false);
                return att.Length == 0 ? "" : ((AssemblyDescriptionAttribute) att[0]).Description;
            }
        }

        public static string Copyright
        {
            get
            {
                var att = Assembly.GetCallingAssembly()
                    .GetCustomAttributes(typeof (AssemblyCopyrightAttribute), false);
                return att.Length == 0 ? "" : ((AssemblyCopyrightAttribute) att[0]).Copyright;
            }
        }

        public static Version Version
        {
            get
            {
                return Assembly.GetCallingAssembly()
                    .GetName()
                    .Version;
            }
        }

        #endregion
    }
}
