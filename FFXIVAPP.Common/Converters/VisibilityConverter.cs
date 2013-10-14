// FFXIVAPP.Common
// VisibilityConverter.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;

#endregion

namespace FFXIVAPP.Common.Converters
{
    public class VisibilityConverter : IValueConverter
    {
        /// <summary>
        /// </summary>
        /// <param name="value"> </param>
        /// <param name="targetType"> </param>
        /// <param name="parameter"> </param>
        /// <param name="culture"> </param>
        /// <returns> </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return (bool) value ? Visibility.Visible : Visibility.Collapsed;
            }
            catch
            {
                return (Regex.IsMatch(value.ToString(), "([Tt]rue|1)")) ? Visibility.Visible : Visibility.Collapsed;
            }
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
            return ((Visibility) value == Visibility.Visible);
        }
    }
}
