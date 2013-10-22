// FFXIVAPP.Client
// LogPublisher.Event.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Text.RegularExpressions;
using System.Timers;
using FFXIVAPP.Client.Memory;
using FFXIVAPP.Client.Plugins.Event;
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
        public static class Event
        {
            public static bool IsPaused = false;
            public static void Process(ChatEntry chatEntry)
            {
                if (IsPaused)
                {
                    return;
                }
                try
                {
                    var line = chatEntry.Line.Replace("  ", " ");
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
                                DispatcherHelper.Invoke(() => SoundPlayerHelper.Play(AppViewModel.Instance.SoundsPath, soundEvent.Sound));
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
}
