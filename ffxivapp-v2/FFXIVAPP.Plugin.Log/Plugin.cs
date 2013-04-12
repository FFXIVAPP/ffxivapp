// FFXIVAPP.Plugin.Log
// Plugin.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using FFXIVAPP.Common.Chat;
using FFXIVAPP.Common.Controls;
using FFXIVAPP.Common.Events;
using FFXIVAPP.Common.RegularExpressions;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.IPluginInterface;
using FFXIVAPP.Plugin.Log.Helpers;
using FFXIVAPP.Plugin.Log.Properties;
using FFXIVAPP.Plugin.Log.Utilities;
using FFXIVAPP.Plugin.Log.Views;
using NLog;

#endregion

namespace FFXIVAPP.Plugin.Log
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
            //handle regular tabs
            try
            {
                var timeStampColor = Settings.Default.TimeStampColor.ToString();
                var timeStamp = chatEntry.TimeStamp.ToString("[HH:mm:ss] ");
                var line = chatEntry.Line.Replace("  ", " ");
                var color = (Common.Constants.Colors.ContainsKey(chatEntry.Code)) ? Common.Constants.Colors[chatEntry.Code][0] : "FFFFFF";
                var isLS = Constants.Linkshells.ContainsKey(chatEntry.Code);
                line = isLS ? Constants.Linkshells[chatEntry.Code] + line : line;
                var playerName = "";
                if (CheckMode(chatEntry.Code, Common.Constants.ChatPublic))
                {
                    playerName = line.Substring(0, line.IndexOf(":", StringComparison.Ordinal));
                    line = line.Replace(playerName + ":", "");
                }
                Common.Constants.FD.AppendFlow(timeStamp, playerName, line, new[]
                {
                    timeStampColor, "#" + color
                }, MainView.View.AllFD._FDR);
                foreach (var flowDoc in PluginViewModel.Instance.Tabs.Select(ti => (xFlowDocument) ((TabItem) ti).Content))
                {
                    var resuccess = false;
                    var xRegularExpression = flowDoc.RegEx.Text;
                    switch (xRegularExpression)
                    {
                        case "*":
                            resuccess = true;
                            break;
                        default:
                            try
                            {
                                var check = new Regex(xRegularExpression);
                                if (SharedRegEx.IsValidRegex(xRegularExpression))
                                {
                                    var reg = check.Match(line);
                                    if (reg.Success)
                                    {
                                        resuccess = true;
                                    }
                                }
                            }
                            catch
                            {
                                resuccess = true;
                            }
                            break;
                    }
                    if (resuccess && flowDoc.Codes.Items.Contains(chatEntry.Code))
                    {
                        Common.Constants.FD.AppendFlow(timeStamp, playerName, line, new[]
                        {
                            timeStampColor, "#" + color
                        }, flowDoc._FDR);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
                Notice = ex.Message;
            }
            //handle translation
            try
            {
                var line = chatEntry.Line.Replace("  ", " ");
                if (Settings.Default.EnableTranslate)
                {
                    if (CheckMode(chatEntry.Code, Common.Constants.ChatSay) && Settings.Default.TranslateSay)
                    {
                        GoogleTranslate.RetreiveLang(line, chatEntry.JP);
                    }
                    if (CheckMode(chatEntry.Code, Common.Constants.ChatTell) && Settings.Default.TranslateTell)
                    {
                        GoogleTranslate.RetreiveLang(line, chatEntry.JP);
                    }
                    if (CheckMode(chatEntry.Code, Common.Constants.ChatParty) && Settings.Default.TranslateParty)
                    {
                        GoogleTranslate.RetreiveLang(line, chatEntry.JP);
                    }
                    if (CheckMode(chatEntry.Code, Common.Constants.ChatShout) && Settings.Default.TranslateShout)
                    {
                        GoogleTranslate.RetreiveLang(line, chatEntry.JP);
                    }
                    if (CheckMode(chatEntry.Code, Common.Constants.ChatLS) && Settings.Default.TranslateLS)
                    {
                        GoogleTranslate.RetreiveLang(line, chatEntry.JP);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
            }
            //handle debug tab
            try
            {
                if (Settings.Default.ShowAsciiDebug)
                {
                    var asciiString = "";
                    for (var j = 0; j < chatEntry.Bytes.Length; j++)
                    {
                        asciiString += chatEntry.Bytes[j].ToString(CultureInfo.CurrentUICulture) + " ";
                    }
                    asciiString = asciiString.Trim();
                    Common.Constants.FD.AppendFlow("", "", asciiString, new[]
                    {
                        "", "#FFFFFFFF"
                    }, MainView.View.DebugFD._FDR);
                }
                var raw = String.Format("{0}[{1}]{2}", chatEntry.Raw.Substring(0, 8), chatEntry.Code, chatEntry.Raw.Substring(12));
                Common.Constants.FD.AppendFlow("", "", raw, new[]
                {
                    "", "#FFFFFFFF"
                }, MainView.View.DebugFD._FDR);
            }
            catch (Exception ex)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
                Notice = ex.Message;
            }
            success = true;
        }

        /// <summary>
        /// </summary>
        /// <param name="chatMode"> </param>
        /// <param name="log"> </param>
        /// <returns> </returns>
        private static bool CheckMode(string chatMode, IEnumerable<string> log)
        {
            return log.Any(t => t == chatMode);
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
