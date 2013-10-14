// FFXIVAPP.Client
// App.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;
using FFXIVAPP.Client.Helpers;
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
                    Settings.Default.Reload();
                    return;
                }
                Settings.Default.Upgrade();
                Settings.Default.Reload();
                Settings.Default.Application_UpgradeRequired = false;
                Common.Constants.EnableNLog = Settings.Default.EnableNLog;
            }
            catch (Exception ex)
            {
                SettingsHelper.Default();
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
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
                    Common.Constants.CharacterName = Constants.CharacterName = Settings.Default.CharacterName;
                    break;
                case "FirstName":
                    Initializer.SetCharacter();
                    break;
                case "LastName":
                    Initializer.SetCharacter();
                    break;
                case "GameLanguage":
                    Common.Constants.GameLanguage = Constants.GameLanguage = Settings.Default.GameLanguage;
                    var lang = Settings.Default.GameLanguage.ToLower();
                    var cultureInfo = new CultureInfo("en");
                    switch (lang)
                    {
                        case "japanese":
                            cultureInfo = new CultureInfo("ja");
                            break;
                        case "german":
                            cultureInfo = new CultureInfo("de");
                            break;
                        case "french":
                            cultureInfo = new CultureInfo("fr");
                            break;
                    }
                    Constants.CultureInfo = Settings.Default.Culture = cultureInfo;
                    LocaleHelper.Update(Settings.Default.Culture);
                    break;
                case "ServerName":
                    Common.Constants.ServerName = Constants.ServerName = Settings.Default.ServerName;
                    break;
                case "EnableNLog":
                    Common.Constants.EnableNLog = Constants.EnableNLog = Settings.Default.EnableNLog;
                    break;
                case "EnableHelpLabels":
                    Constants.EnableHelpLabels = Settings.Default.EnableHelpLabels;
                    break;
                case "TopMost":
                    if (ShellView.View != null)
                    {
                        ShellView.View.Topmost = Settings.Default.TopMost;
                    }
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
