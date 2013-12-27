// FFXIVAPP.IPluginInterface
// PartyEntityEvent.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using FFXIVAPP.Common.Core.Memory;

namespace FFXIVAPP.IPluginInterface.Events
{
    public class PartyEntitiesEvent : EventArgs
    {
        public PartyEntitiesEvent(object sender, List<PartyEntity> partyEntities)
        {
            Sender = sender;
            PartyEntities = partyEntities;
        }

        public object Sender { get; set; }
        public List<PartyEntity> PartyEntities { get; set; }
    }
}
