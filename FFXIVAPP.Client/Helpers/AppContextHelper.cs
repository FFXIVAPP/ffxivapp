// FFXIVAPP.Client
// AppContextHelper.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
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
using System.Linq;
using FFXIVAPP.Client.Delegates;
using FFXIVAPP.Common.Core.Constant;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Core.Network;
using NLog;

namespace FFXIVAPP.Client.Helpers
{
    public class AppContextHelper
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Property Backings

        private static AppContextHelper _instance;
        private List<uint> _pets;

        public static AppContextHelper Instance
        {
            get { return _instance ?? (_instance = new AppContextHelper()); }
            set { _instance = value; }
        }

        public List<uint> Pets
        {
            get
            {
                return _pets ?? (_pets = new List<uint>
                {
                    1398,
                    1399,
                    1400,
                    1401,
                    1402,
                    1403,
                    1404,
                    2095
                });
            }
        }

        public PlayerEntity CurrentUserStats { get; set; }

        #endregion

        public void RaiseNewConstants(ConstantsEntity constantsEntity)
        {
            PluginHost.Instance.RaiseNewConstantsEntity(constantsEntity);
        }

        public void RaiseNewChatLogEntry(ChatLogEntry chatLogEntry)
        {
            if (ChatLogWorkerDelegate.IsPaused)
            {
                return;
            }
            AppViewModel.Instance.ChatHistory.Add(chatLogEntry);
            // THIRD PARTY
            PluginHost.Instance.RaiseNewChatLogEntry(chatLogEntry);
        }

        public void RaiseNewMonsterEntries(List<ActorEntity> actorEntities)
        {
            if (!actorEntities.Any())
            {
                return;
            }
            MonsterWorkerDelegate.ReplaceNPCEntities(new List<ActorEntity>(actorEntities));
            Func<bool> saveToDictionary = delegate
            {
                try
                {
                    var enumerable = MonsterWorkerDelegate.GetUniqueNPCEntities();
                    foreach (var actor in actorEntities)
                    {
                        var exists = enumerable.FirstOrDefault(n => n.ID == actor.ID);
                        if (exists != null)
                        {
                            continue;
                        }
                        MonsterWorkerDelegate.AddUniqueNPCEntity(actor);
                    }
                }
                catch (Exception ex)
                {
                }
                return true;
            };
            saveToDictionary.BeginInvoke(null, saveToDictionary);
            // THIRD PARTY
            PluginHost.Instance.RaiseNewMonsterEntries(actorEntities);
        }

        public void RaiseNewNPCEntries(List<ActorEntity> actorEntities)
        {
            if (!actorEntities.Any())
            {
                return;
            }
            NPCWorkerDelegate.ReplaceNPCEntities(new List<ActorEntity>(actorEntities));
            Func<bool> saveToDictionary = delegate
            {
                try
                {
                    var enumerable = NPCWorkerDelegate.GetUniqueNPCEntities();
                    foreach (var actor in actorEntities)
                    {
                        var exists = enumerable.FirstOrDefault(n => n.NPCID2 == actor.NPCID2);
                        if (exists != null)
                        {
                            continue;
                        }
                        NPCWorkerDelegate.AddUniqueNPCEntity(actor);
                    }
                }
                catch (Exception ex)
                {
                }
                return true;
            };
            saveToDictionary.BeginInvoke(null, saveToDictionary);
            // THIRD PARTY
            PluginHost.Instance.RaiseNewNPCEntries(actorEntities);
        }

        public void RaiseNewPCEntries(List<ActorEntity> actorEntities)
        {
            if (!actorEntities.Any())
            {
                return;
            }
            PCWorkerDelegate.ReplaceNPCEntities(new List<ActorEntity>(actorEntities));
            Func<bool> saveToDictionary = delegate
            {
                try
                {
                    var enumerable = PCWorkerDelegate.GetUniqueNPCEntities();
                    foreach (var actor in actorEntities)
                    {
                        var exists = enumerable.FirstOrDefault(n => String.Equals(n.Name, actor.Name, Constants.InvariantComparer));
                        if (exists != null)
                        {
                            continue;
                        }
                        PCWorkerDelegate.AddUniqueNPCEntity(actor);
                    }
                }
                catch (Exception ex)
                {
                }
                return true;
            };
            saveToDictionary.BeginInvoke(null, saveToDictionary);
            // THIRD PARTY
            PluginHost.Instance.RaiseNewPCEntries(actorEntities);
        }

        public void RaiseNewPlayerEntity(PlayerEntity playerEntity)
        {
            CurrentUserStats = playerEntity;
            // THIRD PARTY
            PluginHost.Instance.RaiseNewPlayerEntity(playerEntity);
        }

        public void RaiseNewTargetEntity(TargetEntity targetEntity)
        {
            // THIRD PARTY
            PluginHost.Instance.RaiseNewTargetEntity(targetEntity);
        }

        public void RaiseNewPartyEntries(List<PartyEntity> partyEntries)
        {
            // THIRD PARTY
            PluginHost.Instance.RaiseNewPartyEntries(partyEntries);
        }

        public void RaiseNewInventoryEntries(List<InventoryEntity> inventoryEntities)
        {
            // THIRD PARTY
            PluginHost.Instance.RaiseNewInventoryEntries(inventoryEntities);
        }

        public void RaiseNewPacket(NetworkPacket networkPacket)
        {
            // THIRD PARTY
            PluginHost.Instance.RaiseNewNetworkPacket(networkPacket);
        }
    }
}
