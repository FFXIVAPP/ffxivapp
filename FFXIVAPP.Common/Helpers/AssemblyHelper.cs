// FFXIVAPP.Common
// AssemblyHelper.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace FFXIVAPP.Common.Helpers {
    public static class AssemblyHelper {
        #region Assembly Property Bindings

        public static string Name {
            get {
                var att = Assembly.GetCallingAssembly()
                                  .GetCustomAttributes(typeof (AssemblyTitleAttribute), false);
                return att.Length == 0 ? "" : ((AssemblyTitleAttribute) att[0]).Title;
            }
        }

        public static string Description {
            get {
                var att = Assembly.GetCallingAssembly()
                                  .GetCustomAttributes(typeof (AssemblyDescriptionAttribute), false);
                return att.Length == 0 ? "" : ((AssemblyDescriptionAttribute) att[0]).Description;
            }
        }

        public static string Copyright {
            get {
                var att = Assembly.GetCallingAssembly()
                                  .GetCustomAttributes(typeof (AssemblyCopyrightAttribute), false);
                return att.Length == 0 ? "" : ((AssemblyCopyrightAttribute) att[0]).Copyright;
            }
        }

        public static Version Version {
            get {
                return Assembly.GetCallingAssembly()
                               .GetName()
                               .Version;
            }
        }

        public static string Guid {
            get {
                var att = Assembly.GetCallingAssembly()
                                  .GetCustomAttributes(typeof (GuidAttribute), true);
                return att.Length == 0 ? "" : ((GuidAttribute) att[0]).Value;
            }
        }

        public static string Hash(string prefix, string salt, string suffix) {
            var ue = new UnicodeEncoding();
            var message = ue.GetBytes(prefix + salt + suffix);
            var hashString = new SHA512Managed();
            var hashValue = hashString.ComputeHash(message);
            return hashValue.Aggregate("", (current, x) => current + String.Format("{0:x2}", x));
        }

        #endregion
    }
}
