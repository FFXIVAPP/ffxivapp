// FFXIVAPP.Client
// SettingsHelper.Log.cs
// 
// © 2013 Ryan Wilson

using FFXIVAPP.Client.SettingsProviders.Log;

namespace FFXIVAPP.Client.Helpers
{
    internal static partial class SettingsHelper
    {
        public static class Log
        {
            public static void Save()
            {
                Settings.Default.Save();
            }
        }
    }
}
