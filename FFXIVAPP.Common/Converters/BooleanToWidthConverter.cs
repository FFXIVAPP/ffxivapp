// FFXIVAPP.Common
// BooleanToWidthConverter.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace FFXIVAPP.Common.Converters
{
    public class BooleanToWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return (bool) value ? Double.NaN : 0;
            }
            catch
            {
                return (Regex.IsMatch(value.ToString(), "([Tt]rue|1)")) ? Double.NaN : 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
