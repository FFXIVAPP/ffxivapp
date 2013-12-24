// FFXIVAPP.Client
// Player.Stats.Healing.cs
// 
// © 2013 Ryan Wilson

using FFXIVAPP.Client.Plugins.Parse.Helpers;
using FFXIVAPP.Client.Plugins.Parse.Models.Stats;
using FFXIVAPP.Client.Properties;

namespace FFXIVAPP.Client.Plugins.Parse.Models.StatGroups
{
    public partial class Player
    {
        /// <summary>
        /// </summary>
        /// <param name="line"> </param>
        /// <param name="isHealingOverTime"></param>
        public void SetHealing(Line line, bool isHealingOverTime = false)
        {
            if (Name == Settings.Default.CharacterName && !Controller.IsHistoryBased)
            {
                //LineHistory.Add(new LineHistory(line));
            }
            var abilityGroup = GetGroup("HealingByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup))
            {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(HealingStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }
            var playerGroup = GetGroup("HealingToPlayers");
            StatGroup subPlayerGroup;
            if (!playerGroup.TryGetGroup(line.Target, out subPlayerGroup))
            {
                subPlayerGroup = new StatGroup(line.Target);
                subPlayerGroup.Stats.AddStats(HealingStatList(null));
                playerGroup.AddGroup(subPlayerGroup);
            }
            var abilities = subPlayerGroup.GetGroup("HealingToPlayersByAction");
            StatGroup subPlayerAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subPlayerAbilityGroup))
            {
                subPlayerAbilityGroup = new StatGroup(line.Action);
                subPlayerAbilityGroup.Stats.AddStats(HealingStatList(subPlayerGroup, true));
                abilities.AddGroup(subPlayerAbilityGroup);
            }
            if (!isHealingOverTime)
            {
                Stats.IncrementStat("TotalHealingActionsUsed");
                subAbilityGroup.Stats.IncrementStat("TotalHealingActionsUsed");
                subPlayerGroup.Stats.IncrementStat("TotalHealingActionsUsed");
                subPlayerAbilityGroup.Stats.IncrementStat("TotalHealingActionsUsed");
            }
            var currentHealing = line.Crit ? line.Amount > 0 ? ParseHelper.GetOriginalAmount(line.Amount, (decimal).5) : 0 : line.Amount;
            ParseHelper.LastHealingByAction.EnsurePlayerAction(line.Source, line.Action, currentHealing);
            Stats.IncrementStat("TotalOverallHealing", line.Amount);
            subAbilityGroup.Stats.IncrementStat("TotalOverallHealing", line.Amount);
            subPlayerGroup.Stats.IncrementStat("TotalOverallHealing", line.Amount);
            subPlayerAbilityGroup.Stats.IncrementStat("TotalOverallHealing", line.Amount);
            if (line.Crit)
            {
                Stats.IncrementStat("HealingCritHit");
                subAbilityGroup.Stats.IncrementStat("HealingCritHit");
                subPlayerGroup.Stats.IncrementStat("HealingCritHit");
                subPlayerAbilityGroup.Stats.IncrementStat("HealingCritHit");
                Stats.IncrementStat("CriticalHealing", line.Amount);
                subAbilityGroup.Stats.IncrementStat("CriticalHealing", line.Amount);
                subPlayerGroup.Stats.IncrementStat("CriticalHealing", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("CriticalHealing", line.Amount);
                if (line.Modifier != 0)
                {
                    var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                    var modStat = "HealingCritMod";
                    Stats.IncrementStat(modStat, mod);
                    subAbilityGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                }
            }
            else
            {
                Stats.IncrementStat("HealingRegHit");
                subAbilityGroup.Stats.IncrementStat("HealingRegHit");
                subPlayerGroup.Stats.IncrementStat("HealingRegHit");
                subPlayerAbilityGroup.Stats.IncrementStat("HealingRegHit");
                Stats.IncrementStat("RegularHealing", line.Amount);
                subAbilityGroup.Stats.IncrementStat("RegularHealing", line.Amount);
                subPlayerGroup.Stats.IncrementStat("RegularHealing", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("RegularHealing", line.Amount);
                if (line.Modifier != 0)
                {
                    var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                    var modStat = "HealingRegMod";
                    Stats.IncrementStat(modStat, mod);
                    subAbilityGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                }
            }
        }
    }
}
