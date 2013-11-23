// FFXIVAPP.Plugin.Event
// LogPublisher.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Text.RegularExpressions;
using System.Timers;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.RegularExpressions;
using FFXIVAPP.Common.Utilities;
using NLog;

#endregion

namespace FFXIVAPP.Plugin.Event.Utilities
{
    public static class LogPublisher
    {
        public static void Process(ChatLogEntry chatLogEntry)
        {
            try
            {
                var line = chatLogEntry.Line.Replace("  ", " ");
                foreach (var item in PluginViewModel.Instance.Events)
                {
                    var resuccess = false;
                    var check = new Regex(item.RegEx);
                    if (SharedRegEx.IsValidRegex(item.RegEx))
                    {
                        var reg = check.Match(line);
                        if (reg.Success)
                        {
                            resuccess = true;
                        }
                    }
                    else
                    {
                        resuccess = (item.RegEx == line);
                    }
                    if (!resuccess)
                    {
                        continue;
                    }
                    var soundEvent = item;
                    Func<bool> playSound = delegate
                    {
                        var delay = soundEvent.Delay;
                        var timer = new Timer(delay > 0 ? delay * 1000 : 1);
                        ElapsedEventHandler timerEventHandler = null;
                        timerEventHandler = delegate
                        {
                            DispatcherHelper.Invoke(() => SoundPlayerHelper.Play(Constants.BaseDirectory, soundEvent.Sound));
                            timer.Elapsed -= timerEventHandler;
                        };
                        timer.Elapsed += timerEventHandler;
                        timer.Start();
                        return true;
                    };
                    playSound.BeginInvoke(null, null);
                }
            }
            catch (Exception ex)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
            }
        }
    }
}
