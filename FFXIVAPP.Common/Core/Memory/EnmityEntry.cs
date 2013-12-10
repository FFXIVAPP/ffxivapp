// FFXIVAPP.Common
// EnmityEntry.cs
// 
// © 2013 Ryan Wilson

using FFXIVAPP.Common.Helpers;

namespace FFXIVAPP.Common.Core.Memory
{
    public class EnmityEntry : IEnmityEntry
    {
        private string _name;

        public string Name
        {
            get { return _name ?? ""; }
            set { _name = StringHelper.TitleCase(value); }
        }

        public uint ID { get; set; }
        public uint Enmity { get; set; }
    }
}
