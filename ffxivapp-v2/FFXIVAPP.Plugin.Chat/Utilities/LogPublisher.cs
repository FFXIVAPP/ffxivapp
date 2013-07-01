// FFXIVAPP.Plugin.Chat
// LogPublisher.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using FFXIVAPP.Common.Chat;
using FFXIVAPP.Common.Utilities;
using NLog;

#endregion

namespace FFXIVAPP.Plugin.Chat.Utilities
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
