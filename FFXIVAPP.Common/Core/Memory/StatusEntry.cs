// FFXIVAPP.Common
// StatusEntry.cs
// 
// © 2013 Ryan Wilson

using FFXIVAPP.Common.Core.Memory.Interfaces;
using FFXIVAPP.Common.Helpers;

namespace FFXIVAPP.Common.Core.Memory
{
    public class StatusEntry : IStatusEntry
    {
        private string _targetName;

        public bool IsCompanyAction { get; set; }

        public string TargetName
        {
            get { return _targetName; }
            set { _targetName = StringHelper.TitleCase(value); }
        }

        public string StatusName { get; set; }
        public short StatusID { get; set; }
        public float Duration { get; set; }
        public uint CasterID { get; set; }

        public bool IsValid()
        {
            return StatusID > 0 && Duration <= 86400 && CasterID > 0;
        }
    }
}
