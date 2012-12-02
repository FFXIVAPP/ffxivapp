// FFXIVAPP
// ChatWorkerDelagate.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using FFXIVAPP.Classes.Helpers;
using FFXIVAPP.Classes.Memory;
using FFXIVAPP.Classes.RegExs;
using FFXIVAPP.Models;
using FFXIVAPP.Properties;
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
        private static FlowDocHelper FD = new FlowDocHelper();

        /// <summary>
        /// </summary>
        public static void OnRawLine(ChatEntry cle)
        {
            var tmp = "";
            for (var j = 0; j < cle.Bytes.Length; j++)
            {
                tmp += cle.Bytes[j].ToString(CultureInfo.CurrentUICulture) + " ";
            }
            tmp = tmp.Trim();
            if (Settings.Default.ShowDebug)
            {
                if (Settings.Default.ASCII)
                {
                    FD.AppendFlow("", tmp, "#FFFFFF", MainWindow.View.LogView.Debug._FDR);
                }
                FD.AppendFlow("", cle.Raw, "#FFFFFF", MainWindow.View.LogView.Debug._FDR);
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
        public static void OnNewline(ChatEntry cle)
        {
            #region Clean Message

            var mTimeStamp = cle.TimeStamp.ToString("[HH:mm:ss] ");
            var mMessage = XmlCleaner.SanitizeXmlString(cle.Line.Replace("  ", " "));
            var mServer = Settings.Default.Server;
            var mColor = (Constants.XColor.ContainsKey(cle.Code)) ? Constants.XColor[cle.Code][0] : "FFFFFF";

            #endregion

            if (cle.Code == "0043" || cle.Code == "0042")
            {
                mMessage = mMessage.Replace(",", "");
                var mReg = Shared.Exp.Match(mMessage);
                if (mReg.Success)
                {
                    var tmpExp = Convert.ToString(mReg.Groups["exp"].Value);
                    MainWindow.TotalExp += int.Parse(tmpExp);
                }
            }

            if (cle.Code == "0020")
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
                if (cle.Code != "" || mMessage != "" || FFXIV.Instance.SIO.Socket.ReadyState.ToString() == "Open")
                {
                    mTimeStamp = mTimeStamp.Trim();
                    if (CheckMode(cle.Code, ShoutLog))
                    {
                        FFXIV.Instance.SIO.SendMessage("message", new Message {type = "Global", server = mServer, time = mTimeStamp, code = cle.Code, message = mMessage});
                    }
                    if (CheckMode(cle.Code, PrivateLog))
                    {
                        FFXIV.Instance.SIO.SendMessage("message", new Message {type = "Private", server = mServer, time = mTimeStamp, code = cle.Code, message = mMessage});
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
                    if (!LogVM.ChatScan[a].Items.Contains(cle.Code))
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
                        FD.AppendFlow(mTimeStamp, mMessage, "#" + mColor, tempFlow);
                    }
                }
                if (Settings.Default.ShowAll)
                {
                    FD.AppendFlow(mTimeStamp, mMessage, "#" + mColor, MainWindow.View.LogView.All._FDR);
                }
                if (Settings.Default.Log_SaveLog)
                {
                    LogXmlWriteLog.AddChatLine(new[] {mMessage, cle.Code, "#" + mColor, mTimeStamp});
                }
                if (Settings.Default.Translate)
                {
                    if (CheckMode(cle.Code, Constants.CmSay) && Settings.Default.Say)
                    {
                        GoogleTranslate.RetreiveLang(mMessage, cle.JP);
                    }
                    if (CheckMode(cle.Code, Constants.CmTell) && Settings.Default.Tell)
                    {
                        GoogleTranslate.RetreiveLang(mMessage, cle.JP);
                    }
                    if (CheckMode(cle.Code, Constants.CmParty) && Settings.Default.Party)
                    {
                        GoogleTranslate.RetreiveLang(mMessage, cle.JP);
                    }
                    if (CheckMode(cle.Code, Constants.CmShout) && Settings.Default.Shout)
                    {
                        GoogleTranslate.RetreiveLang(mMessage, cle.JP);
                    }
                    if (CheckMode(cle.Code, Constants.Cmls) && Settings.Default.LS)
                    {
                        GoogleTranslate.RetreiveLang(mMessage, cle.JP);
                    }
                }
            }

            #endregion

            #region Parse

            if (Settings.Default.EnableParse)
            {
                if (mMessage.Contains("readies") || mMessage.Contains("prépare") || mMessage.Contains("をしようとしている。"))
                {
                    if (cle.Code == "0053" || cle.Code == "0054" || cle.Code == "0055")
                    {
                        FD.AppendFlow(mTimeStamp, mMessage, "#FFFFFF", MainWindow.View.ParseView.MA._FDR);
                    }
                }
                Func<bool> d = delegate
                {
                    EventParser.Instance.ParseAndPublish(Convert.ToUInt16(cle.Code, 16), mMessage);
                    return true;
                };
                d.BeginInvoke(null, null);
            }

            #endregion

            #region Event

            if (Settings.Default.EnableEvent)
            {
                if (Constants.XEvent.Any(s => mMessage == s.Key))
                {
                    SoundPlayerHelper.Play(Constants.XEvent[mMessage]);
                }
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