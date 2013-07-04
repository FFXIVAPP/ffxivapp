// FFXIVAPP.Plugin.Parse
// ParseHelper.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FFXIVAPP.Plugin.Parse.Models;

#endregion

namespace FFXIVAPP.Plugin.Parse.Helpers
{
    public static class ParseHelper
    {
        public static decimal GetOriginalDamage(decimal amount, decimal modifier)
        {
            return Math.Abs((amount / (modifier + 1)) - amount);
        }

        public static decimal GetDamageOverTime(string action, decimal duration)
        {
            var damageOverTimeAction = DamageOverTimeActions.First(d => d.)
            return 0;
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

        private static List<DamageOverTimeAction> _damageOverTimeActions;

        public static List<DamageOverTimeAction> DamageOverTimeActions
        {
            get
            {
                return _damageOverTimeActions ?? (_damageOverTimeActions = new List<DamageOverTimeAction>
                {
                    new DamageOverTimeAction
                    {
                        Name = "Venomous Bite",
                        ActionPotency = 100,
                        DoTPotency = 35,
                        Duration = 9
                    },
                    new DamageOverTimeAction
                    {
                        Name = "Aero",
                        ActionPotency = 50,
                        DoTPotency = 25,
                        Duration = 18
                    },
                    new DamageOverTimeAction
                    {
                        Name = "Aero II",
                        ActionPotency = 50,
                        DoTPotency = 40,
                        Duration = 12
                    },
                    new DamageOverTimeAction
                    {
                        Name = "Circle of Scorn",
                        ActionPotency = 30,
                        DoTPotency = 30,
                        Duration = 15
                    },
                    new DamageOverTimeAction
                    {
                        Name = "Fracture",
                        ActionPotency = 100,
                        DoTPotency = 20,
                        Duration = 18
                    },
                    new DamageOverTimeAction
                    {
                        Name = "Chaos Thrust",
                        ActionPotency = 100,
                        DoTPotency = 20,
                        Duration = 20
                    },
                    new DamageOverTimeAction
                    {
                        Name = "Thunder",
                        ActionPotency = 30,
                        DoTPotency = 40,
                        Duration = 12
                    },
                    new DamageOverTimeAction
                    {
                        Name = "Thunder II",
                        ActionPotency = 50,
                        DoTPotency = 40,
                        Duration = 15
                    },
                    new DamageOverTimeAction
                    {
                        Name = "Thunder III",
                        ActionPotency = 60,
                        DoTPotency = 40,
                        Duration = 18
                    }
                });
            }
        }

        #endregion
    }
}
