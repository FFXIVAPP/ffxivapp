// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AboutView.xaml.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   AboutView.xaml.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Views {
    /// <summary>
    ///     Interaction logic for AboutView.xaml
    /// </summary>
    public partial class AboutView {
        public static AboutView View;

        public AboutView() {
            this.InitializeComponent();
            View = this;
        }
    }
}