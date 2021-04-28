// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppContextHelper.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   AppContextHelper.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Helpers {
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using FFXIVAPP.Common.Core.Constant;
    using FFXIVAPP.Common.Core.Network;

    using NLog;

    using Sharlayan.Core;

    internal class AppContextHelper {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static Lazy<AppContextHelper> _instance = new Lazy<AppContextHelper>(() => new AppContextHelper());

        private List<uint> _pets;

        public static AppContextHelper Instance => _instance.Value;

        public CurrentPlayer CurrentUserStats { get; set; }

        public List<uint> Pets {
            get {
                return this._pets ?? (this._pets = new List<uint> {
                                             1398,
                                             1399,
                                             1400,
                                             1401,
                                             1402,
                                             1403,
                                             1404,
                                             2095,
                                         });
            }
        }

        public void RaiseActionContainersUpdated(List<ActionContainer> actionContainers) {
            PluginHost.Instance.RaiseActionContainersUpdated(actionContainers);
        }

        public void RaiseChatLogItemReceived(ChatLogItem chatLogItem) {
            PluginHost.Instance.RaiseChatLogItemReceived(chatLogItem);
        }

        public void RaiseConstantsUpdated(ConstantsEntity constantsEntity) {
            PluginHost.Instance.RaiseConstantsUpdated(constantsEntity);
        }

        public void RaiseCurrentPlayerUpdated(CurrentPlayer currentPlayer) {
            this.CurrentUserStats = currentPlayer;
            PluginHost.Instance.RaiseCurrentPlayerUpdated(currentPlayer);
        }

        public void RaiseInventoryContainersUpdated(List<InventoryContainer> inventoryContainers) {
            PluginHost.Instance.RaiseInventoryContainersUpdated(inventoryContainers);
        }

        public void RaiseMonsterItemsAdded(ConcurrentDictionary<uint, ActorItem> actorItems) {
            PluginHost.Instance.RaiseMonsterItemsAdded(actorItems);
        }

        public void RaiseMonsterItemsRemoved(ConcurrentDictionary<uint, ActorItem> actorItems) {
            PluginHost.Instance.RaiseMonsterItemsRemoved(actorItems);
        }

        public void RaiseMonsterItemsUpdated(ConcurrentDictionary<uint, ActorItem> actorItems) {
            PluginHost.Instance.RaiseMonsterItemsUpdated(actorItems);
        }

        public void RaiseNetworkPacketReceived(NetworkPacket networkPacket) {
            PluginHost.Instance.RaiseNetworkPacketReceived(networkPacket);
        }

        public void RaiseNPCItemsAdded(ConcurrentDictionary<uint, ActorItem> actorItems) {
            PluginHost.Instance.RaiseNPCItemsAdded(actorItems);
        }

        public void RaiseNPCItemsRemoved(ConcurrentDictionary<uint, ActorItem> actorItems) {
            PluginHost.Instance.RaiseNPCItemsRemoved(actorItems);
        }

        public void RaiseNPCItemsUpdated(ConcurrentDictionary<uint, ActorItem> actorItems) {
            PluginHost.Instance.RaiseNPCItemsUpdated(actorItems);
        }

        public void RaisePartyMembersAdded(ConcurrentDictionary<uint, PartyMember> partyMembers) {
            PluginHost.Instance.RaisePartyMembersAdded(partyMembers);
        }

        public void RaisePartyMembersRemoved(ConcurrentDictionary<uint, PartyMember> partyMembers) {
            PluginHost.Instance.RaisePartyMembersRemoved(partyMembers);
        }

        public void RaisePartyMembersUpdated(ConcurrentDictionary<uint, PartyMember> partyMembers) {
            PluginHost.Instance.RaisePartyMembersUpdated(partyMembers);
        }

        public void RaisePCItemsAdded(ConcurrentDictionary<uint, ActorItem> actorItems) {
            PluginHost.Instance.RaisePCItemsAdded(actorItems);
        }

        public void RaisePCItemsRemoved(ConcurrentDictionary<uint, ActorItem> actorItems) {
            PluginHost.Instance.RaisePCItemsRemoved(actorItems);
        }

        public void RaisePCItemsUpdated(ConcurrentDictionary<uint, ActorItem> actorItems) {
            PluginHost.Instance.RaisePCItemsUpdated(actorItems);
        }

        public void RaiseTargetInfoUpdated(TargetInfo targetInfo) {
            PluginHost.Instance.RaiseTargetInfoUpdated(targetInfo);
        }
    }
}