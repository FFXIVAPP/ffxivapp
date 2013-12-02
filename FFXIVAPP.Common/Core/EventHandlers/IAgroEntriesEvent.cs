// FFXIVAPP.Common
// IAgroEntriesEvent.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Collections.Generic;

#endregion

namespace FFXIVAPP.Common.Core.EventHandlers
{
    public interface IAgroEntriesEvent
    {
        event AgroEntriesEventHandler.Handler OnNewEntries;
        void RaiseEntriesEvent(List<uint> agroEntries);
    }
}
