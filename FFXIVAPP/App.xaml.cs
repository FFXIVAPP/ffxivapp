// FFXIVAPP
// App.xaml.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.
// 

using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;
using NLog;
using NLog.Config;

namespace FFXIVAPP
{
    /// <summary>
    ///   Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static String[] MArgs;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly string[] _directories = {"./Logs/", "./Resources/", "./ScreenShots/"};
        private readonly string[] _xmlResources = {"ATCodes.xml", "Colors.xml", "Settings.xml", "RegularExpressions.xml"};
        private readonly string[] _mainResources = {"FFXIVAPP.exe.nlog", "Ionic.Zip.dll", "Updater.exe"};
        private readonly string[] _cleanResources = {"Ionic.Zip.dll", "Updater.exe"};

        public App()
        {
            try
            {
                Settings.Default.Upgrade();
                Settings.Default.Reload();
            }
            catch
            {
                DefaultSettings();
            }
            Dispatcher.UnhandledException += OnDispatcherUnhandledException;
            Configure();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length > 0)
            {
                MArgs = e.Args;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Error("{0} :\n{1}", e.Exception.Message, e.Exception.StackTrace);
            e.Handled = true;
        }

        #region Startup

        /// <summary>
        /// </summary>
        private void Configure()
        {
            DirectoryStructure();
            ConfigureNLog();
            UpdateResources();
        }

        /// <summary>
        /// </summary>
        private void DirectoryStructure()
        {
            foreach (var item in _directories)
            {
                if (!Directory.Exists(item))
                {
                    Directory.CreateDirectory(item);
                }
            }
        }

        /// <summary>
        /// </summary>
        private void ConfigureNLog()
        {
            if (!File.Exists("./FFXIVAPP.exe.nlog"))
            {
                var resourceUri = new Uri("pack://application:,,,/FFXIVAPP;component/Resources/FFXIVAPP.exe.nlog");
                var resource = GetResourceStream(resourceUri);
                var sr = new StringReader(XElement.Load(resource.Stream).ToString());
                var xr = XmlReader.Create(sr);
                LogManager.Configuration = new XmlLoggingConfiguration(xr, null);
            }
        }

        /// <summary>
        /// </summary>
        private void UpdateResources()
        {
            foreach (var item in _cleanResources)
            {
                Delete(item, "./");
            }
            foreach (var item in _xmlResources)
            {
                Extract(item, "./Resources/");
            }
            foreach (var item in _mainResources)
            {
                Extract(item, "./");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="path"> </param>
        private void Delete(string name, string path)
        {
            var saved = path + name;
            try
            {
                if (File.Exists(saved))
                {
                    File.Delete(saved);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="path"> </param>
        private void Extract(string name, string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var saved = path + name;
            try
            {
                var resourceUri = new Uri("pack://application:,,,/FFXIVAPP;component/Resources/" + name);
                if (!File.Exists(saved))
                {
                    using (var s = GetResourceStream(resourceUri).Stream)
                    {
                        if (s == null)
                        {
                            Logger.Error("ErrorEvent : {0}", "Resource Is Null : '" + name + "'");
                            Current.Shutdown();
                        }
                        var buffer = new byte[s.Length];
                        s.Read(buffer, 0, buffer.Length);
                        using (var sw = new BinaryWriter(File.Open(saved, FileMode.Create)))
                        {
                            sw.Write(buffer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("{0} :\n{1}", ex.Message, ex.StackTrace);
                Current.Shutdown();
            }
        }

        #endregion

        #region Settings Control

        /// <summary>
        /// </summary>
        public static void DefaultSettings()
        {
            Settings.Default.Reset();
            Settings.Default.Reload();
            try
            {
                var p = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Assembly.GetExecutingAssembly().GetName().Name);
                Directory.Delete(p, true);
            }
            catch
            {
            }
            Settings.Default.Save();
        }

        #endregion
    }
}