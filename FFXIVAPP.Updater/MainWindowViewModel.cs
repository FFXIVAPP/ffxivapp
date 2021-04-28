// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   MainWindowViewModel.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Updater {
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.Runtime.CompilerServices;

    [Export(typeof(MainWindowViewModel))]
    internal sealed class MainWindowViewModel : INotifyPropertyChanged {
        private static Lazy<MainWindowViewModel> _instance = new Lazy<MainWindowViewModel>(() => new MainWindowViewModel());

        private string _downloadUri;

        private string _version;

        private string _zipFileName;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public static MainWindowViewModel Instance {
            get {
                return _instance.Value;
            }
        }

        public string DownloadURI {
            get {
                return this._downloadUri;
            }

            set {
                this._downloadUri = value;
                this.RaisePropertyChanged();
            }
        }

        public string Version {
            get {
                return this._version;
            }

            set {
                this._version = value;
                this.RaisePropertyChanged();
            }
        }

        public string ZipFileName {
            get {
                return this._zipFileName;
            }

            set {
                this._zipFileName = value;
                this.RaisePropertyChanged();
            }
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
    }
}