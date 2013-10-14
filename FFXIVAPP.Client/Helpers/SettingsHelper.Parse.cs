// FFXIVAPP.Client
// SettingsHelper.Parse.cs
// 
// © 2013 Ryan Wilson

using FFXIVAPP.Client.SettingsProviders.Parse;

namespace FFXIVAPP.Client.Helpers {
    internal static partial class SettingsHelper {
        public static class Parse {
            public static void Save() {
                Settings.Default.Save();
            }
        }
    }
}
