// FFXIVAPP.Common
// DispatcherHelper.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace FFXIVAPP.Common.Helpers
{
    public class DispatcherHelper
    {
        public static void Invoke(Action action)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, new ThreadStart((action)));
        }
    }
}