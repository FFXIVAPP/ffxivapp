// FFXIVAPP.Common
// IConstantWorker.cs
// 
// © 2013 Ryan Wilson

namespace FFXIVAPP.Common.Core.Constant
{
    public interface IConstantWorker
    {
        event ConstantWorker.NewValuesEventHandler OnNewValues;
        void RaiseValuesEvent(ConstantEntry constantEntry);
    }
}
