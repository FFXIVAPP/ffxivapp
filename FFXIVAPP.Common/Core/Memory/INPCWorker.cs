// FFXIVAPP.Common
// INPCWorker.cs
// 
// © 2013 Ryan Wilson

namespace FFXIVAPP.Common.Core.Memory
{
    public interface INPCWorker
    {
        event NPCWorker.NewEntryEventHandler OnNewEntry;
        void RaiseEntryEvent(ActorEntity actorEntity);
    }
}
