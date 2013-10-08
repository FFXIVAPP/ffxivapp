// FFXIVAPP.Client
// ShellViewModel.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;

#endregion

namespace FFXIVAPP.Client.ViewModels.Plugins.Event
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

        #endregion

        public ShellViewModel()
        {
            PluginInitializer.Event.LoadSettings();
            PluginInitializer.Event.LoadSounds();
            PluginInitializer.Event.LoadSoundEvents();
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
