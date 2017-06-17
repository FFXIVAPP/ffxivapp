// FFXIVAPP.Updater ~ App.cs
// 
// Copyright © 2007 - 2016 Ryan Wilson - All Rights Reserved
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using NLog;

namespace FFXIVAPP.Updater
{
    public partial class App
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        private App()
        {
            Startup += ApplicationStartup;
            StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
            var resourceLocater = new Uri("/FFXIVAPP.Updater;component/App.xaml", UriKind.Relative);
            LoadComponent(this, resourceLocater);
            Dispatcher.UnhandledException += OnDispatcherUnhandledException;
            Dispatcher.UnhandledExceptionFilter += DispatcherOnUnhandledExceptionFilter;
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
        /// <param name="e"> </param>
        private static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            MessageBox.Show(e.Exception.Message);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DispatcherOnUnhandledExceptionFilter(object sender, DispatcherUnhandledExceptionFilterEventArgs e)
        {
            e.RequestCatch = true;
            MessageBox.Show(e.Exception.Message);
        }

        #region Property Bindings

        #endregion
    }
}
