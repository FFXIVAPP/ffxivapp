// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PluginCollectionHelper.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   PluginCollectionHelper.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Helpers {
    using System;
    using System.Collections;
    using System.Linq;

    using FFXIVAPP.Client.Models;

    internal class PluginCollectionHelper : CollectionBase {
        /// <summary>
        /// </summary>
        /// <param name="plugin"> </param>
        public void Add(PluginInstance plugin) {
            this.List.Add(plugin);
        }

        /// <summary>
        /// </summary>
        /// <param name="plugin"> </param>
        /// <returns> </returns>
        public PluginInstance Find(string plugin) {
            try {
                return this.List.Cast<PluginInstance>().FirstOrDefault(pluginInstance => pluginInstance.Instance.Name.Equals(plugin, Constants.InvariantComparer) || pluginInstance.AssemblyPath.Equals(plugin, Constants.InvariantComparer));
            }
            catch (Exception) {
                return null;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="plugin"> </param>
        public void Remove(PluginInstance plugin) {
            this.List.Remove(plugin);
        }
    }
}