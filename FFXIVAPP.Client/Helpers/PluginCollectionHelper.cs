// FFXIVAPP.Client
// PluginCollectionHelper.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Collections;
using System.Linq;
using FFXIVAPP.Client.Models;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Helpers
{
    [DoNotObfuscate]
    internal class PluginCollectionHelper : CollectionBase
    {
        /// <summary>
        ///     add to collection
        /// </summary>
        /// <param name="plugin"> </param>
        public void Add(PluginInstance plugin)
        {
            List.Add(plugin);
        }

        /// <summary>
        ///     remove from collection
        /// </summary>
        /// <param name="plugin"> </param>
        public void Remove(PluginInstance plugin)
        {
            List.Remove(plugin);
        }

        /// <summary>
        ///     find plugin by name
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
