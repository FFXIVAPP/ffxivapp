// FFXIVAPP.Client
// LogPublisher.Parse.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FFXIVAPP.Client.Memory;
using FFXIVAPP.Client.Plugins.Parse.Models.Events;
using FFXIVAPP.Client.Plugins.Parse.Views;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.RegularExpressions;
using FFXIVAPP.Common.Utilities;
using NLog;
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

            public static bool IsPaused = false;

            public static void Process(ChatEntry chatEntry)
            {
                if (IsPaused)
                {
                    return;
                }
                try
                {
                    var timeStampColor = Settings.Default.TimeStampColor.ToString();
                    var timeStamp = chatEntry.TimeStamp.ToString("[HH:mm:ss] ");
                    var line = chatEntry.Line.Replace("  ", " ");
                    var color = (Constants.Colors.ContainsKey(chatEntry.Code)) ? Constants.Colors[chatEntry.Code][0] : "FFFFFF";
                    if (Constants.Parse.Abilities.Contains(chatEntry.Code) && Regex.IsMatch(line, @".+(((cast|use)s?|(lance|utilise)z?)\s|の「)", SharedRegEx.DefaultOptions))
                    {
                        Common.Constants.FD.AppendFlow(timeStamp, "", line, new[]
                        {
                            timeStampColor, "#" + color
                        }, MainView.View.AbilityChatFD._FDR);
                    }
                    if (Constants.Parse.NeedGreed.Any(line.Contains))
                    {
                        NeedGreedHistory.Add(line);
                    }
                    DispatcherHelper.Invoke(() => EventParser.Instance.ParseAndPublish(Convert.ToUInt32(chatEntry.Code, 16), line));
                }
                catch (Exception ex)
                {
                    Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
                }
            }
        }
    }
}
