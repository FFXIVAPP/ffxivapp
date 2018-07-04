// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateUtilities.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   UpdateUtilities.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Utilities {
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    internal static class UpdateUtilities {
        /// <summary>
        ///     Computes a file's checksum
        /// </summary>
        /// <param name="filePath">Path to file</param>
        /// <returns>MD5 checksum for the file, or an empty string if the file doesn't exist.</returns>
        public static string GetFileHash(string filePath) {
            var FileHash = string.Empty;

            if (!File.Exists(filePath)) {
                return FileHash;
            }

            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read)) {
                using (MD5 md5 = new MD5CryptoServiceProvider()) {
                    byte[] retVal = md5.ComputeHash(file);
                    file.Close();
                    var sb = new StringBuilder();
                    foreach (var t in retVal) {
                        sb.Append(t.ToString("x2"));
                    }

                    FileHash = sb.ToString();
                }
            }

            return FileHash;
        }

        /// <summary>
        ///     Compares a file against an expected checksum.
        /// </summary>
        /// <param name="filePath">Path to file</param>
        /// <param name="checksum">Expected MD5 checksum</param>
        /// <returns>True if the file matches. False if the file doesn't match OR doesn't exist.</returns>
        public static bool VerifyFile(string filePath, string checksum) {
            if (!File.Exists(filePath)) {
                return false;
            }

            return GetFileHash(filePath).Equals(checksum, StringComparison.OrdinalIgnoreCase);
        }
    }
}