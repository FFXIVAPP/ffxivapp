// FFXIVAPP.Client
// NPCEntry.cs
// 
// © 2013 Ryan Wilson

namespace FFXIVAPP.Client.Memory
{
    public class NPCEntry
    {
        #region Property Backings

        public string Name { get; set; }
        public int ID { get; set; }
        public int Type { get; set; }

        public NPCType NPCType
        {
            get
            {
                switch (Type)
                {
                    case 1:
                        return NPCType.PC;
                    case 2:
                        return NPCType.Monster;
                    case 3:
                        return NPCType.NPC;
                    case 6:
                        return NPCType.Gathering;
                }
                return NPCType.PC;
            }
        }

        public Coordinate Coordinate { get; set; }
        public float Heading { get; set; }
        public int HPCurrent { get; set; }
        public int HPMax { get; set; }
        public int MPCurrent { get; set; }
        public int MPMax { get; set; }
        public int TPCurrent { get; set; }
        public int TPMax { get; set; }

        #endregion

        #region Declarations

        #endregion
    }
}
