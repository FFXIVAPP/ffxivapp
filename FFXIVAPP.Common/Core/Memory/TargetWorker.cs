// FFXIVAPP.Common
// TargetWorker.cs
// 
// © 2013 Ryan Wilson

#region Usings

using FFXIVAPP.Common.Core.EventHandlers;

#endregion

namespace FFXIVAPP.Common.Core.Memory
{
    // internal, not accessible by plugins
    public class TargetWorker : ITargetEntityEvent
    {
        public event TargetEntityEventHandler.Handler OnNewEntity = delegate { };

        public void RaiseEntityEvent(TargetEntity targetEntity)
        {
            OnNewEntity(targetEntity);
        }
    }
}
