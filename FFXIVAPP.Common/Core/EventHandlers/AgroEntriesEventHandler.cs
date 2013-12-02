// FFXIVAPP.Common
// AgroEntriesEventHandler.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Collections.Generic;

#endregion

namespace FFXIVAPP.Common.Core.EventHandlers
{
    public static class AgroEntriesEventHandler
    {
        public delegate void Handler(List<uint> agroEntries);
    }
}
