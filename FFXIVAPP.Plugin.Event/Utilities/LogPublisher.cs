// FFXIVAPP.Plugin.Event
// LogPublisher.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Text.RegularExpressions;
using FFXIVAPP.Common.Chat;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.RegularExpressions;
using FFXIVAPP.Common.Utilities;
using NLog;

#endregion

namespace FFXIVAPP.Plugin.Event.Utilities
{
    public static class LogPublisher
    {
        public static void Process(ChatEntry chatEntry)
        {
            try
            {
                var line = chatEntry.Line.Replace("  ", " ");
                foreach (var item in PluginViewModel.Instance.Events)
                {
                    var resuccess = false;
                    var check = new Regex(item.Key);
                    if (SharedRegEx.IsValidRegex(item.Key))
                    {
                        var reg = check.Match(line);
                        if (reg.Success)
                        {
                            resuccess = true;
                        }
                    }
                    else
                    {
                        resuccess = (item.Key == line);
                    }
                    if (!resuccess)
                    {
                        continue;
                    }
                    SoundPlayerHelper.Play(Constants.BaseDirectory, item.Value);
                }
            }
            catch (Exception ex)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
            }
        }
    }
}
