// FFXIVAPP.Client
// SettingsHelper.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.IO;

#endregion



namespace FFXIVAPP.Client.Helpers
{
    internal static partial class SettingsHelper
    {
        /// <summary>
        /// </summary>
        public static void Save()
        {
            Client.Save();
            SettingsProviders.Event.Settings.Default.Save();
            SettingsProviders.Log.Settings.Default.Save();
        }

        /// <summary>
        /// </summary>
        public static void Default(string plugin = "")
        {
            switch (plugin)
            {
                case "event":
                    break;
                default:
                    Client.Default();
                    break;
            }
        }
    }
}
