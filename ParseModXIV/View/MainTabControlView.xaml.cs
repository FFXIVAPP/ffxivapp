// Project: ParseModXIV
// File: MainTabControlView.xaml.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ParseModXIV.View
{
    /// <summary>
    /// Interaction logic for MainTabControlViewModel.xaml
    /// </summary>
    public partial class MainTabControlView
    {
        public static MainTabControlView View;

        public MainTabControlView()
        {
            InitializeComponent();
            // Insert code required on object creation below this point.
            View = this;
        }

        //private void List_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Delete)
        //    {
        //        e.Handled = true;
        //    }
        //}

        #region " LISTBOX SORTING "

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sortBy"></param>
        /// <param name="direction"></param>
        /// <param name="lv"></param>
        private static void Sort(string sortBy, ListSortDirection direction, ItemsControl lv)
        {
            var dataView = CollectionViewSource.GetDefaultView(lv.ItemsSource);
            dataView.SortDescriptions.Clear();
            var sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }

        private GridViewColumnHeader _lastHeaderClicked;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortHandler(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;

            if (headerClicked == null)
            {
                return;
            }
            if (headerClicked.Role == GridViewColumnHeaderRole.Padding)
            {
                return;
            }
            ListSortDirection direction;
            if (headerClicked != _lastHeaderClicked)
            {
                direction = ListSortDirection.Ascending;
            }
            else
            {
                direction = _lastDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
            }
            var header = headerClicked.Column.Header as string;
            Sort(header, direction, e.Source as ListView);
            if (direction == ListSortDirection.Ascending)
            {
                headerClicked.Column.HeaderTemplate = Resources["HeaderTemplateArrowUp"] as DataTemplate;
            }
            else
            {
                headerClicked.Column.HeaderTemplate = Resources["HeaderTemplateArrowDown"] as DataTemplate;
            }
            if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
            {
                _lastHeaderClicked.Column.HeaderTemplate = null;
            }
            _lastHeaderClicked = headerClicked;
            _lastDirection = direction;
        }

        #endregion
    }
}