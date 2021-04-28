// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   App.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Updater {
    using System;
    using System.CodeDom.Compiler;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Threading;

    using NLog;

    public partial class App {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private App() {
            this.Startup += this.ApplicationStartup;
            this.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
            var resourceLocater = new Uri("/FFXIVAPP.Updater;component/App.xaml", UriKind.Relative);
            LoadComponent(this, resourceLocater);
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
            this.Dispatcher.UnhandledExceptionFilter += this.DispatcherOnUnhandledExceptionFilter;
        }

        /// <summary>
        ///     Application Entry Point.
        /// </summary>
        [STAThread]
        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        [LoaderOptimization(LoaderOptimization.MultiDomainHost)]
        public static void Main() {
            var app = new App();
            app.Run();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) {
            e.Handled = true;
            MessageBox.Show(e.Exception.Message);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="startupEventArgs"> </param>
        private void ApplicationStartup(object sender, StartupEventArgs startupEventArgs) {
            if (startupEventArgs.Args.Length <= 0) {
                return;
            }

            this.Properties["DownloadUri"] = startupEventArgs.Args[0];
            this.Properties["Version"] = startupEventArgs.Args[1];
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DispatcherOnUnhandledExceptionFilter(object sender, DispatcherUnhandledExceptionFilterEventArgs e) {
            e.RequestCatch = true;
            MessageBox.Show(e.Exception.Message);
        }
    }
}