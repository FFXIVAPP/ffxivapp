// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PluginFile.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   PluginFile.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Models {
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    internal class PluginFile : INotifyPropertyChanged {
        private string _checksum;

        private string _location;

        private string _name;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public string Checksum {
            get {
                return this._checksum;
            }

            set {
                this._checksum = value;
                this.RaisePropertyChanged();
            }
        }

        public string Location {
            get {
                return this._location;
            }

            set {
                this._location = value;
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

        private void RaisePropertyChanged([CallerMemberName,] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
    }
}