// FFXIVAPP.Common
// ITargetEntityEvent.cs
// 
// © 2013 Ryan Wilson

#region Usings

using FFXIVAPP.Common.Core.Memory;

#endregion

namespace FFXIVAPP.Common.Core.EventHandlers
{
    public interface ITargetEntityEvent
    {
        event TargetEntityEventHandler.Handler OnNewEntity;
        void RaiseEntityEvent(TargetEntity targetEntity);
    }
}
