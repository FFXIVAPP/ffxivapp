// Project: Launcher
// File: Globals.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace Launcher.Classes
{
    public static class Globals
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [DllImport("WinModXIV", SetLastError = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int SetTarget(ref Targetmap target);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImport("WinModXIV", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern int StartHook();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImport("WinModXIV", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern int EndHook();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImport("WinModXIV", SetLastError = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr GetDllVersion();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpClassName"></param>
        /// <param name="lpWindowName"></param>
        /// <returns></returns>
        [DllImport("user32", EntryPoint = "FindWindowA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int FindWindow([MarshalAs(UnmanagedType.VBByRefStr)] ref string lpClassName, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpWindowName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lpRect"></param>
        /// <returns></returns>
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int GetWindowRect(int hwnd, ref Rect lpRect);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="nWidth"></param>
        /// <param name="nHeight"></param>
        /// <param name="bRepaint"></param>
        /// <returns></returns>
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool MoveWindow(IntPtr hwnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nIndex"></param>
        /// <param name="dwNewLong"></param>
        /// <returns></returns>
        public static int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public static int GetWindowLong(IntPtr hWnd, int nIndex)
        {
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nIndex"></param>
        /// <param name="dwNewLong"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nIndex"></param>
        /// <param name="dwNewLong"></param>
        /// <returns></returns>
        public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            return IntPtr.Size == 8 ? SetWindowLongPtr64(hWnd, nIndex, dwNewLong) : new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nIndex"></param>
        /// <param name="dwNewLong"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public readonly int Left;
            public readonly int Top;
            public readonly int Right;
            public readonly int Bottom;
        }
    }
}