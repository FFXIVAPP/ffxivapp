// Project: Launcher
// File: App.xaml.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using AppModXIV.Classes;

namespace Launcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static String[] MArgs;

        public App()
        {
            if (Settings.Default.Application_UpgradeRequired)
            {
                try
                {
                    Settings.Default.Upgrade();
                    Settings.Default.Reload();
                }
                catch
                {
                }
                finally
                {
                    Settings.Default.Application_UpgradeRequired = false;
                }
            }

            Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length > 0)
            {
                MArgs = e.Args;
            }
        }

        private static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if (Constants.LogErrors == 1)
            {
                ErrorLogging.LogError(e.Exception.Message + e.Exception.StackTrace + e.Exception.InnerException, "Launcher", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            }
            e.Handled = true;
        }
    }
}