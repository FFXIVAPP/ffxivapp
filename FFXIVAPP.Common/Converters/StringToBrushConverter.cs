// FFXIVAPP.Common
// StringToBrushConverter.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using NLog;

namespace FFXIVAPP.Common.Converters
{
    public class StringToBrushConverter : IValueConverter
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="value"> </param>
        /// <param name="targetType"> </param>
        /// <param name="parameter"> </param>
        /// <param name="culture"> </param>
        /// <returns> </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var brushConverter = new BrushConverter();
            value = (value.ToString()
                          .StartsWith("#")) ? value : "#" + value;
            var result = (Brush) brushConverter.ConvertFrom("#FFFFFFFF");
            try
            {
                result = (Brush) brushConverter.ConvertFrom(value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="value"> </param>
        /// <param name="targetType"> </param>
        /// <param name="parameter"> </param>
        /// <param name="culture"> </param>
        /// <returns> </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new BrushConverter().ConvertFrom("#FFFFFFFF");
        }

        /// <summary>
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public object Convert(object value)
        {
            var brushConverter = new BrushConverter();
            value = (value.ToString()
                          .Substring(0, 1) == "#") ? value : "#" + value;
            var result = (Brush) brushConverter.ConvertFrom("#FFFFFFFF");
            try
            {
                result = (Brush) brushConverter.ConvertFrom(value);
            }
            catch (Exception ex)
            {
            }
            return result;
        }
    }
}
