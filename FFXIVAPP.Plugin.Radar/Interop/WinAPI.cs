// FFXIVAPP.Plugin.Radar
// WinAPI.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using FFXIVAPP.Plugin.Radar.Properties;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Plugin.Radar.Interop
{
    [DoNotObfuscate]
    public static class WinAPI
    {
        public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        private const int WS_EX_TRANSPARENT = 0x00000020;
        private const int GWL_EXSTYLE = (-20);
        public const uint WINEVENT_OUTOFCONTEXT = 0;
        public const uint EVENT_SYSTEM_FOREGROUND = 3;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        public static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            var handle = IntPtr.Zero;
            var Buff = new StringBuilder(nChars);
            handle = GetForegroundWindow();
            return GetWindowText(handle, Buff, nChars) > 0 ? Buff.ToString() : "";
        }

        private static void SetWindowTransparent(IntPtr hwnd)
        {
            var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
        }

        private static void SetWindowLayered(IntPtr hwnd)
        {
            var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            extendedStyle &= ~WS_EX_TRANSPARENT;
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle);
        }

        public static void ToggleClickThrough(Window window)
        {
            try
            {
                var hWnd = new WindowInteropHelper(window).Handle;
                if (Settings.Default.WidgetClickThroughEnabled)
                {
                    SetWindowTransparent(hWnd);
                }
                else
                {
                    SetWindowLayered(hWnd);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
