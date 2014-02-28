// FFXIVAPP.Client
// AppBootstrapper.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Timers;
using FFXIVAPP.Client.Views;
using FFXIVAPP.Common.Utilities;
using NLog;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client
{
    [DoNotObfuscate]
    internal class AppBootstrapper : INotifyPropertyChanged
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Property Bindings

        private static AppBootstrapper _instance;

        public static AppBootstrapper Instance
        {
            get { return _instance ?? (_instance = new AppBootstrapper()); }
        }

        #endregion

        #region Declarations

        #endregion

        /*main entry to app
         * used for:
         *  initializing settings
         *  configuring collections
         *  setting up dependencies
         */

        public Timer ProcessDetachCheckTimer = new Timer(30000);

        private AppBootstrapper()
        {
            if (App.MArgs != null)
            {
                foreach (var argument in App.MArgs)
                {
                    Logging.Log(Logger, String.Format("ArgumentProvided : {0}", argument));
                }
            }
            Constants.IsOpen = false;
            Constants.ProcessID = -1;
            //initialize application data
            AppViewModel.Instance.ConfigurationsPath = Common.Constants.ConfigurationsPath;
            AppViewModel.Instance.LogsPath = Common.Constants.LogsPath;
            AppViewModel.Instance.SavedLogsDirectoryList = new List<string>
            {
                "Say",
                "Shout",
                "Party",
                "Tell",
                "LS1",
                "LS2",
                "LS3",
                "LS4",
                "LS5",
                "LS6",
                "LS7",
                "LS8",
                "FC",
                "Yell"
            };
            AppViewModel.Instance.ScreenShotsPath = Common.Constants.ScreenShotsPath;
            AppViewModel.Instance.SoundsPath = Common.Constants.SoundsPath;
            AppViewModel.Instance.SettingsPath = Common.Constants.SettingsPath;
            AppViewModel.Instance.PluginsSettingsPath = Common.Constants.PluginsSettingsPath;

            #region Initial BootStrapping

            Initializer.SetupCurrentUICulture();
            Initializer.LoadChatCodes();
            Initializer.LoadAutoTranslate();
            Initializer.LoadActions();
            Initializer.LoadColors();
            Initializer.LoadApplicationSettings();
            Initializer.LoadAvailableAudioDevices();
            Initializer.LoadSoundsIntoCache();
            Initializer.LoadPlugins();

            #endregion

            ProcessDetachCheckTimer.Elapsed += ProcessDetachCheckTimerOnElapsed;
        }

        private void ProcessDetachCheckTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            try
            {
                switch (Constants.IsOpen)
                {
                    case true:
                        try
                        {
                            var CurrentFFXIV = Process.GetProcessById(Constants.ProcessID);
                        }
                        catch (ArgumentException ex)
                        {
                            Constants.IsOpen = false;
                        }
                        break;
                    case false:
                        SettingsView.View.PIDSelect.Items.Clear();
                        Initializer.StopMemoryWorkers();
                        Initializer.ResetProcessID();
                        Initializer.StartMemoryWorkers();
                        break;
                }
            }
            catch (Exception ex)
            {
            }
        }

        #region Loading Functions

        #endregion

        #region Utility Functions

        #endregion

        #region Command Bindings

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
