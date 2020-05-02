// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicroStopwatch.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2020 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   MicroStopwatch.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Utilities {
    using System;
    using System.Diagnostics;

    internal class MicroStopwatch : Stopwatch {
        private readonly double _microSecPerTick = 1000000D / Frequency;

        public MicroStopwatch() {
            if (!IsHighResolution) {
                throw new Exception("On this system the high-resolution performance counter is not available");
            }
        }

        public long ElapsedMicroseconds {
            get {
                return (long) (this.ElapsedTicks * this._microSecPerTick);
            }
        }
    }
}