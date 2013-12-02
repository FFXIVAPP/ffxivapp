// FFXIVAPP.Common
// IApplicationContext.cs
// 
// © 2013 Ryan Wilson

#region Usings

using FFXIVAPP.Common.Core.EventHandlers;

#endregion

namespace FFXIVAPP.Common.Core
{
    public interface IApplicationContext
    {
        IConstantEntryEvent ConstantWorker { get; }
        IChatLogEntryEvent ChatLogWorker { get; }
        IActorEntitiesEvent MonsterWorker { get; }
        IActorEntitiesEvent NPCWorker { get; }
        IActorEntitiesEvent PCWorker { get; }
        IPlayerEntityEvent PlayerInfoWorker { get; }
        IAgroEntriesEvent AgroWorker { get; }
    }
}
