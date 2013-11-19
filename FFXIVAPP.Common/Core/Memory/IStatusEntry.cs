// FFXIVAPP.Common
// IStatusEntry.cs
// 
// © 2013 Ryan Wilson

namespace FFXIVAPP.Common.Core.Memory
{
    public interface IStatusEntry
    {
        string TargetName { get; set; }
        byte StatusID { get; set; }
        float Duration { get; set; }
        uint CasterID { get; set; }
        bool IsValid();
    }
}
