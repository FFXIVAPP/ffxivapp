// FFXIVAPP.Client ~ MonsterWorkerDelegate.cs
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
    public static class MonsterWorkerDelegate
    {
        #region Collection Access & Modification

        public static void EnsureMonsterEntity(UInt32 key, ActorEntity entity)
        {
            MonsterEntities.AddOrUpdate(key, entity, (k, v) => entity);
        }

        public static ActorEntity GetMonsterEntity(UInt32 key)
        {
            ActorEntity monster;
            MonsterEntities.TryGetValue(key, out monster);
            return monster;
        }

        public static bool RemoveMonsterEntity(UInt32 key)
        {
            ActorEntity removed;
            return MonsterEntities.TryRemove(key, out removed);
        }

        #endregion

        #region Declarations

        private static ConcurrentDictionary<UInt32, ActorEntity> _monsterEntities;

        public static ConcurrentDictionary<UInt32, ActorEntity> MonsterEntities
        {
            get { return _monsterEntities ?? (_monsterEntities = new ConcurrentDictionary<UInt32, ActorEntity>()); }
            private set { _monsterEntities = value; }
        }

        #endregion
    }
}
