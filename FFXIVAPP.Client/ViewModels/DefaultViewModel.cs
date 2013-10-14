// FFXIVAPP.Client
// DefaultViewModel.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FFXIVAPP.Common.ViewModelBase;

#endregion

namespace FFXIVAPP.Client.ViewModels {
    [Export(typeof (DefaultViewModel))]
    internal sealed class DefaultViewModel : INotifyPropertyChanged {
        #region Property Bindings

        private static DefaultViewModel _instance;

        public static DefaultViewModel Instance {
            get { return _instance ?? (_instance = new DefaultViewModel()); }
        }

        #endregion

        #region Declarations

        public ICommand DefaultCommand { get; private set; }
        public ICommand DefaultCommandT { get; private set; }

        #endregion

        public DefaultViewModel() {
            DefaultCommand = new DelegateCommand(Default);
            DefaultCommandT = new DelegateCommand<object>(DefaultT);
        }

        #region Loading Functions

        #endregion

        #region Utility Functions

        #endregion

        #region Command Bindings

        /// <summary>
        /// </summary>
        public static void Default() {
            //do something here
        }

        /// <summary>
        /// </summary>
        public static void DefaultT(object parameter) {
            //do something here
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
