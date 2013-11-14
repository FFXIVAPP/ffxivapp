// FFXIVAPP.Common
// ConstantWorker.cs
// 
// © 2013 Ryan Wilson

namespace FFXIVAPP.Common.Core.Constant
{
    public class ConstantWorker : IConstantWorker
    {
        public delegate void NewValuesEventHandler(ConstantEntry constantEntry);

        public event NewValuesEventHandler OnNewValues = delegate { };

        public void RaiseValuesEvent(ConstantEntry constantEntry)
        {
            OnNewValues(constantEntry);
        }
    }
}
