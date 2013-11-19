// FFXIVAPP.Common
// IChatLogEntryEvent.cs
// 
// © 2013 Ryan Wilson

#region Usings

using FFXIVAPP.Common.Core.Memory;

#endregion

namespace FFXIVAPP.Common.Core.EventHandlers
{
    public interface IChatLogEntryEvent
    {
        event ChatLogEntryEventHandler.Handler OnNewLine;
        void RaiseLineEvent(ChatLogEntry chatLogEntry);
    }
}
