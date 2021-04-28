// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   MainViewModel.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.ViewModels {
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    using FFXIVAPP.Client.Helpers;
    using FFXIVAPP.Common.ViewModelBase;

    [Export(typeof(MainViewModel)),]
    internal sealed class MainViewModel : INotifyPropertyChanged {
        private static Lazy<MainViewModel> _instance = new Lazy<MainViewModel>(() => new MainViewModel());

        public MainViewModel() {
            this.OpenWebSiteCommand = new DelegateCommand(this.OpenWebSite);
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public static MainViewModel Instance {
            get {
                return _instance.Value;
            }
        }

        public DelegateCommand OpenWebSiteCommand { get; private set; }

        public void OpenWebSite() {
            try {
                Process.Start("https://xivapp.com");
            }
            catch (Exception ex) {
                MessageBoxHelper.ShowMessage(AppViewModel.Instance.Locale["app_WarningMessage"], ex.Message);
            }
        }

        private void RaisePropertyChanged([CallerMemberName,] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
    }
}