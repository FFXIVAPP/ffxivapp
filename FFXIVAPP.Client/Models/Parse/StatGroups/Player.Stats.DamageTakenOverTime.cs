// FFXIVAPP.Client
// Player.Stats.DamageTakenOverTime.cs
// 
// © 2013 Ryan Wilson

using FFXIVAPP.Client.Properties;

namespace FFXIVAPP.Client.Models.Parse.StatGroups
{
    public partial class Player
    {
        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        public void SetDamageTakenOverTime(Line line)
        {
            if (Name == Settings.Default.CharacterName)
            {
                //LineHistory.Add(new LineHistory(line));
            }

            // stubbed
        }
    }
}
