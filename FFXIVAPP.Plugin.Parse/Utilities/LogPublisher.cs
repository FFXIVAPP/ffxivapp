// FFXIVAPP.Plugin.Parse
// LogPublisher.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Linq;
using System.Text.RegularExpressions;
using FFXIVAPP.Common.Chat;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.RegularExpressions;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Plugin.Parse.Models.Events;
using FFXIVAPP.Plugin.Parse.Properties;
using FFXIVAPP.Plugin.Parse.Views;
using NLog;

#endregion

namespace FFXIVAPP.Plugin.Parse.Utilities
{
    public static class LogPublisher
    {
        public static void Process(ChatEntry chatEntry)
        {
            try
            {
                var timeStampColor = Settings.Default.TimeStampColor.ToString();
                var timeStamp = chatEntry.TimeStamp.ToString("[HH:mm:ss] ");
                var line = chatEntry.Line.Replace("  ", " ");
                var color = (Constants.Colors.ContainsKey(chatEntry.Code)) ? Constants.Colors[chatEntry.Code][0] : "FFFFFF";
                if (Constants.Abilities.Contains(chatEntry.Code) && Regex.IsMatch(line, @".+(((cast|use)s?|(lance|utilise)z?)\s|の「)", SharedRegEx.DefaultOptions))
                {
                    Common.Constants.FD.AppendFlow(timeStamp, "", line, new[]
                    {
                        timeStampColor, "#" + color
                    }, MainView.View.AbilityChatFD._FDR);
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
