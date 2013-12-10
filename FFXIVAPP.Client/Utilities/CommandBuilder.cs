// FFXIVAPP.Client
// CommandBuilder.cs
// 
// © 2013 Ryan Wilson

using System.Text.RegularExpressions;
using FFXIVAPP.Common.RegularExpressions;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Utilities
{
    [DoNotObfuscate]
    internal static class CommandBuilder
    {
        public static readonly Regex CommandsRegEx = new Regex(@"^com:(?<plugin>\w+) (?<command>\w+)$", SharedRegEx.DefaultOptions);
    }
}
