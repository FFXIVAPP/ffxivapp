// FFXIVAPP.Client
// MainViewModel.cs
// 
// © 2013 Ryan Wilson

using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Common.ViewModelBase;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.ViewModels
{
    [DoNotObfuscate]
    [Export(typeof (MainViewModel))]
    internal sealed class MainViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static MainViewModel _instance;

        public static MainViewModel Instance
        {
            get { return _instance ?? (_instance = new MainViewModel()); }
        }

        #endregion

        #region Declarations

        public DelegateCommand OpenWebSiteCommand { get; private set; }

        #endregion

        public MainViewModel()
        {
            OpenWebSiteCommand = new DelegateCommand(OpenWebSite);
        }

        #region Loading Functions

        #endregion

        #region Utility Functions

        #endregion

        #region Command Bindings

        public void OpenWebSite()
        {
            try
            {
                Process.Start("http://ffxiv-app.com");
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowMessage(AppViewModel.Instance.Locale["app_WarningMessage"], ex.Message);
            }
        }

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
