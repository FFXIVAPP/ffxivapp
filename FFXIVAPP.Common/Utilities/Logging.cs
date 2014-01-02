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
            switch (String.IsNullOrWhiteSpace(logItem.Message))
            {
                
                case true:
                    if (logItem.Exception == null)
                    {
                        return;
                    }
                    logger.LogException(logItem.LogLevel, logItem.Exception.Message, logItem.Exception);
                    break;
                case false:
                    if (logItem.Exception == null)
                    {
                        logger.Log(logItem.LogLevel, logItem.Message);
                    }
                    else
                    {
                        logger.LogException(logItem.LogLevel, logItem.Message, logItem.Exception);
                    }
                    break;
            }
        }
    }
}
