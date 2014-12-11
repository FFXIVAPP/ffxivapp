using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FFXIVAPP.Client.Utilities
{
    public static class UpdateUtilities
    {
        /// <summary>
        /// Compares a file against an expected checksum.
        /// </summary>
        /// <param name="FilePath">Path to file</param>
        /// <param name="Checksum">Expected MD5 checksum</param>
        /// <returns>True if the file matches. False if the file doesn't match OR doesn't exist.</returns>
        public static bool VerifyFile(string FilePath, string Checksum)
        {
            if (!File.Exists(FilePath))
            {
                return false;
            }

            return GetFileHash(FilePath).Equals(Checksum, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Computes a file's checksum
        /// </summary>
        /// <param name="FilePath">Path to file</param>
        /// <returns>MD5 checksum for the file, or an empty string if the file doesn't exist.</returns>
        public static string GetFileHash(string FilePath)
        {
            string FileHash = "";

            if (!File.Exists(FilePath))
            {
                return FileHash;
            }

            using (FileStream file = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {
                using (MD5 md5 = new MD5CryptoServiceProvider())
                {
                    byte[] retVal = md5.ComputeHash(file);
                    file.Close();

                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < retVal.Length; i++)
                    {
                        sb.Append(retVal[i].ToString("x2"));
                    }
                    FileHash = sb.ToString();
                }
            }

            return FileHash;
        }

    }
}
