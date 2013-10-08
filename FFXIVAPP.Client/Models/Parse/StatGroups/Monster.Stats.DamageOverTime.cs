// FFXIVAPP.Client
// Monster.Stats.DamageOverTime.cs
// 
// © 2013 Ryan Wilson

#region Usings

using FFXIVAPP.Client.Models.Parse.Stats;

#endregion

namespace FFXIVAPP.Client.Models.Parse.StatGroups
{
    public partial class Monster
    {
        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        public void SetupDamageOverTimeAction(Line line)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        public void SetDamageOverTime(Line line)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        public void SetDamageOverTimeFromPlayer(Line line)
        {
            var damageGroup = GetGroup("DamageTakenByPlayers");
            StatGroup subPlayerGroup;
            if (!damageGroup.TryGetGroup(line.Source, out subPlayerGroup))
            {
                subPlayerGroup = new StatGroup(line.Source);
                subPlayerGroup.Stats.AddStats(DamageTakenStatList(null));
                damageGroup.AddGroup(subPlayerGroup);
            }
            var abilities = subPlayerGroup.GetGroup("DamageTakenByPlayersByAction");
            StatGroup subPlayerAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subPlayerAbilityGroup))
            {
                subPlayerAbilityGroup = new StatGroup(line.Action);
                subPlayerAbilityGroup.Stats.AddStats(DamageTakenStatList(subPlayerGroup, true));
                abilities.AddGroup(subPlayerAbilityGroup);
            }
            Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
            subPlayerGroup.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
            subPlayerAbilityGroup.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
            Stats.IncrementStat("DamageTakenDOT", line.Amount);
            subPlayerGroup.Stats.IncrementStat("DamageTakenDOT", line.Amount);
            subPlayerAbilityGroup.Stats.IncrementStat("DamageTakenDOT", line.Amount);
        }
    }
}
