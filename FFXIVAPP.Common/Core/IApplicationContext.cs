// FFXIVAPP.Common
// IApplicationContext.cs
// 
// © 2013 Ryan Wilson

#region Usings

using FFXIVAPP.Common.Core.ChatLog;
using FFXIVAPP.Common.Core.Constant;

#endregion

namespace FFXIVAPP.Common.Core
{
    public interface IApplicationContext
    {
        IChatLogWorker ChatLogWorker { get; }
        IConstantWorker ConstantWorker { get; }
    }
}
