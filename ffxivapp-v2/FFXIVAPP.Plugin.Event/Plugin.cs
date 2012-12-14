// FFXIVAPP.Plugin.Event
// Plugin.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using FFXIVAPP.Common.Chat;
using FFXIVAPP.Common.Events;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.RegularExpressions;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.IPluginInterface;
using FFXIVAPP.Plugin.Event.Helpers;
using FFXIVAPP.Plugin.Event.Properties;
using NLog;

#endregion

namespace FFXIVAPP.Plugin.Event
{
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

        public MessageBoxResult PopupResult
        {
            get { return _popupResult; }
            set
            {
                _popupResult = value;
                PluginViewModel.Instance.OnPopupResultChanged(new PopupResultEvent(value));
            }
        }

        public IPluginHost Host
        {
            get { return _host; }
            set { PHost = _host = value; }
        }

        public Dictionary<string, string> Locale
        {
            get { return _locale ?? (_locale = new Dictionary<string, string>()); }
            set
            {
                _locale = value;
                var locale = LocaleHelper.Update(Common.Constants.CultureInfo);
                foreach (var resource in locale)
                {
                    try
                    {
                        _locale.Add(resource.Key, resource.Value);
                    }
                    catch (Exception ex)
                    {
                        Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
                    }
                }
                PluginViewModel.Instance.Locale = _locale;
                RaisePropertyChanged();
            }
        }

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

        public void Initialize()
        {
            Name = Constants.PluginName;
            Icon = "Logo.png";
            Description = Constants.PluginDescription;
            Copyright = Constants.PluginCopyright;
            Version = Constants.PluginVersion.ToString();
            Notice = "";
        }

        public void Dispose()
        {
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
            var files = Directory.GetFiles(Constants.BaseDirectory).Where(file => Regex.IsMatch(file, @"^.+\.(wav)$")).Select(file => new FileInfo(file));
            foreach (var file in files)
            {
                PluginViewModel.Instance.SoundFiles.Add(file.Name);
            }
            //content gives you access to the base xaml
            return tabItem;
        }

        public void OnNewLine(out bool success, params object[] entry)
        {
            var chatEntry = new ChatEntry();
            chatEntry.Bytes = (byte[]) entry[0];
            chatEntry.Code = (string) entry[1];
            chatEntry.Combined = (string) entry[2];
            chatEntry.JP = (bool) entry[3];
            chatEntry.Line = (string) entry[4];
            chatEntry.Raw = (string) entry[5];
            chatEntry.TimeStamp = (DateTime) entry[6];
            try
            {
                var line = chatEntry.Line.Replace("  ", " ");
                foreach (var item in PluginViewModel.Instance.Events)
                {
                    var resuccess = false;
                    var check = new Regex(item.Key);
                    if (SharedRegEx.IsValidRegex(item.Key))
                    {
                        var reg = check.Match(line);
                        if (reg.Success)
                        {
                            resuccess = true;
                        }
                    }
                    else
                    {
                        resuccess = (item.Key == line);
                    }
                    if (!resuccess)
                    {
                        continue;
                    }
                    var index = PluginViewModel.Instance.Events.TakeWhile(pair => pair.Key != line).Count();
                    SoundPlayerHelper.Play(Constants.BaseDirectory, PluginViewModel.Instance.Events[index].Value);
                }
            }
            catch (Exception ex)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
                Notice = ex.Message;
            }
            success = true;
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
