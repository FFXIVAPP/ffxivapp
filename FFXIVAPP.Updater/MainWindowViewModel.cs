// FFXIVAPP.Updater
// MainWindowViewModel.cs
// 
// © 2013 Ryan Wilson

using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Updater
{
    [DoNotObfuscate]
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

        public MainWindowViewModel()
        {
        }

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
