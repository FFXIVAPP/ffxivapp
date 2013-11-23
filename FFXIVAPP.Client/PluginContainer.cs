// FFXIVAPP.Client
// PluginContainer.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Xml.Linq;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Models;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.IPluginInterface;
using NLog;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client
{
    [DoNotObfuscate]
    internal class PluginContainer : IPluginHost
    {
        #region Property Bindings

        private PluginCollectionHelper _loaded;

        public PluginCollectionHelper Loaded
        {
            get { return _loaded ?? (_loaded = new PluginCollectionHelper()); }
        }

        #endregion

        #region Declarations

        private List<string> _authorizedPublishers = new List<string>();

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="path"> </param>
        public void LoadPlugins(string path = "")
        {
            path = (path == "") ? AppDomain.CurrentDomain.BaseDirectory : path;
            Loaded.Clear();
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
        /// <param name="fileName"> </param>
        private void VerifyPlugin(string fileName)
        {
            try
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("PluginFileName:{0}", fileName));
                var pAssembly = Assembly.LoadFile(fileName);
                var pType = pAssembly.GetType(pAssembly.GetName()
                                                       .Name + ".Plugin");
                var implementsIPlugin = typeof (IPlugin).IsAssignableFrom(pType);
                if (!implementsIPlugin)
                {
                    Logging.Log(LogManager.GetCurrentClassLogger(), "*IPlugin Not Implemented*");
                    return;
                }
                var plugin = new PluginInstance();
                plugin.Instance = (IPlugin) Activator.CreateInstance(pType);
                plugin.AssemblyPath = fileName;
                plugin.Instance.Host = this;
                plugin.Instance.Initialize(ApplicationContextHelper.GetContext());
                Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("Added:{0}", plugin.Instance.Name));
                Loaded.Add(plugin);
            }
            catch (Exception ex)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
            }
        }

        #region Implementaion of IPluginHost

        /// <summary>
        /// </summary>
        /// <param name="pluginName"> </param>
        /// <param name="commands"> </param>
        public void Commands(string pluginName, IEnumerable<string> commands)
        {
            var pluginInstance = Loaded.Find(pluginName);
            if (pluginInstance == null)
            {
                return;
            }
            if (!Settings.Default.AllowPluginCommands)
            {
                var enumerable = commands as List<string> ?? commands.ToList();
                var commandlist = enumerable.Aggregate("", (current, s) => current + (s + ","));
                Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("PluginCommandAborted: {0}: \n{1}", pluginName, commandlist.Substring(0, commandlist.Length - 1)));
                return;
            }
            // return for now as all commands are disabled
            return;
            //foreach (var command in commands)
            //{
            //    var ascii = Encoding.GetEncoding("utf-16");
            //    KeyBoardHelper.SendNotify(ascii.GetBytes(command));
            //}
        }

        /// <summary>
        /// </summary>
        /// <param name="pluginName"></param>
        /// <param name="displayed"> </param>
        /// <param name="content"> </param>
        public void PopupMessage(string pluginName, out bool displayed, object content)
        {
            var popupContent = content as PopupContent;
            if (popupContent == null)
            {
                displayed = false;
                return;
            }
            var pluginInstance = Loaded.Find(popupContent.PluginName);
            if (pluginInstance == null)
            {
                displayed = false;
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
            displayed = true;
        }

        public void ProcessDataByKey(string pluginName, string token, string key, object data)
        {
            var pluginInstance = Loaded.Find(pluginName);
            if (pluginInstance == null || !_authorizedPublishers.Contains(token) || !Constants.IsOpen)
            {
                return;
            }
        }

        #endregion
    }
}
