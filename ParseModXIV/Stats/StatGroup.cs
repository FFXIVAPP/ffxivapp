using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Dynamic;
using System.Linq.Expressions;

namespace ParseModXIV.Stats
{
    public class StatGroup : StatContainer
    {
        public readonly ConcurrentDictionary<string, IStatContainer> Children = new ConcurrentDictionary<string, IStatContainer>();
        private readonly ConcurrentDictionary<string, NumericStat> stats = new ConcurrentDictionary<string, NumericStat>();

        public override ObservableCollection<NumericStat> Stats
        {
            get { return new ObservableCollection<NumericStat>(stats.Values); }
        }

        public StatGroup(string Name, params StatGroup[] children)
        {
            this.Name = Name;
            this.Children = new ConcurrentDictionary<string, IStatContainer>(from c in children select new KeyValuePair<string, IStatContainer>(c.Name, c));
        }

        public StatGroup(string Name)
        {
            this.Name = Name;
        }

        public void AddGroup(StatGroup child)
        {
            Children.TryAdd(child.Name, child);
        }

        public Boolean HasGroup(string name)
        {
            return Children.ContainsKey(name);
        }

        public StatGroup GetGroup(string name)
        {
            StatGroup result;
            return TryGetGroup(name, out result) ? result : null;
        }

        public Boolean TryGetGroup(string name, out StatGroup result)
        {
            IStatContainer g;
            if (Children.TryGetValue(name, out g))
            {
                result = (StatGroup)g;
                return true;
            }
            result = null;
            return false;
        }

        //public Boolean TryGet(string path, out IStatValue<object> result)
        //{
        //    var pathComponents = path.Split(StatContainer.PATH_SEPERATOR.ToCharArray());
        //    if (pathComponents.Length == 1)
        //    {
        //        return stats.TryGetValue(pathComponents.First(), out result);
        //    }
        //    IStatContainer nextGroup;
        //    if (Children.TryGetValue(pathComponents.First(), out nextGroup))
        //    {
        //        return nextGroup.TryGet(String.Join(StatContainer.PATH_SEPERATOR, pathComponents, 1, pathComponents.Length - 1), out result);
        //    }
        //    result = null;
        //    return false;
        //}

        public override bool HasStat(string name)
        {
            return stats.ContainsKey(name);
        }

        public override NumericStat GetStat(string name)
        {
            NumericStat value;
            return stats.TryGetValue(name, out value) ? value : null;
        }

        public override void AddStats(params NumericStat[] toAdd)
        {
            foreach (var s in toAdd)
            {
                stats.TryAdd(s.Name, s);
            }
        }
    }
}