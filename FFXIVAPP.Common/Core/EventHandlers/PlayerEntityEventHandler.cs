// FFXIVAPP.Common
// PlayerEntityEventHandler.cs
// 
// © 2013 Ryan Wilson

#region Usings

using FFXIVAPP.Common.Core.Memory;

#endregion

namespace FFXIVAPP.Common.Core.EventHandlers
{
    public static class PlayerEntityEventHandler
    {
        public delegate void Handler(PlayerEntity playerEntity);
    }
}
