// FFXIVAPP.Client
// DateTimeHelper.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;

#endregion

namespace FFXIVAPP.Client.Helpers {
    internal static class DateTimeHelper {
        /// <summary>
        /// </summary>
        /// <param name="unixTimeStamp"> </param>
        /// <returns> </returns>
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp) {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp)
                                   .ToLocalTime();
            return dtDateTime;
        }
    }
}
