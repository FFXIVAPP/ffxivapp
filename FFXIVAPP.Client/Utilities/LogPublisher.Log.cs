// FFXIVAPP.Client
// LogPublisher.Log.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using FFXIVAPP.Client.Plugins.Log;
using FFXIVAPP.Client.Plugins.Log.Views;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Controls;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.RegularExpressions;
using FFXIVAPP.Common.Utilities;
using NLog;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Utilities
{
    public static partial class LogPublisher
    {
        [DoNotObfuscate]
        public static class Log
        {
            public static bool IsPaused = false;

            public static void Process(ChatLogEntry chatLogEntry)
            {
                if (IsPaused)
                {
                    return;
                }
                try
                {
                    // setup variables
                    var timeStampColor = Settings.Default.TimeStampColor.ToString();
                    var timeStamp = chatLogEntry.TimeStamp.ToString("[HH:mm:ss] ");
                    var line = chatLogEntry.Line.Replace("  ", " ");
                    var rawLine = line;
                    var color = (Constants.Colors.ContainsKey(chatLogEntry.Code)) ? Constants.Colors[chatLogEntry.Code][0] : "FFFFFF";
                    var isLS = Constants.Log.Linkshells.ContainsKey(chatLogEntry.Code);
                    line = isLS ? Constants.Log.Linkshells[chatLogEntry.Code] + line : line;
                    var playerName = "";

                    // handle tabs
                    if (CheckMode(chatLogEntry.Code, Constants.Log.ChatPublic))
                    {
                        playerName = line.Substring(0, line.IndexOf(":", StringComparison.Ordinal));
                        line = line.Replace(playerName + ":", "");
                    }
                    if (Constants.Log.PluginSettings.EnableAll)
                    {
                        Common.Constants.FD.AppendFlow(timeStamp, playerName, line, new[]
                        {
                            timeStampColor, "#" + color
                        }, MainView.View.AllFD._FDR);
                    }
                    DispatcherHelper.Invoke(delegate
                    {
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
                            if (resuccess && flowDoc.Codes.Items.Contains(chatLogEntry.Code))
                            {
                                Common.Constants.FD.AppendFlow(timeStamp, playerName, line, new[]
                                {
                                    timeStampColor, "#" + color
                                }, flowDoc._FDR);
                            }
                        }
                    });
                    // handle translation
                    if (Constants.Log.PluginSettings.EnableTranslate)
                    {
                        if (CheckMode(chatLogEntry.Code, Constants.Log.ChatSay) && Constants.Log.PluginSettings.TranslateSay)
                        {
                            GoogleTranslate.RetreiveLang(rawLine, chatLogEntry.JP);
                        }
                        if (CheckMode(chatLogEntry.Code, Constants.Log.ChatTell) && Constants.Log.PluginSettings.TranslateTell)
                        {
                            GoogleTranslate.RetreiveLang(rawLine, chatLogEntry.JP);
                        }
                        if (CheckMode(chatLogEntry.Code, Constants.Log.ChatParty) && Constants.Log.PluginSettings.TranslateParty)
                        {
                            GoogleTranslate.RetreiveLang(rawLine, chatLogEntry.JP);
                        }
                        if (CheckMode(chatLogEntry.Code, Constants.Log.ChatShout) && Constants.Log.PluginSettings.TranslateShout)
                        {
                            GoogleTranslate.RetreiveLang(rawLine, chatLogEntry.JP);
                        }
                        if (CheckMode(chatLogEntry.Code, Constants.Log.ChatYell) && Constants.Log.PluginSettings.TranslateYell)
                        {
                            GoogleTranslate.RetreiveLang(rawLine, chatLogEntry.JP);
                        }
                        if (CheckMode(chatLogEntry.Code, Constants.Log.ChatLS) && Constants.Log.PluginSettings.TranslateLS)
                        {
                            GoogleTranslate.RetreiveLang(rawLine, chatLogEntry.JP);
                        }
                        if (CheckMode(chatLogEntry.Code, Constants.Log.ChatFC) && Constants.Log.PluginSettings.TranslateFC)
                        {
                            GoogleTranslate.RetreiveLang(rawLine, chatLogEntry.JP);
                        }
                    }
                    // handle debug tab
                    if (Constants.Log.PluginSettings.ShowASCIIDebug)
                    {
                        var asciiString = "";
                        for (var j = 0; j < chatLogEntry.Bytes.Length; j++)
                        {
                            asciiString += chatLogEntry.Bytes[j].ToString(CultureInfo.CurrentUICulture) + " ";
                        }
                        asciiString = asciiString.Trim();
                        Common.Constants.FD.AppendFlow("", "", asciiString, new[]
                        {
                            "", "#FFFFFFFF"
                        }, MainView.View.DebugFD._FDR);
                    }
                    if (Constants.Log.PluginSettings.EnableDebug)
                    {
                        var raw = String.Format("{0}[{1}]{2}", chatLogEntry.Raw.Substring(0, 8), chatLogEntry.Code, chatLogEntry.Raw.Substring(12));
                        Common.Constants.FD.AppendFlow("", "", raw, new[]
                        {
                            "", "#FFFFFFFF"
                        }, MainView.View.DebugFD._FDR);
                    }
                }
                catch (Exception ex)
                {
                    Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
                }
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
        }
    }
}
