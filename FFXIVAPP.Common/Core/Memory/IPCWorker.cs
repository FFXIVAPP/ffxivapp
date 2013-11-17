// FFXIVAPP.Common
// IPCWorker.cs
// 
// © 2013 Ryan Wilson

namespace FFXIVAPP.Common.Core.Memory
{
    public interface IPCWorker
    {
        event PCWorker.NewEntryEventHandler OnNewEntry;
        void RaiseEntryEvent(ActorEntity actorEntity);
    }
}
