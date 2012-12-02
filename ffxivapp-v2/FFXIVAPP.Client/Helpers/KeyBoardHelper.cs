// FFXIVAPP.Client
// KeyBoardHelper.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System;
using System.IO;
using System.Threading;
using FFXIVAPP.Client.Enums;
using FFXIVAPP.Client.Memory;
using FFXIVAPP.Client.Models;

namespace FFXIVAPP.Client.Helpers
{
    public static class KeyBoardHelper
    {
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