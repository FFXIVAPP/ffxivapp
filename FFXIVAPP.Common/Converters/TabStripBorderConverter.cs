// FFXIVAPP.Client
// TabStripBorderConverter.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#endregion

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
