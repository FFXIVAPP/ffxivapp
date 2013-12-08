// FFXIVAPP.IPluginInterface
// ConstantsEntityEvent.cs
// 
// © 2013 Ryan Wilson

using System;
using FFXIVAPP.Common.Core.Constant;

namespace FFXIVAPP.IPluginInterface.Events
{
    public class ConstantsEntityEvent : EventArgs
    {
        public ConstantsEntityEvent(object sender, ConstantsEntity constantsEntity)
        {
            Sender = sender;
            ConstantsEntity = constantsEntity;
        }

        public object Sender { get; set; }
        public ConstantsEntity ConstantsEntity { get; set; }
    }
}
