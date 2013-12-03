// FFXIVAPP.Common
// ApplicationContext.cs
// 
// © 2013 Ryan Wilson

#region Usings

using FFXIVAPP.Common.Core.Constant;
using FFXIVAPP.Common.Core.EventHandlers;
using FFXIVAPP.Common.Core.Memory;

#endregion

namespace FFXIVAPP.Common.Core
{
    public class ApplicationContext : IApplicationContext
    {
        private IChatLogEntryEvent _chatLogWorker;
        private IConstantEntryEvent _constantWorker;
        private IActorEntitiesEvent _monsterWorker;
        private IActorEntitiesEvent _npcWorker;
        private IActorEntitiesEvent _pcWorker;
        private IPlayerEntityEvent _playerInfoWorker;
        private ITargetEntityEvent _targetWorker;

        public IPlayerEntityEvent PlayerInfoWorker
        {
            get { return _playerInfoWorker ?? (_playerInfoWorker = new PlayerInfoWorker()); }
        }

        public IActorEntitiesEvent MonsterWorker
        {
            get { return _monsterWorker ?? (_monsterWorker = new MonsterWorker()); }
        }

        public IActorEntitiesEvent NPCWorker
        {
            get { return _npcWorker ?? (_npcWorker = new NPCWorker()); }
        }

        public IActorEntitiesEvent PCWorker
        {
            get { return _pcWorker ?? (_pcWorker = new PCWorker()); }
        }

        public IChatLogEntryEvent ChatLogWorker
        {
            get { return _chatLogWorker ?? (_chatLogWorker = new ChatLogWorker()); }
        }

        public IConstantEntryEvent ConstantWorker
        {
            get { return _constantWorker ?? (_constantWorker = new ConstantWorker()); }
        }

        public ITargetEntityEvent TargetWorker
        {
            get { return _targetWorker ?? (_targetWorker = new TargetWorker()); }
        }
    }
}
