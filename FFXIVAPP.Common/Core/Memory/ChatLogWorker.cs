// FFXIVAPP.Common
// ChatLogWorker.cs
// 
// © 2013 Ryan Wilson

#region Usings

using FFXIVAPP.Common.Core.EventHandlers;

#endregion

namespace FFXIVAPP.Common.Core.Memory
{
    // internal, not accessible by plugins
    public class ChatLogWorker : IChatLogEntryEvent
    {
        public event ChatLogEntryEventHandler.Handler OnNewLine = delegate { };

        public void RaiseLineEvent(ChatLogEntry chatLogEntry)
        {
            OnNewLine(chatLogEntry);
        }
    }
}
