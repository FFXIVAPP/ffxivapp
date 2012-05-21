using System;
using System.Windows;
using System.Windows.Threading;
using NLog;

namespace UpdateModXIV
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public App()
        {
            Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        private static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Error("ErrorEvent : {0}", e.Exception.Message + e.Exception.StackTrace + e.Exception.InnerException);
            e.Handled = true;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length > 0)
            {
                this.Properties["file"] = e.Args[0];
            }
        }
    }
}
