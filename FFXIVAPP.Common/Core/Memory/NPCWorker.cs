// FFXIVAPP.Common
// NPCWorker.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Collections.Generic;
using FFXIVAPP.Common.Core.EventHandlers;

#endregion

namespace FFXIVAPP.Common.Core.Memory
{
    // internal, not accessible by plugins
    public class NPCWorker : IActorEntitiesEvent
    {
        public event ActorEntitiesEventHandler.Handler OnNewEntities = delegate { };

        public void RaiseEntitiesEvent(List<ActorEntity> actorEntities)
        {
            OnNewEntities(actorEntities);
        }
    }
}
