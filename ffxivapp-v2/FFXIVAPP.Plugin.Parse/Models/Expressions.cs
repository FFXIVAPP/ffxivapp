// FFXIVAPP.Plugin.Parse
// Expressions.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using FFXIVAPP.Plugin.Parse.Helpers;
using FFXIVAPP.Plugin.Parse.RegularExpressions;

namespace FFXIVAPP.Plugin.Parse.Models
{
    internal class Expressions : INotifyPropertyChanged
    {
        private Match _pAction;
        private Match _pUsed;
        private Match _pAdditional;
        private Match _pCounter;
        private Match _pBlock;
        private Match _pParry;
        private Match _pResist;
        private Match _pEvade;
        private Match _pMultiFlag;
        private string[] _pMultiFlagAbility;
        private Match _pMulti;
        private Match _mAction;
        private Match _mResist;
        private Match _mEvade;
        private Match _mUsed;
        private Match _mAdditional;
        private Match _mCounter;
        private Match _mBlock;
        private Match _mParry;

        private string Cleaned { get; set; }
        public string Counter { get; private set; }
        public string Added { get; private set; }
        public string Type { get; private set; }
        public string RAttack { get; private set; }
        public string Attack { get; private set; }
        public string You { get; private set; }

        public Match pAction
        {
            get { return _pAction ?? (_pAction = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _pAction = value;
                RaisePropertyChanged();
            }
        }

        public Match pUsed
        {
            get { return _pUsed ?? (_pUsed = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _pUsed = value;
                RaisePropertyChanged();
            }
        }

        public Match pAdditional
        {
            get { return _pAdditional ?? (_pAdditional = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _pAdditional = value;
                RaisePropertyChanged();
            }
        }

        public Match pCounter
        {
            get { return _pCounter ?? (_pCounter = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _pCounter = value;
                RaisePropertyChanged();
            }
        }

        public Match pBlock
        {
            get { return _pBlock ?? (_pBlock = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _pBlock = value;
                RaisePropertyChanged();
            }
        }

        public Match pParry
        {
            get { return _pParry ?? (_pParry = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _pParry = value;
                RaisePropertyChanged();
            }
        }

        public Match pResist
        {
            get { return _pResist ?? (_pResist = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _pResist = value;
                RaisePropertyChanged();
            }
        }

        public Match pEvade
        {
            get { return _pEvade ?? (_pAction = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _pEvade = value;
                RaisePropertyChanged();
            }
        }

        public Match pMultiFlag
        {
            get { return _pMultiFlag ?? (_pMultiFlag = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _pMultiFlag = value;
                RaisePropertyChanged();
            }
        }

        public string[] pMultiFlagAbility
        {
            get { return _pMultiFlagAbility ?? (_pMultiFlagAbility = new[] {""}); }
            private set
            {
                _pMultiFlagAbility = value;
                RaisePropertyChanged();
            }
        }

        public Match pMulti
        {
            get { return _pMulti ?? (_pMulti = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _pMulti = value;
                RaisePropertyChanged();
            }
        }

        public Match mAction
        {
            get { return _mAction ?? (_mAction = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _mAction = value;
                RaisePropertyChanged();
            }
        }

        public Match mUsed
        {
            get { return _mUsed ?? (_mUsed = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _mUsed = value;
                RaisePropertyChanged();
            }
        }

        public Match mAdditional
        {
            get { return _mAdditional ?? (_mAdditional = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _mAdditional = value;
                RaisePropertyChanged();
            }
        }

        public Match mCounter
        {
            get { return _mCounter ?? (_mCounter = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _mCounter = value;
                RaisePropertyChanged();
            }
        }

        public Match mBlock
        {
            get { return _mBlock ?? (_mBlock = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _mBlock = value;
                RaisePropertyChanged();
            }
        }

        public Match mParry
        {
            get { return _mParry ?? (_mParry = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _mParry = value;
                RaisePropertyChanged();
            }
        }

        public Match mResist
        {
            get { return _mResist ?? (_mResist = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _mResist = value;
                RaisePropertyChanged();
            }
        }

        public Match mEvade
        {
            get { return _mEvade ?? (_mEvade = Regex.Match("ph", @"^\.$")); }
            private set
            {
                _mEvade = value;
                RaisePropertyChanged();
            }
        }

        public Expressions(string line)
        {
            Cleaned = line;
            Initialize();
        }

        private void Initialize()
        {
            switch (Common.Constants.GameLanguage)
            {
                case "French":
                    pAction = PlayerRegEx.ActionFr.Match(Cleaned);
                    pUsed = PlayerRegEx.UsedFr.Match(Cleaned);
                    pAdditional = PlayerRegEx.AdditionalFr.Match(Cleaned);
                    pCounter = PlayerRegEx.CounterFr.Match(Cleaned);
                    pBlock = PlayerRegEx.BlockFr.Match(Cleaned);
                    pParry = PlayerRegEx.ParryFr.Match(Cleaned);
                    pResist = PlayerRegEx.ResistFr.Match(Cleaned);
                    pEvade = PlayerRegEx.EvadeFr.Match(Cleaned);
                    pMultiFlag = PlayerRegEx.MultiFlagFr.Match(Cleaned);
                    pMultiFlagAbility = ParseHelper.MultiFr;
                    pMulti = PlayerRegEx.MultiFr.Match(Cleaned);
                    mAction = MonsterRegEx.ActionFr.Match(Cleaned);
                    mResist = MonsterRegEx.ResistFr.Match(Cleaned);
                    mEvade = MonsterRegEx.EvadeFr.Match(Cleaned);
                    Counter = "Contre";
                    Added = "Effet Supplémentaire";
                    Type = "PV";
                    RAttack = "D'Attaque À Distance";
                    Attack = "Attaque";
                    You = @"^[Vv]ous$";
                    break;
                case "Japanese":
                    pAction = PlayerRegEx.ActionJa.Match(Cleaned);
                    pUsed = PlayerRegEx.UsedJa.Match(Cleaned);
                    pAdditional = PlayerRegEx.AdditionalJa.Match(Cleaned);
                    pCounter = PlayerRegEx.CounterJa.Match(Cleaned);
                    pBlock = PlayerRegEx.BlockJa.Match(Cleaned);
                    pParry = PlayerRegEx.ParryJa.Match(Cleaned);
                    pResist = PlayerRegEx.ResistJa.Match(Cleaned);
                    pEvade = PlayerRegEx.EvadeJa.Match(Cleaned);
                    pMultiFlag = PlayerRegEx.MultiFlagJa.Match(Cleaned);
                    pMultiFlagAbility = ParseHelper.MultiJa;
                    pMulti = PlayerRegEx.MultiJa.Match(Cleaned);
                    mAction = MonsterRegEx.ActionJa.Match(Cleaned);
                    mResist = MonsterRegEx.ResistJa.Match(Cleaned);
                    mEvade = MonsterRegEx.EvadeJa.Match(Cleaned);
                    Counter = "カウンター";
                    Added = "追加効果";
                    Type = "ＨＰ";
                    RAttack = "Ranged Attack";
                    Attack = "Attack";
                    You = @"^\.$";
                    break;
                case "German":
                    pAction = PlayerRegEx.ActionDe.Match(Cleaned);
                    pUsed = PlayerRegEx.UsedDe.Match(Cleaned);
                    pAdditional = PlayerRegEx.AdditionalDe.Match(Cleaned);
                    pCounter = PlayerRegEx.CounterDe.Match(Cleaned);
                    pBlock = PlayerRegEx.BlockDe.Match(Cleaned);
                    pParry = PlayerRegEx.ParryDe.Match(Cleaned);
                    pResist = PlayerRegEx.ResistDe.Match(Cleaned);
                    pEvade = PlayerRegEx.EvadeDe.Match(Cleaned);
                    pMultiFlag = PlayerRegEx.MultiFlagDe.Match(Cleaned);
                    pMultiFlagAbility = ParseHelper.MultiDe;
                    pMulti = PlayerRegEx.MultiDe.Match(Cleaned);
                    mAction = MonsterRegEx.ActionDe.Match(Cleaned);
                    mResist = MonsterRegEx.ResistDe.Match(Cleaned);
                    mEvade = MonsterRegEx.EvadeDe.Match(Cleaned);
                    Counter = "Counter";
                    Added = "Zusatzefeckt";
                    Type = "HP";
                    RAttack = "Ranged Attack";
                    Attack = "Attack";
                    You = @"^[Dd]u$";
                    break;
                default:
                    pAction = PlayerRegEx.ActionEn.Match(Cleaned);
                    pUsed = PlayerRegEx.UsedEn.Match(Cleaned);
                    pAdditional = PlayerRegEx.AdditionalEn.Match(Cleaned);
                    pCounter = PlayerRegEx.CounterEn.Match(Cleaned);
                    pBlock = PlayerRegEx.BlockEn.Match(Cleaned);
                    pParry = PlayerRegEx.ParryEn.Match(Cleaned);
                    pResist = PlayerRegEx.ResistEn.Match(Cleaned);
                    pEvade = PlayerRegEx.EvadeEn.Match(Cleaned);
                    pMultiFlag = PlayerRegEx.MultiFlagEn.Match(Cleaned);
                    pMultiFlagAbility = ParseHelper.MultiEn;
                    pMulti = PlayerRegEx.MultiEn.Match(Cleaned);
                    mAction = MonsterRegEx.ActionEn.Match(Cleaned);
                    mResist = MonsterRegEx.ResistEn.Match(Cleaned);
                    mEvade = MonsterRegEx.EvadeEn.Match(Cleaned);
                    Counter = "Counter";
                    Added = "Additional Effect";
                    Type = "HP";
                    RAttack = "Ranged Attack";
                    Attack = "Attack";
                    You = @"^[Yy]our?$";
                    break;
            }
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