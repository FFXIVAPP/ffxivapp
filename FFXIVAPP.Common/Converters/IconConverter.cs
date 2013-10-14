// FFXIVAPP.Common
// IconConverter.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

#endregion

namespace FFXIVAPP.Common.Converters {
    public class IconConverter : IMultiValueConverter {
        private const string IconPath = "/Plugins/{0}/{1}";

        /// <summary>
        /// </summary>
        /// <param name="values"> </param>
        /// <param name="targetType"> </param>
        /// <param name="parameter"> </param>
        /// <param name="culture"> </param>
        /// <returns> </returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            var source = new BitmapImage(new Uri(Constants.DefaultIcon));
            var folder = values[1];
            var name = values[2];
            var location = String.Format(AppDomain.CurrentDomain.BaseDirectory + IconPath, folder, name);
            return File.Exists(location) ? new BitmapImage(new Uri(location)) : source;
        }

        /// <summary>
        /// </summary>
        /// <param name="value"> </param>
        /// <param name="targetTypes"> </param>
        /// <param name="parameter"> </param>
        /// <param name="culture"> </param>
        /// <returns> </returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
