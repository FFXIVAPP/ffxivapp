// FFXIVAPP.Common
// IApplicationContext.cs
// 
// © 2013 Ryan Wilson

#region Usings

using FFXIVAPP.Common.Core.Constant;
using FFXIVAPP.Common.Core.Memory;

#endregion

namespace FFXIVAPP.Common.Core
{
    public interface IApplicationContext
    {
        IConstantWorker ConstantWorker { get; }
        IChatLogWorker ChatLogWorker { get; }
        INPCWorker NPCWorker { get; }
        IPCWorker PCWorker { get; }
        IMonsterWorker MonsterWorker { get; }
        IGatheringWorker GatheringWorker { get; }
    }
}
