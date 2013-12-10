// FFXIVAPP.Common
// TargetEntity.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;

namespace FFXIVAPP.Common.Core.Memory
{
    public class TargetEntity : ITargetEntity
    {
        private List<EnmityEntry> _enmityEntries;

        public ActorEntity CurrentTarget { get; set; }
        public ActorEntity MouseOverTarget { get; set; }
        public ActorEntity FocusTarget { get; set; }
        public ActorEntity PreviousTarget { get; set; }
        public uint CurrentTargetID { get; set; }

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
