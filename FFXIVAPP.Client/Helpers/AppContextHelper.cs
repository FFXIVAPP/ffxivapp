// FFXIVAPP.Client ~ AppContextHelper.cs
// 
// Copyright © 2007 - 2017 Ryan Wilson - All Rights Reserved
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using FFXIVAPP.Common.Core.Constant;
using FFXIVAPP.Common.Core.Network;
using FFXIVAPP.Memory.Core;
using NLog;

namespace FFXIVAPP.Client.Helpers
{
    public class AppContextHelper
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

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

        public void RaiseNewPartyAddedEntries(List<UInt32> keys)
        {
            if (!keys.Any())
            {
                return;
            }
            // THIRD PARTY
            PluginHost.Instance.RaiseNewPartyAddedEntries(keys);
        }

        public void RaiseNewPartyEntries(ConcurrentDictionary<UInt32, PartyEntity> partyEntries)
        {
            // THIRD PARTY
            PluginHost.Instance.RaiseNewPartyEntries(partyEntries);
        }

        public void RaiseNewPartyRemovedEntries(List<UInt32> keys)
        {
            if (!keys.Any())
            {
                return;
            }
            // THIRD PARTY
            PluginHost.Instance.RaiseNewPartyRemovedEntries(keys);
        }

        public void RaiseNewInventoryEntries(List<InventoryEntity> inventoryEntities)
        {
            // THIRD PARTY
            PluginHost.Instance.RaiseNewInventoryEntries(inventoryEntities);
        }

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

        #region SEND EVERYTIME

        public void RaiseNewConstants(ConstantsEntity constantsEntity)
        {
            PluginHost.Instance.RaiseNewConstantsEntity(constantsEntity);
        }

        public void RaiseNewChatLogEntry(ChatLogEntry chatLogEntry)
        {
            AppViewModel.Instance.ChatHistory.Add(chatLogEntry);
            // THIRD PARTY
            PluginHost.Instance.RaiseNewChatLogEntry(chatLogEntry);
        }

        public void RaiseNewPacket(NetworkPacket networkPacket)
        {
            // THIRD PARTY
            PluginHost.Instance.RaiseNewNetworkPacket(networkPacket);
        }

        #endregion

        #region SEND ONCE VIA REFERENCE

        public void RaiseNewMonsterAddedEntries(List<UInt32> keys)
        {
            if (!keys.Any())
            {
                return;
            }
            // THIRD PARTY
            PluginHost.Instance.RaiseNewMonsterAddedEntries(keys);
        }

        public void RaiseNewMonsterEntries(ConcurrentDictionary<UInt32, ActorEntity> actorEntities)
        {
            if (!actorEntities.Any())
            {
                return;
            }
            // THIRD PARTY
            PluginHost.Instance.RaiseNewMonsterEntries(actorEntities);
        }

        public void RaiseNewMonsterRemovedEntries(List<UInt32> keys)
        {
            if (!keys.Any())
            {
                return;
            }
            // THIRD PARTY
            PluginHost.Instance.RaiseNewMonsterRemovedEntries(keys);
        }

        public void RaiseNewNPCAddedEntries(List<UInt32> keys)
        {
            if (!keys.Any())
            {
                return;
            }
            // THIRD PARTY
            PluginHost.Instance.RaiseNewNPCAddedEntries(keys);
        }

        public void RaiseNewNPCEntries(ConcurrentDictionary<UInt32, ActorEntity> actorEntities)
        {
            if (!actorEntities.Any())
            {
                return;
            }
            // THIRD PARTY
            PluginHost.Instance.RaiseNewNPCEntries(actorEntities);
        }

        public void RaiseNewNPCRemovedEntries(List<UInt32> keys)
        {
            if (!keys.Any())
            {
                return;
            }
            // THIRD PARTY
            PluginHost.Instance.RaiseNewNPCRemovedEntries(keys);
        }

        public void RaiseNewPCAddedEntries(List<UInt32> keys)
        {
            if (!keys.Any())
            {
                return;
            }
            // THIRD PARTY
            PluginHost.Instance.RaiseNewPCAddedEntries(keys);
        }

        public void RaiseNewPCEntries(ConcurrentDictionary<UInt32, ActorEntity> actorEntities)
        {
            if (!actorEntities.Any())
            {
                return;
            }
            // THIRD PARTY
            PluginHost.Instance.RaiseNewPCEntries(actorEntities);
        }

        public void RaiseNewPCRemovedEntries(List<UInt32> keys)
        {
            if (!keys.Any())
            {
                return;
            }
            // THIRD PARTY
            PluginHost.Instance.RaiseNewPCRemovedEntries(keys);
        }

        #endregion
    }
}
