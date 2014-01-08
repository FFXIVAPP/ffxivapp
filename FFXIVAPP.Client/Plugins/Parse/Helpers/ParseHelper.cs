// FFXIVAPP.Client
// ParseHelper.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

        public static class LastAmountByAction
        {
            private static Dictionary<string, List<Tuple<string, decimal>>> Monster = new Dictionary<string, List<Tuple<string, decimal>>>();

            private static Dictionary<string, List<Tuple<string, decimal>>> Player = new Dictionary<string, List<Tuple<string, decimal>>>();

            public static Dictionary<string, decimal> GetMonster(string name)
            {
                var results = new Dictionary<string, decimal>();
                EnsureMonster(name);
                lock (Monster)
                {
                    var actionList = new List<string>();
                    var actionUpdateCount = new Dictionary<string, int>();
                    foreach (var actionTuple in Monster[name])
                    {
                        if (!results.ContainsKey(actionTuple.Item1))
                        {
                            results.Add(actionTuple.Item1, 0);
                        }
                        results[actionTuple.Item1] += actionTuple.Item2;
                        if (!actionList.Contains(actionTuple.Item1))
                        {
                            actionList.Add(actionTuple.Item1);
                        }
                        if (!actionUpdateCount.ContainsKey(actionTuple.Item1))
                        {
                            actionUpdateCount.Add(actionTuple.Item1, 0);
                        }
                        actionUpdateCount[actionTuple.Item1]++;
                    }
                    foreach (var action in actionList)
                    {
                        results[action] = results[action] / actionUpdateCount[action];
                    }
                }
                return results;
            }

            private static void EnsureMonster(string name)
            {
                lock (Monster)
                {
                    if (!Monster.ContainsKey(name))
                    {
                        Monster.Add(name, new List<Tuple<string, decimal>>());
                    }
                }
            }

            public static void EnsureMonsterAction(string name, string action, decimal amount)
            {
                EnsureMonster(name);
                lock (Monster)
                {
                    Monster[name].Add(new Tuple<string, decimal>(action, amount));
                }
            }

            public static Dictionary<string, decimal> GetPlayer(string name)
            {
                var results = new Dictionary<string, decimal>();
                EnsurePlayer(name);
                lock (Player)
                {
                    var actionList = new List<string>();
                    var actionUpdateCount = new Dictionary<string, int>();
                    foreach (var actionTuple in Player[name])
                    {
                        if (!results.ContainsKey(actionTuple.Item1))
                        {
                            results.Add(actionTuple.Item1, 0);
                        }
                        results[actionTuple.Item1] += actionTuple.Item2;
                        if (!actionList.Contains(actionTuple.Item1))
                        {
                            actionList.Add(actionTuple.Item1);
                        }
                        if (!actionUpdateCount.ContainsKey(actionTuple.Item1))
                        {
                            actionUpdateCount.Add(actionTuple.Item1, 0);
                        }
                        actionUpdateCount[actionTuple.Item1]++;
                    }
                    foreach (var action in actionList)
                    {
                        results[action] = results[action] / actionUpdateCount[action];
                    }
                }
                return results;
            }

            private static void EnsurePlayer(string name)
            {
                lock (Player)
                {
                    if (!Player.ContainsKey(name))
                    {
                        Player.Add(name, new List<Tuple<string, decimal>>());
                    }
                }
            }

            public static void EnsurePlayerAction(string name, string action, decimal amount)
            {
                EnsureMonster(name);
                lock (Player)
                {
                    Player[name].Add(new Tuple<string, decimal>(action, amount));
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
                    _healingActions.Add("生命活性法");
                    _healingActions.Add("Lustrate");
                    _healingActions.Add("Loi de revivification");
                    _healingActions.Add("Revitalisierung");
                    _healingActions.Add("チョコメディカ");
                    _healingActions.Add("Choco Medica");
                    _healingActions.Add("Choco-médica");
                    _healingActions.Add("Chocobo-Reseda");
                    _healingActions.Add("チョコケアル");
                    _healingActions.Add("Choco Cure");
                    _healingActions.Add("Choco-soin");
                    _healingActions.Add("Chocobo-Vita");
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
        public static decimal GetBonusAmount(decimal amount, decimal modifier)
        {
            return Math.Abs((amount / (modifier + 1)) - amount);
        }

        /// <summary>
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="modifier"></param>
        /// <returns></returns>
        public static decimal GetOriginalAmount(decimal amount, decimal modifier)
        {
            return Math.Abs(amount - GetBonusAmount(amount, modifier));
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="exp"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTaggedName(string name, Expressions exp, TimelineType type)
        {
            if (type == TimelineType.Unknown)
            {
                return name;
            }
            var tag = "???";
            name = name.Trim();
            if (String.IsNullOrWhiteSpace(name))
            {
                name = "UNKNOWN";
            }
            name = Regex.Replace(name, @"\[[\w]+\]", "")
                        .Trim();
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
                case TimelineType.Other:
                    tag = "O";
                    break;
            }
            return String.Format("[{0}] {1}", tag, name);
        }
    }
}
