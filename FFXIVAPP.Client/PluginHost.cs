// FFXIVAPP.Client
// PluginHost.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Xml.Linq;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Models;
using FFXIVAPP.Client.Reflection;
using FFXIVAPP.Common.Core.Constant;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Core.Parse;
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.IPluginInterface;
using FFXIVAPP.IPluginInterface.Events;
using NLog;
using SmartAssembly.Attributes;
using PlayerEntity = FFXIVAPP.Common.Core.Memory.PlayerEntity;

namespace FFXIVAPP.Client
{
    [DoNotObfuscate]
    internal class PluginHost : MarshalByRefObject, IPluginHost
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

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

        #region Declarations

        public AssemblyReflectionManager AssemblyReflectionManager = new AssemblyReflectionManager();

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="path"></param>
        public void LoadPlugins(string path = "")
        {
            path = (path == "") ? AppDomain.CurrentDomain.BaseDirectory : path;
            if (!Directory.Exists(path))
            {
                return;
            }
            var directories = Directory.GetDirectories(path);
            foreach (var d in directories)
            {
                var settings = String.Format(@"{0}\PluginInfo.xml", d);
                if (!File.Exists(settings))
                {
                    continue;
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
                            VerifyPlugin(String.Format(@"{0}\{1}", d, xValue));
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        public void UnloadPlugins()
        {
            foreach (PluginInstance pInstance in Loaded)
            {
                if (pInstance.Instance != null)
                {
                    pInstance.Instance.Dispose();
                }
                pInstance.Instance = null;
            }
            Loaded.Clear();
        }

        /// <summary>
        /// </summary>
        /// <param name="assemblyPath"></param>
        private void VerifyPlugin(string assemblyPath)
        {
            try
            {
                var pAssembly = Assembly.LoadFile(assemblyPath);
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
                Loaded.Add(plugin);
            }
            catch (Exception ex)
            {
            }
        }

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
            var pluginInstance = App.Plugins.Loaded.Find(popupContent.PluginName);
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

        public event EventHandler<ConstantsEntityEvent> NewConstantsEntity = delegate { };

        public event EventHandler<ChatLogEntryEvent> NewChatLogEntry = delegate { };

        public event EventHandler<ActorEntitiesEvent> NewMonsterEntries = delegate { };

        public event EventHandler<ActorEntitiesEvent> NewNPCEntries = delegate { };

        public event EventHandler<ActorEntitiesEvent> NewPCEntries = delegate { };

        public event EventHandler<PlayerEntityEvent> NewPlayerEntity = delegate { };

        public event EventHandler<TargetEntityEvent> NewTargetEntity = delegate { };

        public event EventHandler<ParseEntityEvent> NewParseEntity = delegate { };

        public event EventHandler<PartyEntitiesEvent> NewPartyEntries = delegate { };

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

        public virtual void RaiseNewMonsterEntries(List<ActorEntity> e)
        {
            var actorEntitiesEvent = new ActorEntitiesEvent(this, e);
            var handler = NewMonsterEntries;
            if (handler != null)
            {
                handler(this, actorEntitiesEvent);
            }
        }

        public virtual void RaiseNewNPCEntries(List<ActorEntity> e)
        {
            var actorEntitiesEvent = new ActorEntitiesEvent(this, e);
            var handler = NewNPCEntries;
            if (handler != null)
            {
                handler(this, actorEntitiesEvent);
            }
        }

        public virtual void RaiseNewPCEntries(List<ActorEntity> e)
        {
            var actorEntitiesEvent = new ActorEntitiesEvent(this, e);
            var handler = NewPCEntries;
            if (handler != null)
            {
                handler(this, actorEntitiesEvent);
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

        public virtual void RaiseNewParseEntity(ParseEntity e)
        {
            var parseEntityEvent = new ParseEntityEvent(this, e);
            var handler = NewParseEntity;
            if (handler != null)
            {
                handler(this, parseEntityEvent);
            }
        }

        public virtual void RaiseNewPartyEntries(List<PartyEntity> e)
        {
            var partyEntitiesEvent = new PartyEntitiesEvent(this, e);
            var handler = NewPartyEntries;
            if (handler != null)
            {
                handler(this, partyEntitiesEvent);
            }
        }

        #endregion
    }
}
