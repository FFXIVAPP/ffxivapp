// FFXIVAPP.Plugin.Log
// LogPublisher.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using FFXIVAPP.Common.Chat;
using FFXIVAPP.Common.Controls;
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
        public static void Process(ChatEntry chatEntry)
        {
            // setup variables
            var timeStampColor = Settings.Default.TimeStampColor.ToString();
            var timeStamp = chatEntry.TimeStamp.ToString("[HH:mm:ss] ");
            var line = chatEntry.Line.Replace("  ", " ");
            var rawLine = line;
            var color = (Common.Constants.Colors.ContainsKey(chatEntry.Code)) ? Common.Constants.Colors[chatEntry.Code][0] : "FFFFFF";
            var isLS = Constants.Linkshells.ContainsKey(chatEntry.Code);
            line = isLS ? Constants.Linkshells[chatEntry.Code] + line : line;
            var playerName = "";
            try
            {
                // handle tabs
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
            }
            try
            {
                // handle translation
                if (Settings.Default.EnableTranslate)
                {
                    if (CheckMode(chatEntry.Code, Common.Constants.ChatSay) && Settings.Default.TranslateSay)
                    {
                        GoogleTranslate.RetreiveLang(rawLine, chatEntry.JP);
                    }
                    if (CheckMode(chatEntry.Code, Common.Constants.ChatTell) && Settings.Default.TranslateTell)
                    {
                        GoogleTranslate.RetreiveLang(rawLine, chatEntry.JP);
                    }
                    if (CheckMode(chatEntry.Code, Common.Constants.ChatParty) && Settings.Default.TranslateParty)
                    {
                        GoogleTranslate.RetreiveLang(rawLine, chatEntry.JP);
                    }
                    if (CheckMode(chatEntry.Code, Common.Constants.ChatShout) && Settings.Default.TranslateShout)
                    {
                        GoogleTranslate.RetreiveLang(rawLine, chatEntry.JP);
                    }
                    if (CheckMode(chatEntry.Code, Common.Constants.ChatYell) && Settings.Default.TranslateYell)
                    {
                        GoogleTranslate.RetreiveLang(rawLine, chatEntry.JP);
                    }
                    if (CheckMode(chatEntry.Code, Common.Constants.ChatLS) && Settings.Default.TranslateLS)
                    {
                        GoogleTranslate.RetreiveLang(rawLine, chatEntry.JP);
                    }
                    if (CheckMode(chatEntry.Code, Common.Constants.ChatFC) && Settings.Default.TranslateFC)
                    {
                        GoogleTranslate.RetreiveLang(rawLine, chatEntry.JP);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
            }
            try
            {
                // handle debug tab
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
