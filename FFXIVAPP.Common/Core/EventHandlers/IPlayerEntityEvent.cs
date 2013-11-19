// FFXIVAPP.Common
// IPlayerEntityEvent.cs
// 
// © 2013 Ryan Wilson

#region Usings

using FFXIVAPP.Common.Core.Memory;

#endregion

namespace FFXIVAPP.Common.Core.EventHandlers
{
    public interface IPlayerEntityEvent
    {
        event PlayerEntityEventHandler.Handler OnNewEntity;
        void RaiseEntityEvent(PlayerEntity playerEntity);
    }
}
