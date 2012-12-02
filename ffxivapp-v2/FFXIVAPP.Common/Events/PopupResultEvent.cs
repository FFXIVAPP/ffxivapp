// FFXIVAPP.Common
// PopupResultEvent.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

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