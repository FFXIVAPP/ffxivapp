// FFXIVAPP
// ChatEntry.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FFXIVAPP.Classes.Memory
{
    public class ChatEntry
    {
        public DateTime TimeStamp { get; set; }
        public string Code { get; set; }
        public string Line { get; set; }
        public string Combined { get; set; }
        public string Raw { get; set; }
        public byte[] Bytes { get; set; }
        public bool JP;

        /// <summary>
        /// </summary>
        /// <param name="raw"> </param>
        public ChatEntry(byte[] raw)
        {
            Bytes = raw;
            Raw = Encoding.UTF8.GetString(raw.ToArray());
            var cut = (Raw.Substring(5, 1) == ":") ? 6 : 5;
            var cleaned = ChatCleaner.Process(raw, CultureInfo.CurrentUICulture, out JP).Trim();
            Line = cleaned.Substring(cut);
            Code = Raw.Substring(0, 4);
            Combined = String.Format("{0}:{1}", Code, Line);
            TimeStamp = DateTime.Now;
        }
    }
}