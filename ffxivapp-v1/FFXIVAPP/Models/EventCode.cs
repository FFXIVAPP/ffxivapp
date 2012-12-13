// FFXIVAPP
// EventCode.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System;

#endregion

namespace FFXIVAPP.Models
{
    public class EventCode
    {
        /// <summary>
        /// </summary>
        public EventCode() {}

        /// <summary>
        /// </summary>
        /// <param name="description"> </param>
        /// <param name="code"> </param>
        /// <param name="group"> </param>
        public EventCode(String description, UInt16 code, EventGroup group)
        {
            Description = description;
            Code = code;
            Group = group;
        }

        public String Description { get; set; }
        public UInt16 Code { get; set; }
        public EventGroup Group { private get; set; }

        /// <summary>
        /// </summary>
        public UInt16 Flags
        {
            get { return (ushort) (Group == null ? 0x0000 : Group.Flags); }
        }

        /// <summary>
        /// </summary>
        public EventDirection Direction
        {
            get { return Group == null ? EventDirection.Unknown : Group.Direction; }
        }

        /// <summary>
        /// </summary>
        public EventSubject Subject
        {
            get { return Group == null ? EventSubject.Unknown : Group.Subject; }
        }

        /// <summary>
        /// </summary>
        public EventType Type
        {
            get { return Group == null ? EventType.Unknown : Group.Type; }
        }
    }
}
