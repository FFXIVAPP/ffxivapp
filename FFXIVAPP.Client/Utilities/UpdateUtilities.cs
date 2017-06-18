// FFXIVAPP.Client ~ UpdateUtilities.cs
// 
// Copyright © 2007 - 2017 Ryan Wilson - All Rights Reserved
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FFXIVAPP.Client.Utilities
{
    public static class UpdateUtilities
    {
        /// <summary>
        ///     Compares a file against an expected checksum.
        /// </summary>
        /// <param name="filePath">Path to file</param>
        /// <param name="checksum">Expected MD5 checksum</param>
        /// <returns>True if the file matches. False if the file doesn't match OR doesn't exist.</returns>
        public static bool VerifyFile(string filePath, string checksum)
        {
            if (!File.Exists(filePath))
            {
                return false;
            }
            return GetFileHash(filePath)
                .Equals(checksum, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     Computes a file's checksum
        /// </summary>
        /// <param name="filePath">Path to file</param>
        /// <returns>MD5 checksum for the file, or an empty string if the file doesn't exist.</returns>
        public static string GetFileHash(string filePath)
        {
            var FileHash = "";

            if (!File.Exists(filePath))
            {
                return FileHash;
            }

            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (MD5 md5 = new MD5CryptoServiceProvider())
                {
                    var retVal = md5.ComputeHash(file);
                    file.Close();
                    var sb = new StringBuilder();
                    foreach (var t in retVal)
                    {
                        sb.Append(t.ToString("x2"));
                    }
                    FileHash = sb.ToString();
                }
            }
            return FileHash;
        }
    }
}
