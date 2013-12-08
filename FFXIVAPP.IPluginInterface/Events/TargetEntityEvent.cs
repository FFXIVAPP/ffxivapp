// FFXIVAPP.IPluginInterface
// TargetEntityEvent.cs
// 
// © 2013 Ryan Wilson

using System;
using FFXIVAPP.Common.Core.Memory;

namespace FFXIVAPP.IPluginInterface.Events
{
    public class TargetEntityEvent : EventArgs
    {
        public TargetEntityEvent(object sender, TargetEntity targetEntity)
        {
            Sender = sender;
            TargetEntity = targetEntity;
        }

        public object Sender { get; set; }
        public TargetEntity TargetEntity { get; set; }
    }
}
