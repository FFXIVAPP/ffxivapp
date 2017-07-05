// FFXIVAPP.Client ~ PluginCollectionHelper.cs
// 
// Copyright © 2007 - 2017 Ryan Wilson - All Rights Reserved
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections;
using System.Linq;
using FFXIVAPP.Client.Models;

namespace FFXIVAPP.Client.Helpers
{
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
            try
            {
                return List.Cast<PluginInstance>()
                           .FirstOrDefault(pluginInstance => pluginInstance.Instance.Name.Equals(plugin, Constants.InvariantComparer) || pluginInstance.AssemblyPath.Equals(plugin, Constants.InvariantComparer));
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
