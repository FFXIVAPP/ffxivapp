// FFXIVAPP
// Constants.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml.Linq;

#endregion

namespace FFXIVAPP.Classes
{
    public static class Constants
    {
        public static IntPtr PHandle;
        public static int PID;
        public static int LogErrors;
        public static Boolean FFXIVOpen;
        public static readonly Dictionary<string, string> XChatCodes = new Dictionary<string, string>();
        public static readonly Dictionary<string, string> XATCodes = new Dictionary<string, string>();
        public static readonly Dictionary<string, string[]> XColor = new Dictionary<string, string[]>();
        public static readonly Dictionary<string, string> XEvent = new Dictionary<string, string>();
        public static readonly Dictionary<string, string> XServerName = new Dictionary<string, string>();
        public static readonly Dictionary<string, string[]> XTab = new Dictionary<string, string[]>();
        public static readonly XDocument XEvents = XDocument.Load("./Settings/Events.xml");
        public static readonly XDocument XColors = XDocument.Load("./Settings/Colors.xml");
        public static readonly XDocument XSettings = XDocument.Load("./Settings/Settings.xml");
        public static Process[] FFXIVPID;
        public static readonly string[] Settings = {"Tab", "Color", "Event"};
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
