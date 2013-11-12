// FFXIVAPP.Client
// UnsafeNativeMethods.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Runtime.InteropServices;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Memory
{
    [DoNotObfuscateType]
    public static class UnsafeNativeMethods
    {
        public enum ProcessAccessFlags
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VMOperation = 0x00000008,
            VMRead = 0x00000010,
            VMWrite = 0x00000020,
            DupHandle = 0x00000040,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            Synchronize = 0x00100000
        }

        /// <summary>
        /// </summary>
        /// <param name="lpKeyState"> </param>
        /// <returns> </returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool NativeGetKeyboardState(byte[] lpKeyState);

        /// <summary>
        /// </summary>
        /// <param name="dwDesiredAccess"></param>
        /// <param name="bInheritHandle"></param>
        /// <param name="dwProcessId"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, UInt32 dwProcessId);

        /// <summary>
        /// </summary>
        /// <param name="hObject"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Int32 CloseHandle(IntPtr hObject);

        /// <summary>
        /// </summary>
        /// <param name="hProcess"> </param>
        /// <param name="lpBaseAddress"> </param>
        /// <param name="lpBuffer"> </param>
        /// <param name="nSize"> </param>
        /// <param name="lpNumberOfBytesRead"> </param>
        /// <returns> </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In] [Out] Byte[] lpBuffer, int nSize, out int lpNumberOfBytesRead);

        /// <summary>
        /// </summary>
        /// <param name="hProcess"> </param>
        /// <param name="lpBaseAddress"> </param>
        /// <param name="lpBuffer"> </param>
        /// <param name="nSize"> </param>
        /// <param name="lpNumberOfBytesRead"> </param>
        /// <returns> </returns>
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In] [Out] IntPtr lpBuffer, int nSize, out int lpNumberOfBytesRead);

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
