// FFXIVAPP.Client
// SettingsHelper.Log.cs
// 
// © 2013 Ryan Wilson

#region Usings

using FFXIVAPP.Client.SettingsProviders.Log;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Helpers
{
    internal static partial class SettingsHelper
    {
        [DoNotObfuscate]
        public static class Log
        {
            public static void Save()
            {
                Settings.Default.Save();
            }
        }
    }
}
