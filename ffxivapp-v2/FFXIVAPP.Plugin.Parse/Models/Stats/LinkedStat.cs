// FFXIVAPP.Plugin.Parse
// LinkedStat.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Collections.Generic;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models.Stats
{
    public abstract class LinkedStat : Stat<decimal>, ILinkedStat
    {
        #region Declarations

        private readonly List<Stat<decimal>> _dependencyList = new List<Stat<decimal>>(50);

        #endregion

        #region Events

        public event EventHandler<StatChangedEvent> OnDependencyValueChanged = delegate { };

        #endregion

        protected LinkedStat(string name, params Stat<decimal>[] dependencies) : base(name, 0m)
        {
            SetupStats(dependencies);
        }

        protected LinkedStat(string name, decimal value) : base(name, 0m) {}

        protected LinkedStat(string name) : base(name, 0m) {}

        /// <summary>
        /// </summary>
        /// <param name="dependency"> </param>
        public virtual void AddDependency(Stat<decimal> dependency)
        {
            _dependencyList.Add(dependency);
            dependency.OnValueChanged += DependencyValueChanged;
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public IEnumerable<Stat<decimal>> GetDependencies()
        {
            return _dependencyList.AsReadOnly();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="previousValue"> </param>
        /// <param name="newValue"> </param>
        public virtual void DoDependencyValueChanged(object sender, object previousValue, object newValue)
        {
            Value = (decimal) newValue;
        }

        /// <summary>
        /// </summary>
        /// <param name="dependencies"> </param>
        private void SetupStats(IEnumerable<Stat<decimal>> dependencies)
        {
            foreach (var dependency in dependencies)
            {
                AddDependency(dependency);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="dependency"> </param>
        public void RemoveDependency(Stat<decimal> dependency)
        {
            if (dependency != null)
            {
                dependency.OnValueChanged -= DependencyValueChanged;
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
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void DependencyValueChanged(object sender, StatChangedEvent e)
        {
            OnDependencyValueChanged(this, new StatChangedEvent(sender, e.PreviousValue, e.NewValue));
            DoDependencyValueChanged(sender, e.PreviousValue, e.NewValue);
        }
    }
}
