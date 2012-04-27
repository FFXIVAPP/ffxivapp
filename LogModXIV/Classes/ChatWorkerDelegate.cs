// Project: LogModXIV
// File: ChatWorkerDelegate.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using AppModXIV.Classes;
using LogModXIV.View;

namespace LogModXIV.Classes
{
    internal static class ChatWorkerDelegate
    {
        public static readonly XmlWriteLog XmlWriteLog = new XmlWriteLog();
        private static FlowDocumentReader _tempFlow;
        private const RegexOptions DefaultOptions = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture;
        private static readonly Regex Exp = new Regex(@"^You earn (?<exp>\d+).+\.$", DefaultOptions);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mLine"></param>
        public static void OnRawLine(string mLine)
        {
            if (MainMenuView.View.gui_StopLogging.IsChecked == false)
            {
                var splitOfLine = mLine.Split(' ');
                var tmpString = splitOfLine.Aggregate("", (current, t) => current + (char) int.Parse(t));
                if (Settings.Default.Gui_ShowDebug)
                {
                    if (Settings.Default.Gui_ShowASCII)
                    {
                        AppendFlow("", mLine, "#FFFFFF", MainTabControlView.View.Debug_FDR);
                    }
                    AppendFlow("", tmpString, "#FFFFFF", MainTabControlView.View.Debug_FDR);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mLine"></param>
        /// <param name="mJp"></param>
        public static void OnNewline(string mLine, Boolean mJp)
        {
            var mTimeStamp = DateTime.Now.ToString("[HH:mm:ss] ");
            var mCode = mLine.Substring(mLine.IndexOf(":", StringComparison.Ordinal) - 4, 4);
            var mMessage = mLine.Substring(mLine.IndexOf(":", StringComparison.Ordinal) + 1);
            if (mMessage.Substring(0, 1) == ":")
            {
                mMessage = mMessage.Substring(1);
            }
            //if (CheckMode(mCode, Constants.CmParty) || CheckMode(mCode, Constants.CmTell))
            //{
            //    const bool pl = false;
            //    if (pl)
            //    {
            //        var PLName = "";
            //        var PLKey = "boot";
            //        if (mMessage.Contains(PLKey) && mMessage.Contains(PLName))
            //        {
            //            KeyHelper.Alt(Keys.D1);
            //        }
            //    }
            //}
            if (mCode == "0043" || mCode == "0042")
            {
                var mReg = Exp.Match(mMessage);
                if (mReg.Success)
                {
                    var tmpExp = Convert.ToString(mReg.Groups["exp"].Value);
                    MainWindow.View.TotalExp += int.Parse(tmpExp);
                }
            }
            if (MainMenuView.View.gui_StopLogging.IsChecked == false)
            {
                for (var a = 0; a <= MainWindow.ChatScan.Length - 2; a++)
                {
                    if (!MainWindow.ChatScan[a].Items.Contains(mCode))
                    {
                        continue;
                    }
                    _tempFlow = MainWindow.DFlowDocumentReader[MainWindow.TabNames[a] + "_FDR"];
                    AppendFlow(mTimeStamp, mMessage, "#" + LmSettings.XColor[mCode], _tempFlow);
                }
                if (Settings.Default.Gui_ShowAll)
                {
                    AppendFlow(mTimeStamp, mMessage, "#" + LmSettings.XColor[mCode], MainTabControlView.View.All_FDR);
                }

                if (Settings.Default.Gui_SaveLog)
                {
                    XmlWriteLog.AddChatLine(new[] { mMessage, mCode, mTimeStamp, "#" + LmSettings.XColor[mCode] });
                }
            }
            if (!Settings.Default.Gui_Translate)
            {
                return;
            }
            if (mJp && Settings.Default.Gui_TranslateJPOnly)
            {
                if (CheckMode(mCode, Constants.CmSay))
                {
                    if (Settings.Default.Gui_TSay)
                    {
                        GoogleTranslate.RetreiveLang(mMessage, true, true);
                    }
                }
                if (CheckMode(mCode, Constants.CmTell))
                {
                    if (Settings.Default.Gui_TTell)
                    {
                        GoogleTranslate.RetreiveLang(mMessage, true, true);
                    }
                }
                if (CheckMode(mCode, Constants.CmParty))
                {
                    if (Settings.Default.Gui_TParty)
                    {
                        GoogleTranslate.RetreiveLang(mMessage, true, true);
                    }
                }
                if (CheckMode(mCode, Constants.CmShout))
                {
                    if (Settings.Default.Gui_TShout)
                    {
                        GoogleTranslate.RetreiveLang(mMessage, true, true);
                    }
                }
                if (CheckMode(mCode, Constants.Cmls))
                {
                    if (Settings.Default.Gui_TLS)
                    {
                        GoogleTranslate.RetreiveLang(mMessage, true, true);
                    }
                }
            }
            else if (!Settings.Default.Gui_TranslateJPOnly)
            {
                if (CheckMode(mCode, Constants.CmSay))
                {
                    if (Settings.Default.Gui_TSay)
                    {
                        GoogleTranslate.RetreiveLang(mMessage, false, mJp);
                    }
                }
                if (CheckMode(mCode, Constants.CmTell))
                {
                    if (Settings.Default.Gui_TTell)
                    {
                        GoogleTranslate.RetreiveLang(mMessage, false, mJp);
                    }
                }
                if (CheckMode(mCode, Constants.CmParty))
                {
                    if (Settings.Default.Gui_TParty)
                    {
                        GoogleTranslate.RetreiveLang(mMessage, false, mJp);
                    }
                }
                if (CheckMode(mCode, Constants.CmTell))
                {
                    if (Settings.Default.Gui_TShout)
                    {
                        GoogleTranslate.RetreiveLang(mMessage, false, mJp);
                    }
                }
                if (CheckMode(mCode, Constants.Cmls))
                {
                    if (Settings.Default.Gui_TLS)
                    {
                        GoogleTranslate.RetreiveLang(mMessage, false, mJp);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chatMode"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        private static Boolean CheckMode(string chatMode, IEnumerable<string> log)
        {
            return log.Any(t => t == chatMode);
        }

        #region " FLOW APPEND FUNCTIONS "

        private static BrushConverter _bc;
        private static Paragraph _pgraph;
        private static Span _tStamp, _tMessage;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void block_Loaded(object sender, RoutedEventArgs e)
        {
            var block = (Block) sender;
            block.BringIntoView();
            block.Loaded -= block_Loaded;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <param name="message"></param>
        /// <param name="color"></param>
        /// <param name="flow"></param>
        public static void AppendFlow(string time, string message, string color, FlowDocumentReader flow)
        {
            _bc = new BrushConverter();
            _pgraph = new Paragraph();
            _tStamp = new Span(new Run(time)) { Foreground = (Brush) _bc.ConvertFrom("#" + Settings.Default.Color_TimeStamp.ToString().Substring(3)), FontWeight = FontWeights.Bold };
            _tMessage = new Span(new Run(message)) { Foreground = (Brush) _bc.ConvertFrom(color) };
            _pgraph.Inlines.Add(_tStamp);
            _pgraph.Inlines.Add(_tMessage);
            flow.Document.Blocks.Add(_pgraph);
            flow.Document.Blocks.LastBlock.Loaded += block_Loaded;
            _tMessage = null;
            _tStamp = null;
            _bc = null;
            _pgraph = null;
        }

        #endregion
    }
}