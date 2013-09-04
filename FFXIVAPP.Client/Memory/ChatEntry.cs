// FFXIVAPP.Client
// ChatEntry.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms.VisualStyles;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Common.Helpers;

#endregion

namespace FFXIVAPP.Client.Memory
{
    public class ChatEntry
    {
        public bool JP;

        /// <summary>
        /// </summary>
        /// <param name="raw"> </param>
        public ChatEntry(byte[] raw)
        {
            try
            {
                Bytes = raw;
                Raw = Encoding.UTF8.GetString(raw.ToArray());
                var cut = (Raw.Substring(13, 1) == ":") ? 14 : 13;
                var cleaned = new ChatCleaner(raw, CultureInfo.CurrentUICulture, out JP).Result;
                Line = XmlHelper.SanitizeXmlString(cleaned.Substring(cut));
                Line = new ChatCleaner(Line).Result;
                Code = Raw.Substring(8, 4);
                Combined = String.Format("{0}:{1}", Code, Line);
            }
            catch (Exception ex)
            {
                Bytes = new byte[0];
                Raw = "";
                Line = "";
                Code = "";
                Combined = "";
            }
            TimeStamp = DateTimeHelper.UnixTimeStampToDateTime(Int32.Parse(Raw.Substring(0, 8), NumberStyles.HexNumber));
        }

        public DateTime TimeStamp { get; private set; }
        public string Code { get; private set; }
        public string Line { get; private set; }
        public string Combined { get; private set; }
        public string Raw { get; private set; }
        public byte[] Bytes { get; private set; }
    }
}
