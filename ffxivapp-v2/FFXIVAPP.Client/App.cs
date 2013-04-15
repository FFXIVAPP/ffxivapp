// FFXIVAPP.Client
// App.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Models;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Utilities;
using NLog;
using NLog.Config;

#endregion

namespace FFXIVAPP.Client
{
    public partial class App
    {
        #region Property Bindings

        internal static readonly PluginContainer Plugins = new PluginContainer();

        public static string[] MArgs { get; private set; }

        #endregion

        private App()
        {
            Startup += ApplicationStartup;
            StartupUri = new Uri("ShellView.xaml", UriKind.Relative);
            var resourceLocater = new Uri("/FFXIVAPP.Client;component/App.xaml", UriKind.Relative);
            LoadComponent(this, resourceLocater);
            Dispatcher.UnhandledException += OnDispatcherUnhandledException;
            Settings.Default.PropertyChanged += SettingsPropertyChanged;
            Settings.Default.SettingChanging += SettingsSettingChanging;
            CheckSettings();
            ConfigureNLog();
            LoadPlugins();
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
            //if (AppDomain.CurrentDomain.FriendlyName != "FFXIVAPP.Shadowed")
            //{
            //    var domain = AppDomain.CreateDomain("FFXIVAPP.Shadowed");
            //    domain.DoCallBack(delegate
            //    {
            //        var app = new App();
            //        app.Run();
            //    });
            //}
            //var startupPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly()
            //                                                .Location);
            //var cachePath = Path.Combine(startupPath, "__cache");
            //var configFile = Path.Combine(startupPath, "FFXIVAPP.Client.exe.config");
            //var assembly = Path.Combine(startupPath, "FFXIVAPP.Client.exe");
            //var setup = new AppDomainSetup
            //{
            //    ApplicationName = "FFXIVAPP.Client",
            //    ShadowCopyFiles = "true",
            //    CachePath = cachePath,
            //    ConfigurationFile = configFile
            //};
            //var domain = AppDomain.CreateDomain("FFXIVAPP.Client", AppDomain.CurrentDomain.Evidence, setup);
            //domain.DoCallBack(delegate
            //{
            //    //domain.ExecuteAssembly(assembly);
            //    //AppDomain.Unload(domain);
            //    //Directory.Delete(cachePath, true);
            //    var app = new App();
            //    app.Run();
            //});
            var app = new App();
            app.Run();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private static void ApplicationStartup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length > 0)
            {
                MArgs = e.Args;
            }
        }

        private static void CheckSettings()
        {
            try
            {
                if (!Settings.Default.Application_UpgradeRequired)
                {
                    return;
                }
                Settings.Default.Upgrade();
                Settings.Default.Reload();
                Settings.Default.Application_UpgradeRequired = false;
            }
            catch (Exception ex)
            {
                SettingsHelper.Default();
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
            }
        }

        private static void LoadPlugins()
        {
            Plugins.LoadPlugins(Directory.GetCurrentDirectory() + @"\Plugins");
            foreach (PluginInstance pluginInstance in Plugins.Loaded)
            {
                try
                {
                    var tabItem = pluginInstance.Instance.CreateTab();
                    var iconfile = String.Format("{0}\\{1}", Path.GetDirectoryName(pluginInstance.AssemblyPath), pluginInstance.Instance.Icon);
                    var icon = new BitmapImage(new Uri(Common.Constants.DefaultIcon));
                    icon = File.Exists(iconfile) ? new BitmapImage(new Uri(iconfile)) : icon;
                    tabItem.HeaderTemplate = TabItemHelper.ImageHeader(icon, pluginInstance.Instance.Name);
                    var info = new Dictionary<string, string>();
                    info.Add("Icon", pluginInstance.Instance.Icon);
                    info.Add("Description", pluginInstance.Instance.Description);
                    info.Add("Copyright", pluginInstance.Instance.Copyright);
                    info.Add("Version", pluginInstance.Instance.Version);
                    AppViewModel.Instance.PluginTabItems.Add(tabItem);
                }
                catch (AppException ex)
                {
                    Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
                }
            }
        }

        /// <summary>
        /// </summary>
        private static void ConfigureNLog()
        {
            if (File.Exists("./FFXIVAPP.Client.exe.nlog"))
            {
                return;
            }
            var resource = ResourceHelper.StreamResource(Common.Constants.AppPack + "Resources/FFXIVAPP.Client.exe.nlog");
            if (resource == null)
            {
                return;
            }
            var stringReader = new StringReader(XElement.Load(resource.Stream)
                                                        .ToString());
            var xmlReader = XmlReader.Create(stringReader);
            LogManager.Configuration = new XmlLoggingConfiguration(xmlReader, null);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var ex = e.Exception;
            Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
            e.Handled = true;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private static void SettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("PropertyChanged : {0}", e.PropertyName));
            switch (e.PropertyName)
            {
                case "CharacterName":
                    Common.Constants.CharacterName = Settings.Default.CharacterName;
                    break;
                case "FirstName":
                    Initializer.SetCharacter();
                    break;
                case "LastName":
                    Initializer.SetCharacter();
                    break;
                case "GameLanguage":
                    Common.Constants.GameLanguage = Settings.Default.GameLanguage;
                    break;
                case "ServerName":
                    Common.Constants.ServerName = Settings.Default.ServerName;
                    try
                    {
                        Settings.Default.ServerNumber = Constants.ServerNumber[Settings.Default.ServerName];
                    }
                    catch (Exception ex)
                    {
                        //Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
                    }
                    break;
                case "ServerNumber":
                    Common.Constants.ServerNumber = Settings.Default.ServerNumber;
                    break;
                case "EnableNLog":
                    Common.Constants.EnableNLog = Settings.Default.EnableNLog;
                    break;
            }
            //SettingsHelper.Save();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private static void SettingsSettingChanging(object sender, SettingChangingEventArgs e)
        {
            Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("SettingChanging : [{0},{1}]", e.SettingKey, e.NewValue));
        }
    }
}
