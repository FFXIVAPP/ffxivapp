// Project: ParseModXIV
// File: MainToolBarViewModel.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using AppModXIV.Commands;
using ParseModXIV.View;

namespace ParseModXIV.ViewModel
{
    internal class MainToolBarViewModel : INotifyPropertyChanged
    {
        private DelegateCommand _command;

        #region " COMMAND FUNCTIONS "

        public ICommand OptionsCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(Options);

                return _command;
            }
        }

        public ICommand MinimalCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(Minimal);

                return _command;
            }
        }

        public ICommand FullCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(Full);

                return _command;
            }
        }

        public ICommand MinimizeCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(Minimize);

                return _command;
            }
        }

        public ICommand RestoreCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(Restore);

                return _command;
            }
        }

        public ICommand MaximizeCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(Maximize);

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

        private static void Options()
        {
        }

        private static void Minimal()
        {
            MainMenuView.View.LayoutRoot.Visibility = Visibility.Collapsed;
            MainStatusView.View.LayoutRoot.Visibility = Visibility.Collapsed;

            MainTabControlView.View.gui_TabControl.ItemContainerStyle = (Style) MainTabControlView.View.FindResource("TabItemCollapsed");
        }

        private static void Full()
        {
            MainMenuView.View.LayoutRoot.Visibility = Visibility.Visible;
            MainStatusView.View.LayoutRoot.Visibility = Visibility.Visible;
            try
            {
                MainTabControlView.View.gui_TabControl.ItemContainerStyle = (Style) MainTabControlView.View.FindResource("TabItemVisible");
            }
            catch
            {
                var s = new Style();
                s.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Visible));
                MainTabControlView.View.gui_TabControl.ItemContainerStyle = s;
            }
        }

        private static void Minimize()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private static void Restore()
        {
            Application.Current.MainWindow.WindowState = WindowState.Normal;
            MainToolBarView.View.gui_Maximize.Visibility = Visibility.Visible;
            MainToolBarView.View.gui_Restore.Visibility = Visibility.Collapsed;
        }

        private void Maximize()
        {
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
            MainToolBarView.View.gui_Maximize.Visibility = Visibility.Collapsed;
            MainToolBarView.View.gui_Restore.Visibility = Visibility.Visible;
        }

        private static void Close()
        {
            Application.Current.Shutdown();
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