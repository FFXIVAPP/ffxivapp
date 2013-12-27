// FFXIVAPP.Common
// PartyEntity.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using FFXIVAPP.Common.Core.Memory.Enums;
using FFXIVAPP.Common.Core.Memory.Interfaces;
using FFXIVAPP.Common.Helpers;

namespace FFXIVAPP.Common.Core.Memory
{
    public class PartyEntity : IPartyEntity
    {
        private string _name;
        private List<StatusEntry> _statusEntries;

        public decimal HPPercent
        {
            get
            {
                try
                {
                    return (decimal) (Convert.ToDouble(HPCurrent) / Convert.ToDouble(HPMax));
                }
                catch
                {
                    return 0;
                }
            }
        }

        public string HPString
        {
            get { return String.Format("{0}/{1} [{2:P2}]", HPCurrent, HPMax, HPPercent); }
        }

        public decimal MPPercent
        {
            get
            {
                try
                {
                    return (decimal) (Convert.ToDouble(MPCurrent) / Convert.ToDouble(MPMax));
                }
                catch
                {
                    return 0;
                }
            }
        }

        public string MPString
        {
            get { return String.Format("{0}/{1} [{2:P2}]", MPCurrent, MPMax, MPPercent); }
        }

        public bool IsValid
        {
            get { return ID > 0 && !String.IsNullOrWhiteSpace(Name); }
        }

        public Coordinate Coordinate { get; set; }

        public string Name
        {
            get { return _name ?? ""; }
            set { _name = StringHelper.TitleCase(value); }
        }

        public uint ID { get; set; }
        public double X { get; set; }
        public double Z { get; set; }
        public double Y { get; set; }
        public Actor.Job Job { get; set; }
        public byte Level { get; set; }
        public int HPCurrent { get; set; }
        public int HPMax { get; set; }
        public int MPCurrent { get; set; }
        public int MPMax { get; set; }

        public List<StatusEntry> StatusEntries
        {
            get { return _statusEntries ?? (_statusEntries = new List<StatusEntry>()); }
            set
            {
                if (_statusEntries == null)
                {
                    _statusEntries = new List<StatusEntry>();
                }
                _statusEntries = value;
            }
        }

        public float GetDistanceTo(ActorEntity compare)
        {
            var distanceX = (float) Math.Abs(X - compare.X);
            var distanceY = (float) Math.Abs(Y - compare.Y);
            var distanceZ = (float) Math.Abs(Z - compare.Z);
            return (float) Math.Sqrt((distanceX * distanceX) + (distanceY * distanceY) + (distanceZ * distanceZ));
        }
    }
}
