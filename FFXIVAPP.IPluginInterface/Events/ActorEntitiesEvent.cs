// FFXIVAPP.IPluginInterface
// ActorEntitiesEvent.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using FFXIVAPP.Common.Core.Memory;

namespace FFXIVAPP.IPluginInterface.Events
{
    public class ActorEntitiesEvent : EventArgs
    {
        public ActorEntitiesEvent(object sender, List<ActorEntity> actorEntities)
        {
            Sender = sender;
            ActorEntities = actorEntities;
        }

        public object Sender { get; set; }
        public List<ActorEntity> ActorEntities { get; set; }
    }
}
