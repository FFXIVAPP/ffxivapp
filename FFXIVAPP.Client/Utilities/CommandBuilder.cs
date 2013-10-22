// FFXIVAPP.Client
// CommandBuilder.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FFXIVAPP.Client.Plugins.Parse.Models;
using FFXIVAPP.Client.Plugins.Parse.Models.Stats;
using FFXIVAPP.Common.RegularExpressions;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Utilities
{
    [DoNotObfuscate]
    internal static class CommandBuilder
    {
        public static readonly Regex CommandsRegEx = new Regex(@"^com:(?<plugin>\w+) (?<command>\w+)$", SharedRegEx.DefaultOptions);

        
    }
}
