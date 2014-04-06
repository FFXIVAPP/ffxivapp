// FFXIVAPP.Client
// EventGroup.cs
// 
// Copyright © 2007 - 2014 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FFXIVAPP.Client.Enums.Parse;

namespace FFXIVAPP.Client.Models.Parse.Events
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

        public UInt64 Flags
        {
            get
            {
                if (Parent == null)
                {
                    return _flags;
                }
                UInt64 combinedFlags = 0x0;
                if ((_flags & EventParser.DirectionMask) != 0)
                {
                    combinedFlags |= (_flags & EventParser.DirectionMask);
                }
                else
                {
                    combinedFlags |= (UInt32) Parent.Direction;
                }
                if ((_flags & EventParser.SubjectMask) != 0)
                {
                    combinedFlags |= (_flags & EventParser.SubjectMask);
                }
                else
                {
                    combinedFlags |= (UInt32) Parent.Subject;
                }
                if ((_flags & EventParser.TypeMask) != 0)
                {
                    combinedFlags |= (_flags & EventParser.TypeMask);
                }
                else
                {
                    combinedFlags |= (UInt32) Parent.Type;
                }
                return combinedFlags;
            }
        }

        public EventDirection Direction
        {
            get { return (EventDirection) (Flags & EventParser.DirectionMask); }
            set
            {
                _flags = ((_flags & ~EventParser.DirectionMask) | (UInt64) value);
                RaisePropertyChanged();
            }
        }

        public EventSubject Subject
        {
            get { return (EventSubject) (Flags & EventParser.SubjectMask); }
            set
            {
                _flags = ((_flags & ~EventParser.SubjectMask) | (UInt64) value);
                RaisePropertyChanged();
            }
        }

        public EventType Type
        {
            get { return (EventType) (Flags & EventParser.TypeMask); }
            set
            {
                _flags = ((_flags & ~EventParser.TypeMask) | (UInt32) value);
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
        private UInt64 _flags;
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
