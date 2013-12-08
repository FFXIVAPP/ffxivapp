// FFXIVAPP.IPluginInterface
// IPluginHost.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using FFXIVAPP.Common.Models;
using FFXIVAPP.IPluginInterface.Events;

#endregion

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
    }
}
