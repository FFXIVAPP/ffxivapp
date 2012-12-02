// FFXIVAPP.Common
// GridViewSort.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace FFXIVAPP.Common.Utilities
{
    public class GridViewSort
    {
        #region Public Attached Properties

        /// <summary>
        /// </summary>
        /// <param name="dependencyObject"> </param>
        /// <returns> </returns>
        public static ICommand GetCommand(DependencyObject dependencyObject)
        {
            return (ICommand) dependencyObject.GetValue(CommandProperty);
        }

        /// <summary>
        /// </summary>
        /// <param name="dependencyObject"> </param>
        /// <param name="command"> </param>
        public static void SetCommand(DependencyObject dependencyObject, ICommand command)
        {
            dependencyObject.SetValue(CommandProperty, command);
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof (ICommand), typeof (GridViewSort), new UIPropertyMetadata(null, (source, e) =>
        {
            var listView = source as ItemsControl;
            if (listView != null)
            {
                if (!GetAutoSort(listView)) // Don't change click handler if AutoSort enabled
                {
                    if (e.OldValue != null && e.NewValue == null)
                    {
                        listView.RemoveHandler(ButtonBase.ClickEvent, new RoutedEventHandler(ColumnHeaderClick));
                    }
                    if (e.OldValue == null && e.NewValue != null)
                    {
                        listView.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(ColumnHeaderClick));
                    }
                }
            }
        }));

        public static bool GetAutoSort(DependencyObject dependencyObject)
        {
            return (bool) dependencyObject.GetValue(AutoSortProperty);
        }

        public static void SetAutoSort(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(AutoSortProperty, value);
        }

        // Using a DependencyProperty as the backing store for AutoSort.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AutoSortProperty = DependencyProperty.RegisterAttached("AutoSort", typeof (bool), typeof (GridViewSort), new UIPropertyMetadata(false, (source, e) =>
        {
            var listView = source as ListView;
            if (listView != null)
            {
                if (GetCommand(listView) == null) // Don't change click handler if a command is set
                {
                    var previousValue = (bool) e.OldValue;
                    var newValue = (bool) e.NewValue;
                    if (previousValue && !newValue)
                    {
                        listView.RemoveHandler(ButtonBase.ClickEvent, new RoutedEventHandler(ColumnHeaderClick));
                    }
                    if (!previousValue && newValue)
                    {
                        listView.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(ColumnHeaderClick));
                    }
                }
            }
        }));

        public static string GetPropertyName(DependencyObject dependencyObject)
        {
            return (string) dependencyObject.GetValue(PropertyNameProperty);
        }

        public static void SetPropertyName(DependencyObject dependencyObject, string name)
        {
            dependencyObject.SetValue(PropertyNameProperty, name);
        }

        // Using a DependencyProperty as the backing store for PropertyName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.RegisterAttached("PropertyName", typeof (string), typeof (GridViewSort), new UIPropertyMetadata(null));

        public static bool GetShowSortGlyph(DependencyObject dependencyObject)
        {
            return (bool) dependencyObject.GetValue(ShowSortGlyphProperty);
        }

        public static void SetShowSortGlyph(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(ShowSortGlyphProperty, value);
        }

        // Using a DependencyProperty as the backing store for ShowSortGlyph.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowSortGlyphProperty = DependencyProperty.RegisterAttached("ShowSortGlyph", typeof (bool), typeof (GridViewSort), new UIPropertyMetadata(true));

        public static ImageSource GetSortGlyphAscending(DependencyObject dependencyObject)
        {
            return (ImageSource) dependencyObject.GetValue(SortGlyphAscendingProperty);
        }

        public static void SetSortGlyphAscending(DependencyObject dependencyObject, ImageSource value)
        {
            dependencyObject.SetValue(SortGlyphAscendingProperty, value);
        }

        // Using a DependencyProperty as the backing store for SortGlyphAscending.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SortGlyphAscendingProperty = DependencyProperty.RegisterAttached("SortGlyphAscending", typeof (ImageSource), typeof (GridViewSort), new UIPropertyMetadata(null));

        public static ImageSource GetSortGlyphDescending(DependencyObject dependencyObject)
        {
            return (ImageSource) dependencyObject.GetValue(SortGlyphDescendingProperty);
        }

        public static void SetSortGlyphDescending(DependencyObject dependencyObject, ImageSource source)
        {
            dependencyObject.SetValue(SortGlyphDescendingProperty, source);
        }

        // Using a DependencyProperty as the backing store for SortGlyphDescending.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SortGlyphDescendingProperty = DependencyProperty.RegisterAttached("SortGlyphDescending", typeof (ImageSource), typeof (GridViewSort), new UIPropertyMetadata(null));

        #endregion

        #region Private Attached Properties

        /// <summary>
        /// </summary>
        /// <param name="dependencyObject"> </param>
        /// <returns> </returns>
        private static GridViewColumnHeader GetSortedColumnHeader(DependencyObject dependencyObject)
        {
            return (GridViewColumnHeader) dependencyObject.GetValue(SortedColumnHeaderProperty);
        }

        /// <summary>
        /// </summary>
        /// <param name="dependencyObject"> </param>
        /// <param name="header"> </param>
        private static void SetSortedColumnHeader(DependencyObject dependencyObject, GridViewColumnHeader header)
        {
            dependencyObject.SetValue(SortedColumnHeaderProperty, header);
        }

        // Using a DependencyProperty as the backing store for SortedColumn.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty SortedColumnHeaderProperty = DependencyProperty.RegisterAttached("SortedColumnHeader", typeof (GridViewColumnHeader), typeof (GridViewSort), new UIPropertyMetadata(null));

        #endregion

        #region Column Header Click Event Handler

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private static void ColumnHeaderClick(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            if (headerClicked != null && headerClicked.Column != null)
            {
                var propertyName = GetPropertyName(headerClicked.Column);
                if (!string.IsNullOrEmpty(propertyName))
                {
                    var listView = GetAncestor<ListView>(headerClicked);
                    if (listView != null)
                    {
                        var command = GetCommand(listView);
                        if (command != null)
                        {
                            if (command.CanExecute(propertyName))
                            {
                                command.Execute(propertyName);
                            }
                        }
                        else if (GetAutoSort(listView))
                        {
                            ApplySort(listView.Items, propertyName, listView, headerClicked);
                        }
                    }
                }
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="dependencyObject"> </param>
        /// <returns> </returns>
        public static T GetAncestor<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);
            while (!(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return (T) parent;
        }

        /// <summary>
        /// </summary>
        /// <param name="view"> </param>
        /// <param name="propertyName"> </param>
        /// <param name="listView"> </param>
        /// <param name="sortedColumnHeader"> </param>
        public static void ApplySort(ICollectionView view, string propertyName, ListView listView, GridViewColumnHeader sortedColumnHeader)
        {
            var direction = ListSortDirection.Ascending;
            if (view.SortDescriptions.Count > 0)
            {
                var currentSort = view.SortDescriptions[0];
                if (currentSort.PropertyName == propertyName)
                {
                    if (currentSort.Direction == ListSortDirection.Ascending)
                    {
                        direction = ListSortDirection.Descending;
                    }
                    else
                    {
                        direction = ListSortDirection.Ascending;
                    }
                }
                view.SortDescriptions.Clear();

                var currentSortedColumnHeader = GetSortedColumnHeader(listView);
                if (currentSortedColumnHeader != null)
                {
                    RemoveSortGlyph(currentSortedColumnHeader);
                }
            }
            if (!string.IsNullOrEmpty(propertyName))
            {
                view.SortDescriptions.Add(new SortDescription(propertyName, direction));
                if (GetShowSortGlyph(listView))
                {
                    AddSortGlyph(sortedColumnHeader, direction, direction == ListSortDirection.Ascending ? GetSortGlyphAscending(listView) : GetSortGlyphDescending(listView));
                }
                SetSortedColumnHeader(listView, sortedColumnHeader);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="columnHeader"> </param>
        /// <param name="direction"> </param>
        /// <param name="sortGlyph"> </param>
        private static void AddSortGlyph(GridViewColumnHeader columnHeader, ListSortDirection direction, ImageSource sortGlyph)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(columnHeader);
            adornerLayer.Add(new SortGlyphAdorner(columnHeader, direction, sortGlyph));
        }

        /// <summary>
        /// </summary>
        /// <param name="columnHeader"> </param>
        private static void RemoveSortGlyph(GridViewColumnHeader columnHeader)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(columnHeader);
            var adorners = adornerLayer.GetAdorners(columnHeader);
            if (adorners != null)
            {
                foreach (var adorner in adorners.OfType<SortGlyphAdorner>())
                {
                    adornerLayer.Remove(adorner);
                }
            }
        }

        #endregion

        #region SortGlyphAdorner Nested Class

        /// <summary>
        /// </summary>
        private class SortGlyphAdorner : Adorner
        {
            private readonly GridViewColumnHeader _columnHeader;
            private readonly ListSortDirection _direction;
            private readonly ImageSource _sortGlyph;

            public SortGlyphAdorner(GridViewColumnHeader columnHeader, ListSortDirection direction, ImageSource sortGlyph) : base(columnHeader)
            {
                _columnHeader = columnHeader;
                _direction = direction;
                _sortGlyph = sortGlyph;
            }

            private Geometry GetDefaultGlyph()
            {
                var x1 = _columnHeader.ActualWidth - 13;
                var x2 = x1 + 10;
                var x3 = x1 + 5;
                var y1 = _columnHeader.ActualHeight/2 - 3;
                var y2 = y1 + 5;

                if (_direction == ListSortDirection.Ascending)
                {
                    var tmp = y1;
                    y1 = y2;
                    y2 = tmp;
                }

                var pathSegmentCollection = new PathSegmentCollection();
                pathSegmentCollection.Add(new LineSegment(new Point(x2, y1), true));
                pathSegmentCollection.Add(new LineSegment(new Point(x3, y2), true));

                var pathFigure = new PathFigure(new Point(x1, y1), pathSegmentCollection, true);

                var pathFigureCollection = new PathFigureCollection();
                pathFigureCollection.Add(pathFigure);

                var pathGeometry = new PathGeometry(pathFigureCollection);
                return pathGeometry;
            }

            protected override void OnRender(DrawingContext drawingContext)
            {
                base.OnRender(drawingContext);

                if (_sortGlyph != null)
                {
                    var x = _columnHeader.ActualWidth - 13;
                    var y = _columnHeader.ActualHeight/2 - 5;
                    var rect = new Rect(x, y, 10, 10);
                    drawingContext.DrawImage(_sortGlyph, rect);
                }
                else
                {
                    drawingContext.DrawGeometry(Brushes.LightGray, new Pen(Brushes.Gray, 1.0), GetDefaultGlyph());
                }
            }
        }

        #endregion
    }
}