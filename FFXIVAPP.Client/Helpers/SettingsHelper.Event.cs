// FFXIVAPP.Client
// SettingsHelper.Event.cs
// 
// © 2013 Ryan Wilson

#region Usings

using FFXIVAPP.Client.SettingsProviders.Event;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Helpers
{
    internal static partial class SettingsHelper
    {
        [DoNotObfuscate]
        public static class Event
        {
            public static void Save()
            {
                Settings.Default.Save();
            }
        }
    }
}
