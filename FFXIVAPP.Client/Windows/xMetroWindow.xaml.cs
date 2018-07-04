// --------------------------------------------------------------------------------------------------------------------
// <copyright file="xMetroWindow.xaml.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   xMetroWindow.xaml.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Windows {
    using System.Collections.Generic;
    using System.Windows;

    using FFXIVAPP.Client.Properties;
    using FFXIVAPP.Common.Helpers;

    using MahApps.Metro.Controls;

    /// <summary>
    ///     Interaction logic for xMetroWindow.xaml
    /// </summary>
    public partial class xMetroWindow {
        public xMetroWindow() {
            this.InitializeComponent();
        }

        private void XMetroWindow_OnLoaded(object sender, RoutedEventArgs e) {
            ThemeHelper.ChangeTheme(
                Settings.Default.Theme,
                new List<MetroWindow> {
                    this
                });
        }
    }
}