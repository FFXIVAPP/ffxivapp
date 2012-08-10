// FFXIVAPP
// ChatWorkerDelagate.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using FFXIVAPP.Classes.Helpers;
using FFXIVAPP.Classes.RegExs;
using FFXIVAPP.Models;
using FFXIVAPP.ViewModels;

namespace FFXIVAPP.Classes
{
    internal static class ChatWorkerDelegate
    {
        public static readonly XmlWriteLog LogXmlWriteLog = new XmlWriteLog();
        public static readonly XmlWriteLog ParseXmlWriteLog = new XmlWriteLog();
        public static readonly XmlWriteLog ParseXmlWriteUnmatchedLog = new XmlWriteLog();
        private static string[] _abilities = new[] {"0053", "0054", "0055"};
        private static readonly string[] ShoutLog = new[] {"0002"};
        private static readonly string[] PrivateLog = new[] {"0001", "0002", "0004", "000E", "0005", "000F", "0006", "0010", "0007", "0011", "0008", "0012", "0009", "0013", "000A", "0014", "000B", "0015", "000C", "001B", "0019", "000D", "0003"};
        private static readonly string[] IgnoreLog = new[] {"0020"};
        private static FlowDocumentHelper FD = new FlowDocumentHelper();

        /// <summary>
        /// </summary>
        /// <param name="mLine"> </param>
        public static void OnRawLine(string mLine)
        {
            var splitOfLine = mLine.Split(' ');
            var tmpString = splitOfLine.Aggregate("", (current, t) => current + (char) int.Parse(t));
            if (Settings.Default.ShowDebug)
            {
                if (Settings.Default.ASCII)
                {
                    FD.AppendFlow("", mLine, "#FFFFFF", MainWindow.View.LogView.Debug._FDR);
                }
                FD.AppendFlow("", tmpString, "#FFFFFF", MainWindow.View.LogView.Debug._FDR);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="mTimeStamp"> </param>
        /// <param name="mCode"> </param>
        /// <param name="mLine"> </param>
        public static void OnDebugline(string mTimeStamp, string mCode, string mLine)
        {
            if (mLine.Contains("readies") || mLine.Contains("prépare") || mLine.Contains("をしようとしている。"))
            {
                if (mCode == "0053" || mCode == "0054" || mCode == "0055")
                {
                    FD.AppendFlow(mTimeStamp, mLine, "#FFFFFF", MainWindow.View.ParseView.MA._FDR);
                }
            }
            Func<bool> d = delegate
            {
                EventParser.Instance.ParseAndPublish(Convert.ToUInt16(mCode, 16), mLine);
                return true;
            };
            d.BeginInvoke(null, null);
        }

        /// <summary>
        /// </summary>
        /// <param name="mLine"> </param>
        /// <param name="mJp"> </param>
        public static void OnNewline(string mLine, Boolean mJp)
        {
            #region Clean Message

            mLine = mLine.Replace("::", ":").Replace("  ", " ");
            var mTimeStamp = DateTime.Now.ToString("[HH:mm:ss] ");
            var mCode = mLine.Substring(mLine.IndexOf(":", StringComparison.Ordinal) - 4, 4);
            var mMessage = mLine.Substring(mLine.IndexOf(":", StringComparison.Ordinal) + 1);
            var mServer = Settings.Default.Server;
            if (mMessage.Substring(0, 1) == ":")
            {
                mMessage = mMessage.Substring(1);
            }
            mMessage = XmlCleaner.SanitizeXmlString(mMessage);

            #endregion

            if (mCode == "0043" || mCode == "0042")
            {
                mMessage = mMessage.Replace(",", "");
                var mReg = Shared.Exp.Match(mMessage);
                if (mReg.Success)
                {
                    var tmpExp = Convert.ToString(mReg.Groups["exp"].Value);
                    MainWindow.TotalExp += int.Parse(tmpExp);
                }
            }

            if (mCode == "0020")
            {
                COMParser.Process(mMessage);
            }

            //if (CheckMode(mCode, Constants.CmParty) || CheckMode(mCode, Constants.CmTell))
            //{
            //    const string plName = "";
            //    const string plKey = "";
            //    if (plName != Settings.Default.CharacterName)
            //    {
            //        if (mMessage.Contains(plKey) && mMessage.Contains(plName))
            //        {
            //            KeyHelper.Alt(Keys.D1);
            //        }
            //    }
            //}

            #region Chat

            if (Settings.Default.EnableChat)
            {
                if (mCode != "" || mMessage != "" || FFXIV.Instance.SIO.Socket.ReadyState.ToString() == "Open")
                {
                    mTimeStamp = mTimeStamp.Trim();
                    if (CheckMode(mCode, ShoutLog))
                    {
                        FFXIV.Instance.SIO.SendMessage("message", new Message {type = "Global", server = mServer, time = mTimeStamp, code = mCode, message = mMessage});
                    }
                    if (CheckMode(mCode, PrivateLog))
                    {
                        FFXIV.Instance.SIO.SendMessage("message", new Message {type = "Private", server = mServer, time = mTimeStamp, code = mCode, message = mMessage});
                    }
                    //if (!CheckMode(mCode, IgnoreLog))
                    //{
                    //    FFXIV.Instance.SIO.SendMessage("message", new Message {command = "battle", server = mServer, time = mTimeStamp, code = mCode, message = mMessage});
                    //}
                }
            }

            #endregion

            #region Log

            if (Settings.Default.EnableLog)
            {
                for (var a = 0; a < LogVM.ChatScan.Length; a++)
                {
                    if (!LogVM.ChatScan[a].Items.Contains(mCode))
                    {
                        continue;
                    }
                    var success = false;
                    const RegexOptions defaultOptions = RegexOptions.Compiled | RegexOptions.ExplicitCapture;
                    switch (LogVM.RegExs[a].ToString())
                    {
                        case "*":
                            success = true;
                            break;
                        default:
                            try
                            {
                                var check = new Regex(LogVM.RegExs[a].ToString(), defaultOptions);
                                var mReg = check.Match(mMessage);
                                if (mReg.Success)
                                {
                                    success = true;
                                }
                            }
                            catch
                            {
                                success = true;
                            }
                            break;
                    }
                    if (success)
                    {
                        var tempFlow = LogVM.DFlowDocReader[LogVM.TabNames[a] + "_FDR"];
                        FD.AppendFlow(mTimeStamp, mMessage, "#" + Constants.XColor[mCode][0], tempFlow);
                    }
                }
                if (Settings.Default.ShowAll)
                {
                    FD.AppendFlow(mTimeStamp, mMessage, "#" + Constants.XColor[mCode][0], MainWindow.View.LogView.All._FDR);
                }
                if (Settings.Default.Log_SaveLog)
                {
                    LogXmlWriteLog.AddChatLine(new[] {mMessage, mCode, "#" + Constants.XColor[mCode][0], mTimeStamp});
                }
                if (Settings.Default.Translate)
                {
                    if (CheckMode(mCode, Constants.CmSay) && Settings.Default.Say)
                    {
                        GoogleTranslate.RetreiveLang(mMessage, mJp);
                    }
                    if (CheckMode(mCode, Constants.CmTell) && Settings.Default.Tell)
                    {
                        GoogleTranslate.RetreiveLang(mMessage, mJp);
                    }
                    if (CheckMode(mCode, Constants.CmParty) && Settings.Default.Party)
                    {
                        GoogleTranslate.RetreiveLang(mMessage, mJp);
                    }
                    if (CheckMode(mCode, Constants.CmShout) && Settings.Default.Shout)
                    {
                        GoogleTranslate.RetreiveLang(mMessage, mJp);
                    }
                    if (CheckMode(mCode, Constants.Cmls) && Settings.Default.LS)
                    {
                        GoogleTranslate.RetreiveLang(mMessage, mJp);
                    }
                }
            }

            #endregion

            #region Parse

            if (Settings.Default.EnableParse)
            {
                if (mMessage.Contains("readies") || mMessage.Contains("prépare") || mLine.Contains("をしようとしている。"))
                {
                    if (mCode == "0053" || mCode == "0054" || mCode == "0055")
                    {
                        FD.AppendFlow(mTimeStamp, mMessage, "#FFFFFF", MainWindow.View.ParseView.MA._FDR);
                    }
                }
                Func<bool> d = delegate
                {
                    EventParser.Instance.ParseAndPublish(Convert.ToUInt16(mCode, 16), mMessage);
                    return true;
                };
                d.BeginInvoke(null, null);
            }

            #endregion
        }

        /// <summary>
        /// </summary>
        /// <param name="chatMode"> </param>
        /// <param name="log"> </param>
        /// <returns> </returns>
        private static Boolean CheckMode(string chatMode, IEnumerable<string> log)
        {
            return log.Any(t => t == chatMode);
        }

        /// <summary>
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        private static string C2H(string input)
        {
            var hex = "";
            foreach (var c in input)
            {
                int tmp = c;
                hex += String.Format("{0:X2}", Convert.ToUInt32(tmp.ToString(CultureInfo.InvariantCulture)));
            }
            return hex;
        }
    }
}