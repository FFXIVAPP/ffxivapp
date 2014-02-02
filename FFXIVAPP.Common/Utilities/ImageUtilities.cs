// FFXIVAPP.Common
// ImageUtilities.cs
// 
// © 2013 Ryan Wilson

using System.IO;
using System.Windows.Media.Imaging;

namespace FFXIVAPP.Common.Utilities
{
    public static class ImageUtilities
    {
        public static BitmapImage LoadImageFromStream(string location)
        {
            BitmapImage result = null;
            if (location != null)
            {
                var bitmapImage = new BitmapImage();
                using (var stream = File.OpenRead(location))
                {
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();
                }
                result = bitmapImage;
            }
            return result;
        }
    }
}
