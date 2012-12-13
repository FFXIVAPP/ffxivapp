// FFXIVAPP
// CommandManagerHelper.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

#endregion

namespace FFXIVAPP.Classes.Helpers
{
    internal class CommandManagerHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="handlers"> </param>
        internal static void CallWeakReferenceHandlers(List<WeakReference> handlers)
        {
            if (handlers == null)
            {
                return;
            }
            var callees = new EventHandler[handlers.Count];
            var count = 0;
            for (var i = handlers.Count - 1; i >= 0; i--)
            {
                var reference = handlers[i];
                var handler = reference.Target as EventHandler;
                if (handler == null)
                {
                    handlers.RemoveAt(i);
                }
                else
                {
                    callees[count] = handler;
                    count++;
                }
            }
            for (var i = 0; i < count; i++)
            {
                var handler = callees[i];
                handler(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="handlers"> </param>
        internal static void AddHandlersToRequerySuggested(IEnumerable<WeakReference> handlers)
        {
            if (handlers == null)
            {
                return;
            }
            foreach (var handler in handlers.Select(handlerRef => handlerRef.Target).OfType<EventHandler>())
            {
                CommandManager.RequerySuggested += handler;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="handlers"> </param>
        internal static void RemoveHandlersFromRequerySuggested(IEnumerable<WeakReference> handlers)
        {
            if (handlers == null)
            {
                return;
            }
            foreach (var handler in handlers.Select(handlerRef => handlerRef.Target).OfType<EventHandler>())
            {
                CommandManager.RequerySuggested -= handler;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="handlers"> </param>
        /// <param name="handler"> </param>
        internal static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler)
        {
            AddWeakReferenceHandler(ref handlers, handler, -1);
        }

        /// <summary>
        /// </summary>
        /// <param name="handlers"> </param>
        /// <param name="handler"> </param>
        /// <param name="defaultListSize"> </param>
        internal static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler, int defaultListSize)
        {
            if (handlers == null)
            {
                handlers = (defaultListSize > 0 ? new List<WeakReference>(defaultListSize) : new List<WeakReference>());
            }

            handlers.Add(new WeakReference(handler));
        }

        /// <summary>
        /// </summary>
        /// <param name="handlers"> </param>
        /// <param name="handler"> </param>
        internal static void RemoveWeakReferenceHandler(List<WeakReference> handlers, EventHandler handler)
        {
            if (handlers == null)
            {
                return;
            }
            for (var i = handlers.Count - 1; i >= 0; i--)
            {
                var reference = handlers[i];
                var existingHandler = reference.Target as EventHandler;
                if ((existingHandler == null) || (existingHandler == handler))
                {
                    handlers.RemoveAt(i);
                }
            }
        }
    }
}
