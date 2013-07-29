// FFXIVAPP.Common
// ScreenCapture.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#endregion

namespace FFXIVAPP.Common.Utilities
{
    public static class ScreenCapture
    {
        /// <summary>
        /// </summary>
        /// <param name="source"> </param>
        /// <param name="scale"> </param>
        /// <param name="quality"> </param>
        /// <returns> </returns>
        public static byte[] GetJpgImage(UIElement source, double scale, int quality)
        {
            var actualHeight = source.RenderSize.Height;
            var actualWidth = source.RenderSize.Width;
            var renderHeight = actualHeight * scale;
            var renderWidth = actualWidth * scale;
            var renderTarget = new RenderTargetBitmap((int) renderWidth, (int) renderHeight, 96, 96, PixelFormats.Pbgra32);
            var sourceBrush = new VisualBrush(source);
            var drawingVisual = new DrawingVisual();
            var drawingContext = drawingVisual.RenderOpen();
            using (drawingContext)
            {
                drawingContext.PushTransform(new ScaleTransform(scale, scale));
                drawingContext.DrawRectangle(sourceBrush, null, new Rect(new Point(0, 0), new Point(actualWidth, actualHeight)));
            }
            renderTarget.Render(drawingVisual);
            var jpgEncoder = new JpegBitmapEncoder
            {
                QualityLevel = quality
            };
            jpgEncoder.Frames.Add(BitmapFrame.Create(renderTarget));
            Byte[] imageArray;
            using (var outputStream = new MemoryStream())
            {
                jpgEncoder.Save(outputStream);
                imageArray = outputStream.ToArray();
            }
            return imageArray;
        }
    }
}
