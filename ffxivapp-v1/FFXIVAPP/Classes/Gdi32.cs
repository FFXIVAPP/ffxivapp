// FFXIVAPP
// Gdi32.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Runtime.InteropServices;

#endregion

namespace FFXIVAPP.Classes
{
    internal abstract class Gdi32
    {
        public const int Srccopy = 0x00CC0020; // BitBlt dwRop parameter

        /// <summary>
        /// </summary>
        /// <param name="hObject"> </param>
        /// <param name="nXDest"> </param>
        /// <param name="nYDest"> </param>
        /// <param name="nWidth"> </param>
        /// <param name="nHeight"> </param>
        /// <param name="hObjectSource"> </param>
        /// <param name="nXSrc"> </param>
        /// <param name="nYSrc"> </param>
        /// <param name="dwRop"> </param>
        /// <returns> </returns>
        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hObjectSource, int nXSrc, int nYSrc, int dwRop);

        /// <summary>
        /// </summary>
        /// <param name="hDc"> </param>
        /// <param name="nWidth"> </param>
        /// <param name="nHeight"> </param>
        /// <returns> </returns>
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDc, int nWidth, int nHeight);

        /// <summary>
        /// </summary>
        /// <param name="hDc"> </param>
        /// <returns> </returns>
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDc);

        /// <summary>
        /// </summary>
        /// <param name="hDc"> </param>
        /// <returns> </returns>
        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hDc);

        /// <summary>
        /// </summary>
        /// <param name="hObject"> </param>
        /// <returns> </returns>
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// </summary>
        /// <param name="hDc"> </param>
        /// <param name="hObject"> </param>
        /// <returns> </returns>
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDc, IntPtr hObject);
    }
}
