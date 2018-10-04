// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UILanguage.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   UILanguage.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Models {
    using System.Globalization;

    public class UILanguage {
        public CultureInfo CultureInfo { get; set; }

        public string ImageURI { get; set; }

        public string Language { get; set; }

        public string Title { get; set; }
    }
}