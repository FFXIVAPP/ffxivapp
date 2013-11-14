// FFXIVAPP.Common
// IChatLogWorker.cs
// 
// © 2013 Ryan Wilson

namespace FFXIVAPP.Common.Core.ChatLog
{
    public interface IChatLogWorker
    {
        event ChatLogWorker.NewLineEventHandler OnNewLine;
        void RaiseLineEvent(ChatLogEntry chatLogEntry);
    }
}
