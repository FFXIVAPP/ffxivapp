// FFXIVAPP.Client
// PopupNonTopMost.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;

#endregion

namespace FFXIVAPP.Client.Controls
{
    public class PopupNonTopmost : Popup
    {
        protected override void OnOpened(EventArgs e)
        {
            var fromVisual = (HwndSource) PresentationSource.FromVisual(Child);
            if (fromVisual == null)
            {
                return;
            }
            var hwnd = fromVisual.Handle;
            RECT rect;
            if (GetWindowRect(hwnd, out rect))
            {
                SetWindowPos(hwnd, -2, rect.Left, rect.Top, (int) Width, (int) Height, 0);
            }
        }

        #region P/Invoke Imports & Definitions

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32", EntryPoint = "SetWindowPos")]
        private static extern int SetWindowPos(IntPtr hWnd, int hwndInsertAfter, int x, int y, int cx, int cy, int wFlags);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        #endregion
    }
}
