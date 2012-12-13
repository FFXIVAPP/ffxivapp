// FFXIVAPP.IPluginInterface
// IPluginHost.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System.Collections.Generic;

#endregion

namespace FFXIVAPP.IPluginInterface
{
    public interface IPluginHost
    {
        void Commands(string pluginName, IEnumerable<string> commands);
        void PopupMessage(out bool displayed, object content);
    }
}
