// FFXIVAPP.Client
// ChatWorkerDelegate.cs
// 
// © 2013 Ryan Wilson

#region Usings

using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Utilities;
using FFXIVAPP.Common.Core.ChatLog;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Delegates
{
    [DoNotObfuscate]
    internal static class ChatWorkerDelegate
    {
        public static bool IsPaused { get; set; }

        /// <summary>
        /// </summary>
        public static void OnNewLine(ChatLogEntry chatLogEntry)
        {
            if (IsPaused)
            {
                return;
            }
            AppViewModel.Instance.ChatHistory.Add(chatLogEntry);
            // process official plugins
            if (chatLogEntry.Line.ToLower()
                            .StartsWith("com:"))
            {
                LogPublisher.HandleCommands(chatLogEntry);
            }
            LogPublisher.Event.Process(chatLogEntry);
            LogPublisher.Log.Process(chatLogEntry);
            LogPublisher.Parse.Process(chatLogEntry);
            ApplicationContextHelper.GetContext()
                                    .ChatLogWorker.RaiseLineEvent(chatLogEntry);
        }
    }
}
