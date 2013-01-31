// FFXIVAPP.Common
// SharedRegEx.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Text.RegularExpressions;
using FFXIVAPP.Common.Utilities;
using NLog;

#endregion

namespace FFXIVAPP.Common.RegularExpressions
{
    public static class SharedRegEx
    {
        public const RegexOptions DefaultOptions = RegexOptions.Compiled | RegexOptions.ExplicitCapture;

        public static readonly Regex Romans = new Regex(@"(?<roman>\b[IVXLCDM]+\b)", DefaultOptions);

        public static readonly Regex CICUID = new Regex(@".+=(?<cicuid>\d+)", DefaultOptions);

        public static readonly Regex Experience = new Regex(@"^You earn (?<exp>\d+).+\.$", DefaultOptions);

        public static readonly Regex ParseCommands = new Regex(@"com:(?<cmd>(show-(mob|total)|parse)) ((?<cm>[\w\s]+):)?(?<sub>[\w\s-']+)( (?<limit>\d))?$", DefaultOptions);

        public static readonly Regex TranslateCommands = new Regex(@"^/\w( \w+ \w+)?$", DefaultOptions);

        /// <summary>
        /// </summary>
        /// <param name="pattern"> </param>
        /// <returns> </returns>
        public static bool IsValidRegex(string pattern)
        {
            var result = true;
            if (String.IsNullOrWhiteSpace(pattern))
            {
                return false;
            }
            try
            {
                result = Regex.IsMatch("", pattern);
            }
            catch (ArgumentException ex)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
                return result;
            }
            return true;
        }
    }
}
