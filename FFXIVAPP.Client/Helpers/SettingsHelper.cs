// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsHelper.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2020 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   SettingsHelper.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Helpers {
    using NLog;

    internal static partial class SettingsHelper {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// </summary>
        public static void Default() {
            Client.Default();
        }

        /// <summary>
        /// </summary>
        public static void Save(bool isUpdating) {
            if (isUpdating) { }

            Client.Save();
            Application.Save();
        }
    }
}