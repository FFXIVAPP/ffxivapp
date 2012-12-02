// FFXIVAPP
// EventCodeComparer.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System.Collections.Generic;

namespace FFXIVAPP.Models
{
    public class EventCodeComparer : IEqualityComparer<EventCode>
    {
        /// <summary>
        /// </summary>
        /// <param name="ec1"> </param>
        /// <param name="ec2"> </param>
        /// <returns> </returns>
        public bool Equals(EventCode ec1, EventCode ec2)
        {
            return (ec1.Code == ec2.Code);
        }

        /// <summary>
        /// </summary>
        /// <param name="ec"> </param>
        /// <returns> </returns>
        public int GetHashCode(EventCode ec)
        {
            return ec.Code.GetHashCode();
        }
    }
}