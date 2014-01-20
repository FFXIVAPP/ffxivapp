// FFXIVAPP.Client
// LogPublisher.Parse.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FFXIVAPP.Client.Models.Parse.Events;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Client.Views.Parse;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.RegularExpressions;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Utilities
{
    public static partial class LogPublisher
    {
        [DoNotObfuscate]
        public static class Parse
        {
            #region Property Bindings

            private static List<string> _needGreedHistory;

            public static List<string> NeedGreedHistory
            {
                get { return _needGreedHistory ?? (_needGreedHistory = new List<string>()); }
                set
                {
                    if (_needGreedHistory == null)
                    {
                        _needGreedHistory = new List<string>();
                    }
                    _needGreedHistory = value;
                }
            }

            #endregion

            public static Queue<ChatLogEntry> ChatLogEntries = new Queue<ChatLogEntry>();
            public static bool IsPaused = false;
            public static bool Processing { get; set; }

            public static void Process(ChatLogEntry chatLogEntry)
            {
                if (IsPaused)
                {
                    return;
                }
                if (Processing)
                {
                    ChatLogEntries.Enqueue(chatLogEntry);
                }
                Processing = true;
                try
                {
                    chatLogEntry.Line = chatLogEntry.Line.Replace("  ", " ");
                    if (Constants.Parse.NeedGreed.Any(chatLogEntry.Line.Contains))
                    {
                        NeedGreedHistory.Add(chatLogEntry.Line);
                    }
                    DispatcherHelper.Invoke(() => EventParser.Instance.ParseAndPublish(chatLogEntry));
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
