// FFXIVAPP.IPluginInterface
// IPluginHost.cs
// 
// © 2013 Ryan Wilson

using System;
using FFXIVAPP.Common.Models;
using FFXIVAPP.IPluginInterface.Events;

namespace FFXIVAPP.IPluginInterface
{
    public interface IPluginHost
    {
        void PopupMessage(string pluginName, PopupContent content);
        event EventHandler<ConstantsEntityEvent> NewConstantsEntity;
        event EventHandler<ChatLogEntryEvent> NewChatLogEntry;
        event EventHandler<ActorEntitiesEvent> NewMonsterEntries;
        event EventHandler<ActorEntitiesEvent> NewNPCEntries;
        event EventHandler<ActorEntitiesEvent> NewPCEntries;
        event EventHandler<PlayerEntityEvent> NewPlayerEntity;
        event EventHandler<TargetEntityEvent> NewTargetEntity;
        event EventHandler<ParseEntityEvent> NewParseEntity;
        event EventHandler<PartyEntitiesEvent> NewPartyEntries;
    }
}
