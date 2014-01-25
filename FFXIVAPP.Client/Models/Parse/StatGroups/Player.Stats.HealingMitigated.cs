// FFXIVAPP.Client
// Player.Stats.HealingMitigated.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Text.RegularExpressions;
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
        public void SetHealingMitigated(Line line)
        {
            if (Name == Settings.Default.CharacterName)
            {
                //LineHistory.Add(new LineHistory(line));
            }

            var currentHealing = line.Crit ? line.Amount > 0 ? ParseHelper.GetOriginalAmount(line.Amount, (decimal) .5) : 0 : line.Amount;
            if (currentHealing > 0)
            {
                ParseHelper.LastAmountByAction.EnsurePlayerAction(line.Source, line.Action, currentHealing);
            }

            var abilityGroup = GetGroup("HealingMitigatedByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup))
            {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(HealingMitigatedStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }
            var playerGroup = GetGroup("HealingMitigatedToPlayers");
            StatGroup subPlayerGroup;
            if (!playerGroup.TryGetGroup(line.Target, out subPlayerGroup))
            {
                subPlayerGroup = new StatGroup(line.Target);
                subPlayerGroup.Stats.AddStats(HealingMitigatedStatList(null));
                playerGroup.AddGroup(subPlayerGroup);
            }
            var abilities = subPlayerGroup.GetGroup("HealingMitigatedToPlayersByAction");
            StatGroup subPlayerAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subPlayerAbilityGroup))
            {
                subPlayerAbilityGroup = new StatGroup(line.Action);
                subPlayerAbilityGroup.Stats.AddStats(HealingMitigatedStatList(subPlayerGroup, true));
                abilities.AddGroup(subPlayerAbilityGroup);
            }
            Stats.IncrementStat("TotalHealingMitigatedActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalHealingMitigatedActionsUsed");
            subPlayerGroup.Stats.IncrementStat("TotalHealingMitigatedActionsUsed");
            subPlayerAbilityGroup.Stats.IncrementStat("TotalHealingMitigatedActionsUsed");
            Stats.IncrementStat("TotalOverallHealingMitigated", line.Amount);
            subAbilityGroup.Stats.IncrementStat("TotalOverallHealingMitigated", line.Amount);
            subPlayerGroup.Stats.IncrementStat("TotalOverallHealingMitigated", line.Amount);
            subPlayerAbilityGroup.Stats.IncrementStat("TotalOverallHealingMitigated", line.Amount);
            if (line.Crit)
            {
                Stats.IncrementStat("HealingMitigatedCritHit");
                Stats.IncrementStat("CriticalHealingMitigated", line.Amount);
                subAbilityGroup.Stats.IncrementStat("HealingMitigatedCritHit");
                subAbilityGroup.Stats.IncrementStat("CriticalHealingMitigated", line.Amount);
                subPlayerGroup.Stats.IncrementStat("HealingMitigatedCritHit");
                subPlayerGroup.Stats.IncrementStat("CriticalHealingMitigated", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("HealingMitigatedCritHit");
                subPlayerAbilityGroup.Stats.IncrementStat("CriticalHealingMitigated", line.Amount);
                if (line.Modifier != 0)
                {
                    var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                    var modStat = "HealingMitigatedCritMod";
                    Stats.IncrementStat(modStat, mod);
                    subAbilityGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                }
            }
            else
            {
                Stats.IncrementStat("HealingMitigatedRegHit");
                Stats.IncrementStat("RegularHealingMitigated", line.Amount);
                subAbilityGroup.Stats.IncrementStat("HealingMitigatedRegHit");
                subAbilityGroup.Stats.IncrementStat("RegularHealingMitigated", line.Amount);
                subPlayerGroup.Stats.IncrementStat("HealingMitigatedRegHit");
                subPlayerGroup.Stats.IncrementStat("RegularHealingMitigated", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("HealingMitigatedRegHit");
                subPlayerAbilityGroup.Stats.IncrementStat("RegularHealingMitigated", line.Amount);
                if (line.Modifier != 0)
                {
                    var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                    var modStat = "HealingMitigatedRegMod";
                    Stats.IncrementStat(modStat, mod);
                    subAbilityGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                }
            }
        }

        public void SetupHealingMitigated(Line line, string type = "")
        {
            var cleanedAction = Regex.Replace(line.Action, @" \[.+\]", "");
            line.Action = String.Format("{0} [☯]", cleanedAction);
            switch (type)
            {
                case "adloquium":
                    if (line.Crit)
                    {
                        line.Amount = line.Amount * 2;
                    }
                    break;
                case "succor":
                    break;
                default:
                    break;
            }
            SetHealingMitigated(line);
        }
    }
}
