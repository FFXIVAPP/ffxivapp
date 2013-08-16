// FFXIVAPP.Client
// ChatWorkerDelegate.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using FFXIVAPP.Client.Memory;
using FFXIVAPP.Client.Models;
using FFXIVAPP.Common.Utilities;
using NLog;

#endregion

namespace FFXIVAPP.Client.Delegates
{
    internal static class ChatWorkerDelegate
    {
        public static bool IsPaused = false;

        /// <summary>
        /// </summary>
        public static void OnNewLine(ChatEntry chatEntry)
        {
            if (IsPaused)
            {
                return;
            }
            var entry = new object[]
            {
                chatEntry.Bytes, chatEntry.Code, chatEntry.Combined, chatEntry.JP, chatEntry.Line, chatEntry.Raw, chatEntry.TimeStamp
            };
            AppViewModel.Instance.ChatHistory.Add(chatEntry);
            foreach (PluginInstance pluginInstance in App.Plugins.Loaded)
            {
                bool success;
                pluginInstance.Instance.OnNewLine(out success, entry);
                if (success)
                {
                    continue;
                }
                var notice = pluginInstance.Instance.Notice;
                var exception = pluginInstance.Instance.Trace;
                Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("MessageFailed {0}:\n{1}", notice, exception.StackTrace));
            }
        }
    }
}
