// FFXIVAPP.Plugin.Event
// Plugin.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using FFXIVAPP.Common.Chat;
using FFXIVAPP.Common.Events;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.IPluginInterface;
using FFXIVAPP.Plugin.Event.Helpers;
using FFXIVAPP.Plugin.Event.Properties;
using FFXIVAPP.Plugin.Event.Utilities;
using NLog;

#endregion

namespace FFXIVAPP.Plugin.Event
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
                var locale = LocaleHelper.Update(Constants.CultureInfo);
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
            Name = Common.Constants.Name;
            Icon = "Logo.png";
            Description = Common.Constants.Description;
            Copyright = Common.Constants.Copyright;
            Version = Common.Constants.Version.ToString();
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
            var files = Directory.GetFiles(Constants.BaseDirectory)
                .Where(file => Regex.IsMatch(file, @"^.+\.(wav)$"))
                .Select(file => new FileInfo(file));
            foreach (var file in files)
            {
                PluginViewModel.Instance.SoundFiles.Add(file.Name);
            }
            //content gives you access to the base xaml
            return tabItem;
        }

        public UserControl CreateControl()
        {
            var content = new ShellView();
            content.Loaded += ShellViewModel.Loaded;
            //do your gui stuff here
            var files = Directory.GetFiles(Constants.BaseDirectory)
                .Where(file => Regex.IsMatch(file, @"^.+\.(wav)$"))
                .Select(file => new FileInfo(file));
            foreach (var file in files)
            {
                PluginViewModel.Instance.SoundFiles.Add(file.Name);
            }
            //content gives you access to the base xaml
            return content;
        }

        public void OnNewLine(out bool success, params object[] entry)
        {
            try
            {
                var chatEntry = new ChatEntry
                {
                    Bytes = (byte[]) entry[0],
                    Code = (string) entry[1],
                    Combined = (string) entry[2],
                    JP = (bool) entry[3],
                    Line = Regex.Replace((string) entry[4], "[ ]+", " "),
                    Raw = (string) entry[5],
                    TimeStamp = (DateTime) entry[6]
                };
                LogPublisher.Process(chatEntry);
            }
            catch (Exception ex)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
                Notice = ex.Message;
            }
            success = true;
        }

        public void SetConstants(ConstantsType type, object data)
        {
            switch (type)
            {
                case ConstantsType.AutoTranslate:
                    Constants.AutoTranslate = data as Dictionary<string, string>;
                    break;
                case ConstantsType.ChatCodes:
                    Constants.ChatCodes = data as Dictionary<string, string>;
                    break;
                case ConstantsType.ChatCodesXml:
                    Constants.ChatCodesXml = data as string;
                    break;
                case ConstantsType.Colors:
                    Constants.Colors = data as Dictionary<string, string[]>;
                    break;
                case ConstantsType.CultureInfo:
                    Constants.CultureInfo = data as CultureInfo;
                    break;
                case ConstantsType.CharacterName:
                    Constants.CharacterName = data as string;
                    break;
                case ConstantsType.ServerName:
                    Constants.ServerName = data as string;
                    break;
                case ConstantsType.GameLanguage:
                    Constants.GameLanguage = data as string;
                    break;
            }
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
