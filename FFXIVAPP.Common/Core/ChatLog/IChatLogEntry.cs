// FFXIVAPP.Common
// IChatLogEntry.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;

#endregion

namespace FFXIVAPP.Common.Core.ChatLog
{
    public interface IChatLogEntry
    {
        DateTime TimeStamp { get; set; }
        string Code { get; set; }
        string Line { get; set; }
        string Combined { get; set; }
        string Raw { get; set; }
        byte[] Bytes { get; set; }
        bool JP { get; set; }
    }
}
