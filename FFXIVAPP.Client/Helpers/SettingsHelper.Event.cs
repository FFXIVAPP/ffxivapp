// FFXIVAPP.Client
// SettingsHelper.Event.cs
// 
// © 2013 Ryan Wilson

using FFXIVAPP.Client.SettingsProviders.Event;

namespace FFXIVAPP.Client.Helpers {
    internal static partial class SettingsHelper {
        public static class Event {
            public static void Save() {
                Settings.Default.Save();
            }
        }
    }
}
