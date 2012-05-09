// AppModXIV
// Hashing.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;

namespace AppModXIV.Security
{
    public class Hashing
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="un"></param>
        /// <returns></returns>
        public string CheckHash(string un)
        {
            return GetHash(GetRegistered(), un);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="salt"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        private static string GetHash(string salt, string userName)
        {
            var hashTool = new SHA512Managed();
            var keyAsByte = Encoding.UTF8.GetBytes(salt + "fish'n'chips" + userName);
            var encryptedBytes = hashTool.ComputeHash(keyAsByte);
            hashTool.Clear();
            return Convert.ToBase64String(encryptedBytes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static string GetRegistered()
        {
            var tempkey = string.Empty;
            var registry = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            registry = registry.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            if (registry != null)
            {
                try
                {
                    tempkey = registry.GetValue("ProductId").ToString();
                }
                catch
                {
                    registry.Close();
                }
            }
            registry = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            registry = registry.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            if (registry != null)
            {
                try
                {
                    tempkey = registry.GetValue("ProductId").ToString();
                }
                catch
                {
                    registry.Close();
                }
            }
            return tempkey != "" ? tempkey : "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetAdapter()
        {
            var final = "";
            var nics = NetworkInterface.GetAllNetworkInterfaces();
            if (nics.Length > 0)
            {
                var adapter = nics[0];
                var address = adapter.GetPhysicalAddress();
                var bytes = address.GetAddressBytes();
                for (var i = 0; i < bytes.Length; i++)
                {
                    final = final + bytes[i].ToString("X2");
                    if (i != bytes.Length - 1)
                    {
                        final = final + "-";
                    }
                }
            }
            return final;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string CalculateMd5Hash(string input)
        {
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}