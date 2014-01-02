// FFXIVAPP.Client
// ActorEntityHelper.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Linq;
using FFXIVAPP.Client.Memory;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Core.Memory.Enums;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Helpers
{
    [DoNotObfuscate]
    public static class ActorEntityHelper
    {
        public static ActorEntity ResolveActorFromMemory(Structures.NPCEntry actor, string name)
        {
            var entry = new ActorEntity
            {
                Name = name,
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
                Distance = actor.Distance,
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
                TargetType = actor.TargetType,
                IsCasting = actor.IsCasting,
                CastingID = actor.CastingID,
                CastingTargetID = actor.CastingTargetID,
                CastingProgress = actor.CastingProgress,
                CastingTime = actor.CastingTime
            };
            if (entry.HPMax == 0)
            {
                entry.HPMax = 1;
            }
            if (entry.TargetID == -536870912)
            {
                entry.TargetID = 0;
            }
            if (entry.CastingTargetID == 3758096384)
            {
                entry.CastingTargetID = 0;
            }
            entry.MapIndex = 0;
            var limit = 60;
            switch (actor.Type)
            {
                case Actor.Type.PC:
                    limit = 30;
                    break;
            }
            for (var i = 0; i < limit; i++)
            {
                var statusEntry = new StatusEntry
                {
                    TargetName = entry.Name,
                    StatusID = actor.Statuses[i].StatusID,
                    Duration = actor.Statuses[i].Duration,
                    CasterID = actor.Statuses[i].CasterID
                };
                try
                {
                    var statusInfo = StatusEffectHelper.StatusInfo(statusEntry.StatusID);
                    statusEntry.IsCompanyAction = statusInfo.CompanyAction;
                    var statusKey = "";
                    switch (Settings.Default.GameLanguage)
                    {
                        case "English":
                            statusKey = statusInfo.Name.English;
                            break;
                        case "French":
                            statusKey = statusInfo.Name.French;
                            break;
                        case "German":
                            statusKey = statusInfo.Name.German;
                            break;
                        case "Japanese":
                            statusKey = statusInfo.Name.Japanese;
                            break;
                    }
                    statusEntry.StatusName = statusKey;
                }
                catch (Exception ex)
                {
                    statusEntry.StatusName = "UNKNOWN";
                }
                if (statusEntry.IsValid())
                {
                    entry.StatusEntries.Add(statusEntry);
                }
            }
            return entry;
        }
    }
}
