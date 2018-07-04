// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildNumber.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   BuildNumber.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Models {
    internal class BuildNumber {
        public BuildNumber() {
            this.Major = 0;
            this.Minor = 0;
            this.Build = 0;
            this.Revision = 0;
        }

        public int Build { get; set; }

        public int Major { get; set; }

        public int Minor { get; set; }

        public int Revision { get; set; }
    }
}