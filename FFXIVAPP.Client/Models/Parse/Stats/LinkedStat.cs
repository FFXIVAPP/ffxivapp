// FFXIVAPP.Client
// LinkedStat.cs
// 
// Copyright © 2007 - 2014 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

using System;
using System.Collections.Generic;
using System.Linq;

namespace FFXIVAPP.Client.Models.Parse.Stats
{
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
