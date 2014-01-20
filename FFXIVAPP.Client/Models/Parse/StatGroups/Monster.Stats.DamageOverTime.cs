// FFXIVAPP.Client
// Monster.Stats.DamageOverTime.cs
// 
// © 2013 Ryan Wilson

namespace FFXIVAPP.Client.Models.Parse.StatGroups
{
    public partial class Monster
    {
        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        public void SetDamageOverTime(Line line)
        {
            if (!Controller.IsHistoryBased)
            {
                //LineHistory.Add(new LineHistory(line));
            }

            // stubbed
        }
    }
}
