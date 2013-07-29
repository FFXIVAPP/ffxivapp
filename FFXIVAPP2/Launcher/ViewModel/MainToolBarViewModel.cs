// Launcher
// MainToolBarViewModel.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using AppModXIV.Commands;

namespace Launcher.ViewModel
{
    internal class MainToolBarViewModel
    {
        private DelegateCommand _command;

        #region " COMMAND FUNCTIONS "

        public ICommand MinimizeCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(Minimize);

                return _command;
            }
        }

        public ICommand CloseCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(Close);

                return _command;
            }
        }

        #endregion

        #region " GUI FUNCTIONS "

        private static void Minimize()
        {
            MainWindow.View.WindowState = WindowState.Minimized;
        }

        private static void Close()
        {
            MainWindow.View.Close();
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