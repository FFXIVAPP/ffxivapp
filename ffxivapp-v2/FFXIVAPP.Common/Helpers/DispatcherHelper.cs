// FFXIVAPP.Common
// DispatcherHelper.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

#endregion

namespace FFXIVAPP.Common.Helpers
{
    public static class DispatcherHelper
    {
        public static void Invoke(Action action)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, new ThreadStart((action)));
        }
    }
}
