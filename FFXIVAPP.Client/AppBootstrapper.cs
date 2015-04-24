// FFXIVAPP.Client
// AppBootstrapper.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Timers;
using FFXIVAPP.Client.Views;
using FFXIVAPP.Common.Utilities;
using NLog;

namespace FFXIVAPP.Client
{
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
            Initializer.LoadAvailableNetworkDevices();
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
