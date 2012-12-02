// FFXIVAPP
// EventGroup.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System;
using System.Collections.Generic;

namespace FFXIVAPP.Models
{
    public class EventGroup
    {
        private String Name { get; set; }
        private readonly List<EventGroup> _children = new List<EventGroup>();
        private EventGroup _parent;
        private UInt16 _flags;
        public List<EventCode> Codes { get; private set; }

        /// <summary>
        /// </summary>
        public UInt16 Flags
        {
            get
            {
                if (Parent == null)
                {
                    return _flags;
                }
                UInt16 combinedFlags = 0x0000;
                if ((_flags & EventParser.DirectionMask) != 0)
                {
                    combinedFlags |= (UInt16) (_flags & EventParser.DirectionMask);
                }
                else
                {
                    combinedFlags |= (UInt16) Parent.Direction;
                }
                if ((_flags & EventParser.SubjectMask) != 0)
                {
                    combinedFlags |= (UInt16) (_flags & EventParser.SubjectMask);
                }
                else
                {
                    combinedFlags |= (UInt16) Parent.Subject;
                }
                if ((_flags & EventParser.TypeMask) != 0)
                {
                    combinedFlags |= (UInt16) (_flags & EventParser.TypeMask);
                }
                else
                {
                    combinedFlags |= (UInt16) Parent.Type;
                }
                return combinedFlags;
            }
        }

        /// <summary>
        /// </summary>
        public EventDirection Direction
        {
            get { return (EventDirection) (Flags & EventParser.DirectionMask); }
            set { _flags = (UInt16) ((_flags & ~EventParser.DirectionMask) | (UInt16) value); }
        }

        /// <summary>
        /// </summary>
        public EventSubject Subject
        {
            get { return (EventSubject) (Flags & EventParser.SubjectMask); }
            set { _flags = (UInt16) ((_flags & ~EventParser.SubjectMask) | (UInt16) value); }
        }

        /// <summary>
        /// </summary>
        public EventType Type
        {
            get { return (EventType) (Flags & EventParser.TypeMask); }
            set { _flags = (UInt16) ((_flags & ~EventParser.TypeMask) | (UInt16) value); }
        }

        /// <summary>
        /// </summary>
        private EventGroup Parent
        {
            get { return _parent; }
            set
            {
                if ((_parent != null) && (value != null))
                {
                    _parent._children.Remove(this);
                }
                if (value == null)
                {
                    return;
                }
                _parent = value;
                value._children.Add(this);
            }
        }

        /// <summary>
        /// </summary>
        public EventGroup()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        public EventGroup(String name)
        {
            Init(name, null);
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="parent"> </param>
        public EventGroup(String name, EventGroup parent)
        {
            Init(name, parent);
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="parent"> </param>
        private void Init(String name, EventGroup parent)
        {
            Name = name;
            Parent = parent;
        }

        /// <summary>
        /// </summary>
        /// <param name="kid"> </param>
        /// <returns> </returns>
        public EventGroup AddChild(EventGroup kid)
        {
            kid.Parent = this;
            return this;
        }
    }
}