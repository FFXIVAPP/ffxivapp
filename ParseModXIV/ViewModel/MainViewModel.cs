// ParseModXIV
// MainViewModel.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using AppModXIV.Commands;

namespace ParseModXIV.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private DelegateCommand _command;

        #region " COMMAND FUNCTIONS "

        public ICommand MainCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(Main);

                return _command;
            }
        }

        public ICommand SettingsCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(Settings);

                return _command;
            }
        }

        public ICommand AboutCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(About);

                return _command;
            }
        }

        #endregion

        #region " GUI FUNCTIONS "

        private static void Main()
        {
            MainView.View.SettingsView.Visibility = Visibility.Collapsed;
            MainView.View.TabControlView.Visibility = Visibility.Visible;
            Character();
        }

        private static void Settings()
        {
            MainView.View.SettingsView.Visibility = Visibility.Visible;
            MainView.View.TabControlView.Visibility = Visibility.Collapsed;
            Character();
        }

        private static void About()
        {
            MainView.View.SettingsView.Visibility = Visibility.Collapsed;
            MainView.View.TabControlView.Visibility = Visibility.Collapsed;
            Character();
        }

        private static void Character()
        {
            SettingsViewModel.Character();
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