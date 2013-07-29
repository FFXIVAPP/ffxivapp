// ParseModXIV
// EventCode.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;

namespace ParseModXIV.Model
{
    public class EventCode
    {
        public String Description { get; set; }
        public UInt16 Code { get; set; }
        public EventGroup Group { private get; set; }

        /// <summary>
        /// 
        /// </summary>
        public UInt16 Flags
        {
            get { return (ushort) (Group == null ? 0x0000 : Group.Flags); }
        }

        /// <summary>
        /// 
        /// </summary>
        public EventDirection Direction
        {
            get { return Group == null ? EventDirection.Unknown : Group.Direction; }
        }

        /// <summary>
        /// 
        /// </summary>
        public EventSubject Subject
        {
            get { return Group == null ? EventSubject.Unknown : Group.Subject; }
        }

        /// <summary>
        /// 
        /// </summary>
        public EventType Type
        {
            get { return Group == null ? EventType.Unknown : Group.Type; }
        }

        /// <summary>
        /// 
        /// </summary>
        public EventCode()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        /// <param name="code"></param>
        /// <param name="group"></param>
        public EventCode(String description, UInt16 code, EventGroup group)
        {
            Description = description;
            Code = code;
            Group = group;
        }
    }
}