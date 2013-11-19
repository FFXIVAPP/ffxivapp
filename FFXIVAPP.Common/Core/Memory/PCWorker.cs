// FFXIVAPP.Common
// PCWorker.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Collections.Generic;
using FFXIVAPP.Common.Core.EventHandlers;

#endregion

namespace FFXIVAPP.Common.Core.Memory
{
    public class PCWorker : IActorEntitiesEvent
    {
        public event ActorEntitiesEventHandler.Handler OnNewEntities = delegate { };

        public void RaiseEntitiesEvent(List<ActorEntity> actorEntities)
        {
            OnNewEntities(actorEntities);
        }
    }
}
