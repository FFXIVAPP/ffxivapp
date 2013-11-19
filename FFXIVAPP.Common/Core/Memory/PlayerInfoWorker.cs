// FFXIVAPP.Common
// PlayerInfoWorker.cs
// 
// © 2013 Ryan Wilson

#region Usings

using FFXIVAPP.Common.Core.EventHandlers;

#endregion

namespace FFXIVAPP.Common.Core.Memory
{
    public class PlayerInfoWorker : IPlayerEntityEvent
    {
        public event PlayerEntityEventHandler.Handler OnNewEntity = delegate { };

        public void RaiseEntityEvent(PlayerEntity playerEntity)
        {
            OnNewEntity(playerEntity);
        }
    }
}
