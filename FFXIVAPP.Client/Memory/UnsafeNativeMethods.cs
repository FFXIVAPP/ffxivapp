// FFXIVAPP.Client
// UnsafeNativeMethods.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Runtime.InteropServices;

#endregion

namespace FFXIVAPP.Client.Memory
{
    public static class UnsafeNativeMethods
    {
        public const int ProcessAllAccess = 0x1F0FFF;

        /// <summary>
        /// </summary>
        /// <param name="lpKeyState"> </param>
        /// <returns> </returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool NativeGetKeyboardState(byte[] lpKeyState);

        /// <summary>
        /// </summary>
        /// <param name="hwnd"> </param>
        /// <param name="hwndInsertAfter"> </param>
        /// <param name="x"> </param>
        /// <param name="y"> </param>
        /// <param name="cx"> </param>
        /// <param name="cy"> </param>
        /// <param name="wFlags"> </param>
        /// <returns> </returns>
        [DllImport("user32", EntryPoint = "SetWindowPos")]
        public static extern int SetWindowPos(IntPtr hwnd, int hwndInsertAfter, int x, int y, int cx, int cy, int wFlags);

        /// <summary>
        /// </summary>
        /// <param name="dwDesiredAccess"> </param>
        /// <param name="bInheritHandle"> </param>
        /// <param name="dwProcessId"> </param>
        /// <returns> </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        /// <summary>
        /// </summary>
        /// <param name="hObject"> </param>
        /// <returns> </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);

        /// <summary>
        /// </summary>
        /// <param name="hProcess"> </param>
        /// <param name="lpBaseAddress"> </param>
        /// <param name="lpBuffer"> </param>
        /// <param name="nSize"> </param>
        /// <param name="lpNumberOfBytesWritten"> </param>
        /// <returns> </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, Byte[] lpBuffer, int nSize, int lpNumberOfBytesWritten);

        /// <summary>
        /// </summary>
        /// <param name="hProcess"> </param>
        /// <param name="lpBaseAddress"> </param>
        /// <param name="lpBuffer"> </param>
        /// <param name="nSize"> </param>
        /// <param name="lpNumberOfBytesRead"> </param>
        /// <returns> </returns>
        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory([Out] IntPtr hProcess, [Out] IntPtr lpBaseAddress, [Out] byte[] lpBuffer, [Out] UIntPtr nSize, [Out] IntPtr lpNumberOfBytesRead);

        /// <summary>
        /// </summary>
        /// <param name="hProcess"> </param>
        /// <param name="lpBaseAddress"> </param>
        /// <param name="lpBuffer"> </param>
        /// <param name="iSize"> </param>
        /// <param name="lpNumberOfBytesRead"> </param>
        /// <returns> </returns>
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int iSize, ref int lpNumberOfBytesRead);

        /// <summary>
        /// </summary>
        /// <param name="hProcess"> </param>
        /// <param name="lpBaseAddress"> </param>
        /// <param name="lpBuffer"> </param>
        /// <param name="nSize"> </param>
        /// <param name="lpNumberOfBytesWritten"> </param>
        /// <returns> </returns>
        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory([In] [Out] IntPtr hProcess, [In] [Out] IntPtr lpBaseAddress, [In] [Out] byte[] lpBuffer, [In] [Out] UIntPtr nSize, [In] [Out] ref IntPtr lpNumberOfBytesWritten);

        /// <summary>
        /// </summary>
        /// <param name="hWnd"> </param>
        /// <param name="msg"> </param>
        /// <param name="wParam"> </param>
        /// <param name="lParam"> </param>
        /// <returns> </returns>
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SendNotifyMessageW(IntPtr hWnd, uint msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        /// <summary>
        /// </summary>
        /// <param name="hWnd"> </param>
        /// <param name="msg"> </param>
        /// <param name="wParam"> </param>
        /// <param name="lParam"> </param>
        /// <returns> </returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        /// <summary>
        /// </summary>
        /// <param name="hWnd"> </param>
        /// <param name="msg"> </param>
        /// <param name="wParam"> </param>
        /// <param name="lParam"> </param>
        /// <returns> </returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, uint msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        /// <summary>
        /// </summary>
        /// <param name="hProcess"> </param>
        /// <param name="counters"> </param>
        /// <param name="size"> </param>
        /// <returns> </returns>
        [DllImport("psapi.dll", SetLastError = true)]
        public static extern int GetProcessMemoryInfo(IntPtr hProcess, out ProcessMemoryCounters counters, int size);

        /// <summary>
        /// </summary>
        /// <param name="hProcess"> </param>
        /// <param name="lpAddress"> </param>
        /// <param name="lpBuffer"> </param>
        /// <param name="dwLength"> </param>
        /// <returns> </returns>
        [DllImport("kernel32.dll")]
        public static extern int VirtualQueryEx(IntPtr hProcess, uint lpAddress, out MemoryBasicInformation lpBuffer, uint dwLength);

        [StructLayout(LayoutKind.Sequential)]
        public struct MemoryBasicInformation
        {
            public readonly int BaseAddress;
            public readonly int AllocationBase;
            public readonly int AllocationProtect;
            public readonly int RegionSize;
            public readonly int State;
            public readonly int Protect;
            public readonly int Type;
        }

        [StructLayout(LayoutKind.Sequential, Size = 40)]
        public struct ProcessMemoryCounters
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
    }
}
