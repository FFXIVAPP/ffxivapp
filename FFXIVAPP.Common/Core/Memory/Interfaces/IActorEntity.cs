// FFXIVAPP.Common
// IActorEntity.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;
using FFXIVAPP.Common.Core.Memory.Enums;

namespace FFXIVAPP.Common.Core.Memory.Interfaces
{
    public interface IActorEntity
    {
        uint MapIndex { get; set; }
        string Name { get; set; }
        uint ID { get; set; }
        uint NPCID1 { get; set; }
        uint NPCID2 { get; set; }
        Actor.Type Type { get; set; }
        Actor.TargetType TargetType { get; set; }
        byte Distance { get; set; }
        byte GatheringStatus { get; set; }
        double X { get; set; }
        double Z { get; set; }
        double Y { get; set; }
        float Heading { get; set; }
        byte GatheringInvisible { get; set; }
        uint Fate { get; set; }
        uint ModelID { get; set; }
        Actor.ActionStatus ActionStatus { get; set; }
        bool IsGM { get; set; }
        Actor.Icon Icon { get; set; }
        Actor.Status Status { get; set; }
        int TargetID { get; set; }
        Actor.Job Job { get; set; }
        byte Level { get; set; }
        byte GrandCompany { get; set; }
        byte GrandCompanyRank { get; set; }
        byte Title { get; set; }
        int HPCurrent { get; set; }
        int HPMax { get; set; }
        int MPCurrent { get; set; }
        int MPMax { get; set; }
        int TPCurrent { get; set; }
        short GPCurrent { get; set; }
        short GPMax { get; set; }
        short CPCurrent { get; set; }
        short CPMax { get; set; }
        byte Race { get; set; }
        Actor.Sex Sex { get; set; }
        List<StatusEntry> StatusEntries { get; set; }
        bool IsCasting { get; set; }
        short CastingID { get; set; }
        float CastingProgress { get; set; }
        float CastingTime { get; set; }
    }
}
