// FFXIVAPP.Client
// ActorEntityHelper.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
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
            var entry = new ActorEntity();
            try
            {
                entry.Name = name;
                entry.ID = actor.ID;
                entry.NPCID1 = actor.NPCID1;
                entry.NPCID2 = actor.NPCID2;
                entry.OwnerID = actor.OwnerID;
                entry.Type = actor.Type;
                entry.Coordinate = new Coordinate(actor.X, actor.Z, actor.Y);
                entry.GatheringStatus = actor.GatheringStatus;
                entry.X = actor.X;
                entry.Z = actor.Z;
                entry.Y = actor.Y;
                entry.Heading = actor.Heading;
                entry.Distance = actor.Distance;
                entry.GatheringInvisible = actor.GatheringInvisible;
                entry.Fate = actor.Fate;
                entry.ModelID = actor.ModelID;
                entry.Icon = actor.Icon;
                entry.Status = actor.Status;
                entry.TargetID = actor.TargetID;
                entry.Level = actor.Level;
                entry.HPCurrent = actor.HPCurrent;
                entry.HPMax = actor.HPMax;
                entry.MPCurrent = actor.MPCurrent;
                entry.MPMax = actor.MPMax;
                entry.TPCurrent = actor.TPCurrent;
                entry.TPMax = 1000;
                entry.GPCurrent = actor.GPCurrent;
                entry.GPMax = actor.GPMax;
                entry.CPCurrent = actor.CPCurrent;
                entry.CPMax = actor.CPMax;
                entry.GrandCompany = actor.GrandCompany;
                entry.GrandCompanyRank = actor.GrandCompanyRank;
                entry.IsGM = actor.IsGM;
                entry.Job = actor.Job;
                entry.Race = actor.Race;
                entry.Sex = actor.Sex;
                entry.ActionStatus = actor.ActionStatus;
                entry.Title = actor.Title;
                entry.TargetType = actor.TargetType;
                entry.IsCasting = actor.IsCasting;
                entry.CastingID = actor.CastingID;
                entry.CastingTargetID = actor.CastingTargetID;
                entry.CastingProgress = actor.CastingProgress;
                entry.CastingTime = actor.CastingTime;
                entry.ClaimedByID = actor.ClaimedByID;
                entry.TargetID = 0;
                if (actor.TargetID > 0)
                {
                    entry.TargetID = actor.TargetID;
                }
                else
                {
                    if (actor.PCTargetID > 0)
                    {
                        entry.TargetID = actor.PCTargetID;
                    }
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
            }
            catch (Exception ex)
            {
            }
            CleanXPValue(ref entry);
            return entry;
        }

        public static ActorEntity ResolveActorFromBytes(byte[] source)
        {
            var entry = new ActorEntity();
            try
            {
                entry.ActionStatus = (Actor.ActionStatus) source[0x188];
                entry.CPCurrent = BitConverter.ToInt16(source, 0x16B6);
                entry.CPMax = BitConverter.ToInt16(source, 0x16B8);
                entry.CastingID = BitConverter.ToInt16(source, 0x3174);
                entry.CastingProgress = BitConverter.ToSingle(source, 0x31A4);
                entry.CastingTargetID = BitConverter.ToUInt32(source, 0x3180);
                entry.CastingTime = BitConverter.ToSingle(source, 0x31A8);
                entry.ClaimedByID = BitConverter.ToUInt32(source, 0x198);
                entry.X = BitConverter.ToSingle(source, 0xA0);
                entry.Y = BitConverter.ToSingle(source, 0xA8);
                entry.Z = BitConverter.ToSingle(source, 0xA4);
                entry.Coordinate = new Coordinate(entry.X, entry.Z, entry.Y);
                entry.Distance = source[0x8D];
                entry.Fate = BitConverter.ToUInt32(source, 0xE4);
                entry.GPCurrent = BitConverter.ToInt16(source, 0x16B2);
                entry.GPMax = BitConverter.ToInt16(source, 0x16B4);
                entry.GatheringInvisible = source[0x11C];
                entry.GatheringStatus = source[0x8E];
                entry.GrandCompany = source[0x169A];
                entry.GrandCompanyRank = source[0x169B];
                entry.HPCurrent = BitConverter.ToInt32(source, 0x16A0);
                entry.HPMax = BitConverter.ToInt32(source, 0x16A4);
                entry.Heading = BitConverter.ToSingle(source, 0xB0);
                entry.ID = BitConverter.ToUInt32(source, 0x74);
                entry.Icon = (Actor.Icon) source[0x194];
                entry.IsCasting = BitConverter.ToBoolean(source, 0x3170);
                entry.IsGM = BitConverter.ToBoolean(source, 0x193);
                entry.Job = (Actor.Job) source[0x1698];
                entry.Level = source[0x1699];
                entry.MPCurrent = BitConverter.ToInt32(source, 0x16A8);
                entry.MPMax = BitConverter.ToInt32(source, 0x16AC);
                entry.MapIndex = 0;
                entry.ModelID = BitConverter.ToUInt32(source, 0x184);
                entry.NPCID1 = BitConverter.ToUInt32(source, 0x78);
                entry.NPCID2 = BitConverter.ToUInt32(source, 0x80);
                entry.Name = MemoryHandler.Instance.GetStringFromBytes(source, 48);
                entry.OwnerID = BitConverter.ToUInt32(source, 0x84);
                entry.Race = source[0x2E58];
                entry.Sex = (Actor.Sex) source[0x2E59];
                entry.Status = (Actor.Status) source[0x196];
                entry.TPCurrent = BitConverter.ToInt16(source, 0x16B0);
                entry.TPMax = 1000;
                entry.TargetType = (Actor.TargetType) source[0x8C];
                entry.Title = source[0x169E];
                entry.Type = (Actor.Type) source[0x8A];
                entry.TargetID = 0;
                var targetID = BitConverter.ToUInt32(source, 0x1A0);
                var pcTargetID = BitConverter.ToUInt32(source, 0xA78);
                if (targetID > 0)
                {
                    entry.TargetID = (int) targetID;
                }
                else
                {
                    if (pcTargetID > 0)
                    {
                        entry.TargetID = (int) pcTargetID;
                    }
                }
                if (entry.CastingTargetID == 3758096384)
                {
                    entry.CastingTargetID = 0;
                }
                entry.MapIndex = 0;
                var limit = 60;
                switch (entry.Type)
                {
                    case Actor.Type.PC:
                        limit = 30;
                        break;
                }
                entry.StatusEntries = new List<StatusEntry>();
                const int statusSize = 12;
                var statusesSource = new byte[limit * statusSize];
                Buffer.BlockCopy(source, 0x2FF8, statusesSource, 0, limit * 12);
                for (var i = 0; i < limit; i++)
                {
                    var statusSource = new byte[statusSize];
                    Buffer.BlockCopy(statusesSource, i * statusSize, statusSource, 0, statusSize);
                    var statusEntry = new StatusEntry
                    {
                        TargetName = entry.Name,
                        StatusID = BitConverter.ToInt16(statusSource, 0x0),
                        Duration = BitConverter.ToSingle(statusSource, 0x4),
                        CasterID = BitConverter.ToUInt32(statusSource, 0x8)
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
            }
            catch (Exception ex)
            {
            }
            CleanXPValue(ref entry);
            return entry;
        }

        private static void CleanXPValue(ref ActorEntity entity)
        {
            if (entity.HPCurrent < 0 || entity.HPMax < 0)
            {
                entity.HPCurrent = 1;
                entity.HPMax = 1;
            }
            if (entity.HPCurrent > entity.HPMax)
            {
                if (entity.HPMax == 0)
                {
                    entity.HPCurrent = 1;
                    entity.HPMax = 1;
                }
                else
                {
                    entity.HPCurrent = entity.HPMax;
                }
            }
            if (entity.MPCurrent < 0 || entity.MPMax < 0)
            {
                entity.MPCurrent = 1;
                entity.MPMax = 1;
            }
            if (entity.MPCurrent > entity.MPMax)
            {
                if (entity.MPMax == 0)
                {
                    entity.MPCurrent = 1;
                    entity.MPMax = 1;
                }
                else
                {
                    entity.MPCurrent = entity.MPMax;
                }
            }
            if (entity.GPCurrent < 0 || entity.GPMax < 0)
            {
                entity.GPCurrent = 1;
                entity.GPMax = 1;
            }
            if (entity.GPCurrent > entity.GPMax)
            {
                if (entity.GPMax == 0)
                {
                    entity.GPCurrent = 1;
                    entity.GPMax = 1;
                }
                else
                {
                    entity.GPCurrent = entity.GPMax;
                }
            }
            if (entity.CPCurrent < 0 || entity.CPMax < 0)
            {
                entity.CPCurrent = 1;
                entity.CPMax = 1;
            }
            if (entity.CPCurrent > entity.CPMax)
            {
                if (entity.CPMax == 0)
                {
                    entity.CPCurrent = 1;
                    entity.CPMax = 1;
                }
                else
                {
                    entity.CPCurrent = entity.CPMax;
                }
            }
        }
    }
}
