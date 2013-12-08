// FFXIVAPP.IPluginInterface
// ParseEntityEvent.cs
// 
// © 2013 Ryan Wilson

using System;
using FFXIVAPP.Common.Core.Parse;

namespace FFXIVAPP.IPluginInterface.Events
{
    public class ParseEntityEvent : EventArgs
    {
        public ParseEntityEvent(object sender, ParseEntity parseEntity)
        {
            Sender = sender;
            ParseEntity = parseEntity;
        }

        public object Sender { get; set; }
        public ParseEntity ParseEntity { get; set; }
    }
}
