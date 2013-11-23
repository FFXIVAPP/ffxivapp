// FFXIVAPP.Client
// English.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Windows;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Localization
{
    [DoNotObfuscate]
    public static partial class English
    {
        private static readonly ResourceDictionary Dictionary = new ResourceDictionary();

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public static ResourceDictionary Context()
        {
            Dictionary.Clear();
            SetClientLocale();
            SetParseLocale();
            return Dictionary;
        }
    }
}
