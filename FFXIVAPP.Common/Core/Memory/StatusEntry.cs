// FFXIVAPP.Common
// StatusEntry.cs
// 
// © 2013 Ryan Wilson

using FFXIVAPP.Common.Helpers;

namespace FFXIVAPP.Common.Core.Memory
{
    public class StatusEntry : IStatusEntry
    {
        private string _targetName;

        public string TargetName
        {
            get { return _targetName; }
            set { _targetName = StringHelper.TitleCase(value); }
        }

        public byte StatusID { get; set; }
        public float Duration { get; set; }
        public uint CasterID { get; set; }

        public bool IsValid()
        {
            return StatusID > 0 && CasterID > 0;
        }
    }
}
