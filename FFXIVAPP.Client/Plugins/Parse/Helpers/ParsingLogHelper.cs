// FFXIVAPP.Client
// ParsingLogHelper.cs
// 
// © 2013 Ryan Wilson

using System;
using FFXIVAPP.Client.Plugins.Parse.Models;
using FFXIVAPP.Client.Plugins.Parse.Models.Events;
using FFXIVAPP.Common.Utilities;
using NLog;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Plugins.Parse.Helpers
{
    [DoNotObfuscate]
    public static class ParsingLogHelper
    {
        public static void Log(Logger logger, string type, Event e, Expressions exp = null)
        {
            var cleaned = String.Format("{0}:{1}", e.ChatLogEntry.Code, e.ChatLogEntry.Line);
            if (exp != null)
            {
                cleaned = String.Format("{0}:{1}", e.Code, exp.Cleaned);
            }
            var data = String.Format("Unknown {0} Line -> [Type:{1}][Subject:{2}][Direction:{3}] {4}", type, e.Type, e.Subject, e.Direction, cleaned);
            Logging.Log(logger, data);
        }

        public static void Error(Logger logger, string type, Event e, Exception ex)
        {
            var data = String.Format("{0} Error: [{1}] Line -> {2} StackTrace: \n{3}", type, ex.Message, e.ChatLogEntry.Line, ex.StackTrace);
            Logging.Log(logger, data, ex);
        }
    }
}
