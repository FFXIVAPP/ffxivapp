// FFXIVAPP.Client
// PluginHost.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Xml.Linq;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Models;
using FFXIVAPP.Client.Reflection;
using FFXIVAPP.Common.Core.Constant;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Core.Network;
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.IPluginInterface;
using FFXIVAPP.IPluginInterface.Events;
using NLog;

namespace FFXIVAPP.Client
{
    internal class PluginHost : MarshalByRefObject, IPluginHost
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Declarations

        public AssemblyReflectionManager AssemblyReflectionManager = new AssemblyReflectionManager();

        #endregion

        private List<string> DependencyUpgrades = new List<string>
        {
            "FFXIVAPP.Common",
            "FFXIVAPP.IPluginInterface",
            "MahApps.Metro",
            "HtmlAgilityPack",
            "NAudio",
            "Newtonsoft.Json",
            "NLog",
            "System.Windows.Interactivity"
        };

        /// <summary>
        /// </summary>
        /// <param name="path"></param>
        public void LoadPlugins(string path)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                return;
            }
            try
            {
                if (Directory.Exists(path))
                {
                    var directories = Directory.GetDirectories(path);
                    foreach (var directory in directories)
                    {
                        LoadPlugin(directory);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="path"></param>
        public void LoadPlugin(string path)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                return;
            }
            try
            {
                path = Directory.Exists(path) ? path : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
                var settings = String.Format(@"{0}\PluginInfo.xml", path);
                if (!File.Exists(settings))
                {
                    return;
                }
                var xDoc = XDocument.Load(settings);
                foreach (var xElement in xDoc.Descendants()
                                             .Elements("Main"))
                {
                    var xKey = (string) xElement.Attribute("Key");
                    var xValue = (string) xElement.Element("Value");
                    if (String.IsNullOrWhiteSpace(xKey) || String.IsNullOrWhiteSpace(xValue))
                    {
                        return;
                    }
                    switch (xKey)
                    {
                        case "FileName":
                            VerifyPlugin(String.Format(@"{0}\{1}", path, xValue));
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// </summary>
        public void UnloadPlugins()
        {
            foreach (var pluginInstance in Loaded.Cast<PluginInstance>()
                                                 .Where(pluginInstance => pluginInstance.Instance != null))
            {
                pluginInstance.Instance.Dispose();
            }
            Loaded.Clear();
        }

        /// <summary>
        /// </summary>
        public void UnloadPlugin(string name)
        {
            var plugin = Loaded.Find(name);
            if (plugin != null)
            {
                plugin.Instance.Dispose();
                Loaded.Remove(plugin);
            }
        }

        private bool HostAssemblyValidation(string name, Version version)
        {
            var reference = Assembly.GetExecutingAssembly()
                                    .GetReferencedAssemblies()
                                    .FirstOrDefault(a => a.Name == name);
            if (reference == null)
            {
                return true;
            }
            return version.CompareTo(reference.Version) == 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="assemblyPath"></param>
        private void VerifyPlugin(string assemblyPath)
        {
            try
            {
                var bytes = File.ReadAllBytes(assemblyPath);
                var pAssembly = Assembly.Load(bytes);
                var references = pAssembly.GetReferencedAssemblies();
                var load = true;
                foreach (var valid in references.Where(a => DependencyUpgrades.Contains(a.Name))
                                                .Select(assembly => HostAssemblyValidation(assembly.Name, assembly.Version))
                                                .Where(valid => !valid))
                {
                    load = false;
                }
                var pType = pAssembly.GetType(pAssembly.GetName()
                                                       .Name + ".Plugin");
                var implementsIPlugin = typeof (IPlugin).IsAssignableFrom(pType);
                if (!implementsIPlugin)
                {
                    Logging.Log(Logger, String.Format("*IPlugin Not Implemented* :: {0}", pAssembly.GetName()
                                                                                                   .Name));
                    return;
                }
                var plugin = new PluginInstance
                {
                    Instance = (IPlugin) Activator.CreateInstance(pType),
                    AssemblyPath = assemblyPath
                };
                plugin.Instance.Initialize(Instance);
                plugin.Loaded = load;
                Loaded.Add(plugin);
            }
            catch (Exception ex)
            {
            }
        }

        #region Property Bindings

        private static PluginHost _instance;
        private PluginCollectionHelper _loaded;

        public PluginCollectionHelper Loaded
        {
            get { return _loaded ?? (_loaded = new PluginCollectionHelper()); }
            set
            {
                if (_loaded == null)
                {
                    _loaded = new PluginCollectionHelper();
                }
                _loaded = value;
            }
        }

        public static PluginHost Instance
        {
            get { return _instance ?? (_instance = new PluginHost()); }
            set { _instance = value; }
        }

        #endregion

        #region Implementaion of IPluginHost

        /// <summary>
        /// </summary>
        /// <param name="pluginName"></param>
        /// <param name="popupContent"></param>
        public void PopupMessage(string pluginName, PopupContent popupContent)
        {
            if (popupContent == null)
            {
                return;
            }
            var pluginInstance = App.Plugins.Loaded.Find(pluginName);
            if (pluginInstance == null)
            {
                return;
            }
            var title = String.Format("[{0}] {1}", pluginName, popupContent.Title);
            var message = popupContent.Message;
            Action cancelAction = null;
            if (popupContent.CanCancel)
            {
                cancelAction = delegate { pluginInstance.Instance.PopupResult = MessageBoxResult.Cancel; };
            }
            MessageBoxHelper.ShowMessageAsync(title, message, delegate { pluginInstance.Instance.PopupResult = MessageBoxResult.OK; }, cancelAction);
        }

        public void DisplayInGameText(string pluginName, string message)
        {
            if (message == null)
            {
                return;
            }
            var pluginInstance = App.Plugins.Loaded.Find(pluginName);
            if (pluginInstance == null)
            {
                return;
            }
            Constants.HookProcess?.OverlayInterface?.DisplayInGameText(String.Format("{0} :: {1}", pluginName, message));
        }

        public void AddOrUpdateInGameOverlay(string pluginName, Guid overlayID, Stream imageStream, float alpha, PointF location)
        {
            if (imageStream == null)
            {
                return;
            }
            var pluginInstance = App.Plugins.Loaded.Find(pluginName);
            if (pluginInstance == null)
            {
                return;
            }
            Constants.HookProcess?.OverlayInterface?.AddOrUpdateInGameOverlay(overlayID, imageStream, alpha, location);
        }

        public void RemoveInGameOverlay(string pluginName, Guid overlayID)
        {
            var pluginInstance = App.Plugins.Loaded.Find(pluginName);
            if (pluginInstance == null)
            {
                return;
            }
            Constants.HookProcess?.OverlayInterface?.RemoveInGameOverlay(overlayID);
        }

        public event EventHandler<ConstantsEntityEvent> NewConstantsEntity = delegate { };

        public event EventHandler<ChatLogEntryEvent> NewChatLogEntry = delegate { };

        public event EventHandler<ActorEntitiesAddedEvent> NewMonsterEntriesAdded = delegate { };

        public event EventHandler<ActorEntitiesEvent> NewMonsterEntries = delegate { };

        public event EventHandler<ActorEntitiesRemovedEvent> NewNPCEntriesRemoved = delegate { };

        public event EventHandler<ActorEntitiesAddedEvent> NewNPCEntriesAdded = delegate { };

        public event EventHandler<ActorEntitiesEvent> NewNPCEntries = delegate { };

        public event EventHandler<ActorEntitiesRemovedEvent> NewMonsterEntriesRemoved = delegate { };

        public event EventHandler<ActorEntitiesAddedEvent> NewPCEntriesAdded = delegate { };

        public event EventHandler<ActorEntitiesEvent> NewPCEntries = delegate { };

        public event EventHandler<ActorEntitiesRemovedEvent> NewPCEntriesRemoved = delegate { };

        public event EventHandler<PlayerEntityEvent> NewPlayerEntity = delegate { };

        public event EventHandler<TargetEntityEvent> NewTargetEntity = delegate { };

        public event EventHandler<PartyEntitiesAddedEvent> NewPartyEntriesAdded = delegate { };

        public event EventHandler<PartyEntitiesEvent> NewPartyEntries = delegate { };

        public event EventHandler<PartyEntitiesRemovedEvent> NewPartyEntriesRemoved = delegate { };

        public event EventHandler<InventoryEntitiesEvent> NewInventoryEntries = delegate { };

        public event EventHandler<NetworkPacketEvent> NewNetworkPacket = delegate { };

        public virtual void RaiseNewConstantsEntity(ConstantsEntity e)
        {
            var constantsEntityEvent = new ConstantsEntityEvent(this, e);
            var handler = NewConstantsEntity;
            if (handler != null)
            {
                handler(this, constantsEntityEvent);
            }
        }

        public virtual void RaiseNewChatLogEntry(ChatLogEntry e)
        {
            var chatLogEntryEvent = new ChatLogEntryEvent(this, e);
            var handler = NewChatLogEntry;
            if (handler != null)
            {
                handler(this, chatLogEntryEvent);
            }
        }

        public virtual void RaiseNewMonsterAddedEntries(List<UInt32> e)
        {
            var actorEntitiesAddedEvent = new ActorEntitiesAddedEvent(this, e);
            var handler = NewMonsterEntriesAdded;
            if (handler != null)
            {
                handler(this, actorEntitiesAddedEvent);
            }
        }

        public virtual void RaiseNewMonsterEntries(ConcurrentDictionary<UInt32, ActorEntity> e)
        {
            var actorEntitiesEvent = new ActorEntitiesEvent(this, e);
            var handler = NewMonsterEntries;
            if (handler != null)
            {
                handler(this, actorEntitiesEvent);
            }
        }

        public virtual void RaiseNewMonsterRemovedEntries(List<UInt32> e)
        {
            var actorEntitiesRemovedEvent = new ActorEntitiesRemovedEvent(this, e);
            var handler = NewMonsterEntriesRemoved;
            if (handler != null)
            {
                handler(this, actorEntitiesRemovedEvent);
            }
        }

        public virtual void RaiseNewNPCAddedEntries(List<UInt32> e)
        {
            var actorEntitiesAddedEvent = new ActorEntitiesAddedEvent(this, e);
            var handler = NewNPCEntriesAdded;
            if (handler != null)
            {
                handler(this, actorEntitiesAddedEvent);
            }
        }

        public virtual void RaiseNewNPCEntries(ConcurrentDictionary<UInt32, ActorEntity> e)
        {
            var actorEntitiesEvent = new ActorEntitiesEvent(this, e);
            var handler = NewNPCEntries;
            if (handler != null)
            {
                handler(this, actorEntitiesEvent);
            }
        }

        public virtual void RaiseNewNPCRemovedEntries(List<UInt32> e)
        {
            var actorEntitiesRemovedEvent = new ActorEntitiesRemovedEvent(this, e);
            var handler = NewNPCEntriesRemoved;
            if (handler != null)
            {
                handler(this, actorEntitiesRemovedEvent);
            }
        }

        public virtual void RaiseNewPCAddedEntries(List<UInt32> e)
        {
            var actorEntitiesAddedEvent = new ActorEntitiesAddedEvent(this, e);
            var handler = NewPCEntriesAdded;
            if (handler != null)
            {
                handler(this, actorEntitiesAddedEvent);
            }
        }

        public virtual void RaiseNewPCEntries(ConcurrentDictionary<UInt32, ActorEntity> e)
        {
            var actorEntitiesEvent = new ActorEntitiesEvent(this, e);
            var handler = NewPCEntries;
            if (handler != null)
            {
                handler(this, actorEntitiesEvent);
            }
        }

        public virtual void RaiseNewPCRemovedEntries(List<UInt32> e)
        {
            var actorEntitiesRemovedEvent = new ActorEntitiesRemovedEvent(this, e);
            var handler = NewPCEntriesRemoved;
            if (handler != null)
            {
                handler(this, actorEntitiesRemovedEvent);
            }
        }

        public virtual void RaiseNewPlayerEntity(PlayerEntity e)
        {
            var playerEntityEvent = new PlayerEntityEvent(this, e);
            var handler = NewPlayerEntity;
            if (handler != null)
            {
                handler(this, playerEntityEvent);
            }
        }

        public virtual void RaiseNewTargetEntity(TargetEntity e)
        {
            var targetEntityEvent = new TargetEntityEvent(this, e);
            var handler = NewTargetEntity;
            if (handler != null)
            {
                handler(this, targetEntityEvent);
            }
        }

        public virtual void RaiseNewPartyAddedEntries(List<UInt32> e)
        {
            var partyEntitiesAddedEvent = new PartyEntitiesAddedEvent(this, e);
            var handler = NewPartyEntriesAdded;
            if (handler != null)
            {
                handler(this, partyEntitiesAddedEvent);
            }
        }

        public virtual void RaiseNewPartyEntries(ConcurrentDictionary<UInt32, PartyEntity> e)
        {
            var partyEntitiesEvent = new PartyEntitiesEvent(this, e);
            var handler = NewPartyEntries;
            if (handler != null)
            {
                handler(this, partyEntitiesEvent);
            }
        }

        public virtual void RaiseNewPartyRemovedEntries(List<UInt32> e)
        {
            var partyEntitiesRemovedEvent = new PartyEntitiesRemovedEvent(this, e);
            var handler = NewPartyEntriesRemoved;
            if (handler != null)
            {
                handler(this, partyEntitiesRemovedEvent);
            }
        }

        public virtual void RaiseNewInventoryEntries(List<InventoryEntity> e)
        {
            var inventoryEntitiesEvent = new InventoryEntitiesEvent(this, e);
            var handler = NewInventoryEntries;
            if (handler != null)
            {
                handler(this, inventoryEntitiesEvent);
            }
        }

        public virtual void RaiseNewNetworkPacket(NetworkPacket e)
        {
            var networkPacketEvent = new NetworkPacketEvent(this, e);
            var handler = NewNetworkPacket;
            if (handler != null)
            {
                handler(this, networkPacketEvent);
            }
        }

        #endregion
    }
}
