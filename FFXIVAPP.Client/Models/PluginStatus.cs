// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PluginStatus.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   PluginStatus.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Models {
    internal enum PluginStatus {
        NotInstalled,

        Installed,

        UpdateAvailable,

        OutOfDate,
    }
}