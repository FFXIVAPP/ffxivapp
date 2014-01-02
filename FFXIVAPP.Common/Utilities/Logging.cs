// FFXIVAPP.Common
// Logging.cs
// 
// © 2013 Ryan Wilson

using System;
using FFXIVAPP.Common.Models;
using NLog;

namespace FFXIVAPP.Common.Utilities
{
    public static class Logging
    {
        /// <summary>
        /// Default trace logger. Use Log(logger, logItem) to specify trace level
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Log(Logger logger, string message = "", Exception exception = null)
        {
            Log(logger, new LogItem(message, exception));
        }

        /// <summary>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="logItem"></param>
        public static void Log(Logger logger, LogItem logItem)
        {
            if (!Constants.EnableNLog)
            {
                return;
            }
            var message = String.IsNullOrWhiteSpace(logItem.Message) ? " :: Log Message Undefined :: " : logItem.Message;
            const string format = "HandlingEvent : {0}\n\n";
            const string exceptionFormat = "HandlingEvent : {0} ::\n Extended Info ::\n{1}\n{2}\n\n";
            var finalized = "";
            var ex = logItem.Exception;
            finalized = ex == null ? String.Format(format, message) : String.Format(exceptionFormat, message, ex.Message, ex.StackTrace);
            switch (logItem.LogLevel.ToString())
            {
                case "Debug":
                    logger.Debug(finalized);
                    break;
                case "Error":
                    logger.Error(finalized);
                    break;
                case "Fatal":
                    logger.Fatal(finalized);
                    break;
                case "Info":
                    logger.Info(finalized);
                    break;
                case "Trace":
                    logger.Trace(finalized);
                    break;
                case "Warn":
                    logger.Warn(finalized);
                    break;
            }
        }
    }
}
