// AppModXIV
// Constants.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AppModXIV.Classes
{
    public static class Constants
    {
        public static string DllVersion = "";
        public static IntPtr PHandle;
        public static int Pid;
        public static int LogErrors;
        public static Boolean FfxivOpen;
        public static readonly Dictionary<string, string> XAtCodes = new Dictionary<string, string>();
        public static Process[] FfxivPid;
        public static readonly string[] CmSay = {"0001"};
        public static readonly string[] CmTell = {"000D", "0003"};
        public static readonly string[] CmParty = {"0004"};
        public static readonly string[] CmShout = {"0002"};
        public static readonly string[] Cmls = {"000E", "0005", "000F", "0006", "0010", "0007", "0011", "0008", "0012", "0009", "0013", "000A", "0014", "000B", "0015", "000C"};
    }
}