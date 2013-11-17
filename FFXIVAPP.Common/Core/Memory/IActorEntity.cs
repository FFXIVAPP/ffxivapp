// FFXIVAPP.Common
// IActorEntity.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Collections.Generic;

#endregion

namespace FFXIVAPP.Common.Core.Memory
{
    public interface IActorEntity
    {
        string Name { get; set; }
        uint ID { get; set; }
        uint NPCID1 { get; set; }
        uint NPCID2 { get; set; }
        byte Type { get; set; }
        byte TargetType { get; set; }
        byte Distance { get; set; }
        float X { get; set; }
        float Z { get; set; }
        float Y { get; set; }
        float Heading { get; set; }
        uint Fate { get; set; }
        uint ModelID { get; set; }
        byte Status { get; set; }
        bool IsGM { get; set; }
        byte Icon { get; set; }
        byte Claimed { get; set; }
        int TargetID { get; set; }
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
    }
}
