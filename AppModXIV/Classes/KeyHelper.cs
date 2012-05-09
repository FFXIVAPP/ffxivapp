// AppModXIV
// KeyHelper.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System.Collections.Generic;
using AppModXIV.Memory;

namespace AppModXIV.Classes
{
    public static class KeyHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public static void Alt(Keys key)
        {
            UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.KeyDown, (int) Keys.Menu, 0);
            UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.KeyDown, (int) key, 0);
            UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.Char, (int) key, 0);
            UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.KeyUp, (int) key, 0);
            UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.KeyUp, (int) Keys.Menu, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public static void Ctrl(Keys key)
        {
            UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.KeyDown, (int) Keys.ControlKey, 0);
            UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.KeyDown, (int) key, 0);
            UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.Char, (int) key, 0);
            UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.KeyUp, (int) key, 0);
            UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.KeyUp, (int) Keys.ControlKey, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        public static void SendNotify(IEnumerable<byte> bytes)
        {
            //KeyPressNotify(Keys.Escape);
            foreach (var b in bytes)
            {
                UnsafeNativeMethods.SendNotifyMessageW(Constants.PHandle, WindowsMessageEvents.Char, b, 0);
            }
            KeyPressNotify(Keys.Return);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Paste()
        {
            UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.KeyDown, (int) Keys.ControlKey, 0);
            UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.KeyDown, (int) Keys.V, 0);
            UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.KeyUp, (int) Keys.V, 0);
            UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.KeyUp, (int) Keys.ControlKey, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        private static void KeyPressNotify(Keys key)
        {
            UnsafeNativeMethods.SendNotifyMessageW(Constants.PHandle, WindowsMessageEvents.KeyDown, (int) key, 0);
            UnsafeNativeMethods.SendNotifyMessageW(Constants.PHandle, WindowsMessageEvents.Char, (int) key, 0);
            UnsafeNativeMethods.SendNotifyMessageW(Constants.PHandle, WindowsMessageEvents.KeyUp, (int) key, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public static void KeyPress(Keys key)
        {
            UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.KeyDown, (int) key, 0);
            UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.Char, (int) key, 0);
            UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.KeyUp, (int) key, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public static void KeyPressInt(int key)
        {
            UnsafeNativeMethods.SendMessage(Constants.PHandle, WindowsMessageEvents.Char, key, 0);
        }
    }
}