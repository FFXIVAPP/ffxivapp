// FFXIVAPP.Common
// ActorEntitiesEventHandler.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Collections.Generic;
using FFXIVAPP.Common.Core.Memory;

#endregion

namespace FFXIVAPP.Common.Core.EventHandlers
{
    public static class ActorEntitiesEventHandler
    {
        public delegate void Handler(List<ActorEntity> actorEntities);
    }
}
