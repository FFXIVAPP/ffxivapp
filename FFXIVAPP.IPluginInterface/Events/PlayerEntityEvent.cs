// FFXIVAPP.IPluginInterface
// PlayerEntityEvent.cs
// 
// © 2013 Ryan Wilson

using System;
using FFXIVAPP.Common.Core.Memory;

namespace FFXIVAPP.IPluginInterface.Events
{
    public class PlayerEntityEvent : EventArgs
    {
        public PlayerEntityEvent(object sender, PlayerEntity playerEntity)
        {
            Sender = sender;
            PlayerEntity = playerEntity;
        }

        public object Sender { get; set; }
        public PlayerEntity PlayerEntity { get; set; }
    }
}
