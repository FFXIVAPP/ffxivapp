// Project: LogModXIV
// File: MainMenuViewModel.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using AppModXIV.Commands;
using LogModXIV.Classes;
using LogModXIV.View;

namespace LogModXIV.ViewModel
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

        public ICommand SaveLogCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(SaveLog);

                return _command;
            }
        }

        public ICommand ShowAllCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(ShowAll);

                return _command;
            }
        }

        public ICommand ShowDebugCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(ShowDebug);

                return _command;
            }
        }

        public ICommand ShowAsciiCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(ShowAscii);

                return _command;
            }
        }

        public ICommand TopMostCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(TopMost);

                return _command;
            }
        }

        public ICommand OptionsCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(Options);

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

        public ICommand LogFontCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(LogFont);

                return _command;
            }
        }

        public ICommand ChatlogBgColorCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(ChatlogBgColor);

                return _command;
            }
        }

        public ICommand TimeStampColorCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(TimeStampColor);

                return _command;
            }
        }

        public ICommand TranslateCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(Translate);

                return _command;
            }
        }

        public ICommand TranslateToEchoCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(TranslateToEcho);

                return _command;
            }
        }

        public ICommand SendToGameCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(SendToGame);

                return _command;
            }
        }

        public ICommand SendRomanizationCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(SendRomanization);

                return _command;
            }
        }

        public ICommand SayCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(Say);

                return _command;
            }
        }

        public ICommand TellCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(Tell);

                return _command;
            }
        }

        public ICommand PartyCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(Party);

                return _command;
            }
        }

        public ICommand LsCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(Ls);

                return _command;
            }
        }

        public ICommand ShoutCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(Shout);

                return _command;
            }
        }

        public ICommand TranslateJpOnlyCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(TranslateJpOnly);

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

        private static void SaveLog()
        {
            Settings.Default.Gui_SaveLog = !Settings.Default.Gui_SaveLog;
        }

        private static void ShowAll()
        {
            Settings.Default.Gui_ShowAll = !Settings.Default.Gui_ShowAll;
            if (Settings.Default.Gui_ShowAll)
            {
                Settings.Default.Gui_AllVisibility = Visibility.Visible;
            }
            else
            {
                Settings.Default.Gui_AllVisibility = Visibility.Collapsed;
                MainTabControlView.View.gui_TabControl.SelectedIndex = 1;
            }
        }

        private static void ShowDebug()
        {
            Settings.Default.Gui_ShowDebug = !Settings.Default.Gui_ShowDebug;
            if (Settings.Default.Gui_ShowDebug)
            {
                Settings.Default.Gui_DebugVisibility = Visibility.Visible;
            }
            else
            {
                Settings.Default.Gui_DebugVisibility = Visibility.Collapsed;
                MainTabControlView.View.gui_TabControl.SelectedIndex = 1;
            }
        }

        private static void ShowAscii()
        {
            Settings.Default.Gui_ShowASCII = !Settings.Default.Gui_ShowASCII;
        }

        private static void TopMost()
        {
            Settings.Default.TopMost = !Settings.Default.TopMost;
        }

        private static void Options()
        {
        }

        private static void StopLogging()
        {
            MainMenuView.View.gui_StopLogging.IsChecked = !MainMenuView.View.gui_StopLogging.IsChecked;
        }

        private static void LogFont()
        {
        }

        private static void ChatlogBgColor()
        {
        }

        private static void TimeStampColor()
        {
        }

        private static void Translate()
        {
            Settings.Default.Gui_Translate = !Settings.Default.Gui_Translate;
        }

        private static void TranslateToEcho()
        {
            Settings.Default.Gui_TranslateToEcho = !Settings.Default.Gui_TranslateToEcho;
        }

        private static void SendToGame()
        {
            Settings.Default.Gui_SendToGame = !Settings.Default.Gui_SendToGame;
        }

        private static void SendRomanization()
        {
            Settings.Default.Gui_SendRomanization = !Settings.Default.Gui_SendRomanization;
        }

        private static void Say()
        {
            Settings.Default.Gui_TSay = !Settings.Default.Gui_TSay;
        }

        private static void Tell()
        {
            Settings.Default.Gui_TTell = !Settings.Default.Gui_TTell;
        }

        private static void Party()
        {
            Settings.Default.Gui_TParty = !Settings.Default.Gui_TParty;
        }

        private static void Ls()
        {
            Settings.Default.Gui_TLS = !Settings.Default.Gui_TLS;
        }

        private static void Shout()
        {
            Settings.Default.Gui_TShout = !Settings.Default.Gui_TShout;
        }

        private static void TranslateJpOnly()
        {
            Settings.Default.Gui_TranslateJPOnly = !Settings.Default.Gui_TranslateJPOnly;
        }

        private static void About()
        {
            MessageBox.Show("Created by Ryan Wilson.\nCopyright (c) 2010-2012, Ryan Wilson. All rights reserved.", "About!", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private static void SetProcess()
        {
            LogMod.Instance.StopLogging();
            LogMod.SetPid();
            LogMod.Instance.StartLogging();
        }

        private static void RefreshList()
        {
            MainMenuView.View.gui_PIDSelect.Items.Clear();
            LogMod.Instance.StopLogging();
            LogMod.ResetPid();
            LogMod.Instance.StartLogging();
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