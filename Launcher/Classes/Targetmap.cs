// Launcher
// Targetmap.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System.Runtime.InteropServices;

namespace Launcher.Classes
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct Targetmap
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string path;

        public long dxversion;
        public long flags;
        public int initx;
        public int inity;
        public int minx;
        public int miny;
        public int maxx;
        public int maxy;
    }
}