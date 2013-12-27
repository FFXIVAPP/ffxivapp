// FFXIVAPP.Client
// ActorEntityHelper.cs
// 
// © 2013 Ryan Wilson

using System.Linq;
using FFXIVAPP.Client.Memory;
using FFXIVAPP.Common.Core.Memory;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Helpers
{
    [DoNotObfuscate]
    public static class ActorEntityHelper
    {
        public static ActorEntity ResolveActorFromMemory(Structures.NPCEntry actor)
        {
            var entry = new ActorEntity
            {
                ID = actor.ID,
                NPCID1 = actor.NPCID1,
                NPCID2 = actor.NPCID2,
                Type = actor.Type,
                Coordinate = new Coordinate(actor.X, actor.Z, actor.Y),
                GatheringStatus = actor.GatheringStatus,
                X = actor.X,
                Z = actor.Z,
                Y = actor.Y,
                Heading = actor.Heading,
                GatheringInvisible = actor.GatheringInvisible,
                Fate = actor.Fate,
                ModelID = actor.ModelID,
                Icon = actor.Icon,
                Status = actor.Status,
                TargetID = actor.TargetID,
                Level = actor.Level,
                HPCurrent = actor.HPCurrent,
                HPMax = actor.HPMax,
                MPCurrent = actor.MPCurrent,
                MPMax = actor.MPMax,
                TPCurrent = actor.TPCurrent,
                TPMax = 1000,
                GPCurrent = actor.GPCurrent,
                GPMax = actor.GPMax,
                CPCurrent = actor.CPCurrent,
                CPMax = actor.CPMax,
                GrandCompany = actor.GrandCompany,
                GrandCompanyRank = actor.GrandCompanyRank,
                IsGM = actor.IsGM,
                Job = actor.Job,
                Race = actor.Race,
                Sex = actor.Sex,
                ActionStatus = actor.ActionStatus,
                Title = actor.Title,
                TargetType = actor.TargetType
            };
            if (entry.HPMax == 0)
            {
                entry.HPMax = 1;
            }
            if (entry.TargetID == -536870912)
            {
                entry.TargetID = -1;
            }
            entry.MapIndex = 0;
            foreach (var statusEntry in actor.Statuses.Select(status => new StatusEntry
            {
                TargetName = entry.Name,
                StatusID = status.StatusID,
                Duration = status.Duration,
                CasterID = status.CasterID
            })
                                             .Where(statusEntry => statusEntry.IsValid()))
            {
                entry.StatusEntries.Add(statusEntry);
            }
            return entry;
        }
    }
}
