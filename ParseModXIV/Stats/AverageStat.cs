// Project: ParseModXIV
// File: AverageStat.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;

namespace ParseModXIV.Stats
{
    public class AverageStat : LinkedStat
    {
        private int _numUpdates;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="total"></param>
        public AverageStat(string name, Stat<decimal> total) : base(name, 0)
        {
            SetupDepends(total);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="total"></param>
        private void SetupDepends(Stat<decimal> total)
        {
            AddDependency(total);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public AverageStat(string name) : base(name, 0)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="o"></param>
        /// <param name="n"></param>
        public override void DoDependencyValueChanged(object sender, object o, object n)
        {
            var newValue = (Decimal) n;
            Value = newValue / ++_numUpdates;
        }
    }
}