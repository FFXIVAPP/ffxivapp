// FFXIVAPP.Common
// TargetEntityEventHandler.cs
// 
// © 2013 Ryan Wilson

#region Usings

using FFXIVAPP.Common.Core.Memory;

#endregion

namespace FFXIVAPP.Common.Core.EventHandlers
{
    public static class TargetEntityEventHandler
    {
        public delegate void Handler(TargetEntity targetEntity);
    }
}
