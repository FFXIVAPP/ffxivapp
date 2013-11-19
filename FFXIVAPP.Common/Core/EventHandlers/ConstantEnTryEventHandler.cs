// FFXIVAPP.Common
// ConstantEntryEventHandler.cs
// 
// © 2013 Ryan Wilson

#region Usings

using FFXIVAPP.Common.Core.Constant;

#endregion

namespace FFXIVAPP.Common.Core.EventHandlers
{
    public static class ConstantEntryEventHandler
    {
        public delegate void Handler(ConstantEntry constantEntry);
    }
}
