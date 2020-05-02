// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandBuilder.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2020 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   CommandBuilder.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Utilities {
    using System.Text.RegularExpressions;

    using FFXIVAPP.Common.RegularExpressions;

    internal static class CommandBuilder {
        public static readonly Regex CommandsRegEx = new Regex(@"^com:(?<plugin>\w+) (?<command>\w+)$", SharedRegEx.DefaultOptions);
    }
}