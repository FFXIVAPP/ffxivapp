// FFXIVAPP.Common
// ChatLogWorker.cs
// 
// © 2013 Ryan Wilson

#region Usings

#endregion

namespace FFXIVAPP.Common.Core.Memory
{
    // internal, not accessible by plugins
    public class ChatLogWorker : IChatLogWorker
    {
        public delegate void NewLineEventHandler(ChatLogEntry chatLogEntry);

        public event NewLineEventHandler OnNewLine = delegate { };

        public void RaiseLineEvent(ChatLogEntry chatLogEntry)
        {
            OnNewLine(chatLogEntry);
        }
    }
}
