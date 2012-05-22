using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ParseModXIV.Classes
{
    static class SortHandler
    {
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

        private static GridViewColumnHeader _lastHeaderClicked;
        private static ListSortDirection _lastDirection = ListSortDirection.Ascending;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Handler(object sender, RoutedEventArgs e)
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
            //if (direction == ListSortDirection.Ascending)
            //{
            //    headerClicked.Column.HeaderTemplate = Resources["HeaderTemplateArrowUp"] as DataTemplate;
            //}
            //else
            //{
            //    headerClicked.Column.HeaderTemplate = Resources["HeaderTemplateArrowDown"] as DataTemplate;
            //}
            if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
            {
                _lastHeaderClicked.Column.HeaderTemplate = null;
            }
            _lastHeaderClicked = headerClicked;
            _lastDirection = direction;
        }
    }
}
