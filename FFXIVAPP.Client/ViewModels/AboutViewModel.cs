// FFXIVAPP.Client
// AboutViewModel.cs
// 
// © 2013 Ryan Wilson

using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Threading;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.ViewModelBase;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.ViewModels
{
    [DoNotObfuscate]
    [Export(typeof (AboutViewModel))]
    internal sealed class AboutViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static AboutViewModel _instance;

        public static AboutViewModel Instance
        {
            get { return _instance ?? (_instance = new AboutViewModel()); }
        }

        #endregion

        #region Declarations

        public ICommand ManualUpdateCommand { get; private set; }

        #endregion

        public AboutViewModel()
        {
            ManualUpdateCommand = new DelegateCommand(ManualUpdate);
        }

        #region Loading Functions

        #endregion

        #region Utility Functions

        #endregion

        #region Command Bindings

        /// <summary>
        /// </summary>
        private void ManualUpdate()
        {
            DispatcherHelper.Invoke(() => ShellView.CloseApplication(true), DispatcherPriority.Send);
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
