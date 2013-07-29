// FFXIVAPP.Plugin.Chat
// PluginException.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Runtime.Serialization;

#endregion

namespace FFXIVAPP.Plugin.Chat
{
    [Serializable]
    public class PluginException : Exception
    {
        public PluginException()
        {
        }

        public PluginException(string message) : base(message)
        {
        }

        public PluginException(string message, Exception inner) : base(message, inner)
        {
        }

        protected PluginException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
