// FFXIVAPP.Client
// EventCode.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FFXIVAPP.Client.Plugins.Parse.Enums;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Plugins.Parse.Models.Events
{
    [DoNotObfuscate]
    public class EventCode : INotifyPropertyChanged
    {
        #region Property Bindings

        private string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged();
            }
        }

        public UInt32 Code
        {
            get { return _code; }
            set
            {
                _code = value;
                RaisePropertyChanged();
            }
        }

        private EventGroup Group
        {
            get { return _group; }
            set
            {
                _group = value;
                RaisePropertyChanged();
            }
        }

        public UInt32 Flags
        {
            get { return (ushort) (Group == null ? 0x00000000 : Group.Flags); }
        }

        public EventDirection Direction
        {
            get { return Group == null ? EventDirection.Unknown : Group.Direction; }
        }

        public EventSubject Subject
        {
            get { return Group == null ? EventSubject.Unknown : Group.Subject; }
        }

        public EventType Type
        {
            get { return Group == null ? EventType.Unknown : Group.Type; }
        }

        #endregion

        private uint _code;
        private string _description;
        private EventGroup _group;

        public EventCode()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="description"> </param>
        /// <param name="code"> </param>
        /// <param name="group"> </param>
        public EventCode(string description, UInt32 code, EventGroup group)
        {
            Description = description;
            Code = code;
            Group = group;
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
