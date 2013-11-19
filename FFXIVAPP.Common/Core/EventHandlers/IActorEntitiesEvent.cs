// FFXIVAPP.Common
// IActorEntitiesEvent.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Collections.Generic;
using FFXIVAPP.Common.Core.Memory;

#endregion

namespace FFXIVAPP.Common.Core.EventHandlers
{
    public interface IActorEntitiesEvent
    {
        event ActorEntitiesEventHandler.Handler OnNewEntities;
        void RaiseEntitiesEvent(List<ActorEntity> actorEntities);
    }
}
