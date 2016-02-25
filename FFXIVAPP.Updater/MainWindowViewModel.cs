// FFXIVAPP.Updater ~ MainWindowViewModel.cs
// 
// Copyright © 2007 - 2016 Ryan Wilson - All Rights Reserved
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;

namespace FFXIVAPP.Updater
{
    [Export(typeof (MainWindowViewModel))]
    internal sealed class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static MainWindowViewModel _instance;
        private string _downloadUri;
        private string _version;
        private string _zipFileName;

        public static MainWindowViewModel Instance
        {
            get { return _instance ?? (_instance = new MainWindowViewModel()); }
        }

        public string DownloadURI
        {
            get { return _downloadUri; }
            set
            {
                _downloadUri = value;
                RaisePropertyChanged();
            }
        }

        public string Version
        {
            get { return _version; }
            set
            {
                _version = value;
                RaisePropertyChanged();
            }
        }

        public string ZipFileName
        {
            get { return _zipFileName; }
            set
            {
                _zipFileName = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Declarations

        #endregion

        #region Loading Functions

        #endregion

        #region Utility Functions

        #endregion

        #region Command Bindings

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
