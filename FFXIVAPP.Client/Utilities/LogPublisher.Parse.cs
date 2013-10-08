// FFXIVAPP.Client
// LogPublisher.Parse.cs
// 
// © 2013 Ryan Wilson


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FFXIVAPP.Client.Memory;
using FFXIVAPP.Client.Models.Parse;
using FFXIVAPP.Client.Models.Parse.Events;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Client.Views.Plugins.Parse;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.RegularExpressions;
using FFXIVAPP.Common.Utilities;
using NLog;

namespace FFXIVAPP.Client.Utilities
{
    public static partial class LogPublisher
    {
        public static class Parse
        {
            public static void Process(ChatEntry chatEntry)
            {
                try
                {
                    var timeStampColor = Settings.Default.TimeStampColor.ToString();
                    var timeStamp = chatEntry.TimeStamp.ToString("[HH:mm:ss] ");
                    var line = chatEntry.Line.Replace("  ", " ");
                    var color = (Constants.Colors.ContainsKey(chatEntry.Code)) ? Constants.Colors[chatEntry.Code][0] : "FFFFFF";
                    // process commands
                    if (chatEntry.Code == "0038")
                    {
                        var parseCommands = CommandBuilder.CommandsRegEx.Match(chatEntry.Line.Trim());
                        if (parseCommands.Success)
                        {
                            var cmd = parseCommands.Groups["cmd"].Success ? parseCommands.Groups["cmd"].Value : "";
                            var sub = parseCommands.Groups["sub"].Success ? parseCommands.Groups["sub"].Value : "";
                            switch (cmd)
                            {
                                case "parse":
                                    switch (sub)
                                    {
                                        case "reset":
                                            ParseControl.Instance.Reset();
                                            break;
                                        case "toggle":
                                            ParseControl.Instance.Toggle();
                                            break;
                                    }
                                    break;
                                default:
                                    List<string> temp;
                                    CommandBuilder.GetCommands(chatEntry.Line, out temp);
                                    //if (temp != null)
                                    //{
                                    //    Host.Commands(PName, temp);
                                    //}
                                    break;
                            }
                        }
                    }
                    if (Constants.Parse.Abilities.Contains(chatEntry.Code) && Regex.IsMatch(line, @".+(((cast|use)s?|(lance|utilise)z?)\s|の「)", SharedRegEx.DefaultOptions))
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
}
