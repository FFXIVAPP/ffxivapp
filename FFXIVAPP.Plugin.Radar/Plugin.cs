// FFXIVAPP.Plugin.Radar
// Plugin.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using FFXIVAPP.Common.Events;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.IPluginInterface;
using FFXIVAPP.Localization;
using FFXIVAPP.Plugin.Radar.Properties;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Plugin.Radar
{
    [DoNotObfuscate]
    [Export(typeof (IPlugin))]
    public class Plugin : IPlugin, INotifyPropertyChanged
    {
        #region Property Bindings

        public static IPluginHost PHost { get; private set; }
        public static string PName { get; private set; }

        #endregion

        #region Declarations

        #endregion

        private IPluginHost _host;
        private Dictionary<string, string> _locale;
        private string _name;
        private MessageBoxResult _popupResult;

        public IPluginHost Host
        {
            get { return _host; }
            set { PHost = _host = value; }
        }

        public MessageBoxResult PopupResult
        {
            get { return _popupResult; }
            set
            {
                _popupResult = value;
                PluginViewModel.Instance.OnPopupResultChanged(new PopupResultEvent(value));
            }
        }

        public Dictionary<string, string> Locale
        {
            get { return _locale ?? (_locale = new Dictionary<string, string>()); }
            set
            {
                _locale = value;
                var locale = LocaleHelper.ResolveOne(Constants.CultureInfo, "radar")
                                         .Cast<DictionaryEntry>()
                                         .ToDictionary(item => (string)item.Key, item => (string)item.Value);
                foreach (var resource in locale)
                {
                    try
                    {
                        _locale.Add(resource.Key, resource.Value);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                PluginViewModel.Instance.Locale = _locale;
                RaisePropertyChanged();
            }
        }

        public string FriendlyName { get; set; }

        public string Name
        {
            get { return _name; }
            private set { PName = _name = value; }
        }

        public string Icon { get; private set; }

        public string Description { get; private set; }

        public string Copyright { get; private set; }

        public string Version { get; private set; }

        public string Notice { get; private set; }

        public Exception Trace { get; private set; }

        public void Initialize(IPluginHost pluginHost)
        {
            Host = pluginHost;
            FriendlyName = "Radar";
            Name = AssemblyHelper.Name;
            Icon = "Logo.png";
            Description = AssemblyHelper.Description;
            Copyright = AssemblyHelper.Copyright;
            Version = AssemblyHelper.Version.ToString();
            Notice = "";
        }


        public void Dispose(bool isUpdating = false)
        {
            EventSubscriber.UnSubscribe();
            /*
             * If the isUpdating is true it means the application will be force closing/killed.
             * You wil have to choose what you want to do in this case.
             * By default the settings class clears the settings object and recreates it; but if killed untimely it will not save.
             * 
             * Suggested use is to not save settings if updating. Other disposing events could happen based on your needs.
             */
            if (isUpdating)
            {
                return;
            }
            Settings.Default.Save();
        }

        public TabItem CreateTab()
        {
            var content = new ShellView();
            content.Loaded += ShellViewModel.Loaded;
            var tabItem = new TabItem
            {
                Header = Name,
                Content = content
            };
            //do your gui stuff here
            EventSubscriber.Subscribe();
            //content gives you access to the base xaml
            return tabItem;
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
