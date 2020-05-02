// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppException.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2020 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   AppException.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client {
    using System;
    using System.Runtime.Serialization;

    [Serializable,]
    internal class AppException : Exception {
        public AppException() { }

        public AppException(string message) : base(message) { }

        public AppException(string message, Exception inner) : base(message, inner) { }

        protected AppException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}