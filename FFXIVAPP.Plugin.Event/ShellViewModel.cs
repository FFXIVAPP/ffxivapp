// FFXIVAPP.Plugin.Event
// ShellViewModel.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

#endregion

namespace FFXIVAPP.Plugin.Event
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
            Initializer.LoadSettings();
            Initializer.LoadSounds();
            Initializer.LoadSoundEvents();
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
