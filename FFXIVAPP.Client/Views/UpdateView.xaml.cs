﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateView.xaml.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   UpdateView.xaml.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Views {
    /// <summary>
    ///     Interaction logic for UpdateView.xaml
    /// </summary>
    public partial class UpdateView {
        public static UpdateView View;

        public UpdateView() {
            this.InitializeComponent();
            View = this;
        }
    }
}