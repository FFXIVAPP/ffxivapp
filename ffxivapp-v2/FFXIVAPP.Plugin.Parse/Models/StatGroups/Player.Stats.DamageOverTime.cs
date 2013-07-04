// FFXIVAPP.Plugin.Parse
// Player.Stats.DamageOverTime.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups
{
    public partial class Player
    {
        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        private void SetupDamageOverTimeAction(Line line)
        {
            if (DamageOverTimeActions.ContainsKey(line.Action))
            {
                DamageOverTimeActions[line.Action].HandleDamage();
                DamageOverTimeActions.Remove(line.Action);
            }
            DamageOverTimeActions.Add(line.Action, new DamageOverTimeAction(line));
        }

        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        public static void SetDamageOverTime(Line line)
        {
        }
    }
}
