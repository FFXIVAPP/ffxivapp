// FFXIVAPP.Client
// StatusEntry.cs
// 
// © 2013 Ryan Wilson

#region Usings

using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Memory
{
    [DoNotObfuscate]
    public class StatusEntry
    {
        #region Property Backings

        public string TargetName { get; set; }
        public uint StatusID { get; set; }
        public float Duration { get; set; }
        public uint CasterID { get; set; }

        public bool IsValid
        {
            get { return StatusID > 0 && CasterID > 0; }
        }

        #endregion

        #region Declarations

        #endregion
    }
}
