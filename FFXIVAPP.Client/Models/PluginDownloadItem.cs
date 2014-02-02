// FFXIVAPP.Client
// PluginDownloadItem.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models
{
    [DoNotObfuscate]
    public class PluginDownloadItem : INotifyPropertyChanged
    {
        private List<PluginFile> _files;
        private string _name;
        private string _sourceUri;
        private PluginStatus _status;
        private string _latestVersion;
        private string _currentVersion;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
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
