// Project: Launcher
// File: Settings.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System.ComponentModel;
using System.Configuration;

namespace Launcher
{
    internal sealed partial class Settings
    {
        /// <summary>
        /// Initialization
        /// </summary>
        private Settings()
        {
            SettingChanging += Settings_SettingChanging;
            PropertyChanged += Settings_PropertyChanged;
            SettingsLoaded += Settings_SettingsLoaded;
            SettingsSaving += Settings_SettingsSaving;
        }

        /// <summary>
        /// Raised before a setting's value is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Settings_SettingsSaving(object sender, CancelEventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Raised after a setting's value is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Settings_SettingsLoaded(object sender, SettingsLoadedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Raised after the setting values are loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Settings_SettingChanging(object sender, SettingChangingEventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Raised before the setting values are saved.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}