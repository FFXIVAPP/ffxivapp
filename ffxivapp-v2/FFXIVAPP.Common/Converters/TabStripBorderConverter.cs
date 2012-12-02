// FFXIVAPP.Common
// TabStripBorderConverter.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FFXIVAPP.Common.Converters
{
    public class TabStripBorderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var thickness = new Thickness();
            switch (value.ToString())
            {
                case "Left":
                    thickness.Left = 1;
                    break;
                case "Right":
                    thickness.Right = 1;
                    break;
                case "Top":
                    thickness.Top = 1;
                    break;
                case "Bottom":
                    thickness.Bottom = 1;
                    break;
            }
            return thickness;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}