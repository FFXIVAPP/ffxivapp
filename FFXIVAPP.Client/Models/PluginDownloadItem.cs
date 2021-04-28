// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PluginDownloadItem.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   PluginDownloadItem.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Models {
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    internal class PluginDownloadItem : INotifyPropertyChanged {
        private string _currentVersion;

        private string _description;

        private List<PluginFile> _files;

        private string _friendlyName;

        private string _latestVersion;

        private string _name;

        private string _sourceUri;

        private PluginStatus _status;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public string CurrentVersion {
            get {
                return this._currentVersion;
            }

            set {
                this._currentVersion = value;
                this.RaisePropertyChanged();
            }
        }

        public string Description {
            get {
                return this._description;
            }

            set {
                this._description = value;
                this.RaisePropertyChanged();
            }
        }

        public List<PluginFile> Files {
            get {
                return this._files ?? (this._files = new List<PluginFile>());
            }

            set {
                this._files = value;
                this.RaisePropertyChanged();
            }
        }

        public string FriendlyName {
            get {
                return this._friendlyName;
            }

            set {
                this._friendlyName = value;
                this.RaisePropertyChanged();
            }
        }

        public string LatestVersion {
            get {
                return this._latestVersion;
            }

            set {
                this._latestVersion = value;
                this.RaisePropertyChanged();
            }
        }

        public string Name {
            get {
                return this._name;
            }

            set {
                this._name = value;
                this.RaisePropertyChanged();
            }
        }

        public string SourceURI {
            get {
                return this._sourceUri;
            }

            set {
                this._sourceUri = value;
                this.RaisePropertyChanged();
            }
        }

        public PluginStatus Status {
            get {
                return this._status;
            }

            set {
                this._status = value;
                this.RaisePropertyChanged();
            }
        }

        private void RaisePropertyChanged([CallerMemberName,] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
    }
}