// ParseModXIV
// TabControlViewModel.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using AppModXIV.Commands;
using ParseModXIV.UserControls;

namespace ParseModXIV.ViewModel
{
    internal class TabControlViewModel : INotifyPropertyChanged
    {
        private DelegateCommand _command;

        #region " COMMAND FUNCTIONS "

        public ICommand EmptyCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(Empty);

                return _command;
            }
        }

        #endregion

        #region " GUI FUNCTIONS "

        private static void Empty()
        {
            
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion
    }
}