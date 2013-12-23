// FFXIVAPP.Client
// SettingsHelper.Application.cs
// 
// © 2013 Ryan Wilson

using FFXIVAPP.Client.SettingsProviders.Application;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Helpers
{
    internal static partial class SettingsHelper
    {
        [DoNotObfuscate]
        public static class Application
        {
            /// <summary>
            /// </summary>
            public static void Save()
            {
                Settings.Default.Save();
            }
        }
    }
}
