// FFXIVAPP.Client
// Monster.Stats.DamageTakenOverTime.cs
// 
// © 2013 Ryan Wilson

using FFXIVAPP.Client.Models.Parse.Stats;

namespace FFXIVAPP.Client.Models.Parse.StatGroups
{
    public partial class Monster
    {
        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        public void SetDamageTakenOverTime(Line line)
        {
            if ((LimitBreaks.IsLimit(line.Action)) && Constants.Parse.PluginSettings.IgnoreLimitBreaks)
            {
                return;
            }

            //LineHistory.Add(new LineHistory(line));

            var abilityGroup = GetGroup("DamageTakenOverTimeByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup))
            {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(DamageTakenOverTimeStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }
            var damageGroup = GetGroup("DamageTakenOverTimeByPlayers");
            StatGroup subPlayerGroup;
            if (!damageGroup.TryGetGroup(line.Source, out subPlayerGroup))
            {
                subPlayerGroup = new StatGroup(line.Source);
                subPlayerGroup.Stats.AddStats(DamageTakenOverTimeStatList(null));
                damageGroup.AddGroup(subPlayerGroup);
            }
            var abilities = subPlayerGroup.GetGroup("DamageTakenOverTimeByPlayersByAction");
            StatGroup subPlayerAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subPlayerAbilityGroup))
            {
                subPlayerAbilityGroup = new StatGroup(line.Action);
                subPlayerAbilityGroup.Stats.AddStats(DamageTakenOverTimeStatList(subPlayerGroup, true));
                abilities.AddGroup(subPlayerAbilityGroup);
            }
            Stats.IncrementStat("TotalDamageTakenOverTimeActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalDamageTakenOverTimeActionsUsed");
            subPlayerGroup.Stats.IncrementStat("TotalDamageTakenOverTimeActionsUsed");
            subPlayerAbilityGroup.Stats.IncrementStat("TotalDamageTakenOverTimeActionsUsed");
            Stats.IncrementStat("TotalOverallDamageTakenOverTime", line.Amount);
            subAbilityGroup.Stats.IncrementStat("TotalOverallDamageTakenOverTime", line.Amount);
            subPlayerGroup.Stats.IncrementStat("TotalOverallDamageTakenOverTime", line.Amount);
            subPlayerAbilityGroup.Stats.IncrementStat("TotalOverallDamageTakenOverTime", line.Amount);
            if (line.Crit)
            {
                Stats.IncrementStat("DamageTakenOverTimeCritHit");
                subAbilityGroup.Stats.IncrementStat("DamageTakenOverTimeCritHit");
                subPlayerGroup.Stats.IncrementStat("DamageTakenOverTimeCritHit");
                subPlayerAbilityGroup.Stats.IncrementStat("DamageTakenOverTimeCritHit");
                Stats.IncrementStat("CriticalDamageTakenOverTime", line.Amount);
                subAbilityGroup.Stats.IncrementStat("CriticalDamageTakenOverTime", line.Amount);
                subPlayerGroup.Stats.IncrementStat("CriticalDamageTakenOverTime", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("CriticalDamageTakenOverTime", line.Amount);
            }
            else
            {
                Stats.IncrementStat("DamageTakenOverTimeRegHit");
                subAbilityGroup.Stats.IncrementStat("DamageTakenOverTimeRegHit");
                subPlayerGroup.Stats.IncrementStat("DamageTakenOverTimeRegHit");
                subPlayerAbilityGroup.Stats.IncrementStat("DamageTakenOverTimeRegHit");
                Stats.IncrementStat("RegularDamageTakenOverTime", line.Amount);
                subAbilityGroup.Stats.IncrementStat("RegularDamageTakenOverTime", line.Amount);
                subPlayerGroup.Stats.IncrementStat("RegularDamageTakenOverTime", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("RegularDamageTakenOverTime", line.Amount);
            }
        }
    }
}
