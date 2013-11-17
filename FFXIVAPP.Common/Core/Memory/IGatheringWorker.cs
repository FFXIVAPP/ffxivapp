// FFXIVAPP.Common
// IGatheringWorker.cs
// 
// © 2013 Ryan Wilson

namespace FFXIVAPP.Common.Core.Memory
{
    public interface IGatheringWorker
    {
        event GatheringWorker.NewEntryEventHandler OnNewEntry;
        void RaiseEntryEvent(ActorEntity actorEntity);
    }
}
