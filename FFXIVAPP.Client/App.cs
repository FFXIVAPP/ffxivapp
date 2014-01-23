// FFXIVAPP.Client
// App.cs
// 
// © 2013 Ryan Wilson

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
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.Utilities;
using NLog;
using NLog.Config;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client
{
    [DoNotObfuscate]
    public partial class App
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Property Bindings

        internal static readonly PluginHost Plugins = new PluginHost();

        public static string[] MArgs { get; private set; }

        #endregion

        private App()
        {
            Startup += ApplicationStartup;
            StartupUri = new Uri("ShellView.xaml", UriKind.Relative);
            var resourceLocater = new Uri("/FFXIVAPP.Client;component/App.xaml", UriKind.Relative);
            LoadComponent(this, resourceLocater);
            ConfigureNLog();
            Dispatcher.UnhandledException += OnDispatcherUnhandledException;
            Dispatcher.UnhandledExceptionFilter += OnUnhandledExceptionFilter;
            Settings.Default.PropertyChanged += SettingsPropertyChanged;
            Settings.Default.SettingChanging += SettingsSettingChanging;
            CheckSettings();
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
            Common.Constants.EnableNLog = Settings.Default.EnableNLog;
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
            }
            catch (Exception ex)
            {
                SettingsHelper.Default();
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
            using (var xmlReader = XmlReader.Create(stringReader))
            {
                LogManager.Configuration = new XmlLoggingConfiguration(xmlReader, null);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            var ex = e.Exception;
            Logging.Log(Logger, new LogItem("", ex, LogLevel.Error));
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnhandledExceptionFilter(object sender, DispatcherUnhandledExceptionFilterEventArgs e)
        {
            e.RequestCatch = true;
            var ex = e.Exception;
            Logging.Log(Logger, new LogItem("", ex, LogLevel.Error));
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private static void SettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Logging.Log(Logger, String.Format("PropertyChanged : {0}", e.PropertyName));
            switch (e.PropertyName)
            {
                case "CharacterName":
                    Constants.CharacterName = Settings.Default.CharacterName;
                    break;
                case "FirstName":
                    Initializer.SetCharacter();
                    break;
                case "LastName":
                    Initializer.SetCharacter();
                    break;
                case "GameLanguage":
                    Constants.GameLanguage = Settings.Default.GameLanguage;
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
                    Constants.ServerName = Settings.Default.ServerName;
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
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private static void SettingsSettingChanging(object sender, SettingChangingEventArgs e)
        {
            Logging.Log(Logger, String.Format("SettingChanging : [{0},{1}]", e.SettingKey, e.NewValue));
        }
    }
}
