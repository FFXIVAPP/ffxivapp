// FFXIVAPP.Plugin.Chat
// SampleViewModel.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FFXIVAPP.Common.ViewModelBase;

#endregion

namespace FFXIVAPP.Plugin.Chat.ViewModels
{
    internal sealed class SampleViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static SampleViewModel _instance;

        public static SampleViewModel Instance
        {
            get { return _instance ?? (_instance = new SampleViewModel()); }
        }

        #endregion

        #region Declarations

        public ICommand SampleCommand { get; private set; }
        public ICommand SampleCommandT { get; private set; }

        #endregion

        public SampleViewModel()
        {
            SampleCommand = new DelegateCommand(Sample);
            SampleCommandT = new DelegateCommand<object>(SampleT);
        }

        #region Loading Functions

        #endregion

        #region Utility Functions

        #endregion

        #region Command Bindings

        /// <summary>
        /// </summary>
        public static void Sample()
        {
            //do something here
        }

        /// <summary>
        /// </summary>
        public static void SampleT(object parameter)
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
