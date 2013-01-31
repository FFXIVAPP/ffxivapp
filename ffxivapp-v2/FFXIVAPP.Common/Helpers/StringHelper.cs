// FFXIVAPP.Common
// StringHelper.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Globalization;
using FFXIVAPP.Common.RegularExpressions;

#endregion

namespace FFXIVAPP.Common.Helpers
{
    public static class StringHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="s"> </param>
        /// <param name="all"> </param>
        /// <returns> </returns>
        public static string TitleCase(string s, bool all = true)
        {
            var result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(all ? s.ToLower() : s);
            var reg = SharedRegEx.Romans.Match(s);
            if (reg.Success)
            {
                var replace = Convert.ToString(reg.Groups["roman"].Value);
                var original = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(replace.ToLower());
                result = result.Replace(original, replace.ToUpper());
            }
            return result;
        }
    }
}
