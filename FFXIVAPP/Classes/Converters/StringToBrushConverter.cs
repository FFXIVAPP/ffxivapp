// FFXIVAPP
// StringToBrushConverter.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using NLog;

namespace FFXIVAPP.Classes.Converters
{
    public class StringToBrushConverter : IValueConverter
    {
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
            catch
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
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var b = new BrushConverter();
            value = (value.ToString().Substring(0, 1) == "#") ? value : "#" + value;
            var result = (Brush) b.ConvertFrom("#FFFFFFFF");
            try
            {
                result = (Brush) b.ConvertFrom(value);
            }
            catch
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
            throw new NotImplementedException();
        }
    }
}