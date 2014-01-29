// FFXIVAPP.Plugin.Radar
// PluginException.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Runtime.Serialization;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Plugin.Radar
{
    [DoNotObfuscate]
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
