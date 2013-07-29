using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

using ParseModXIV.Classes;
using System.IO;
using System.Reflection;
using AppModXIV;

namespace ParseModXIV
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
                AppModXIV.ErrorLogging.LogError(e.Exception.Message + e.Exception.StackTrace + e.Exception.InnerException, "ParseModXIV", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            }
            e.Handled = true;
        }

        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    if (e.Args != null && e.Args.Count() > 0)
        //    {
        //        this.Properties["ArbitraryArgName"] = e.Args[0];

        //    }
        //    base.OnStartup(e);
        //}
    }
}
