// FFXIVAPP.Plugin.Sample
// LogPublisher.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using NLog;

#endregion

namespace FFXIVAPP.Plugin.Sample.Utilities
{
    public static class LogPublisher
    {
        public static void Process(ChatEntry chatEntry)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
            }
        }
    }
}
