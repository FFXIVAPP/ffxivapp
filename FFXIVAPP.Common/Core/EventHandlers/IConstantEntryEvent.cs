// FFXIVAPP.Common
// IConstantEntryEvent.cs
// 
// © 2013 Ryan Wilson

#region Usings

using FFXIVAPP.Common.Core.Constant;

#endregion

namespace FFXIVAPP.Common.Core.EventHandlers
{
    public interface IConstantEntryEvent
    {
        event ConstantEntryEventHandler.Handler OnNewValues;
        void RaiseValuesEvent(ConstantEntry constantEntry);
    }
}
