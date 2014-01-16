// FFXIVAPP.Client
// Monster.Stats.Healing.cs
// 
// © 2013 Ryan Wilson

namespace FFXIVAPP.Client.Models.Parse.StatGroups
{
    public partial class Monster
    {
        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        public void SetHealing(Line line)
        {
            if (!Controller.IsHistoryBased)
            {
                //LineHistory.Add(new LineHistory(line));
            }
        }
    }
}
