// FFXIVAPP.Common
// IStatusEntry.cs
// 
// © 2013 Ryan Wilson

namespace FFXIVAPP.Common.Core.Memory.Interfaces
{
    public interface IStatusEntry
    {
        bool IsCompanyAction { get; set; }
        string TargetName { get; set; }
        string StatusName { get; set; }
        short StatusID { get; set; }
        float Duration { get; set; }
        uint CasterID { get; set; }
        bool IsValid();
    }
}
