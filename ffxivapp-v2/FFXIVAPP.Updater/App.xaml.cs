// FFXIVAPP.Updater
// App.xaml.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.Windows;
using System.Windows.Threading;

#endregion

namespace FFXIVAPP.Updater
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Dispatcher.UnhandledException += DispatcherOnUnhandledException;
        }

        private static void DispatcherOnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs dispatcherUnhandledExceptionEventArgs)
        {
            dispatcherUnhandledExceptionEventArgs.Handled = true;
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length <= 0)
            {
                return;
            }
            Properties["DownloadUri"] = e.Args[0];
            Properties["Version"] = e.Args[1];
        }
    }
}
