// FFXIVAPP.Client
// ChatEntry.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System;
using System.Globalization;
using System.Linq;
using System.Text;
using FFXIVAPP.Common.Helpers;

namespace FFXIVAPP.Client.Memory
{
    public class ChatEntry
    {
        public DateTime TimeStamp { get; private set; }
        public string Code { get; private set; }
        public string Line { get; private set; }
        public string Combined { get; private set; }
        public string Raw { get; private set; }
        public byte[] Bytes { get; private set; }
        public bool JP;

        /// <summary>
        /// </summary>
        /// <param name="raw"> </param>
        public ChatEntry(byte[] raw)
        {
            Bytes = raw;
            Raw = Encoding.UTF8.GetString(raw.ToArray());
            var cut = (Raw.Substring(5, 1) == ":") ? 6 : 5;
            var cleaner = new ChatCleaner(raw, CultureInfo.CurrentUICulture, out JP);
            var cleaned = cleaner.Result;
            Line = XmlHelper.SanitizeXmlString(cleaned.Substring(cut));
            Code = Raw.Substring(0, 4);
            Combined = String.Format("{0}:{1}", Code, Line);
            TimeStamp = DateTime.Now;
        }
    }
}