// FFXIVAPP.Client
// UpdateUtilities.cs
// 
// Copyright © 2007 - 2014 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

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
