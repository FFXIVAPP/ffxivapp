// FFXIVAPP.Client
// DamageContainer.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;
using FFXIVAPP.Common.Core.Memory;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Utilities
{
    [DoNotObfuscate]
    public class DamageContainer
    {
        public DamageContainer(ActorEntity actorEntity)
        {
            NPCEntry = actorEntity;
            DamageEntries = new List<DamageEntry>();
        }

        public ActorEntity NPCEntry { get; set; }
        public List<DamageEntry> DamageEntries { get; set; }
    }
}
