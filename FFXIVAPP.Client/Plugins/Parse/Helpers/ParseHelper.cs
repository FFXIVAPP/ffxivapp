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
                    var original = Player[name][action];
                    Player[name][action] = (original + amount) / 2;
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

        private static List<string> _healingActions;

        public static List<string> HealingActions
        {
            get
            {
                if (_healingActions == null)
                {
                    _healingActions = new List<string>();
                    _healingActions.Add("内丹");
                    _healingActions.Add("Second Wind");
                    _healingActions.Add("Second souffle");
                    _healingActions.Add("Chakra");
                    _healingActions.Add("ケアル");
                    _healingActions.Add("Cure");
                    _healingActions.Add("Soin");
                    _healingActions.Add("Vita");
                    _healingActions.Add("メディカ");
                    _healingActions.Add("Medica");
                    _healingActions.Add("Médica");
                    _healingActions.Add("Reseda");
                    _healingActions.Add("ケアルガ");
                    _healingActions.Add("Cure III");
                    _healingActions.Add("Méga Soin");
                    _healingActions.Add("Vitaga");
                    _healingActions.Add("メディカラ");
                    _healingActions.Add("Medica II");
                    _healingActions.Add("Extra Médica");
                    _healingActions.Add("Resedra");
                    _healingActions.Add("ケアルラ");
                    _healingActions.Add("Cure II");
                    _healingActions.Add("Extra Soin");
                    _healingActions.Add("Vitra");
                    _healingActions.Add("鼓舞激励の策");
                    _healingActions.Add("Adloquium");
                    _healingActions.Add("Traité du réconfort");
                    _healingActions.Add("Adloquium");
                    _healingActions.Add("士気高揚の策");
                    _healingActions.Add("Succor");
                    _healingActions.Add("Traité du soulagement");
                    _healingActions.Add("Kurieren");
                    _healingActions.Add("フィジク");
                    _healingActions.Add("Physick");
                    _healingActions.Add("Médecine");
                    _healingActions.Add("Physick");
                    _healingActions.Add("光の癒し");
                    _healingActions.Add("Embrace");
                    _healingActions.Add("Embrassement");
                    _healingActions.Add("Sanfte Umarmung");
                    _healingActions.Add("光の囁き");
                    _healingActions.Add("Whispering Dawn");
                    _healingActions.Add("Murmure de l'aurore");
                    _healingActions.Add("Erhebendes Flüstern");
                    _healingActions.Add("光の癒し");
                    _healingActions.Add("Embrace");
                    _healingActions.Add("Embrassement");
                    _healingActions.Add("Sanfte Umarmung");
                    _healingActions.Add("チョコメディカ");
                    _healingActions.Add("Choco Medica");
                    _healingActions.Add("Choco-médica");
                    _healingActions.Add("Chocobo-Reseda");
                    _healingActions.Add("チョコケアル");
                    _healingActions.Add("Choco Cure");
                    _healingActions.Add("Choco-soin");
                    _healingActions.Add("Chocobo-Vita");
                    _healingActions.Add("チョコリジェネ");
                    _healingActions.Add("Choco Regen");
                    _healingActions.Add("Choco-récup");
                    _healingActions.Add("Chocobo-Regena");
                    //_healingActions.Add("リジェネ");
                    //_healingActions.Add("Regen");
                    //_healingActions.Add("Récup");
                    //_healingActions.Add("Regena");
                }
                return _healingActions;
            }
            set { _healingActions = value; }
        }

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
            var tag = "???";
            name = name.Trim();
            if (String.IsNullOrWhiteSpace(name))
            {
                name = "UNKNOWN";
            }
            name = Regex.Replace(name, @" \[[\w]+\]", "");
            var petFound = false;
            foreach (var pet in _pets.Where(pet => String.Equals(pet, name, Constants.InvariantComparer)))
            {
                petFound = true;
            }
            switch (type)
            {
                case TimelineType.You:
                    tag = exp.YouString;
                    break;
                case TimelineType.Party:
                    tag = "P";
                    break;
                case TimelineType.Alliance:
                    tag = "A";
                    break;
            }
            return String.Format("{0} [{1}]", name, tag);
        }
    }
}
