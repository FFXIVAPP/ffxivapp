using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

using System.IO;
using System.Reflection;
using AppModXIV;

namespace LogModXIV
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
            : base()
        {
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (Constants.LogErrors == 1)
            {
                AppModXIV.ErrorLogging.LogError(e.Exception.Message + e.Exception.StackTrace + e.Exception.InnerException, "LogModXIV", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            }
            e.Handled = true;
        }
    }
}
