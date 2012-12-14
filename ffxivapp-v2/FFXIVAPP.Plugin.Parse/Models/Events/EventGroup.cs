// FFXIVAPP.Plugin.Parse
// EventGroup.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FFXIVAPP.Plugin.Parse.Enums;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models.Events
{
    public class EventGroup : INotifyPropertyChanged
    {
        #region Property Bindings

        private List<EventCode> _codes;
        private string _name;

        private string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        public List<EventCode> Codes
        {
            get { return _codes ?? (_codes = new List<EventCode>()); }
            private set
            {
                _codes = value;
                RaisePropertyChanged();
            }
        }

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

        public EventDirection Direction
        {
            get { return (EventDirection) (Flags & EventParser.DirectionMask); }
            set
            {
                _flags = (UInt16) ((_flags & ~EventParser.DirectionMask) | (UInt16) value);
                RaisePropertyChanged();
            }
        }

        public EventSubject Subject
        {
            get { return (EventSubject) (Flags & EventParser.SubjectMask); }
            set
            {
                _flags = (UInt16) ((_flags & ~EventParser.SubjectMask) | (UInt16) value);
                RaisePropertyChanged();
            }
        }

        public EventType Type
        {
            get { return (EventType) (Flags & EventParser.TypeMask); }
            set
            {
                _flags = (UInt16) ((_flags & ~EventParser.TypeMask) | (UInt16) value);
                RaisePropertyChanged();
            }
        }

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
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Declarations

        private readonly List<EventGroup> _children = new List<EventGroup>();
        private UInt16 _flags;
        private EventGroup _parent;

        #endregion

        /// <summary>
        /// </summary>
        public EventGroup()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="parent"> </param>
        public EventGroup(string name, EventGroup parent = null)
        {
            Init(name, parent);
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="parent"> </param>
        private void Init(string name, EventGroup parent)
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

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
