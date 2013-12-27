// FFXIVAPP.Common
// ITargetEntity.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;

namespace FFXIVAPP.Common.Core.Memory.Interfaces
{
    public interface ITargetEntity
    {
        ActorEntity CurrentTarget { get; set; }
        ActorEntity MouseOverTarget { get; set; }
        ActorEntity FocusTarget { get; set; }
        ActorEntity PreviousTarget { get; set; }
        uint CurrentTargetID { get; set; }
        List<EnmityEntry> EnmityEntries { get; set; }
    }
}
