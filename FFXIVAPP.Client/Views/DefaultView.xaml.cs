// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultView.xaml.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2020 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   DefaultView.xaml.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Views {
    /// <summary>
    ///     Interaction logic for DefaultView.xaml
    /// </summary>
    public partial class DefaultView {
        public static DefaultView View;

        public DefaultView() {
            this.InitializeComponent();
            View = this;
        }
    }
}