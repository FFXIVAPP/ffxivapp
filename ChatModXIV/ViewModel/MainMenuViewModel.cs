// Project: ChatModXIV
// File: MainMenuViewModel.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using AppModXIV.Classes;
using AppModXIV.Commands;
using ChatModXIV.Classes;
using ChatModXIV.View;

namespace ChatModXIV.ViewModel
{
    internal class MainMenuViewModel : INotifyPropertyChanged
    {
        private DelegateCommand _command;

        #region " COMMAND FUNCTIONS "

        public ICommand DefaultSettingsCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(DefaultSettings);

                return _command;
            }
        }

        public ICommand ExitCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(Exit);

                return _command;
            }
        }

        public ICommand ConnectToggleCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(ConnectToggle);

                return _command;
            }
        }

        public ICommand StopLoggingCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(StopLogging);

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

        public ICommand SetProcessCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(SetProcess);

                return _command;
            }
        }

        public ICommand RefreshListCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(RefreshList);

                return _command;
            }
        }

        #endregion

        #region " GUI FUNCTIONS "

        private static void DefaultSettings()
        {
            Settings.Default.Reset();
            Settings.Default.Reload();
        }

        private static void Exit()
        {
            Application.Current.Shutdown();
        }

        private static void ConnectToggle()
        {
            Settings.Default.Save();
            if (MainWindow.View.Connected)
            {
                MainWindow.View.PreClose();
                MainWindow.View.UpdateControls(false);
            }
            else
            {
                if (Constants.FfxivOpen)
                {
                    MainWindow.View.Connect();
                }
                else
                {
                    MessageBox.Show("Please Open Final Fantasy XIV & Restart ChatModXIV");
                }
            }
        }

        private static void StopLogging()
        {
            MainMenuView.View.gui_StopLogging.IsChecked = !MainMenuView.View.gui_StopLogging.IsChecked;
        }

        private static void About()
        {
            MessageBox.Show("Created by Ryan Wilson.\nCopyright (c) 2010-2012, Ryan Wilson. All rights reserved.", "About!", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private static void SetProcess()
        {
            ChatMod.Instance.StopLogging();
            ChatMod.SetPid();
            ChatMod.Instance.StartLogging();
        }

        private static void RefreshList()
        {
            MainMenuView.View.gui_PIDSelect.Items.Clear();
            ChatMod.Instance.StopLogging();
            ChatMod.ResetPid();
            ChatMod.Instance.StartLogging();
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