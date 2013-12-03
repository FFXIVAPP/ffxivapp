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
        uint ID { get; set; }
        List<EnmityEntry> EnmityEntries { get; set; }
    }
}
