// FFXIVAPP.Plugin.Parse
// Plugin.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using FFXIVAPP.Common.Chat;
using FFXIVAPP.Common.Events;
using FFXIVAPP.Common.RegularExpressions;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.IPluginInterface;
using FFXIVAPP.Plugin.Parse.Helpers;
using FFXIVAPP.Plugin.Parse.Models.Events;
using FFXIVAPP.Plugin.Parse.Properties;
using FFXIVAPP.Plugin.Parse.Utilities;
using FFXIVAPP.Plugin.Parse.Views;
using NLog;

#endregion

namespace FFXIVAPP.Plugin.Parse
{
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
            //content gives you access to the base xaml
            return tabItem;
        }

        public UserControl CreateControl()
        {
            var content = new ShellView();
            content.Loaded += ShellViewModel.Loaded;
            //do your gui stuff here
            //content gives you access to the base xaml
            return content;
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
                var timeStampColor = Settings.Default.TimeStampColor.ToString();
                var timeStamp = chatEntry.TimeStamp.ToString("[HH:mm:ss] ");
                var line = chatEntry.Line.Replace("  ", " ");
                var color = (Common.Constants.Colors.ContainsKey(chatEntry.Code)) ? Common.Constants.Colors[chatEntry.Code][0] : "FFFFFF";
                if (Constants.Abilities.Contains(chatEntry.Code) && Regex.IsMatch(line, @".+(uses)\s", SharedRegEx.DefaultOptions))
                {
                    Common.Constants.FD.AppendFlow(timeStamp, "", line, new[]
                    {
                        timeStampColor, "#" + color
                    }, MainView.View.AbilityChatFD._FDR);
                }
                if (chatEntry.Code == "0020")
                {
                    List<string> temp;
                    CommandBuilder.GetCommands(line, out temp);
                    if (temp != null)
                    {
                        Host.Commands(Name, temp);
                    }
                }
                Func<bool> funcParse = delegate
                {
                    EventParser.Instance.ParseAndPublish(Convert.ToUInt32(chatEntry.Code, 16), line);
                    return true;
                };
                funcParse.BeginInvoke(null, null);
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
