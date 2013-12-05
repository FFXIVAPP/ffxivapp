// FFXIVAPP.Common
// ITargetEntity.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Collections.Generic;

#endregion

namespace FFXIVAPP.Common.Core.Memory
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
