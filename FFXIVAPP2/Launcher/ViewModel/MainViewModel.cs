// Launcher
// MainViewModel.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AppModXIV.Classes;
using AppModXIV.Commands;
using Launcher.Classes;
using Launcher.View;
using Microsoft.Win32;

namespace Launcher.ViewModel
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private DelegateCommand _command;
        private readonly OpenFileDialog _ofd = new OpenFileDialog();

        #region " COMMAND FUNCTIONS "

        public ICommand GetHookCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(GetHook);

                return _command;
            }
        }

        public ICommand GetLaunchCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(GetLaunch);

                return _command;
            }
        }

        public ICommand LaunchCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(Launch);

                return _command;
            }
        }

        public ICommand Cb1Command
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(Cb1);

                return _command;
            }
        }

        public ICommand Cb2Command
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(Cb2);

                return _command;
            }
        }

        public ICommand Cb5Command
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(Cb5);

                return _command;
            }
        }

        public ICommand Cb6Command
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(Cb6);

                return _command;
            }
        }

        public ICommand Cb7Command
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(Cb7);

                return _command;
            }
        }

        public ICommand Cb8Command
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(Cb8);

                return _command;
            }
        }

        #endregion

        #region " GUI FUNCTIONS "

        private void GetHook()
        {
            if (_ofd.ShowDialog() == true)
            {
                MainView.View.gui_HookPath.Text = _ofd.FileName;
            }
        }

        private void GetLaunch()
        {
            if (_ofd.ShowDialog() == true)
            {
                MainView.View.gui_LaunchPath.Text = _ofd.FileName;
            }
        }

        private static void Launch()
        {
            MainWindow.View.WindowState = WindowState.Minimized;
            Globals.EndHook();
            Settings.Default.Save();
            LoadOptions();

            MainWindow.SetupHook();
            MainWindow.View.Launch();
        }

        private static void RedrawCheckbox()
        {
            SetFlags(BitField.Flag.F1, MainView.View.checkBox1);
            SetFlags(BitField.Flag.F2, MainView.View.checkBox2);
            SetFlags(BitField.Flag.F5, MainView.View.checkBox5);
            SetFlags(BitField.Flag.F6, MainView.View.checkBox6);
            SetFlags(BitField.Flag.F7, MainView.View.checkBox7);
            SetFlags(BitField.Flag.F8, MainView.View.checkBox8);
        }

        private static void SetFlags(BitField.Flag flag, CheckBox cb)
        {
            cb.IsChecked = MainWindow.BitField.AllOn(flag);
        }

        private static void Redraw()
        {
            Settings.Default.Flags = MainWindow.BitField.ToStringDec();
        }

        private static void Toggle(BitField.Flag flag)
        {
            MainWindow.BitField.SetToggle(flag);
        }

        private static void Cb1()
        {
            if (MainWindow.BitField.AllOn(BitField.Flag.F1) != MainView.View.checkBox1.IsChecked)
            {
                Toggle(BitField.Flag.F1);
            }
            Redraw();
        }

        private static void Cb2()
        {
            if (MainWindow.BitField.AllOn(BitField.Flag.F2) != MainView.View.checkBox2.IsChecked)
            {
                Toggle(BitField.Flag.F2);
            }
            Redraw();
        }

        private static void Cb5()
        {
            if (MainWindow.BitField.AllOn(BitField.Flag.F5) != MainView.View.checkBox5.IsChecked)
            {
                Toggle(BitField.Flag.F5);
            }
            Redraw();
        }

        private static void Cb6()
        {
            if (MainWindow.BitField.AllOn(BitField.Flag.F6) != MainView.View.checkBox6.IsChecked)
            {
                Toggle(BitField.Flag.F6);
            }
            Redraw();
        }

        private static void Cb7()
        {
            if (MainWindow.BitField.AllOn(BitField.Flag.F7) != MainView.View.checkBox7.IsChecked)
            {
                Toggle(BitField.Flag.F7);
            }
            Redraw();
        }

        private static void Cb8()
        {
            if (MainWindow.BitField.AllOn(BitField.Flag.F8) != MainView.View.checkBox8.IsChecked)
            {
                Toggle(BitField.Flag.F8);
            }
            Redraw();
        }

        public static void LoadOptions()
        {
            MainWindow.BitField.Mask = Convert.ToUInt64(Settings.Default.Flags, 10);
            MainWindow.BitField.Mask = MainWindow.BitField.Mask;
            RedrawCheckbox();
            Redraw();
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