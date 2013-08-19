// FFXIVAPP.IPluginInterface
// IPluginHost.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Collections.Generic;

#endregion

namespace FFXIVAPP.IPluginInterface
{
    public interface IPluginHost
    {
        void Commands(string pluginName, IEnumerable<string> commands);
        void PopupMessage(string pluginName, out bool displayed, object content);
        void GetConstants(string pluginName);
    }
}
