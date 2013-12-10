// FFXIVAPP.Client
// EventCodeComparer.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Plugins.Parse.Models.Events
{
    [DoNotObfuscate]
    public class EventCodeComparer : IEqualityComparer<EventCode>
    {
        /// <summary>
        /// </summary>
        /// <param name="eventCode1"> </param>
        /// <param name="eventCode2"> </param>
        /// <returns> </returns>
        public bool Equals(EventCode eventCode1, EventCode eventCode2)
        {
            return (eventCode1.Code == eventCode2.Code);
        }

        /// <summary>
        /// </summary>
        /// <param name="eventCode"> </param>
        /// <returns> </returns>
        public int GetHashCode(EventCode eventCode)
        {
            return eventCode.Code.GetHashCode();
        }
    }
}
