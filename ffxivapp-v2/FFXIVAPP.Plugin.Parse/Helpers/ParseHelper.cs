// FFXIVAPP.Plugin.Parse
// ParseHelper.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Collections;
using System.Collections.Generic;

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

        #region Job Info

        public static Hashtable GetJob()
        {
            Hashtable offsets;
            switch (Common.Constants.CultureInfo.TwoLetterISOLanguageName)
            {
                case "ja":
                case "de":
                case "fr":
                default:
                    offsets = new Hashtable();
                    break;
            }
            return offsets;
        }

        #endregion

        #region Damage Over Time Actions

        private static Dictionary<string, List<int>> _damageOverTimeActions;

        public static Dictionary<string, List<int>> DamageOverTimeActions
        {
            get
            {
                return _damageOverTimeActions ?? (_damageOverTimeActions = new Dictionary<string, List<int>>
                {
                    {
                        "Venomous Bite", new List<int>
                        {
                            100,
                            35,
                            9
                        }
                    },
                    {
                        "Wind Bite", new List<int>
                        {
                            60,
                            40,
                            18
                        }
                    },
                    {
                        "Aero", new List<int>
                        {
                            50,
                            25,
                            18
                        }
                    },
                    {
                        "Aero II", new List<int>
                        {
                            50,
                            40,
                            12
                        }
                    },
                    {
                        "Circle of Scorn", new List<int>
                        {
                            30,
                            30,
                            15
                        }
                    },
                    {
                        "Fracture", new List<int>
                        {
                            100,
                            20,
                            18
                        }
                    },
                    {
                        "Chaos Thrust", new List<int>
                        {
                            100,
                            20,
                            20
                        }
                    },
                    {
                        "Touch of Death", new List<int>
                        {
                            25,
                            20,
                            30
                        }
                    },
                    {
                        "Demolish", new List<int>
                        {
                            0,
                            40,
                            18
                        }
                    },
                    {
                        "Thunder", new List<int>
                        {
                            30,
                            40,
                            12
                        }
                    },
                    {
                        "Thunder II", new List<int>
                        {
                            50,
                            40,
                            15
                        }
                    },
                    {
                        "Thunder III", new List<int>
                        {
                            60,
                            40,
                            18
                        }
                    }
                });
            }
        }

        #endregion
    }
}
