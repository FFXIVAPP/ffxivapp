// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PluginInstance.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   PluginInstance.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Models {
    using FFXIVAPP.IPluginInterface;

    internal class PluginInstance {
        public PluginInstance() {
            this.AssemblyPath = string.Empty;
            this.Loaded = false;
        }

        public string AssemblyPath { get; set; }

        public IPlugin Instance { get; set; }

        public bool Loaded { get; set; }
    }
}