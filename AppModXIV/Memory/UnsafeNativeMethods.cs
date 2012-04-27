// Project: AppModXIV
// File: UnsafeNativeMethods.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace AppModXIV.Memory
{
    public static class UnsafeNativeMethods
    {
        public const int ProcessAllAccess = 0x1F0FFF;

        [StructLayout(LayoutKind.Sequential, Size = 40)]
        internal struct ProcessMemoryCounters
        {
            private readonly int cb;
            private readonly int PageFaultCount;
            private readonly int PeakWorkingSetSize;
            private readonly int WorkingSetSize;
            private readonly int QuotaPeakPagedPoolUsage;
            private readonly int QuotaPagedPoolUsage;
            private readonly int QuotaPeakNonPagedPoolUsage;
            private readonly int QuotaNonPagedPoolUsage;
            public readonly int PagefileUsage;
            private readonly int PeakPagefileUsage;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MemoryBasicInformation
        {
            public readonly int BaseAddress;
            private readonly int AllocationBase;
            private readonly int AllocationProtect;
            public readonly int RegionSize;
            public readonly int State;
            public readonly int Protect;
            private readonly int Type;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dwDesiredAccess"></param>
        /// <param name="bInheritHandle"></param>
        /// <param name="dwProcessId"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, Boolean bInheritHandle, int dwProcessId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hObject"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern Boolean CloseHandle(IntPtr hObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="lpBaseAddress"></param>
        /// <param name="lpBuffer"></param>
        /// <param name="nSize"></param>
        /// <param name="lpNumberOfBytesWritten"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern Boolean ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, Byte[] lpBuffer, int nSize, int lpNumberOfBytesWritten);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="lpBaseAddress"></param>
        /// <param name="lpBuffer"></param>
        /// <param name="nSize"></param>
        /// <param name="lpNumberOfBytesRead"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern Boolean ReadProcessMemory([Out] IntPtr hProcess, [Out] IntPtr lpBaseAddress, [Out] byte[] lpBuffer, [Out] UIntPtr nSize, [Out] IntPtr lpNumberOfBytesRead);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="lpBaseAddress"></param>
        /// <param name="lpBuffer"></param>
        /// <param name="iSize"></param>
        /// <param name="lpNumberOfBytesRead"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int iSize, ref int lpNumberOfBytesRead);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="lpBaseAddress"></param>
        /// <param name="lpBuffer"></param>
        /// <param name="nSize"></param>
        /// <param name="lpNumberOfBytesWritten"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern Boolean WriteProcessMemory([In, Out] IntPtr hProcess, [In, Out] IntPtr lpBaseAddress, [In, Out] byte[] lpBuffer, [In, Out] UIntPtr nSize, [In, Out] ref IntPtr lpNumberOfBytesWritten);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern Boolean SendNotifyMessageW(IntPtr hWnd, int msg, int wParam, int lParam);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("User32.Dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="counters"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [DllImport("psapi.dll", SetLastError = true)]
        internal static extern int GetProcessMemoryInfo(IntPtr hProcess, out ProcessMemoryCounters counters, int size);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="lpAddress"></param>
        /// <param name="lpBuffer"></param>
        /// <param name="dwLength"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        internal static extern int VirtualQueryEx(IntPtr hProcess, uint lpAddress, out MemoryBasicInformation lpBuffer, int dwLength);
    }
}