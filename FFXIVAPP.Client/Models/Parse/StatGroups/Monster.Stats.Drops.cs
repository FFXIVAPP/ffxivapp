// FFXIVAPP.Client
// Monster.Stats.Drops.cs
// 
// © 2013 Ryan Wilson

using FFXIVAPP.Client.Models.Parse.Stats;

namespace FFXIVAPP.Client.Models.Parse.StatGroups
{
    public partial class Monster
    {
        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        public void SetDrop(string name)
        {
            var dropGroup = GetGroup("DropsByMonster");
            StatGroup subGroup;
            if (!dropGroup.TryGetGroup(name, out subGroup))
            {
                subGroup = new StatGroup(name);
                subGroup.Stats.AddStats(DropStatList());
                dropGroup.AddGroup(subGroup);
            }
            subGroup.Stats.IncrementStat("TotalDrops");
        }
    }
}
