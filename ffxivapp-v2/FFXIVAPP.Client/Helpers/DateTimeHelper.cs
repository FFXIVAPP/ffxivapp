// FFXIVAPP.Client
// DateTimeHelper.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System;

#endregion

namespace FFXIVAPP.Client.Helpers
{
    internal static class DateTimeHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="unixTimeStamp"> </param>
        /// <returns> </returns>
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp)
                                   .ToLocalTime();
            return dtDateTime;
        }
    }
}
