// FFXIVAPP.Plugin.Event
// LogPublisher.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Linq;
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
                    var index = PluginViewModel.Instance.Events.TakeWhile(pair => pair.Key != line)
                        .Count();
                    SoundPlayerHelper.Play(Constants.BaseDirectory, PluginViewModel.Instance.Events[index].Value);
                }
            }
            catch (Exception ex)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
            }
        }
    }
}
