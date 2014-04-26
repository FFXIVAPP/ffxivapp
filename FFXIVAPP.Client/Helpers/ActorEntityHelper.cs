// FFXIVAPP.Client
// ActorEntityHelper.cs
// 
// Copyright © 2007 - 2014 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

using System;
using System.Collections.Generic;
using FFXIVAPP.Client.Memory;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Core.Memory.Enums;
using FFXIVAPP.Common.Helpers;

namespace FFXIVAPP.Client.Helpers
{
    public static class ActorEntityHelper
    {
        public static ActorEntity ResolveActorFromBytes(byte[] source)
        {
            var entry = new ActorEntity();
            try
            {
                entry.MapIndex = 0;
                entry.TargetID = 0;
                entry.Name = MemoryHandler.Instance.GetStringFromBytes(source, 48);
                entry.ID = BitConverter.ToUInt32(source, 0x74);
                entry.NPCID1 = BitConverter.ToUInt32(source, 0x78);
                entry.NPCID2 = BitConverter.ToUInt32(source, 0x80);
                entry.OwnerID = BitConverter.ToUInt32(source, 0x84);
                entry.Type = (Actor.Type) source[0x8A];
                entry.TargetType = (Actor.TargetType) source[0x8C];
                entry.Distance = source[0x8D];
                entry.GatheringStatus = source[0x8E];
                entry.X = BitConverter.ToSingle(source, 0xA0);
                entry.Z = BitConverter.ToSingle(source, 0xA4);
                entry.Y = BitConverter.ToSingle(source, 0xA8);
                entry.Heading = BitConverter.ToSingle(source, 0xB0);
                entry.Fate = BitConverter.ToUInt32(source, 0xE4); // ??
                entry.GatheringInvisible = source[0x11C]; // ??
                entry.ModelID = BitConverter.ToUInt32(source, 0x184);
                entry.ActionStatus = (Actor.ActionStatus) source[0x18C];
                entry.IsGM = BitConverter.ToBoolean(source, 0x193); // ?
                entry.Icon = (Actor.Icon) source[0x19C];
                entry.Status = (Actor.Status) source[0x19E];
                entry.ClaimedByID = BitConverter.ToUInt32(source, 0x1A0);
                var targetID = BitConverter.ToUInt32(source, 0x1A8);
                var pcTargetID = BitConverter.ToUInt32(source, 0xAA8);
                entry.Job = (Actor.Job) source[0x1830];
                entry.Level = source[0x1831];
                entry.GrandCompany = source[0x1833];
                entry.GrandCompanyRank = source[0x1834];
                entry.Title = source[0x1836];
                entry.HPCurrent = BitConverter.ToInt32(source, 0x1838);
                entry.HPMax = BitConverter.ToInt32(source, 0x183C);
                entry.MPCurrent = BitConverter.ToInt32(source, 0x1840);
                entry.MPMax = BitConverter.ToInt32(source, 0x1844);
                entry.TPCurrent = BitConverter.ToInt16(source, 0x1848);
                entry.TPMax = 1000;
                entry.GPCurrent = BitConverter.ToInt16(source, 0x184A);
                entry.GPMax = BitConverter.ToInt16(source, 0x184C);
                entry.CPCurrent = BitConverter.ToInt16(source, 0x184E);
                entry.CPMax = BitConverter.ToInt16(source, 0x1850);
                entry.Race = source[0x2E58]; // ??
                entry.Sex = (Actor.Sex) source[0x2E59]; //?
                entry.IsCasting = BitConverter.ToBoolean(source, 0x3330);
                entry.CastingID = BitConverter.ToInt16(source, 0x3334);
                entry.CastingTargetID = BitConverter.ToUInt32(source, 0x3340);
                entry.CastingProgress = BitConverter.ToSingle(source, 0x3364);
                entry.CastingTime = BitConverter.ToSingle(source, 0x3368);
                entry.Coordinate = new Coordinate(entry.X, entry.Z, entry.Y);
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
                Buffer.BlockCopy(source, 0x31B8, statusesSource, 0, limit * 12);
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
