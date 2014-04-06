// FFXIVAPP.Client
// HistoryContainer.cs
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

using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace FFXIVAPP.Client.Models.Parse.History
{
    public class HistoryContainer : IHistoryContainer
    {
        private readonly ConcurrentDictionary<string, HistoryStat<decimal>> _statDict = new ConcurrentDictionary<string, HistoryStat<decimal>>();

        #region Implementation of IEnumerable

        public IEnumerator<HistoryStat<decimal>> GetEnumerator()
        {
            return _statDict.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public string Name { get; set; }

        public HistoryStat<decimal> GetStat(string name)
        {
            HistoryStat<decimal> result;
            _statDict.TryGetValue(name, out result);
            return result;
        }

        public HistoryStat<decimal> EnsureStatValue(string name, decimal value)
        {
            HistoryStat<decimal> stat;
            if (HasStat(name))
            {
                stat = GetStat(name);
                stat.Value = value;
            }
            else
            {
                stat = new NumericHistoryStat(name, value);
                Add(stat);
            }
            return stat;
        }

        public decimal GetStatValue(string name)
        {
            return HasStat(name) ? GetStat(name)
                .Value : 0;
        }

        #region Implementation of ICollection<Stat<decimal>>

        public void Add(HistoryStat<decimal> stat)
        {
            _statDict.TryAdd(stat.Name, stat);
        }

        public void Clear()
        {
            _statDict.Clear();
        }

        public bool Contains(HistoryStat<decimal> stat)
        {
            return _statDict.ContainsKey(stat.Name);
        }

        public void CopyTo(HistoryStat<decimal>[] array, int arrayIndex)
        {
            _statDict.Values.CopyTo(array, arrayIndex);
        }

        public bool Remove(HistoryStat<decimal> stat)
        {
            HistoryStat<decimal> removed;
            if (_statDict.TryRemove(stat.Name, out removed))
            {
                return true;
            }
            return false;
        }

        public int Count
        {
            get { return _statDict.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion

        public bool HasStat(string name)
        {
            return _statDict.ContainsKey(name);
        }
    }
}
