// FFXIVAPP.Client ~ PluginDownloadItem.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
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

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FFXIVAPP.Client.Models
{
    public class PluginDownloadItem : INotifyPropertyChanged
    {
        private string _currentVersion;
        private string _description;
        private List<PluginFile> _files;
        private string _friendlyName;
        private string _latestVersion;
        private string _name;
        private string _sourceUri;
        private PluginStatus _status;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        public string FriendlyName
        {
            get { return _friendlyName; }
            set
            {
                _friendlyName = value;
                RaisePropertyChanged();
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged();
            }
        }

        public string CurrentVersion
        {
            get { return _currentVersion; }
            set
            {
                _currentVersion = value;
                RaisePropertyChanged();
            }
        }

        public string LatestVersion
        {
            get { return _latestVersion; }
            set
            {
                _latestVersion = value;
                RaisePropertyChanged();
            }
        }

        public List<PluginFile> Files
        {
            get { return _files ?? (_files = new List<PluginFile>()); }
            set
            {
                _files = value;
                RaisePropertyChanged();
            }
        }

        public PluginStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                RaisePropertyChanged();
            }
        }

        public string SourceURI
        {
            get { return _sourceUri; }
            set
            {
                _sourceUri = value;
                RaisePropertyChanged();
            }
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
