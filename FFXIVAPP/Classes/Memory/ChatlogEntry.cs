using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FFXIVAPP.Classes.Memory
{
    public class ChatlogEntry
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
        public ChatlogEntry(byte[] raw)
        {
            Bytes = raw;
            Raw = Encoding.UTF8.GetString(raw.ToArray());
            var cut = (Raw.Substring(5, 1) == ":") ? 6 : 5;
            var cleaned = ChatlogCleaner.Process(raw, CultureInfo.CurrentUICulture, out JP);
            Line = cleaned.Substring(cut);
            Code = Raw.Substring(0, 4);
            Combined = String.Format("{0}:{1}", Code, Line);
            TimeStamp = DateTime.Now;
        }
    }
}
