// FFXIVAPP.Client
// ParseHelper.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FFXIVAPP.Client.Plugins.Parse.Enums;
using FFXIVAPP.Client.Plugins.Parse.Models;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Plugins.Parse.Helpers
{
    [DoNotObfuscate]
    public static class ParseHelper
    {
        // setup pet info that comes through as "YOU"
        private static List<string> _pets = new List<string>
        {
            "eos",
            "selene",
            "topaz carbuncle",
            "emerald carbuncle",
            "ifrit-egi",
            "titan-egi",
            "garuda-egi",
            "amber carbuncle",
            "carbuncle topaze",
            "carbuncle émeraude",
            "carbuncle ambre",
            "topas-karfunkel",
            "smaragd-karfunkel",
            "bernstein-karfunkel",
            "フェアリー・エオス",
            "フェアリー・セレネ",
            "カーバンクル・トパーズ",
            "カーバンクル・エメラルド",
            "イフリート・エギ",
            "タイタン・エギ",
            "ガルーダ・エギ",
            "カーバンクル・アンバー"
        };

        /// <summary>
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="modifier"></param>
        /// <returns></returns>
        public static decimal GetBonusDamage(decimal amount, decimal modifier)
        {
            return Math.Abs((amount / (modifier + 1)) - amount);
        }

        /// <summary>
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="modifier"></param>
        /// <returns></returns>
        public static decimal GetOriginalDamage(decimal amount, decimal modifier)
        {
            return Math.Abs(amount - GetBonusDamage(amount, modifier));
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="exp"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetPetFromPlayer(string name, Expressions exp, TimelineType type)
        {
            foreach (var pet in _pets.Where(pet => String.Equals(pet, name, Constants.InvariantComparer)))
            {
                switch (type)
                {
                    case TimelineType.You:
                        return String.Format("{0} [{1}]", name, exp.YouString);
                    case TimelineType.Party:
                        return String.Format("{0} [P]", name);
                    case TimelineType.Alliance:
                        return String.Format("{0} [A]", name);
                }
            }
            return String.Format("{0} [{1}]", name, "???");
        }
    }
}
