// FFXIVAPP.Common
// ColumnDefinitionExtended.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Windows;
using System.Windows.Controls;

namespace FFXIVAPP.Common.WPF
{
    public class ColumnDefinitionExtended : ColumnDefinition
    {
        // Variables
        public static DependencyProperty VisibleProperty;

        // Properties

        // Constructors
        static ColumnDefinitionExtended()
        {
            VisibleProperty = DependencyProperty.Register("Visible", typeof (Boolean), typeof (ColumnDefinitionExtended), new PropertyMetadata(true, new PropertyChangedCallback(OnVisibleChanged)));

            WidthProperty.OverrideMetadata(typeof (ColumnDefinitionExtended), new FrameworkPropertyMetadata(new GridLength(1, GridUnitType.Star), null, new CoerceValueCallback(CoerceWidth)));

            MinWidthProperty.OverrideMetadata(typeof (ColumnDefinitionExtended), new FrameworkPropertyMetadata((Double) 0, null, new CoerceValueCallback(CoerceMinWidth)));
        }

        public Boolean Visible
        {
            get { return (Boolean) GetValue(VisibleProperty); }
            set { SetValue(VisibleProperty, value); }
        }

        // Get/Set
        public static void SetVisible(DependencyObject obj, Boolean nVisible)
        {
            obj.SetValue(VisibleProperty, nVisible);
        }

        public static Boolean GetVisible(DependencyObject obj)
        {
            return (Boolean) obj.GetValue(VisibleProperty);
        }

        private static void OnVisibleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            obj.CoerceValue(WidthProperty);
            obj.CoerceValue(MinWidthProperty);
        }

        private static Object CoerceWidth(DependencyObject obj, Object nValue)
        {
            return (((ColumnDefinitionExtended) obj).Visible) ? nValue : new GridLength(0);
        }

        private static Object CoerceMinWidth(DependencyObject obj, Object nValue)
        {
            return (((ColumnDefinitionExtended) obj).Visible) ? nValue : (Double) 0;
        }
    }
}
