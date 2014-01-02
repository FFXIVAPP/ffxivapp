// FFXIVAPP.Common
// LogItem.cs
// 
// © 2013 Ryan Wilson

using System;
using NLog;

namespace FFXIVAPP.Common.Models
{
    public class LogItem
    {
        public LogItem(string message = "", Exception exception = null, LogLevel logLevel = null)
        {
            LogLevel = logLevel ?? LogLevel.Trace;
            Message = message;
            Exception = exception;
        }

        public LogLevel LogLevel { get; set; }
        public Exception Exception { get; set; }
        public string Message { get; set; }
    }
}
