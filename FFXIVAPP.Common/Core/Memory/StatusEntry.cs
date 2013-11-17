// FFXIVAPP.Common
// StatusEntry.cs
// 
// © 2013 Ryan Wilson

namespace FFXIVAPP.Common.Core.Memory
{
    public class StatusEntry : IStatusEntry
    {
        public byte StatusID { get; set; }
        public float Duration { get; set; }
        public uint CasterID { get; set; }
    }
}
