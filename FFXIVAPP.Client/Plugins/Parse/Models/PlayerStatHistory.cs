// FFXIVAPP.Client
// PlayerStatHistory.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.Linq;
using FFXIVAPP.Client.Plugins.Parse.Models.Stats;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Plugins.Parse.Models
{
    [DoNotObfuscate]
    public class PlayerStatHistory
    {
        public PlayerStatHistory(StatGroup player)
        {
            DateTime = DateTime.Now;
            PlayerStats = new Dictionary<string, object>();
            var playerName = player.Name;
            PlayerStats.Add(playerName, new Dictionary<string, object>
            {
                {
                    "Stats", new Dictionary<string, object>()
                },
                {
                    "DamageByAction", new Dictionary<string, object>()
                },
                {
                    "DamageToMonsters", new Dictionary<string, object>()
                },
                {
                    "HealingByAction", new Dictionary<string, object>()
                },
                {
                    "HealingToPlayers", new Dictionary<string, object>()
                },
                {
                    "DamageTakenByAction", new Dictionary<string, object>()
                },
                {
                    "DamageTakenByMonsters", new Dictionary<string, object>()
                }
            });
            PlayerStats[playerName]["Stats"] = player.Stats.ToDictionary(s => s.Name, s => s.Value);
            // damage
            var damageByAction = player.GetGroup("DamageByAction");
            foreach (var action in damageByAction)
            {
                PlayerStats[playerName]["DamageByAction"].Add(action.Name, action.Stats.ToDictionary(s => s.Name, s => s.Value));
            }
            var damageToMonsters = player.GetGroup("DamageToMonsters");
            foreach (var monster in damageToMonsters)
            {
                PlayerStats[playerName]["DamageToMonsters"].Add(monster.Name, new Dictionary<string, object>
                {
                    {
                        "Stats", monster.Stats.ToDictionary(s => s.Name, s => s.Value)
                    },
                    {
                        "DamageToMonstersByAction", monster.GetGroup("DamageToMonstersByAction")
                                                           .ToDictionary(a => a.Name, a => a.Stats.ToDictionary(s => s.Name, s => s.Value))
                    }
                });
            }
            // healing
            var healingByAction = player.GetGroup("HealingByAction");
            foreach (var action in healingByAction)
            {
                PlayerStats[playerName]["HealingByAction"].Add(action.Name, action.Stats.ToDictionary(s => s.Name, s => s.Value));
            }
            var healingToPlayers = player.GetGroup("HealingToPlayers");
            foreach (var monster in healingToPlayers)
            {
                PlayerStats[playerName]["HealingToPlayers"].Add(monster.Name, new Dictionary<string, object>
                {
                    {
                        "Stats", monster.Stats.ToDictionary(s => s.Name, s => s.Value)
                    },
                    {
                        "HealingToPlayersByAction", monster.GetGroup("HealingToPlayersByAction")
                                                           .ToDictionary(a => a.Name, a => a.Stats.ToDictionary(s => s.Name, s => s.Value))
                    }
                });
            }
            // damage taken
            var damageTakenByAction = player.GetGroup("DamageTakenByAction");
            foreach (var action in damageTakenByAction)
            {
                PlayerStats[playerName]["DamageTakenByAction"].Add(action.Name, action.Stats.ToDictionary(s => s.Name, s => s.Value));
            }
            var damageTakenByMonsters = player.GetGroup("DamageTakenByMonsters");
            foreach (var monster in damageTakenByMonsters)
            {
                PlayerStats[playerName]["DamageTakenByMonsters"].Add(monster.Name, new Dictionary<string, object>
                {
                    {
                        "Stats", monster.Stats.ToDictionary(s => s.Name, s => s.Value)
                    },
                    {
                        "DamageTakenByMonstersByAction", monster.GetGroup("DamageTakenByMonstersByAction")
                                                                .ToDictionary(a => a.Name, a => a.Stats.ToDictionary(s => s.Name, s => s.Value))
                    }
                });
            }
        }

        public DateTime DateTime { get; set; }
        public dynamic PlayerStats { get; set; }
    }
}
