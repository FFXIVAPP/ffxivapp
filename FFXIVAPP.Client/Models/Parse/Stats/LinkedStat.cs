// FFXIVAPP.Client
// LinkedStat.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.Linq;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models.Parse.Stats
{
    [DoNotObfuscate]
    public abstract class LinkedStat : Stat<decimal>, ILinkedStat
    {
        private List<Stat<decimal>> _dependencies;

        #region Declarations

        private List<Stat<decimal>> Dependencies
        {
            get { return _dependencies ?? (_dependencies = new List<Stat<decimal>>()); }
            set { _dependencies = value; }
        }

        #endregion

        #region Events

        public event EventHandler<StatChangedEvent> OnDependencyValueChanged = delegate { };

        #endregion

        protected LinkedStat(string name, params Stat<decimal>[] dependencies) : base(name, 0m)
        {
            SetupStats(dependencies);
        }

        protected LinkedStat(string name, decimal value) : base(name, 0m)
        {
        }

        protected LinkedStat(string name) : base(name, 0m)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="dependency"> </param>
        public virtual void AddDependency(Stat<decimal> dependency)
        {
            dependency.OnValueChanged += DependencyValueChanged;
            Dependencies.Add(dependency);
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public IEnumerable<Stat<decimal>> GetDependencies()
        {
            return Dependencies.AsReadOnly();
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
        public void ClearDependencies()
        {
            if (Dependencies.Any())
            {
                foreach (var dependency in Dependencies)
                {
                    dependency.OnValueChanged -= DependencyValueChanged;
                }
            }
            Dependencies.Clear();
        }

        /// <summary>
        /// </summary>
        /// <param name="dependency"> </param>
        public void RemoveDependency(Stat<decimal> dependency)
        {
            if (!Dependencies.Any())
            {
                return;
            }
            dependency.OnValueChanged -= DependencyValueChanged;
            Dependencies.Remove(dependency);
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public IEnumerable<Stat<decimal>> CloneDependentStats()
        {
            return GetDependencies();
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
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void DependencyValueChanged(object sender, StatChangedEvent e)
        {
            OnDependencyValueChanged(this, new StatChangedEvent(sender, e.PreviousValue, e.NewValue));
            DoDependencyValueChanged(sender, e.PreviousValue, e.NewValue);
        }
    }
}
