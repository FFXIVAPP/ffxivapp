// FFXIVAPP.Updater
// App.cs
// 
// © 2013 Ryan Wilson

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace FFXIVAPP.Updater
{
    public partial class App
    {
        #region Property Bindings

        #endregion

        private App()
        {
            Startup += ApplicationStartup;
            StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
            var resourceLocater = new Uri("/FFXIVAPP.Updater;component/App.xaml", UriKind.Relative);
            LoadComponent(this, resourceLocater);
            Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        /// <summary>
        ///     Application Entry Point.
        /// </summary>
        [STAThread]
        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        [LoaderOptimization(LoaderOptimization.MultiDomainHost)]
        public static void Main()
        {
            var app = new App();
            app.Run();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="startupEventArgs"> </param>
        private void ApplicationStartup(object sender, StartupEventArgs startupEventArgs)
        {
            if (startupEventArgs.Args.Length <= 0)
            {
                return;
            }
            Properties["DownloadUri"] = startupEventArgs.Args[0];
            Properties["Version"] = startupEventArgs.Args[1];
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="dispatcherUnhandledExceptionEventArgs"> </param>
        private static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs dispatcherUnhandledExceptionEventArgs)
        {
            dispatcherUnhandledExceptionEventArgs.Handled = true;
        }
    }
}
