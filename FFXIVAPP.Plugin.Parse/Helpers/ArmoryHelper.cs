// FFXIVAPP.Plugin.Parse
// ArmoryHelper.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.Collections;

#endregion

namespace FFXIVAPP.Plugin.Parse.Helpers
{
    public static class ArmoryHelper
    {
        public static string GetClassOrJob(string key)
        {
            Hashtable offsets;
            switch (Constants.CultureInfo.TwoLetterISOLanguageName)
            {
                case "ja":
                case "de":
                case "fr":
                default:
                    offsets = new Hashtable();
                    break;
            }
            return offsets[key] as string;
        }
    }
}
