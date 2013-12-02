// FFXIVAPP.Common
// AgroWorker.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Collections.Generic;
using FFXIVAPP.Common.Core.EventHandlers;

#endregion

namespace FFXIVAPP.Common.Core.Memory
{
    // internal, not accessible by plugins
    public class AgroWorker : IAgroEntriesEvent
    {
        public event AgroEntriesEventHandler.Handler OnNewEntries = delegate { };

        public void RaiseEntriesEvent(List<uint> agroEntries)
        {
            OnNewEntries(agroEntries);
        }
    }
}
