// Project: ChatModXIV
// File: ChatWorkerDelegate.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ChatModXIV.Classes
{
    internal static class ChatWorkerDelegate
    {
        private static readonly string[] ShoutLog = new[] { "0002" };
        private static readonly string[] PrivateLog = new[] { "0001", "0002", "0004", "000E", "0005", "000F", "0006", "0010", "0007", "0011", "0008", "0012", "0009", "0013", "000A", "0014", "000B", "0015", "000C", "001B", "0019", "000D", "0003" };
        private static readonly string[] IgnoreLog = new[] { "0020" };

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
        /// <param name="mLine"></param>
        /// <param name="mJp"></param>
        public static void OnNewline(string mLine, Boolean mJp)
        {
            mLine = mLine.Replace("::", ":").Replace("  ", " ");
            var mServer = Settings.Default.Server;
            var mTimeStamp = DateTime.Now.ToString("[HH:mm:ss]");
            var mCode = mLine.Substring(0, 4);
            var mMessage = mLine.Substring(mLine.IndexOf(":", StringComparison.Ordinal) + 1);
            if (mCode == "" || mMessage == "")
            {
                return;
            }
            if (MainWindow.View.Socket.ReadyState.ToString() != "Open")
            {
                return;
            }
            const bool debug = false;
            if (!debug)
            {
                if (CheckMode(mCode, ShoutLog))
                {
                    MainWindow.View.SendMessage("message", new Message { Type = "Global", Server = mServer, Time = mTimeStamp, Code = mCode, Data = mMessage });
                }

                if (CheckMode(mCode, PrivateLog))
                {
                    MainWindow.View.SendMessage("message", new Message { Type = "Private", Server = mServer, Time = mTimeStamp, Code = mCode, Data = mMessage });
                }

                if (!CheckMode(mCode, IgnoreLog))
                {
                    MainWindow.View.SendMessage("message", new Message { Command = "battle", Server = mServer, Time = mTimeStamp, Code = mCode, Data = mMessage });
                }
            }
            if (!debug)
            {
                return;
            }
            if (CheckMode(mCode, PrivateLog))
            {
                MainWindow.View.SendMessage("message", new Message() { Type = "Global", Server = mServer, Time = mTimeStamp, Code = mCode, Data = mMessage });
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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