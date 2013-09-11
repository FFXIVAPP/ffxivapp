// FFXIVAPP.Plugin.Parse
// ShellViewModel.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using FFXIVAPP.Common.Helpers;

#endregion

namespace FFXIVAPP.Plugin.Parse
{
    public sealed class ShellViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static ShellViewModel _instance;

        public static ShellViewModel Instance
        {
            get { return _instance ?? (_instance = new ShellViewModel()); }
        }

        #endregion

        #region Declarations

        #endregion

        public ShellViewModel()
        {
            Initializer.LoadConstants();
            Initializer.LoadSettings();
            Initializer.LoadPlayerRegEx();
            Initializer.LoadMonsterRegEx();
        }

        internal static void Loaded(object sender, RoutedEventArgs e)
        {
            Initializer.ApplyTheming();
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
