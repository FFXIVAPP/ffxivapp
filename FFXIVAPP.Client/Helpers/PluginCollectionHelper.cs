// FFXIVAPP.Client
// PluginCollectionHelper.cs
// 
// © 2013 Ryan Wilson

using System.Collections;
using System.Linq;
using FFXIVAPP.Client.Models;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Helpers
{
    [DoNotObfuscate]
    internal class PluginCollectionHelper : CollectionBase
    {
        /// <summary>
        /// </summary>
        /// <param name="plugin"> </param>
        public void Add(PluginInstance plugin)
        {
            List.Add(plugin);
        }

        /// <summary>
        /// </summary>
        /// <param name="plugin"> </param>
        public void Remove(PluginInstance plugin)
        {
            List.Remove(plugin);
        }

        /// <summary>
        /// </summary>
        /// <param name="plugin"> </param>
        /// <returns> </returns>
        public PluginInstance Find(string plugin)
        {
            return List.Cast<PluginInstance>()
                       .FirstOrDefault(pluginInstance => (pluginInstance.Instance.Name.Equals(plugin)) || pluginInstance.AssemblyPath.Equals(plugin));
        }
    }
}
