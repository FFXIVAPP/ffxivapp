// AppModXIV
// User32.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace AppModXIV.Classes
{
    internal abstract class User32
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public readonly int left;
            public readonly int top;
            public readonly int right;
            public readonly int bottom;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hDc"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);
    }
}