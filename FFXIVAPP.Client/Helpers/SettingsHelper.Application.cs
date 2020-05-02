// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsHelper.Application.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2020 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   SettingsHelper.Application.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Helpers {
    using FFXIVAPP.Client.SettingsProviders.Application;

    internal static partial class SettingsHelper {
        public static class Application {
            /// <summary>
            /// </summary>
            public static void Save() {
                Settings.Default.Save();
            }
        }
    }
}