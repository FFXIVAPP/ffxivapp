// ParseModXIV
// ChatWorkerDelegate.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using AppModXIV.Classes;
using ParseModXIV.Model;

namespace ParseModXIV.Classes
{
    internal static class ChatWorkerDelegate
    {
        public static readonly XmlWriteLog XmlWriteLog = new XmlWriteLog();
        public static readonly XmlWriteLog XmlWriteUnmatchedLog = new XmlWriteLog();
        private static string[] _abilities = new[] {"0053", "0054", "0055"};
        private static readonly SpeechSynthesizer Speech;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mLine"></param>
        public static void OnRawLine(string mLine)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mTimeStamp"></param>
        /// <param name="mCode"></param>
        /// <param name="mLine"></param>
        public static void OnDebugline(string mTimeStamp, string mCode, string mLine)
        {
            if (mLine.Contains("readies") || mLine.Contains("prépare") || mLine.Contains("をしようとしている。"))
            {
                if (mCode == "0053" || mCode == "0054" || mCode == "0055")
                {
                    AppendFlow(mTimeStamp, mLine, "#FFFFFFFF", MainWindow.View.MainTabControlViewModel.MobAbility_FDR);
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
            if (mCode == "0020")
            {
                Commands.Process(mMessage);
            }
            if (!ParseMod.Instance.IsLogging)
            {
                return;
            }
            if (mMessage.Contains("readies") || mMessage.Contains("prépare") || mLine.Contains("をしようとしている。"))
            {
                if (mCode == "0053" || mCode == "0054" || mCode == "0055")
                {
                    AppendFlow(mTimeStamp, mMessage, "#FFFFFFFF", MainWindow.View.MainTabControlViewModel.MobAbility_FDR);
                }
            }
            Func<bool> d = delegate
            {
                EventParser.Instance.ParseAndPublish(Convert.ToUInt16(mCode, 16), mMessage);
                return true;
            };
            d.BeginInvoke(null, null);
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
        private static void AppendFlow(string time, string message, string color, FlowDocumentReader flow)
        {
            _bc = new BrushConverter();
            _pgraph = new Paragraph();
            _tStamp = new Span(new Run(time)) {Foreground = (Brush) _bc.ConvertFrom("#" + Settings.Default.Color_TimeStamp.ToString().Substring(3)), FontWeight = FontWeights.Bold};
            _tMessage = new Span(new Run(message)) {Foreground = (Brush) _bc.ConvertFrom(color)};
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