// FFXIVAPP.Client ~ NPCWorkerDelegate.cs
// 
// Copyright © 2007 - 2016 Ryan Wilson - All Rights Reserved
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
    public static class NPCWorkerDelegate
    {
        #region Collection Access & Modification

        public static void EnsureNPCEntity(UInt32 key, ActorEntity entity)
        {
            NPCEntities.AddOrUpdate(key, entity, (k, v) => entity);
        }

        public static ActorEntity GetNPCEntity(UInt32 key)
        {
            ActorEntity npc;
            NPCEntities.TryGetValue(key, out npc);
            return npc;
        }

        public static bool RemoveNPCEntity(UInt32 key)
        {
            ActorEntity removed;
            return NPCEntities.TryRemove(key, out removed);
        }

        #endregion

        #region Declarations

        private static ConcurrentDictionary<UInt32, ActorEntity> _npcEntities;

        public static ConcurrentDictionary<UInt32, ActorEntity> NPCEntities
        {
            get { return _npcEntities ?? (_npcEntities = new ConcurrentDictionary<UInt32, ActorEntity>()); }
            private set { _npcEntities = value; }
        }

        #endregion
    }
}
