// FFXIVAPP.Client
// ShellViewModel.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FFXIVAPP.Common.ViewModelBase;

#endregion

namespace FFXIVAPP.Client.ViewModels.Plugins.Parse
{
    [Export(typeof (ShellViewModel))]
    internal sealed class ShellViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static ShellViewModel _instance;

        public static ShellViewModel Instance
        {
            get { return _instance ?? (_instance = new ShellViewModel()); }
        }

        #endregion

        #region Declarations

        public ICommand DefaultCommand { get; private set; }
        public ICommand DefaultCommandT { get; private set; }

        #endregion

        public ShellViewModel()
        {
            PluginInitializer.Parse.LoadSettings();
            PluginInitializer.Parse.LoadPlayerRegEx();
            PluginInitializer.Parse.LoadMonsterRegEx();
        }

        #region Loading Functions

        #endregion

        #region Utility Functions

        #endregion

        #region Command Bindings

        /// <summary>
        /// </summary>
        public static void Default()
        {
            //do something here
        }

        /// <summary>
        /// </summary>
        public static void DefaultT(object parameter)
        {
            //do something here
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
