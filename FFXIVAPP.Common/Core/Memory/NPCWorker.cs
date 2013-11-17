// FFXIVAPP.Common
// NPCWorker.cs
// 
// © 2013 Ryan Wilson

#region Usings

#endregion

namespace FFXIVAPP.Common.Core.Memory
{
    // internal, not accessible by plugins
    public class NPCWorker : INPCWorker
    {
        public delegate void NewEntryEventHandler(ActorEntity actorEntity);

        public void RaiseEntryEvent(ActorEntity actorEntity)
        {
            OnNewEntry(actorEntity);
        }

        public event NewEntryEventHandler OnNewEntry = delegate { };
    }
}
