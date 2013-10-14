// FFXIVAPP.Common
// ChatEntry.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;

#endregion

namespace FFXIVAPP.Common.Chat {
    public struct ChatEntry {
        public DateTime TimeStamp { get; set; }
        public string Code { get; set; }
        public string Line { get; set; }
        public string Combined { get; set; }
        public string Raw { get; set; }
        public byte[] Bytes { get; set; }
        public bool JP { get; set; }
    }
}
