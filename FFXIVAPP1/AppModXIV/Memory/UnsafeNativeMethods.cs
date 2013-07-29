using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace AppModXIV.Memory
{
    public class UnsafeNativeMethods
    {
        public const int PROCESS_ALL_ACCESS = 0x1F0FFF;

        [StructLayout(LayoutKind.Sequential, Size = 40)]
        internal struct PROCESS_MEMORY_COUNTERS
        {
            public int cb;
            public int PageFaultCount;
            public int PeakWorkingSetSize;
            public int WorkingSetSize;
            public int QuotaPeakPagedPoolUsage;
            public int QuotaPagedPoolUsage;
            public int QuotaPeakNonPagedPoolUsage;
            public int QuotaNonPagedPoolUsage;
            public int PagefileUsage;
            public int PeakPagefileUsage;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_BASIC_INFORMATION
        {
            public int BaseAddress;
            public int AllocationBase;
            public int AllocationProtect;
            public int RegionSize;
            public int State;
            public int Protect;
            public int Type;
        }

        /// <summary>
        /// Allows a user to open a Process.
        /// Depends on kernal32.dll
        /// </summary>
        /// <param name="dwDesiredAccess"></param>
        /// <param name="bInheritHandle"></param>
        /// <param name="dwProcessId"></param>
        /// <returns>int</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, Boolean bInheritHandle, int dwProcessId);

        /// <summary>
        /// Allows a User to Close the handle which is already opened.
        /// Depends on kernal32.dll
        /// </summary>
        /// <param name="hObject"></param>
        /// <returns>int</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern Boolean CloseHandle(IntPtr hObject);

        /// <summary>
        /// Allows a User to read memory in a Remote process.
        /// Depends on Kernal32.dll
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="lpBaseAddress"></param>
        /// <param name="lpBuffer"></param>
        /// <param name="nSize"></param>
        /// <param name="lpNumberOfBytesWritten"></param>
        /// <returns>int</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern Boolean ReadProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        Byte[] lpBuffer,
        int nSize,
        int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        public static extern Boolean ReadProcessMemory([Out()]
            IntPtr hProcess, [Out()]
            IntPtr lpBaseAddress, [Out()]
            byte[] lpBuffer, [Out()]
            UIntPtr nSize, [Out()]
            IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean ReadProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        IntPtr lpBuffer,
        int iSize,
        ref int lpNumberOfBytesRead);

        /// <summary>
        /// Allows a User to write memory in a Remote process.
        /// Depends on Kernal32.dll
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="lpBaseAddress"></param>
        /// <param name="lpBuffer"></param>
        /// <param name="nSize"></param>
        /// <param name="lpNumberOfBytesWritten"></param>
        /// <returns>int</returns>
        [DllImport("kernel32.dll")]
        public static extern Boolean WriteProcessMemory([In(), Out()]
            IntPtr hProcess, [In(), Out()]
            IntPtr lpBaseAddress, [In(), Out()]
            byte[] lpBuffer, [In(), Out()]
            UIntPtr nSize, [In(), Out()]
            ref IntPtr lpNumberOfBytesWritten);

        [DllImport("User32.Dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("psapi.dll", SetLastError = true)]
        internal static extern int GetProcessMemoryInfo(IntPtr hProcess, out PROCESS_MEMORY_COUNTERS counters, int size);

        [DllImport("kernel32.dll")]
        internal static extern int VirtualQueryEx(IntPtr hProcess, uint lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, int dwLength);
    }
}
