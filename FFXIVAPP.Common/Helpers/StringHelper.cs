// FFXIVAPP.Common
// StringHelper.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using FFXIVAPP.Common.RegularExpressions;

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
            if (String.IsNullOrWhiteSpace(s.Trim()))
            {
                return "";
            }
            s = TrimAndCleanSpaces(s);
            var result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(all ? s.ToLower() : s);
            var reg = SharedRegEx.Romans.Match(s);
            if (reg.Success)
            {
                var replace = Convert.ToString(reg.Groups["roman"].Value);
                var original = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(replace.ToLower());
                result = result.Replace(original, replace.ToUpper());
            }
            var titles = Regex.Matches(result, @"(?<num>\d+)(?<designator>\w+)", RegexOptions.IgnoreCase);
            foreach (Match title in titles)
            {
                var num = Convert.ToString(title.Groups["num"].Value);
                var designator = Convert.ToString(title.Groups["designator"].Value);
                result = result.Replace(String.Format("{0}{1}", num, designator), String.Format("{0}{1}", num, designator.ToLower()));
            }
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string TrimAndCleanSpaces(string name)
        {
            return Regex.Replace(name, @"[ ]+", " ")
                        .Trim();
        }

        /// <summary>
        /// </summary>
        /// <param name="hexValue"></param>
        /// <returns></returns>
        public static string HexToString(string hexValue)
        {
            var sb = new StringBuilder();
            for (var i = 0; i <= hexValue.Length - 2; i += 2)
            {
                sb.Append(Convert.ToChar(Int32.Parse(hexValue.Substring(i, 2), NumberStyles.HexNumber)));
            }
            return sb.ToString();
        }
    }
}
