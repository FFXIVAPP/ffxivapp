// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Settings.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Settings.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.SettingsProviders.Application {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Xml.Linq;
    using FFXIVAPP.Client;
    using FFXIVAPP.Client.Models;
    using FFXIVAPP.Client.ViewModels;
    using FFXIVAPP.Common.Helpers;
    using FFXIVAPP.Common.Models;
    using FFXIVAPP.Common.Utilities;
    using Newtonsoft.Json;
    using NLog;

    internal static class Settings {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static Lazy<SettingModel> _default = new Lazy<SettingModel>(() => {
            if (string.IsNullOrEmpty(settingsPath)) {
                settingsPath = Path.Combine(FFXIVAPP.Common.Constants.SettingsPath, "ApplicationSettings.json");
            }

            var config = new SettingModel();
            if (File.Exists(settingsPath)) {
                config = JsonConvert.DeserializeObject<SettingModel>(File.ReadAllText(settingsPath));
            }

            return config;
        });

        private static string settingsPath;

        public static SettingModel Default {
            get {
                return _default.Value;
            }
        }

        public static void Reload() {
            Default.DefaultSettings();
        }

        public static void Reset() {
            DefaultSettings();
            /* TODO: Settings.Reset
            foreach (var key in Constants.Settings) {
                SettingsProperty settingsProperty = Default.Properties[key];
                if (settingsProperty == null) {
                    continue;
                }

                var value = settingsProperty.DefaultValue.ToString();
                SetValue(key, value, CultureInfo.InvariantCulture);
            }
            */
        }

        public static void Save() {
            File.WriteAllText(settingsPath, JsonConvert.SerializeObject(Default, Formatting.Indented));
        }

        private static void DefaultSettings() {
            Default.DefaultSettings();
        }
    }
}