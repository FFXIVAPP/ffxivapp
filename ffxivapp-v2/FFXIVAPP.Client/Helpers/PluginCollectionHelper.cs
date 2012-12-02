// FFXIVAPP.Client
// PluginCollectionHelper.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System.Collections;
using System.Linq;
using FFXIVAPP.Client.Models;

namespace FFXIVAPP.Client.Helpers
{
    internal class PluginCollectionHelper : CollectionBase
    {
        /// <summary>
        ///   add to collection
        /// </summary>
        /// <param name="plugin"> </param>
        public void Add(PluginInstance plugin)
        {
            List.Add(plugin);
        }

        /// <summary>
        ///   remove from collection
        /// </summary>
        /// <param name="plugin"> </param>
        public void Remove(PluginInstance plugin)
        {
            List.Remove(plugin);
        }

        /// <summary>
        ///   find plugin by name
        /// </summary>
        /// <param name="plugin"> </param>
        /// <returns> </returns>
        public PluginInstance Find(string plugin)
        {
            return List.Cast<PluginInstance>().FirstOrDefault(pluginInstance => (pluginInstance.Instance.Name.Equals(plugin)) || pluginInstance.AssemblyPath.Equals(plugin));
        }
    }
}