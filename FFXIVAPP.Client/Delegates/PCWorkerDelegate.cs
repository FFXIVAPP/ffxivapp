// FFXIVAPP.Client
// FFXIVAPP & Related Plugins/Modules
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
    public static class PCWorkerDelegate
    {
        #region Collection Access & Modification

        public static void EnsurePCEntity(UInt32 key, ActorEntity entity)
        {
            PCEntities.AddOrUpdate(key, entity, (k, v) => entity);
        }

        public static ActorEntity GetPCEntity(UInt32 key)
        {
            ActorEntity pc;
            PCEntities.TryGetValue(key, out pc);
            return pc;
        }

        public static bool RemovePCEntity(UInt32 key)
        {
            ActorEntity removed;
            return PCEntities.TryRemove(key, out removed);
        }

        #endregion

        #region Declarations

        private static ConcurrentDictionary<UInt32, ActorEntity> _pcEntities;

        public static ConcurrentDictionary<UInt32, ActorEntity> PCEntities
        {
            get { return _pcEntities ?? (_pcEntities = new ConcurrentDictionary<UInt32, ActorEntity>()); }
            private set { _pcEntities = value; }
        }

        public static ActorEntity CurrentUser { get; set; }

        #endregion
    }
}
