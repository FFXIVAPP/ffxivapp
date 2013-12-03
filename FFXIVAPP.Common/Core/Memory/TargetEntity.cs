// FFXIVAPP.Common
// TargetEntity.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Collections.Generic;

#endregion

namespace FFXIVAPP.Common.Core.Memory
{
    public class TargetEntity : ITargetEntity
    {
        private List<EnmityEntry> _enmityEntries;
        public uint ID { get; set; }

        public List<EnmityEntry> EnmityEntries
        {
            get { return _enmityEntries ?? (_enmityEntries = new List<EnmityEntry>()); }
            set
            {
                if (_enmityEntries == null)
                {
                    _enmityEntries = new List<EnmityEntry>();
                }
                _enmityEntries = value;
            }
        }
    }
}
