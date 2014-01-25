// FFXIVAPP.Client
// Player.Stats.DamageOverTime.cs
// 
// © 2013 Ryan Wilson

using FFXIVAPP.Client.Helpers.Parse;
using FFXIVAPP.Client.Models.Parse.Stats;
using FFXIVAPP.Client.Properties;

namespace FFXIVAPP.Client.Models.Parse.StatGroups
{
    public partial class Player
    {
        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        public void SetDamageOverTime(Line line)
        {
            if (Name == Settings.Default.CharacterName)
            {
                //LineHistory.Add(new LineHistory(line));
            }

            if ((LimitBreaks.IsLimit(line.Action)) && Constants.Parse.PluginSettings.IgnoreLimitBreaks)
            {
                return;
            }

            var currentDamage = line.Crit ? line.Amount > 0 ? ParseHelper.GetOriginalAmount(line.Amount, (decimal) .5) : 0 : line.Amount;
            if (currentDamage > 0)
            {
                ParseHelper.LastAmountByAction.EnsurePlayerAction(line.Source, line.Action, currentDamage);
            }

            var abilityGroup = GetGroup("DamageOverTimeByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup))
            {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(DamageOverTimeStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }
            var monsterGroup = GetGroup("DamageOverTimeToMonsters");
            StatGroup subMonsterGroup;
            if (!monsterGroup.TryGetGroup(line.Target, out subMonsterGroup))
            {
                subMonsterGroup = new StatGroup(line.Target);
                subMonsterGroup.Stats.AddStats(DamageOverTimeStatList(null));
                monsterGroup.AddGroup(subMonsterGroup);
            }
            var monsters = subMonsterGroup.GetGroup("DamageOverTimeToMonstersByAction");
            StatGroup subMonsterAbilityGroup;
            if (!monsters.TryGetGroup(line.Action, out subMonsterAbilityGroup))
            {
                subMonsterAbilityGroup = new StatGroup(line.Action);
                subMonsterAbilityGroup.Stats.AddStats(DamageOverTimeStatList(subMonsterGroup, true));
                monsters.AddGroup(subMonsterAbilityGroup);
            }
            Stats.IncrementStat("TotalDamageOverTimeActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalDamageOverTimeActionsUsed");
            subMonsterGroup.Stats.IncrementStat("TotalDamageOverTimeActionsUsed");
            subMonsterAbilityGroup.Stats.IncrementStat("TotalDamageOverTimeActionsUsed");
            Stats.IncrementStat("TotalOverallDamageOverTime", line.Amount);
            subAbilityGroup.Stats.IncrementStat("TotalOverallDamageOverTime", line.Amount);
            subMonsterGroup.Stats.IncrementStat("TotalOverallDamageOverTime", line.Amount);
            subMonsterAbilityGroup.Stats.IncrementStat("TotalOverallDamageOverTime", line.Amount);
            if (line.Crit)
            {
                Stats.IncrementStat("DamageOverTimeCritHit");
                subAbilityGroup.Stats.IncrementStat("DamageOverTimeCritHit");
                subMonsterGroup.Stats.IncrementStat("DamageOverTimeCritHit");
                subMonsterAbilityGroup.Stats.IncrementStat("DamageOverTimeCritHit");
                Stats.IncrementStat("CriticalDamageOverTime", line.Amount);
                subAbilityGroup.Stats.IncrementStat("CriticalDamageOverTime", line.Amount);
                subMonsterGroup.Stats.IncrementStat("CriticalDamageOverTime", line.Amount);
                subMonsterAbilityGroup.Stats.IncrementStat("CriticalDamageOverTime", line.Amount);
            }
            else
            {
                Stats.IncrementStat("DamageOverTimeRegHit");
                subAbilityGroup.Stats.IncrementStat("DamageOverTimeRegHit");
                subMonsterGroup.Stats.IncrementStat("DamageOverTimeRegHit");
                subMonsterAbilityGroup.Stats.IncrementStat("DamageOverTimeRegHit");
                Stats.IncrementStat("RegularDamageOverTime", line.Amount);
                subAbilityGroup.Stats.IncrementStat("RegularDamageOverTime", line.Amount);
                subMonsterGroup.Stats.IncrementStat("RegularDamageOverTime", line.Amount);
                subMonsterAbilityGroup.Stats.IncrementStat("RegularDamageOverTime", line.Amount);
            }
        }
    }
}
