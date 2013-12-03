// FFXIVAPP.Common
// IEnmityEntry.cs
// 
// © 2013 Ryan Wilson

namespace FFXIVAPP.Common.Core.Memory
{
    public interface IEnmityEntry
    {
        string Name { get; set; }
        uint ID { get; set; }
        uint Enmity { get; set; }
    }
}
