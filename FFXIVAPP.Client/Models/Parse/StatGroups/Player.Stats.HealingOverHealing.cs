// FFXIVAPP.Client
// Player.Stats.HealingOverHealing.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Text.RegularExpressions;
using FFXIVAPP.Client.Enums.Parse;
using FFXIVAPP.Client.Helpers.Parse;
using FFXIVAPP.Client.Models.Parse.Stats;
using FFXIVAPP.Client.Properties;

namespace FFXIVAPP.Client.Models.Parse.StatGroups
{
    public partial class Player
    {
        /// <summary>
        /// </summary>
        /// <param name="line"> </param>
        public void SetHealingOverHealing(Line line)
        {
            if (Name == Settings.Default.CharacterName && !Controller.IsHistoryBased)
            {
                //LineHistory.Add(new LineHistory(line));
            }

            var currentHealing = line.Crit ? line.Amount > 0 ? ParseHelper.GetOriginalAmount(line.Amount, (decimal) .5) : 0 : line.Amount;
            if (currentHealing > 0)
            {
                ParseHelper.LastAmountByAction.EnsurePlayerAction(line.Source, line.Action, currentHealing);
            }

            var abilityGroup = GetGroup("HealingOverHealingByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup))
            {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(HealingOverHealingStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }
            var playerGroup = GetGroup("HealingOverHealingToPlayers");
            StatGroup subPlayerGroup;
            if (!playerGroup.TryGetGroup(line.Target, out subPlayerGroup))
            {
                subPlayerGroup = new StatGroup(line.Target);
                subPlayerGroup.Stats.AddStats(HealingOverHealingStatList(null));
                playerGroup.AddGroup(subPlayerGroup);
            }
            var abilities = subPlayerGroup.GetGroup("HealingOverHealingToPlayersByAction");
            StatGroup subPlayerAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subPlayerAbilityGroup))
            {
                subPlayerAbilityGroup = new StatGroup(line.Action);
                subPlayerAbilityGroup.Stats.AddStats(HealingOverHealingStatList(subPlayerGroup, true));
                abilities.AddGroup(subPlayerAbilityGroup);
            }
            Stats.IncrementStat("TotalHealingOverHealingActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalHealingOverHealingActionsUsed");
            subPlayerGroup.Stats.IncrementStat("TotalHealingOverHealingActionsUsed");
            subPlayerAbilityGroup.Stats.IncrementStat("TotalHealingOverHealingActionsUsed");
            Stats.IncrementStat("TotalOverallHealingOverHealing", line.Amount);
            subAbilityGroup.Stats.IncrementStat("TotalOverallHealingOverHealing", line.Amount);
            subPlayerGroup.Stats.IncrementStat("TotalOverallHealingOverHealing", line.Amount);
            subPlayerAbilityGroup.Stats.IncrementStat("TotalOverallHealingOverHealing", line.Amount);
            if (line.Crit)
            {
                Stats.IncrementStat("HealingOverHealingCritHit");
                Stats.IncrementStat("CriticalHealingOverHealing", line.Amount);
                subAbilityGroup.Stats.IncrementStat("HealingOverHealingCritHit");
                subAbilityGroup.Stats.IncrementStat("CriticalHealingOverHealing", line.Amount);
                subPlayerGroup.Stats.IncrementStat("HealingOverHealingCritHit");
                subPlayerGroup.Stats.IncrementStat("CriticalHealingOverHealing", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("HealingOverHealingCritHit");
                subPlayerAbilityGroup.Stats.IncrementStat("CriticalHealingOverHealing", line.Amount);
                if (line.Modifier != 0)
                {
                    var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                    var modStat = "HealingOverHealingCritMod";
                    Stats.IncrementStat(modStat, mod);
                    subAbilityGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                }
            }
            else
            {
                Stats.IncrementStat("HealingOverHealingRegHit");
                Stats.IncrementStat("RegularHealingOverHealing", line.Amount);
                subAbilityGroup.Stats.IncrementStat("HealingOverHealingRegHit");
                subAbilityGroup.Stats.IncrementStat("RegularHealingOverHealing", line.Amount);
                subPlayerGroup.Stats.IncrementStat("HealingOverHealingRegHit");
                subPlayerGroup.Stats.IncrementStat("RegularHealingOverHealing", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("HealingOverHealingRegHit");
                subPlayerAbilityGroup.Stats.IncrementStat("RegularHealingOverHealing", line.Amount);
                if (line.Modifier != 0)
                {
                    var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                    var modStat = "HealingOverHealingRegMod";
                    Stats.IncrementStat(modStat, mod);
                    subAbilityGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                }
            }
        }

        private void SetupHealingOverHealing(Line line, HealingType healingType)
        {
            var cleanedAction = Regex.Replace(line.Action, @" \[.+\]", "");
            switch (healingType)
            {
                case HealingType.Normal:
                    line.Action = String.Format("{0} [∞]", cleanedAction);
                    SetHealingOverHealing(line);
                    break;
                case HealingType.HealingOverTime:
                    line.Action = String.Format("{0} [•][∞]", cleanedAction);
                    SetHealingOverHealing(line);
                    break;
            }
        }
    }
}
