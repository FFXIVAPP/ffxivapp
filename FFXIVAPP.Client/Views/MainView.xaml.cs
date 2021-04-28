// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainView.xaml.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   MainView.xaml.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Views {
    /// <summary>
    ///     Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView {
        public static MainView View;

        public MainView() {
            this.InitializeComponent();
            View = this;
        }
    }
}