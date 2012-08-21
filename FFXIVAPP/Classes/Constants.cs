// FFXIVAPP
// Constants.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace FFXIVAPP.Classes
{
    public static class Constants
    {
        public static string DLLVersion = "";
        public static IntPtr PHandle;
        public static int PID;
        public static int LogErrors;
        public static Boolean FFXIVOpen;
        public static readonly Dictionary<string, string> XAtCodes = new Dictionary<string, string>();
        public static readonly Dictionary<string, string> XChatCodes = new Dictionary<string, string>();
        public static readonly Dictionary<string, string[]> XColor = new Dictionary<string, string[]>();
        public static readonly Dictionary<string, string> XServerName = new Dictionary<string, string>();
        public static readonly Dictionary<string, string[]> XTab = new Dictionary<string, string[]>();
        public static readonly Dictionary<string, Regex> XPlayerRegEx = new Dictionary<string, Regex>();
        public static readonly Dictionary<string, Regex> XMonsterRegEx = new Dictionary<string, Regex>();
        public static Process[] FFXIVPID;
        public static readonly string[] Settings = {"Tab", "Color"};
        public static readonly string[] CmSay = {"0001"};
        public static readonly string[] CmTell = {"000D", "0003"};
        public static readonly string[] CmParty = {"0004"};
        public static readonly string[] CmShout = {"0002"};
        public static readonly string[] Cmls = {"000E", "0005", "000F", "0006", "0010", "0007", "0011", "0008", "0012", "0009", "0013", "000A", "0014", "000B", "0015", "000C"};

        public static bool IsValidRegex(string pattern)
        {
            var result = true;
            if (String.IsNullOrWhiteSpace(pattern))
            {
                return false;
            }
            try
            {
                result = Regex.IsMatch("", pattern);
            }
            catch (ArgumentException)
            {
                return result;
            }
            return true;
        }
    }
}