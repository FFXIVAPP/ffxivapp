// FFXIVAPP.Common
// IPartyEntity.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;
using FFXIVAPP.Common.Core.Memory.Enums;

namespace FFXIVAPP.Common.Core.Memory.Interfaces
{
    public interface IPartyEntity
    {
        string Name { get; set; }
        uint ID { get; set; }
        double X { get; set; }
        double Z { get; set; }
        double Y { get; set; }
        Actor.Job Job { get; set; }
        byte Level { get; set; }
        int HPCurrent { get; set; }
        int HPMax { get; set; }
        int MPCurrent { get; set; }
        int MPMax { get; set; }
        List<StatusEntry> StatusEntries { get; set; }
    }
}
