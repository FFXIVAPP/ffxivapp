// FFXIVAPP.Client
// ParseHelper.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.Linq;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Plugins.Parse.Enums;
using FFXIVAPP.Client.Plugins.Parse.Models;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Plugins.Parse.Helpers
{
    [DoNotObfuscate]
    public static class ParseHelper
    {
        #region LastDamage Helper Dictionaries

        public static class LastDamageByAction
        {
            private static Dictionary<string, Dictionary<string, decimal>> Monster = new Dictionary<string, Dictionary<string, decimal>>();

            private static Dictionary<string, Dictionary<string, decimal>> Player = new Dictionary<string, Dictionary<string, decimal>>();

            public static Dictionary<string, decimal> GetMonster(string name)
            {
                if (!Monster.ContainsKey(name))
                {
                    Monster.Add(name, new Dictionary<string, decimal>());        
                }
                return Monster[name];
            }

            public static void EnsureMonsterAction(string name, string action, decimal amount)
            {
                GetMonster(name);
                if (Monster[name].ContainsKey(action))
                {
                    Monster[name][action] = amount;
                }
                else
                {
                    Monster[name].Add(action, amount);
                }
            }

            public static Dictionary<string, decimal> GetPlayer(string name)
            {
                if (!Player.ContainsKey(name))
                {
                    Player.Add(name, new Dictionary<string, decimal>());
                }
                return Player[name];
            }

            public static void EnsurePlayerAction(string name, string action, decimal amount)
            {
                GetPlayer(name);
                if (Player[name].ContainsKey(action))
                {
                    Player[name][action] = amount;
                }
                else
                {
                    Player[name].Add(action, amount);
                }
            }
        }

        #endregion

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
