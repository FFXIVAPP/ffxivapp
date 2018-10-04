using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Sharlayan.Models;

namespace FFXIVAPP.Client.Converters
{
    public class ProcessModelToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ProcessModel)
            {
                var p = (ProcessModel)value;
                var bits = p.IsWin64 ? "64-Bit" : "32-Bit";
                return $"[{p.Process?.Id}] {bits}";
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new BindingNotification(new Exception("Not possible to convert back to ProcessModel"), BindingErrorType.Error);
        }
    }
}