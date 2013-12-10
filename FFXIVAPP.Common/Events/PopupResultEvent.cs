// FFXIVAPP.Common
// PopupResultEvent.cs
// 
// © 2013 Ryan Wilson

using System;

namespace FFXIVAPP.Common.Events
{
    public class PopupResultEvent : EventArgs
    {
        #region Property Bindings

        public object NewValue { get; private set; }

        #endregion

        public PopupResultEvent(object newValue)
        {
            NewValue = newValue;
        }
    }
}
