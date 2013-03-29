// FFXIVAPP.Client
// AppBootstrapper.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Utilities;
using NLog;

#endregion

namespace FFXIVAPP.Client
{
    internal class AppBootstrapper : INotifyPropertyChanged
    {
        #region Property Bindings

        private static AppBootstrapper _instance;

        public static AppBootstrapper Instance
        {
            get { return _instance ?? (_instance = new AppBootstrapper()); }
        }

        public static bool HasPlugins
        {
            get { return App.Plugins.Loaded.Count > 0; }
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

        private AppBootstrapper()
        {
            if (App.MArgs != null)
            {
                foreach (var s in App.MArgs)
                {
                    Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("ArgumentProvided : {0}", s));
                }
            }
            Common.Constants.IsOpen = false;
            Common.Constants.ProcessID = -1;
            //initialize application data
            AppViewModel.Instance.ConfigurationsPath = "./Configurations/";
            AppViewModel.Instance.LogsPath = "./Logs/";
            AppViewModel.Instance.ScreenShotsPath = "./ScreenShots/";
            Initializer.SetupCurrentUICulture();
            Initializer.LoadChatCodes();
            Initializer.LoadAutoTranslate();
            Initializer.LoadColors();
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
