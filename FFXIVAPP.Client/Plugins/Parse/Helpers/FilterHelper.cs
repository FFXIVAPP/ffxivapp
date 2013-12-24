// FFXIVAPP.Client
// FilterHelper.cs
// 
// © 2013 Ryan Wilson

using System;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Plugins.Parse.Helpers
{
    [DoNotObfuscate]
    public static class FilterHelper
    {
        private static bool IsEnabled(UInt64 filters, UInt64 filter)
        {
            return (filters & filter) != 0;
        }

        public static UInt64 Toggle(UInt64 filters, UInt64 filter)
        {
            return IsEnabled(filters, filter) ? Disable(filters, filter) : Enable(filters, filter);
        }

        public static UInt64 Enable(UInt64 filters, UInt64 filter)
        {
            if (IsEnabled(filters, filter))
            {
                return filters;
            }
            return (filters | filter);
        }

        public static UInt64 Disable(UInt64 filters, UInt64 filter)
        {
            if (IsEnabled(filters, filter))
            {
                return (filters & (~filter));
            }
            return filters;
        }
    }
}
