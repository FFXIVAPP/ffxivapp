// FFXIVAPP.Common
// ChatLogEntryEventHandler.cs
// 
// © 2013 Ryan Wilson

#region Usings

using FFXIVAPP.Common.Core.Memory;

#endregion

namespace FFXIVAPP.Common.Core.EventHandlers
{
    public static class ChatLogEntryEventHandler
    {
        public delegate void Handler(ChatLogEntry chatLogEntry);
    }
}
