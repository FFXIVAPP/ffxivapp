// FFXIVAPP
// KeyHelper.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.IO;
using System.Threading;
using FFXIVAPP.Classes.Memory;
using NLog;

namespace FFXIVAPP.Classes.Helpers
{
    internal static class KeyHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="key"> </param>
        public static void Alt(Keys key)
        {
            if (Constants.PHandle != null)
            {
                UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.KeyDown, (IntPtr) Keys.Menu, null);
                UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.KeyDown, (IntPtr) key, null);
                UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.Char, (IntPtr) key, null);
                UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.KeyUp, (IntPtr) key, null);
                UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.KeyUp, (IntPtr) Keys.Menu, null);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="key"> </param>
        public static void Ctrl(Keys key)
        {
            if (Constants.PHandle != null)
            {
                UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.KeyDown, (IntPtr) Keys.ControlKey, null);
                UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.KeyDown, (IntPtr) key, null);
                UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.Char, (IntPtr) key, null);
                UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.KeyUp, (IntPtr) key, null);
                UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.KeyUp, (IntPtr) Keys.ControlKey, null);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="bytes"> </param>
        public static void SendNotify(byte[] bytes)
        {
            if (Constants.PHandle != null)
            {
                Thread.Sleep(100);
                var input = new MemoryStream(bytes);
                var reader = new BinaryReader(input);
                while (input.Position < input.Length)
                {
                    UnsafeNativeMethods.SendNotifyMessageW(Constants.PHandle, 0x102, (IntPtr) reader.ReadInt16(), null);
                }
                KeyPressNotify(Keys.Return);
            }
        }

        /// <summary>
        /// </summary>
        public static void Paste()
        {
            if (Constants.PHandle != null)
            {
                UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.KeyDown, (IntPtr) Keys.ControlKey, null);
                UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.KeyDown, (IntPtr) Keys.V, null);
                UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.KeyUp, (IntPtr) Keys.V, null);
                UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.KeyUp, (IntPtr) Keys.ControlKey, null);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="key"> </param>
        private static void KeyPressNotify(Keys key)
        {
            if (Constants.PHandle != null)
            {
                UnsafeNativeMethods.SendNotifyMessageW(Constants.PHandle, WindowsMessageEvents.KeyDown, (IntPtr) key, null);
                UnsafeNativeMethods.SendNotifyMessageW(Constants.PHandle, WindowsMessageEvents.Char, (IntPtr) key, null);
                UnsafeNativeMethods.SendNotifyMessageW(Constants.PHandle, WindowsMessageEvents.KeyUp, (IntPtr) key, null);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="key"> </param>
        public static void KeyPress(Keys key)
        {
            if (Constants.PHandle != null)
            {
                UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.KeyDown, (IntPtr) key, null);
                UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.Char, (IntPtr) key, null);
                UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.KeyUp, (IntPtr) key, null);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="key"> </param>
        public static void KeyPressIntPtr(IntPtr key)
        {
            if (Constants.PHandle != null)
            {
                UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.Char, key, null);
            }
        }
    }
}