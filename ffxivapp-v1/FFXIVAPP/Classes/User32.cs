// FFXIVAPP
// User32.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Runtime.InteropServices;

#endregion

namespace FFXIVAPP.Classes
{
    internal abstract class User32
    {
        /// <summary>
        /// </summary>
        /// <returns> </returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        /// <summary>
        /// </summary>
        /// <param name="hWnd"> </param>
        /// <returns> </returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        /// <summary>
        /// </summary>
        /// <param name="hWnd"> </param>
        /// <param name="hDc"> </param>
        /// <returns> </returns>
        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

        /// <summary>
        /// </summary>
        /// <param name="hWnd"> </param>
        /// <param name="rect"> </param>
        /// <returns> </returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public readonly int left;
            public readonly int top;
            public readonly int right;
            public readonly int bottom;
        }
    }
}
