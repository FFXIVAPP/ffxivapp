// FFXIVAPP.Common
// ApplicationContext.cs
// 
// © 2013 Ryan Wilson

#region Usings

using FFXIVAPP.Common.Core.Constant;
using FFXIVAPP.Common.Core.Memory;

#endregion

namespace FFXIVAPP.Common.Core
{
    public class ApplicationContext : IApplicationContext
    {
        private IChatLogWorker _chatLogWorker;
        private IConstantWorker _constantWorker;
        private IGatheringWorker _gatheringWorker;
        private IMonsterWorker _monsterWorker;
        private INPCWorker _npcWorker;
        private IPCWorker _pcWorker;

        public IConstantWorker ConstantWorker
        {
            get { return _constantWorker ?? (_constantWorker = new ConstantWorker()); }
        }

        public IChatLogWorker ChatLogWorker
        {
            get { return _chatLogWorker ?? (_chatLogWorker = new ChatLogWorker()); }
        }

        public INPCWorker NPCWorker
        {
            get { return _npcWorker ?? (_npcWorker = new NPCWorker()); }
        }

        public IPCWorker PCWorker
        {
            get { return _pcWorker ?? (_pcWorker = new PCWorker()); }
        }

        public IMonsterWorker MonsterWorker
        {
            get { return _monsterWorker ?? (_monsterWorker = new MonsterWorker()); }
        }

        public IGatheringWorker GatheringWorker
        {
            get { return _gatheringWorker ?? (_gatheringWorker = new GatheringWorker()); }
        }
    }
}
