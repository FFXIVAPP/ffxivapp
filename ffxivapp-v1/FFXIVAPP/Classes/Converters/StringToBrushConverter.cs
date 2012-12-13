// FFXIVAPP
// StringToBrushConverter.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

#endregion

namespace FFXIVAPP.Classes.Converters
{
    internal class StringToBrushConverter : IValueConverter
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
            var b = new BrushConverter();
            value = (value.ToString().Substring(0, 1) == "#") ? value : "#" + value;
            var result = (Brush) b.ConvertFrom("#FFFFFFFF");
            try
            {
                result = (Brush) b.ConvertFrom(value);
            }
            catch (Exception ex) {}
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public object Convert(object value)
        {
            var b = new BrushConverter();
            value = (value.ToString().Substring(0, 1) == "#") ? value : "#" + value;
            var result = (Brush) b.ConvertFrom("#FFFFFFFF");
            try
            {
                result = (Brush) b.ConvertFrom(value);
            }
            catch (Exception ex) {}
            return result;
        }
    }
}
