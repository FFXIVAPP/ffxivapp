// FFXIVAPP.Plugin.Parse
// Plugin.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using FFXIVAPP.Common.Chat;
using FFXIVAPP.Common.Events;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.IPluginInterface;
using FFXIVAPP.Plugin.Parse.Helpers;
using FFXIVAPP.Plugin.Parse.Models;
using FFXIVAPP.Plugin.Parse.Properties;
using FFXIVAPP.Plugin.Parse.Utilities;
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
            Name = AssemblyHelper.Name;
            Icon = "Logo.png";
            Description = AssemblyHelper.Description;
            Copyright = AssemblyHelper.Copyright;
            Version = AssemblyHelper.Version.ToString();
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
                // process commands
                if (chatEntry.Code == "0038")
                {
                    var parseCommands = CommandBuilder.CommandsRegEx.Match(chatEntry.Line.Trim());
                    if (parseCommands.Success)
                    {
                        var cmd = parseCommands.Groups["cmd"].Success ? parseCommands.Groups["cmd"].Value : "";
                        var sub = parseCommands.Groups["sub"].Success ? parseCommands.Groups["sub"].Value : "";
                        switch (cmd)
                        {
                            case "parse":
                                switch (sub)
                                {
                                    case "reset":
                                        ParseControl.Instance.Reset();
                                        break;
                                    case "toggle":
                                        ParseControl.Instance.Toggle();
                                        break;
                                }
                                break;
                            default:
                                List<string> temp;
                                CommandBuilder.GetCommands(chatEntry.Line, out temp);
                                if (temp != null)
                                {
                                    Host.Commands(PName, temp);
                                }
                                break;
                        }
                    }
                }
                // process logs
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
                case ConstantsType.EnableNLog:
                    Constants.EnableNLog = data is bool && (bool)data;
                    break;
                case ConstantsType.EnableHelpLabels:
                    PluginViewModel.Instance.EnableHelpLabels = Constants.EnableHelpLabels = data is bool && (bool)data;
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
