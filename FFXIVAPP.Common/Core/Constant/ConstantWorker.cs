// FFXIVAPP.Common
// ConstantWorker.cs
// 
// © 2013 Ryan Wilson

#region Usings

using FFXIVAPP.Common.Core.EventHandlers;

#endregion

namespace FFXIVAPP.Common.Core.Constant
{
    public class ConstantWorker : IConstantEntryEvent
    {
        public event ConstantEntryEventHandler.Handler OnNewValues = delegate { };

        public void RaiseValuesEvent(ConstantEntry constantEntry)
        {
            OnNewValues(constantEntry);
        }
    }
}
