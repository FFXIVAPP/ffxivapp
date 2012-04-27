// Project: AppModXIV
// File: ScreenCapture.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace AppModXIV.Classes
{
    public class ScreenCapture
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static Image CaptureScreen()
        {
            return CaptureWindow(User32.GetDesktopWindow());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        private static Image CaptureWindow(IntPtr handle)
        {
            var hdcSrc = User32.GetWindowDC(handle);
            var windowRect = new User32.Rect();
            User32.GetWindowRect(handle, ref windowRect);
            var width = windowRect.right - windowRect.left;
            var height = windowRect.bottom - windowRect.top;
            var hdcDest = Gdi32.CreateCompatibleDC(hdcSrc);
            var hBitmap = Gdi32.CreateCompatibleBitmap(hdcSrc, width, height);
            var hOld = Gdi32.SelectObject(hdcDest, hBitmap);
            Gdi32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, Gdi32.Srccopy);
            Gdi32.SelectObject(hdcDest, hOld);
            Gdi32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);
            Image img = Image.FromHbitmap(hBitmap);
            Gdi32.DeleteObject(hBitmap);
            return img;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        public static void CaptureWindowToFile(IntPtr handle, string filename, ImageFormat format)
        {
            var img = CaptureWindow(handle);
            img.Save(filename, format);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        public void CaptureScreenToFile(string filename, ImageFormat format)
        {
            var img = CaptureScreen();
            img.Save(filename, format);
        }
    }
}