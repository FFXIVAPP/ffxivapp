// FFXIVAPP.Plugin.Parse
// CommandBuilder.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using FFXIVAPP.Common.RegularExpressions;
using FFXIVAPP.Plugin.Parse.Models;
using FFXIVAPP.Plugin.Parse.Models.Stats;

#endregion

namespace FFXIVAPP.Plugin.Parse.Utilities
{
    internal static class CommandBuilder
    {
        /// <summary>
        /// </summary>
        /// <param name="line"> </param>
        /// <param name="commands"> </param>
        public static void GetCommands(string line, out List<string> commands)
        {
            List<string> temp = null;
            var parseCommands = SharedRegEx.ParseCommands.Match(line);
            if (!parseCommands.Success)
            {
                commands = null;
                return;
            }
            var cmd = parseCommands.Groups["cmd"].Success ? parseCommands.Groups["cmd"].Value : "";
            var cm = parseCommands.Groups["cm"].Success ? parseCommands.Groups["cm"].Value : "p";
            var sub = parseCommands.Groups["sub"].Success ? parseCommands.Groups["sub"].Value : "";
            var limit = parseCommands.Groups["limit"].Success ? Convert.ToInt32(parseCommands.Groups["limit"].Value) : 1000;
            limit = (limit == 0) ? 1000 : limit;
            var ptline = ParseControl.Instance.Timeline.Party;
            switch (cmd)
            {
                case "show-mob":
                    var results = new Dictionary<string, int>();
                    temp = new List<string>
                    {
                        String.Format("/{0} * {1} *", cm, sub)
                    };
                    foreach (var player in ptline)
                    {
                        StatGroup m;
                        if (!player.TryGetGroup("Monsters", out m))
                        {
                            continue;
                        }
                        foreach (var stats in m.Where(s => s.Name == sub)
                                               .Select(r => r.Stats))
                        {
                            results.Add(player.Name, (int) Math.Ceiling(stats.GetStatValue("TotalOverallDamageTaken")));
                        }
                    }
                    temp.AddRange(results.OrderByDescending(i => i.Value)
                                         .Select(item => String.Format("/{0} ", cm) + item.Key + ": " + item.Value));
                    break;
                case "show-total":
                    string t;
                    switch (sub)
                    {
                        case "damage":

                            t = PluginViewModel.Instance.Locale["parse_PartyDamageTabHeader"];
                            temp = new List<string>
                            {
                                String.Format("/{0} * {1} *", cm, t)
                            };
                            foreach (var item in ptline.OrderByDescending(i => i.Stats.GetStatValue("TotalOverallDamage"))
                                                       .Take(limit))
                            {
                                var amount = Math.Ceiling(item.Stats.GetStatValue("TotalOverallDamage"));
                                temp.Add(String.Format("/{0} ", cm) + item.Name + ": " + amount);
                            }
                            break;
                        case "healing":
                            t = PluginViewModel.Instance.Locale["parse_PartyHealingTabHeader"];
                            temp = new List<string>
                            {
                                String.Format("/{0} * {1} *", cm, t)
                            };
                            foreach (var item in ptline.OrderByDescending(i => i.Stats.GetStatValue("TotalOverallHealing"))
                                                       .Take(limit))
                            {
                                var amount = Math.Ceiling(item.Stats.GetStatValue("TotalOverallHealing"));
                                temp.Add(String.Format("/{0} ", cm) + item.Name + ": " + amount);
                            }
                            break;
                        case "damagetaken":
                            t = PluginViewModel.Instance.Locale["parse_PartyDamageTakenTabHeader"];
                            temp = new List<string>
                            {
                                String.Format("/{0} * {1} *", cm, t)
                            };
                            foreach (var item in ptline.OrderByDescending(i => i.Stats.GetStatValue("TotalOverallDamageTaken"))
                                                       .Take(limit))
                            {
                                var amount = Math.Ceiling(item.Stats.GetStatValue("TotalOverallDamageTaken"));
                                temp.Add(String.Format("/{0} ", cm) + item.Name + ": " + amount);
                            }
                            break;
                        case "dps":
                            t = "DPS";
                            temp = new List<string>
                            {
                                String.Format("/{0} * {1} *", cm, t)
                            };
                            foreach (var item in ptline.OrderBy(i => Math.Ceiling((decimal) i.GetStatValue("DPS")))
                                                       .Take(limit))
                            {
                                var amount = Math.Ceiling(item.Stats.GetStatValue("DPS"));
                                temp.Add(String.Format("/{0} ", cm) + item.Name + ": " + amount);
                            }
                            break;
                    }
                    break;
            }
            if (temp != null)
            {
                commands = temp.Count == 1 ? null : temp;
                return;
            }
            commands = null;
        }
    }
}
