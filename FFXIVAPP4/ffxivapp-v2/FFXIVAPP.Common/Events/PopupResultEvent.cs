// FFXIVAPP.Common
// PopupResultEvent.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;

#endregion

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
