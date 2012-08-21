// FFXIVAPP
// MainWindowVM.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using FFXIVAPP.Classes;
using FFXIVAPP.Classes.Commands;
using FFXIVAPP.Classes.Helpers;

namespace FFXIVAPP.ViewModels
{
    public class MainWindowVM
    {
        private static string _currentView = "main";
        public ICommand SwitchViewCommand { get; private set; }
        public ICommand ScreenShotCommand { get; private set; }

        public MainWindowVM()
        {
            SwitchViewCommand = new DelegateCommand<string>(SwitchView);
            ScreenShotCommand = new DelegateCommand(ScreenShot);
        }

        #region GUI Functions

        /// <summary>
        /// </summary>
        /// <param name="t"> </param>
        public static void SwitchView(string t)
        {
            if (_currentView == t)
            {
                return;
            }
            CollapseView(_currentView);
            switch (t)
            {
                case "main":
                    MainWindow.View.MainView.Visibility = Visibility.Visible;
                    ThemeHelper.ChangeTheme(Settings.Default.Theme);
                    break;
                case "chat":
                    MainWindow.View.ChatView.Visibility = Visibility.Visible;
                    ThemeHelper.ChangeTheme(Settings.Default.ChatTheme);
                    break;
                case "log":
                    MainWindow.View.LogView.Visibility = Visibility.Visible;
                    ThemeHelper.ChangeTheme(Settings.Default.LogTheme);
                    break;
                case "parse":
                    MainWindow.View.ParseView.Visibility = Visibility.Visible;
                    ThemeHelper.ChangeTheme(Settings.Default.ParseTheme);
                    break;
                case "settings":
                    MainWindow.View.SettingsView.Visibility = Visibility.Visible;
                    ThemeHelper.ChangeTheme(Settings.Default.Theme);
                    break;
                case "about":
                    MainWindow.View.AboutView.Visibility = Visibility.Visible;
                    ThemeHelper.ChangeTheme(Settings.Default.Theme);
                    break;
            }
            _currentView = t;
        }

        /// <summary>
        /// </summary>
        private static void ScreenShot()
        {
            var fileName = MainWindow.View.Ipath + DateTime.Now.ToString("dd.MM.yyyy-HH.mm.ss_") + ".jpg";
            var screenshot = ScreenCapture.GetJpgImage(MainWindow.View, 1, 100);
            var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
            using (var s = new BinaryWriter(fileStream))
            {
                s.Write(screenshot);
                s.Close();
            }
        }

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="view"> </param>
        private static void CollapseView(string view)
        {
            switch (view)
            {
                case "main":
                    MainWindow.View.MainView.Visibility = Visibility.Collapsed;
                    break;
                case "chat":
                    MainWindow.View.ChatView.Visibility = Visibility.Collapsed;
                    break;
                case "log":
                    MainWindow.View.LogView.Visibility = Visibility.Collapsed;
                    break;
                case "parse":
                    MainWindow.View.ParseView.Visibility = Visibility.Collapsed;
                    break;
                case "settings":
                    SettingsVM.SaveCharacter();
                    MainWindow.View.SettingsView.Visibility = Visibility.Collapsed;
                    break;
                case "about":
                    MainWindow.View.AboutView.Visibility = Visibility.Collapsed;
                    break;
            }
        }
    }
}