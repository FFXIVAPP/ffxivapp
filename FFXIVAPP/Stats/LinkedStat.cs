// FFXIVAPP
// LinkedStat.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Collections.Generic;

namespace FFXIVAPP.Stats
{
    public abstract class LinkedStat : Stat<decimal>, ILinkedStat
    {
        private readonly List<Stat<decimal>> _dependencyList = new List<Stat<decimal>>(20);
        public event EventHandler<StatChangedEvent> OnDependencyValueChanged;

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="dependencies"> </param>
        protected LinkedStat(String name, params Stat<Decimal>[] dependencies) : base(name)
        {
            SetupStats(dependencies);
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="value"> </param>
        protected LinkedStat(String name, Decimal value) : base(name, value)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        protected LinkedStat(String name) : base(name, 0m)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="dependencies"> </param>
        private void SetupStats(IEnumerable<Stat<decimal>> dependencies)
        {
            foreach (var d in dependencies)
            {
                AddDependency(d);
            }
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        protected IEnumerable<Stat<decimal>> CloneDependentStats()
        {
            return GetDependencies();
        }

        /// <summary>
        /// </summary>
        /// <param name="dependency"> </param>
        public virtual void AddDependency(Stat<Decimal> dependency)
        {
            _dependencyList.Add(dependency);
            dependency.OnValueChanged += dependency_OnValueChanged;
        }

        /// <summary>
        /// </summary>
        /// <param name="dependency"> </param>
        public void RemoveDependency(Stat<Decimal> dependency)
        {
            if (dependency != null)
            {
                dependency.OnValueChanged -= dependency_OnValueChanged;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public IEnumerable<Stat<Decimal>> GetDependencies()
        {
            return _dependencyList.AsReadOnly();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void dependency_OnValueChanged(object sender, StatChangedEvent e)
        {
            var onChange = OnDependencyValueChanged;
            if (onChange != null)
            {
                onChange(this, new StatChangedEvent(sender, e.OldValue, e.NewValue));
            }
            DoDependencyValueChanged(sender, e.OldValue, e.NewValue);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="oldValue"> </param>
        /// <param name="newValue"> </param>
        public virtual void DoDependencyValueChanged(object sender, object oldValue, object newValue)
        {
            Value = (Decimal) newValue;
        }
    }
}