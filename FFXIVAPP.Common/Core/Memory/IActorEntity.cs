// FFXIVAPP.Common
// IActorEntity.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;

namespace FFXIVAPP.Common.Core.Memory
{
    public interface IActorEntity
    {
        uint MapIndex { get; set; }
        string Name { get; set; }
        uint ID { get; set; }
        uint NPCID1 { get; set; }
        uint NPCID2 { get; set; }
        byte Type { get; set; }
        byte TargetType { get; set; }
        byte Distance { get; set; }
        byte GatheringStatus { get; set; }
        double X { get; set; }
        double Z { get; set; }
        double Y { get; set; }
        float Heading { get; set; }
        byte GatheringInvisible { get; set; }
        uint Fate { get; set; }
        uint ModelID { get; set; }
        byte Status { get; set; }
        bool IsGM { get; set; }
        byte Icon { get; set; }
        byte Claimed { get; set; }
        int TargetID { get; set; }
        byte Job { get; set; }
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
        byte Sex { get; set; }
        List<StatusEntry> StatusEntries { get; set; }
        bool IsCasting { get; set; }
        short CastingID { get; set; }
        float CastingProgress { get; set; }
        float CastingTime { get; set; }
    }
}
