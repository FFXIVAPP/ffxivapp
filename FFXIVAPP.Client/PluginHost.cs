// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PluginHost.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   PluginHost.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client {
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Xml.Linq;

    using FFXIVAPP.Client.Helpers;
    using FFXIVAPP.Client.Models;
    using FFXIVAPP.Client.Reflection;
    using FFXIVAPP.Common.Core.Constant;
    using FFXIVAPP.Common.Models;
    using FFXIVAPP.Common.Utilities;
    using FFXIVAPP.IPluginInterface;
    using FFXIVAPP.IPluginInterface.Events;

    using NLog;

    using Sharlayan.Core;

    internal class PluginHost : MarshalByRefObject, IPluginHost {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static Lazy<PluginHost> _instance = new Lazy<PluginHost>(() => new PluginHost());

        public AssemblyReflectionManager AssemblyReflectionManager = new AssemblyReflectionManager();

        private PluginCollectionHelper _loaded;

        public event EventHandler<ActionContainersEvent> ActionContainersUpdated = delegate { };

        public event EventHandler<ChatLogItemEvent> ChatLogItemReceived = delegate { };

        public event EventHandler<ConstantsEntityEvent> ConstantsUpdated = delegate { };
        public event EventHandler<CurrentUserEvent> CurrentUserUpdated = delegate { };

        public event EventHandler<InventoryContainersEvent> InventoryContainersUpdated = delegate { };

        public event EventHandler<ActorItemsAddedEvent> MonsterItemsAdded = delegate { };

        public event EventHandler<ActorItemsRemovedEvent> MonsterItemsRemoved = delegate { };

        public event EventHandler<ActorItemsEvent> MonsterItemsUpdated = delegate { };

        public event EventHandler<ActorItemsAddedEvent> NPCItemsAdded = delegate { };

        public event EventHandler<ActorItemsRemovedEvent> NPCItemsRemoved = delegate { };

        public event EventHandler<ActorItemsEvent> NPCItemsUpdated = delegate { };

        public event EventHandler<PartyMembersAddedEvent> PartyMembersAdded = delegate { };

        public event EventHandler<PartyMembersRemovedEvent> PartyMembersRemoved = delegate { };

        public event EventHandler<PartyMembersEvent> PartyMembersUpdated = delegate { };

        public event EventHandler<ActorItemsAddedEvent> PCItemsAdded = delegate { };

        public event EventHandler<ActorItemsRemovedEvent> PCItemsRemoved = delegate { };

        public event EventHandler<ActorItemsEvent> PCItemsUpdated = delegate { };

        public event EventHandler<PlayerInfoEvent> PlayerInfoUpdated = delegate { };

        public event EventHandler<TargetInfoEvent> TargetInfoUpdated = delegate { };

        public static PluginHost Instance {
            get {
                return _instance.Value;
            }
        }

        public PluginCollectionHelper Loaded {
            get {
                return this._loaded ?? (this._loaded = new PluginCollectionHelper());
            }

            set {
                if (this._loaded == null) {
                    this._loaded = new PluginCollectionHelper();
                }

                this._loaded = value;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="path"></param>
        public void LoadPlugin(string path) {
            if (string.IsNullOrWhiteSpace(path)) {
                return;
            }

            try {
                path = Directory.Exists(path)
                           ? path
                           : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
                var settings = $@"{path}\PluginInfo.xml";
                if (!File.Exists(settings)) {
                    return;
                }

                XDocument xDoc = XDocument.Load(settings);
                foreach (XElement xElement in xDoc.Descendants().Elements("Main")) {
                    var xKey = (string) xElement.Attribute("Key");
                    var xValue = (string) xElement.Element("Value");
                    if (string.IsNullOrWhiteSpace(xKey) || string.IsNullOrWhiteSpace(xValue)) {
                        return;
                    }

                    switch (xKey) {
                        case "FileName":
                            this.VerifyPlugin($@"{path}\{xValue}");
                            break;
                    }
                }
            }
            catch (Exception ex) {
                Logging.Log(Logger, new LogItem(ex, true));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="path"></param>
        public void LoadPlugins(string path) {
            if (string.IsNullOrWhiteSpace(path)) {
                return;
            }

            try {
                if (Directory.Exists(path)) {
                    string[] directories = Directory.GetDirectories(path);
                    foreach (var directory in directories) {
                        this.LoadPlugin(directory);
                    }
                }
            }
            catch (Exception ex) {
                Logging.Log(Logger, new LogItem(ex, true));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="pluginName"></param>
        /// <param name="popupContent"></param>
        public void PopupMessage(string pluginName, PopupContent popupContent) {
            if (popupContent == null) {
                return;
            }

            PluginInstance pluginInstance = App.Plugins.Loaded.Find(popupContent.PluginName);
            if (pluginInstance == null) {
                return;
            }

            var title = $"[{pluginName}] {popupContent.Title}";
            var message = popupContent.Message;
            Action cancelAction = null;
            if (popupContent.CanCancel) {
                cancelAction = delegate {
                    pluginInstance.Instance.PopupResult = MessageBoxResult.Cancel;
                };
            }

            MessageBoxHelper.ShowMessageAsync(
                title, message, delegate {
                    pluginInstance.Instance.PopupResult = MessageBoxResult.OK;
                }, cancelAction);
        }

        public virtual void RaiseActionContainersUpdated(ConcurrentBag<ActionContainer> actionContainers) {
            var raised = new ActionContainersEvent(this, actionContainers);
            EventHandler<ActionContainersEvent> handler = this.ActionContainersUpdated;
            handler?.Invoke(this, raised);
        }

        public virtual void RaiseChatLogItemReceived(ChatLogItem chatLogItem) {
            var raised = new ChatLogItemEvent(this, chatLogItem);
            EventHandler<ChatLogItemEvent> handler = this.ChatLogItemReceived;
            handler?.Invoke(this, raised);
        }

        public virtual void RaiseConstantsUpdated(ConstantsEntity constantsEntity) {
            var raised = new ConstantsEntityEvent(this, constantsEntity);
            EventHandler<ConstantsEntityEvent> handler = this.ConstantsUpdated;
            handler?.Invoke(this, raised);
        }

        public virtual void RaiseCurrentUserUpdated(ActorItem currentUser) {
            var raised = new CurrentUserEvent(this, currentUser);
            EventHandler<CurrentUserEvent> handler = this.CurrentUserUpdated;
            handler?.Invoke(this, raised);
        }

        public virtual void RaiseInventoryContainersUpdated(ConcurrentBag<InventoryContainer> inventoryContainers) {
            var raised = new InventoryContainersEvent(this, inventoryContainers);
            EventHandler<InventoryContainersEvent> handler = this.InventoryContainersUpdated;
            handler?.Invoke(this, raised);
        }

        public virtual void RaiseMonsterItemsAdded(ConcurrentDictionary<uint, ActorItem> actorItems) {
            var raised = new ActorItemsAddedEvent(this, actorItems);
            EventHandler<ActorItemsAddedEvent> handler = this.MonsterItemsAdded;
            handler?.Invoke(this, raised);
        }

        public virtual void RaiseMonsterItemsRemoved(ConcurrentDictionary<uint, ActorItem> actorItems) {
            var raised = new ActorItemsRemovedEvent(this, actorItems);
            EventHandler<ActorItemsRemovedEvent> handler = this.MonsterItemsRemoved;
            handler?.Invoke(this, raised);
        }

        public virtual void RaiseMonsterItemsUpdated(ConcurrentDictionary<uint, ActorItem> actorItems) {
            var raised = new ActorItemsEvent(this, actorItems);
            EventHandler<ActorItemsEvent> handler = this.MonsterItemsUpdated;
            handler?.Invoke(this, raised);
        }

        public virtual void RaiseNPCItemsAdded(ConcurrentDictionary<uint, ActorItem> actorItems) {
            var raised = new ActorItemsAddedEvent(this, actorItems);
            EventHandler<ActorItemsAddedEvent> handler = this.NPCItemsAdded;
            handler?.Invoke(this, raised);
        }

        public virtual void RaiseNPCItemsRemoved(ConcurrentDictionary<uint, ActorItem> actorItems) {
            var raised = new ActorItemsRemovedEvent(this, actorItems);
            EventHandler<ActorItemsRemovedEvent> handler = this.NPCItemsRemoved;
            handler?.Invoke(this, raised);
        }

        public virtual void RaiseNPCItemsUpdated(ConcurrentDictionary<uint, ActorItem> actorItems) {
            var raised = new ActorItemsEvent(this, actorItems);
            EventHandler<ActorItemsEvent> handler = this.NPCItemsUpdated;
            handler?.Invoke(this, raised);
        }

        public virtual void RaisePartyMembersAdded(ConcurrentDictionary<uint, PartyMember> partyMembers) {
            var raised = new PartyMembersAddedEvent(this, partyMembers);
            EventHandler<PartyMembersAddedEvent> handler = this.PartyMembersAdded;
            handler?.Invoke(this, raised);
        }

        public virtual void RaisePartyMembersRemoved(ConcurrentDictionary<uint, PartyMember> partyMembers) {
            var raised = new PartyMembersRemovedEvent(this, partyMembers);
            EventHandler<PartyMembersRemovedEvent> handler = this.PartyMembersRemoved;
            handler?.Invoke(this, raised);
        }

        public virtual void RaisePartyMembersUpdated(ConcurrentDictionary<uint, PartyMember> partyMembers) {
            var raised = new PartyMembersEvent(this, partyMembers);
            EventHandler<PartyMembersEvent> handler = this.PartyMembersUpdated;
            handler?.Invoke(this, raised);
        }

        public virtual void RaisePCItemsAdded(ConcurrentDictionary<uint, ActorItem> actorItems) {
            var raised = new ActorItemsAddedEvent(this, actorItems);
            EventHandler<ActorItemsAddedEvent> handler = this.PCItemsAdded;
            handler?.Invoke(this, raised);
        }

        public virtual void RaisePCItemsRemoved(ConcurrentDictionary<uint, ActorItem> actorItems) {
            var raised = new ActorItemsRemovedEvent(this, actorItems);
            EventHandler<ActorItemsRemovedEvent> handler = this.PCItemsRemoved;
            handler?.Invoke(this, raised);
        }

        public virtual void RaisePCItemsUpdated(ConcurrentDictionary<uint, ActorItem> actorItems) {
            var raised = new ActorItemsEvent(this, actorItems);
            EventHandler<ActorItemsEvent> handler = this.PCItemsUpdated;
            handler?.Invoke(this, raised);
        }

        public virtual void RaisePlayerInfoUpdated(PlayerInfo playerInfo) {
            var raised = new PlayerInfoEvent(this, playerInfo);
            EventHandler<PlayerInfoEvent> handler = this.PlayerInfoUpdated;
            handler?.Invoke(this, raised);
        }

        public virtual void RaiseTargetInfoUpdated(TargetInfo targetInfo) {
            var raised = new TargetInfoEvent(this, targetInfo);
            EventHandler<TargetInfoEvent> handler = this.TargetInfoUpdated;
            handler?.Invoke(this, raised);
        }

        /// <summary>
        /// </summary>
        public void UnloadPlugin(string name) {
            PluginInstance plugin = this.Loaded.Find(name);
            if (plugin != null) {
                plugin.Instance.Dispose();
                this.Loaded.Remove(plugin);
            }
        }

        /// <summary>
        /// </summary>
        public void UnloadPlugins() {
            foreach (PluginInstance pluginInstance in this.Loaded.Cast<PluginInstance>().Where(pluginInstance => pluginInstance.Instance != null)) {
                pluginInstance.Instance.Dispose();
            }

            this.Loaded.Clear();
        }

        /// <summary>
        /// </summary>
        /// <param name="assemblyPath"></param>
        private void VerifyPlugin(string assemblyPath) {
            try {
                byte[] bytes = File.ReadAllBytes(assemblyPath);
                Assembly pAssembly = Assembly.Load(bytes);
                Type pType = pAssembly.GetType(pAssembly.GetName().Name + ".Plugin");
                var implementsIPlugin = typeof(IPlugin).IsAssignableFrom(pType);
                if (!implementsIPlugin) {
                    Logging.Log(Logger, $"*IPlugin Not Implemented* :: {pAssembly.GetName().Name}");
                    return;
                }

                var plugin = new PluginInstance {
                    Instance = (IPlugin) Activator.CreateInstance(pType),
                    AssemblyPath = assemblyPath,
                };
                plugin.Instance.Initialize(Instance);
                plugin.Loaded = true;
                this.Loaded.Add(plugin);
            }
            catch (Exception ex) {
                Logging.Log(Logger, new LogItem(ex, true));
            }
        }
    }
}