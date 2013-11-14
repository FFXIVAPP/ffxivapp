// FFXIVAPP.Common
// ApplicationContext.cs
// 
// © 2013 Ryan Wilson

#region Usings

using FFXIVAPP.Common.Core.ChatLog;
using FFXIVAPP.Common.Core.Constant;

#endregion

namespace FFXIVAPP.Common.Core
{
    public class ApplicationContext : IApplicationContext
    {
        private IChatLogWorker _chatLogWorker;
        private IConstantWorker _constantWorker;

        public IChatLogWorker ChatLogWorker
        {
            get { return _chatLogWorker ?? (_chatLogWorker = new ChatLogWorker()); }
        }

        public IConstantWorker ConstantWorker
        {
            get { return _constantWorker ?? (_constantWorker = new ConstantWorker()); }
        }
    }
}
