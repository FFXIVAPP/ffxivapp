// FFXIVAPP.IPluginInterface
// ChatLogEntryEvent.cs
// 
// © 2013 Ryan Wilson

using System;
using FFXIVAPP.Common.Core.Memory;

namespace FFXIVAPP.IPluginInterface.Events
{
    public class ChatLogEntryEvent : EventArgs
    {
        public ChatLogEntryEvent(object sender, ChatLogEntry chatLogEntry)
        {
            Sender = sender;
            ChatLogEntry = chatLogEntry;
        }

        public object Sender { get; set; }
        public ChatLogEntry ChatLogEntry { get; set; }
    }
}
