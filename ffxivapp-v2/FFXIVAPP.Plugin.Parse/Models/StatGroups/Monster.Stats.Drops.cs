// FFXIVAPP.Plugin.Parse
// Monster.Stats.Drops.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using FFXIVAPP.Plugin.Parse.Models.Stats;

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups
{
    public partial class Monster
    {
        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        public void SetDropStat(string name)
        {
            var dropGroup = GetGroup("Drops");
            StatGroup subGroup;
            if (!dropGroup.TryGetGroup(name, out subGroup))
            {
                subGroup = new StatGroup(name);
                subGroup.Stats.AddStats(DropStatList());
                dropGroup.AddGroup(subGroup);
            }
            subGroup.Stats.GetStat("Total").Value += 1;
        }
    }
}