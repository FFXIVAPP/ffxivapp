// FFXIVAPP
// FightList.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Threading;

namespace FFXIVAPP.Models
{
    public sealed class FightList : ConcurrentStack<Fight>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        /// <summary>
        /// </summary>
        /// <param name="fights"> </param>
        public FightList(params Fight[] fights) : base(fights)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="f"> </param>
        public void Add(Fight f)
        {
            Push(f);
            DoCollectionChanged(NotifyCollectionChangedAction.Add, f);
        }

        /// <summary>
        /// </summary>
        /// <param name="result"> </param>
        /// <returns> </returns>
        public Boolean TryGetLastOrCurrent(out Fight result)
        {
            return TryPeek(out result);
        }

        /// <summary>
        /// </summary>
        /// <param name="mobName"> </param>
        /// <param name="result"> </param>
        /// <returns> </returns>
        public Boolean TryGetLastOrCurrent(String mobName, out Fight result)
        {
            foreach (var f in this.Where(f => f.MobName.ToLower() == mobName.ToLower()))
            {
                result = f;
                return true;
            }
            result = null;
            return false;
        }

        #region ConcurrentStack Hides

        /// <summary>
        /// </summary>
        /// <param name="f"> </param>
        private new void Push(Fight f)
        {
            base.Push(f);
        }

        /// <summary>
        /// </summary>
        /// <param name="fights"> </param>
        private new void PushRange(Fight[] fights)
        {
            base.PushRange(fights);
        }

        /// <summary>
        /// </summary>
        /// <param name="result"> </param>
        /// <returns> </returns>
        private new Boolean TryPeek(out Fight result)
        {
            return base.TryPeek(out result);
        }

        /// <summary>
        /// </summary>
        /// <param name="result"> </param>
        /// <returns> </returns>
        private new Boolean TryPop(out Fight result)
        {
            return base.TryPop(out result);
        }

        /// <summary>
        /// </summary>
        /// <param name="fights"> </param>
        /// <param name="startIndex"> </param>
        /// <param name="count"> </param>
        /// <returns> </returns>
        private new int TryPopRange(Fight[] fights, int startIndex, int count)
        {
            return base.TryPopRange(fights, startIndex, count);
        }

        /// <summary>
        /// </summary>
        /// <param name="fights"> </param>
        /// <returns> </returns>
        private new int TryPopRange(Fight[] fights)
        {
            return base.TryPopRange(fights);
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void DoPropertyChanged(String name)
        {
            var propChanged = PropertyChanged;
            if (propChanged != null)
            {
                propChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion

        #region Implementation of INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void DoCollectionChanged(NotifyCollectionChangedAction howSo, params Fight[] whatChanged)
        {
            var collectionChanged = CollectionChanged;
            if (collectionChanged == null)
            {
                return;
            }
            var dispatcher = (from eh in collectionChanged.GetInvocationList() let dpo = eh.Target as DispatcherObject where dpo != null select dpo.Dispatcher).FirstOrDefault();
            if (dispatcher != null && dispatcher.CheckAccess() == false)
            {
                dispatcher.Invoke(DispatcherPriority.DataBind, (Action) (() => DoCollectionChanged(howSo, whatChanged)));
            }
            else
            {
                collectionChanged(this, new NotifyCollectionChangedEventArgs(howSo, whatChanged));
            }
        }

        #endregion
    }
}