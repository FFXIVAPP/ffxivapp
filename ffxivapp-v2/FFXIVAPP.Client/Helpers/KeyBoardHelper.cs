// FFXIVAPP.Client
// KeyBoardHelper.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using FFXIVAPP.Client.Enums;
using FFXIVAPP.Client.Memory;
using FFXIVAPP.Client.Models;

#endregion

namespace FFXIVAPP.Client.Helpers
{
    public static class KeyBoardHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="keyStates"> </param>
        /// <returns> </returns>
        private static bool GetKeyboardState(byte[] keyStates)
        {
            if (keyStates == null)
            {
                return false;
            }
            return keyStates.Length == 256 && UnsafeNativeMethods.NativeGetKeyboardState(keyStates);
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        private static IEnumerable<byte> GetKeyboardState()
        {
            var keyStates = new byte[256];
            if (!GetKeyboardState(keyStates))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return keyStates;
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        private static bool AnyKeyPressed()
        {
            var keyState = GetKeyboardState();
            return keyState.Skip(8)
                           .Any(state => (state & 0x80) != 0);
        }

        /// <summary>
        /// </summary>
        /// <param name="key"> </param>
        public static void Alt(Keys key)
        {
            if (Common.Constants.ProcessHandle != null)
            {
                UnsafeNativeMethods.SendMessage(Common.Constants.ProcessHandle, WindowsMessageEvents.KeyDown, (IntPtr) Keys.Menu, null);
                UnsafeNativeMethods.SendMessage(Common.Constants.ProcessHandle, WindowsMessageEvents.KeyDown, (IntPtr) key, null);
                UnsafeNativeMethods.SendMessage(Common.Constants.ProcessHandle, WindowsMessageEvents.Char, (IntPtr) key, null);
                UnsafeNativeMethods.SendMessage(Common.Constants.ProcessHandle, WindowsMessageEvents.KeyUp, (IntPtr) key, null);
                UnsafeNativeMethods.SendMessage(Common.Constants.ProcessHandle, WindowsMessageEvents.KeyUp, (IntPtr) Keys.Menu, null);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="key"> </param>
        public static void Ctrl(Keys key)
        {
            if (Common.Constants.ProcessHandle != null)
            {
                UnsafeNativeMethods.SendMessage(Common.Constants.ProcessHandle, WindowsMessageEvents.KeyDown, (IntPtr) Keys.ControlKey, null);
                UnsafeNativeMethods.SendMessage(Common.Constants.ProcessHandle, WindowsMessageEvents.KeyDown, (IntPtr) key, null);
                UnsafeNativeMethods.SendMessage(Common.Constants.ProcessHandle, WindowsMessageEvents.Char, (IntPtr) key, null);
                UnsafeNativeMethods.SendMessage(Common.Constants.ProcessHandle, WindowsMessageEvents.KeyUp, (IntPtr) key, null);
                UnsafeNativeMethods.SendMessage(Common.Constants.ProcessHandle, WindowsMessageEvents.KeyUp, (IntPtr) Keys.ControlKey, null);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="bytes"> </param>
        public static void SendNotify(byte[] bytes)
        {
            if (Common.Constants.ProcessHandle != null)
            {
                Thread.Sleep(100);
                var input = new MemoryStream(bytes);
                var reader = new BinaryReader(input);
                while (input.Position < input.Length)
                {
                    UnsafeNativeMethods.SendNotifyMessageW(Common.Constants.ProcessHandle, 0x102, (IntPtr) reader.ReadInt16(), null);
                }
                KeyPressNotify(Keys.Return);
            }
        }

        /// <summary>
        /// </summary>
        public static void Paste()
        {
            if (Common.Constants.ProcessHandle != null)
            {
                UnsafeNativeMethods.SendMessage(Common.Constants.ProcessHandle, WindowsMessageEvents.KeyDown, (IntPtr) Keys.ControlKey, null);
                UnsafeNativeMethods.SendMessage(Common.Constants.ProcessHandle, WindowsMessageEvents.KeyDown, (IntPtr) Keys.V, null);
                UnsafeNativeMethods.SendMessage(Common.Constants.ProcessHandle, WindowsMessageEvents.KeyUp, (IntPtr) Keys.V, null);
                UnsafeNativeMethods.SendMessage(Common.Constants.ProcessHandle, WindowsMessageEvents.KeyUp, (IntPtr) Keys.ControlKey, null);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="key"> </param>
        private static void KeyPressNotify(Keys key)
        {
            if (Common.Constants.ProcessHandle != null)
            {
                UnsafeNativeMethods.SendNotifyMessageW(Common.Constants.ProcessHandle, WindowsMessageEvents.KeyDown, (IntPtr) key, null);
                UnsafeNativeMethods.SendNotifyMessageW(Common.Constants.ProcessHandle, WindowsMessageEvents.Char, (IntPtr) key, null);
                UnsafeNativeMethods.SendNotifyMessageW(Common.Constants.ProcessHandle, WindowsMessageEvents.KeyUp, (IntPtr) key, null);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="key"> </param>
        public static void KeyPress(Keys key)
        {
            if (Common.Constants.ProcessHandle != null)
            {
                UnsafeNativeMethods.SendMessage(Common.Constants.ProcessHandle, WindowsMessageEvents.KeyDown, (IntPtr) key, null);
                UnsafeNativeMethods.SendMessage(Common.Constants.ProcessHandle, WindowsMessageEvents.Char, (IntPtr) key, null);
                UnsafeNativeMethods.SendMessage(Common.Constants.ProcessHandle, WindowsMessageEvents.KeyUp, (IntPtr) key, null);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="key"> </param>
        public static void KeyPressIntPtr(IntPtr key)
        {
            if (Common.Constants.ProcessHandle != null)
            {
                UnsafeNativeMethods.SendMessage(Common.Constants.ProcessHandle, WindowsMessageEvents.Char, key, null);
            }
        }
    }
}
