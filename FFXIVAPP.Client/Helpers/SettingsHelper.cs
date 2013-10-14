// FFXIVAPP.Client
// SettingsHelper.cs
// 
// © 2013 Ryan Wilson

#region Usings

#endregion

namespace FFXIVAPP.Client.Helpers
{
    internal static partial class SettingsHelper
    {
        /// <summary>
        /// </summary>
        public static void Save(bool isUpdating)
        {
            if (isUpdating)
            {
            }
            Client.Save();
            Event.Save();
            Log.Save();
            Parse.Save();
        }

        /// <summary>
        /// </summary>
        public static void Default()
        {
            Client.Default();
        }
    }
}
