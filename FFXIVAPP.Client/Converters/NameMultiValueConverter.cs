// FFXIVAPP.Client
// NameMultiValueConverter.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Globalization;
using System.Windows.Data;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Converters
{
    [DoNotObfuscate]
    public class NameMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return String.Format("{0} ({1})", values[0], values[1]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
