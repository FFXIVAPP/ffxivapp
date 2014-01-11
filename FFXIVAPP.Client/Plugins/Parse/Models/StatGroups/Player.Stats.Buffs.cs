// FFXIVAPP.Client
// Player.Stats.Buffs.cs
// 
// © 2013 Ryan Wilson

using System;
using FFXIVAPP.Client.Plugins.Parse.Models.Stats;
using FFXIVAPP.Client.Properties;

namespace FFXIVAPP.Client.Plugins.Parse.Models.StatGroups
{
    public partial class Player
    {
        /// <summary>
        /// </summary>
        /// <param name="line"> </param>
        public void SetBuff(Line line)
        {
            if (Name == Settings.Default.CharacterName && !Controller.IsHistoryBased)
            {
                //LineHistory.Add(new LineHistory(line));
            }

            var abilityGroup = GetGroup("BuffByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup))
            {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(BuffStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }
            var playerGroup = GetGroup("BuffToPlayers");
            StatGroup subPlayerGroup;
            if (!playerGroup.TryGetGroup(line.Target, out subPlayerGroup))
            {
                subPlayerGroup = new StatGroup(line.Target);
                subPlayerGroup.Stats.AddStats(BuffStatList(null));
                playerGroup.AddGroup(subPlayerGroup);
            }
            var abilities = subPlayerGroup.GetGroup("BuffToPlayersByAction");
            StatGroup subPlayerAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subPlayerAbilityGroup))
            {
                subPlayerAbilityGroup = new StatGroup(line.Action);
                subPlayerAbilityGroup.Stats.AddStats(BuffStatList(subPlayerGroup, true));
                abilities.AddGroup(subPlayerAbilityGroup);
            }
            Stats.IncrementStat("TotalBuffTime");
            subAbilityGroup.Stats.IncrementStat("TotalBuffTime");
            subPlayerGroup.Stats.IncrementStat("TotalBuffTime");
            subPlayerAbilityGroup.Stats.IncrementStat("TotalBuffTime");
            AdjustBuffTime(this);
            AdjustBuffTime(subAbilityGroup);
            AdjustBuffTime(subPlayerGroup);
            AdjustBuffTime(subPlayerAbilityGroup);
        }

        private void AdjustBuffTime(StatGroup statGroup)
        {
            var timeSpan = TimeSpan.FromSeconds((double) statGroup.Stats.GetStatValue("TotalBuffTime"));
            statGroup.Stats.GetStat("TotalBuffHours")
                     .Value = timeSpan.Hours;
            statGroup.Stats.GetStat("TotalBuffMinutes")
                     .Value = timeSpan.Minutes;
            statGroup.Stats.GetStat("TotalBuffSeconds")
                     .Value = timeSpan.Seconds;
        }
    }
}
