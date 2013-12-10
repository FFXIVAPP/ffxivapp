// FFXIVAPP.Client
// LogPublisher.cs
// 
// © 2013 Ryan Wilson

using FFXIVAPP.Client.Plugins.Parse.Models;
using FFXIVAPP.Common.Core.Memory;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Utilities
{
    [DoNotObfuscate]
    public static partial class LogPublisher
    {
        public static void Process()
        {
        }

        public static void HandleCommands(ChatLogEntry chatLogEntry)
        {
            // process commands
            if (chatLogEntry.Code == "0038")
            {
                var commandsRegEx = CommandBuilder.CommandsRegEx.Match(chatLogEntry.Line.Trim());
                if (commandsRegEx.Success)
                {
                    var plugin = commandsRegEx.Groups["plugin"].Success ? commandsRegEx.Groups["plugin"].Value : "";
                    var command = commandsRegEx.Groups["command"].Success ? commandsRegEx.Groups["command"].Value : "";
                    switch (plugin)
                    {
                        case "parse":
                            switch (command)
                            {
                                case "on":
                                    Parse.IsPaused = false;
                                    break;
                                case "off":
                                    Parse.IsPaused = true;
                                    break;
                                case "reset":
                                    ParseControl.Instance.Reset();
                                    break;
                            }
                            break;
                    }
                }
            }
        }
    }
}
