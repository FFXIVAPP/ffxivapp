// FFXIVAPP.Common
// DispatcherHelper.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace FFXIVAPP.Common.Helpers
{
    public static class DispatcherHelper
    {
        public static void Invoke(Action action, DispatcherPriority dispatcherPriority = DispatcherPriority.Background)
        {
            Application.Current.Dispatcher.BeginInvoke(dispatcherPriority, new ThreadStart((action)));
        }
    }
}
