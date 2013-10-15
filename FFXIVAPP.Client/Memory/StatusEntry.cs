// FFXIVAPP.Client
// StatusEntry.cs
// 
// © 2013 Ryan Wilson

using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Memory
{
    [DoNotObfuscate]
    public class StatusEntry
    {
        #region Property Backings

        public uint ID { get; set; }
        public float Duration { get; set; }
        public uint OwnerID { get; set; }

        public bool IsValid
        {
            get
            {
                if (ID <= 0)
                {
                    return false;
                }
                if (OwnerID <= 0)
                {
                    return false;
                }
                return true;
            }
        }

        #endregion

        #region Declarations

        #endregion
    }
}
