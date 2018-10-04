// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PluginSourceItem.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   PluginSourceItem.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Models {
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class PluginSourceItem : INotifyPropertyChanged {
        private bool _enabled;

        private Guid _key;

        private string _sourceURI;

        public PluginSourceItem() {
            this.Enabled = true;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public bool Enabled {
            get {
                return this._enabled;
            }

            set {
                this._enabled = value;
                this.RaisePropertyChanged();
            }
        }

        public Guid Key {
            get {
                return this._key;
            }

            set {
                this._key = value;
                this.RaisePropertyChanged();
            }
        }

        public string SourceURI {
            get {
                return this._sourceURI;
            }

            set {
                this._sourceURI = value;
                this.RaisePropertyChanged();
            }
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
    }
}