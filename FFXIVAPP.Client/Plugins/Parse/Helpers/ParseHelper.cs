// FFXIVAPP.Client
// ParseHelper.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
        /// <returns></returns>
        public static string GetPetFromPlayer(string name, Expressions exp)
        {
            if (Regex.IsMatch(name, exp.You))
            {
                name = String.IsNullOrWhiteSpace(Constants.CharacterName) ? "You" : Constants.CharacterName;
            }
            return _pets.Contains(name.ToLower()) ? String.Format("{0} [{1}]", name, exp.YouString) : name;
        }
    }
}
