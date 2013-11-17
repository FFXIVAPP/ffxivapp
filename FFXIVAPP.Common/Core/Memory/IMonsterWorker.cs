// FFXIVAPP.Common
// IMonsterWorker.cs
// 
// © 2013 Ryan Wilson

namespace FFXIVAPP.Common.Core.Memory
{
    public interface IMonsterWorker
    {
        event MonsterWorker.NewEntryEventHandler OnNewEntry;
        void RaiseEntryEvent(ActorEntity actorEntity);
    }
}
