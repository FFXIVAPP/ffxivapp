// FFXIVAPP.Plugin.Parse
// ParseHelper.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;

#endregion

namespace FFXIVAPP.Plugin.Parse.Helpers
{
    public static class ParseHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="modifier"></param>
        /// <returns></returns>
        public static decimal GetOriginalDamage(decimal amount, decimal modifier)
        {
            return Math.Abs((amount / (modifier + 1)) - amount);
        }

        #region Damage Over Time Actions

        #endregion
    }
}
