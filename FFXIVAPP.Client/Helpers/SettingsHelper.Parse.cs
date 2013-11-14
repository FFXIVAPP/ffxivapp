// FFXIVAPP.Client
// SettingsHelper.Parse.cs
// 
// © 2013 Ryan Wilson

#region Usings

using FFXIVAPP.Client.SettingsProviders.Parse;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Helpers
{
    internal static partial class SettingsHelper
    {
        [DoNotObfuscate]
        public static class Parse
        {
            public static void Save()
            {
                Settings.Default.Save();
            }
        }
    }
}
