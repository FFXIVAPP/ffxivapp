// FFXIVAPP.Plugin.Log
// LogPublisher.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using FFXIVAPP.Common.Controls;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.RegularExpressions;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Plugin.Log.Properties;
using FFXIVAPP.Plugin.Log.Views;
using NLog;

#endregion

namespace FFXIVAPP.Plugin.Log.Utilities
{
    public static class LogPublisher
    {
        public static void Process(ChatLogEntry chatLogEntry)
        {
            try
            {
                // setup variables
                var timeStampColor = Settings.Default.TimeStampColor.ToString();
                var timeStamp = chatLogEntry.TimeStamp.ToString("[HH:mm:ss] ");
                var line = chatLogEntry.Line.Replace("  ", " ");
                var rawLine = line;
                var color = (Constants.Colors.ContainsKey(chatLogEntry.Code)) ? Constants.Colors[chatLogEntry.Code][0] : "FFFFFF";
                var isLS = Constants.Linkshells.ContainsKey(chatLogEntry.Code);
                line = isLS ? Constants.Linkshells[chatLogEntry.Code] + line : line;
                var playerName = "";

                // handle tabs
                if (CheckMode(chatLogEntry.Code, Constants.ChatPublic))
                {
                    playerName = line.Substring(0, line.IndexOf(":", StringComparison.Ordinal));
                    line = line.Replace(playerName + ":", "");
                }
                if (Settings.Default.EnableAll)
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
                if (Settings.Default.EnableTranslate)
                {
                    if (CheckMode(chatLogEntry.Code, Constants.ChatSay) && Settings.Default.TranslateSay)
                    {
                        GoogleTranslate.RetreiveLang(rawLine, chatLogEntry.JP);
                    }
                    if (CheckMode(chatLogEntry.Code, Constants.ChatTell) && Settings.Default.TranslateTell)
                    {
                        GoogleTranslate.RetreiveLang(rawLine, chatLogEntry.JP);
                    }
                    if (CheckMode(chatLogEntry.Code, Constants.ChatParty) && Settings.Default.TranslateParty)
                    {
                        GoogleTranslate.RetreiveLang(rawLine, chatLogEntry.JP);
                    }
                    if (CheckMode(chatLogEntry.Code, Constants.ChatShout) && Settings.Default.TranslateShout)
                    {
                        GoogleTranslate.RetreiveLang(rawLine, chatLogEntry.JP);
                    }
                    if (CheckMode(chatLogEntry.Code, Constants.ChatYell) && Settings.Default.TranslateYell)
                    {
                        GoogleTranslate.RetreiveLang(rawLine, chatLogEntry.JP);
                    }
                    if (CheckMode(chatLogEntry.Code, Constants.ChatLS) && Settings.Default.TranslateLS)
                    {
                        GoogleTranslate.RetreiveLang(rawLine, chatLogEntry.JP);
                    }
                    if (CheckMode(chatLogEntry.Code, Constants.ChatFC) && Settings.Default.TranslateFC)
                    {
                        GoogleTranslate.RetreiveLang(rawLine, chatLogEntry.JP);
                    }
                }
                // handle debug tab
                if (Settings.Default.ShowASCIIDebug)
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
                if (Settings.Default.EnableDebug)
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
