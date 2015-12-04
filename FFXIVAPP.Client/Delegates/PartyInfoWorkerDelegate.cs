// FFXIVAPP.Client ~ PartyInfoWorkerDelegate.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
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
using FFXIVAPP.Memory.Core;

namespace FFXIVAPP.Client.Delegates
{
    public static class PartyInfoWorkerDelegate
    {
        #region Collection Access & Modification

        public static void EnsurePartyEntity(UInt32 key, PartyEntity entity)
        {
            PartyEntities.AddOrUpdate(key, entity, (k, v) => entity);
        }

        public static PartyEntity GetPartyEntity(UInt32 key)
        {
            PartyEntity party;
            PartyEntities.TryGetValue(key, out party);
            return party;
        }

        public static bool RemovePartyEntity(UInt32 key)
        {
            PartyEntity removed;
            return PartyEntities.TryRemove(key, out removed);
        }

        #endregion

        #region Declarations

        private static ConcurrentDictionary<UInt32, PartyEntity> _partyEntities;

        public static ConcurrentDictionary<UInt32, PartyEntity> PartyEntities
        {
            get { return _partyEntities ?? (_partyEntities = new ConcurrentDictionary<UInt32, PartyEntity>()); }
            private set { _partyEntities = value; }
        }

        #endregion
    }
}
