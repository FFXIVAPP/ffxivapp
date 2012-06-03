// ParseModXIV
// App.xaml.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Windows;
using System.Windows.Threading;
using NLog;

namespace ParseModXIV
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static String[] MArgs;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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
            Logger.Error("ErrorEvent : {0}", e.Exception.Message + e.Exception.StackTrace + e.Exception.InnerException);
            e.Handled = true;
        }
    }
}